using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.InboundOrderProcessing.Resolver;
using UL.Aria.Service.Parser;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Update.Configuration;
using UL.Aria.Service.Update.Results;
using UL.Aria.Common;

namespace UL.Aria.Service.Update.Managers
{
    /// <summary>
    /// Provides delegates that update Orders.
    /// </summary>
    [Entity(EntityTypeEnumDto.IncomingOrder)]
    public class IncomingOrderUpdateManager  : UpdateManagerBase, IUpdateManager
    {
        private readonly IIncomingOrderProvider _incomingOrderProvider;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IXmlParser _parser;  
        private const EntityTypeEnumDto EntityType = EntityTypeEnumDto.IncomingOrder;



        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectUpdateManager" /> class.
        /// </summary>
        /// <param name="incomingOrderProvider">The incoming order provider.</param>
        /// <param name="parserResolver">The parser resolver.</param>
        /// <param name="fileLogger">The file logger.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public IncomingOrderUpdateManager(IIncomingOrderProvider incomingOrderProvider, IXmlParserResolver parserResolver, IFileLogger fileLogger, IUpdateConfigurationSource config, IMapperRegistry mapperRegistry)
            :base(fileLogger, config)
        {
            this._incomingOrderProvider = incomingOrderProvider;
            _mapperRegistry = mapperRegistry;
            _parser = parserResolver.Resolve(EntityType.ToString());
        }

        /// <summary>
        /// Gets the file log mapping function.
        /// </summary>
        /// <returns></returns>
        internal Func<string, TaskExecutionResult> GetFileLogMappingFunction()
        {
            Func<string, TaskExecutionResult> func = (stringToMap) =>
            {
                TaskExecutionResult result = null;
                var values = stringToMap.Split('\t');
                if (values.Length == 4)
                {
                    result = new TaskExecutionResult(new Lookup()
                    {
                        Id = Guid.Parse(values[0]),
                        Name = values[1]
                    },
                        values[2].ParseOrDefault(false),
                       values[3]);
                }

                return result;
            };

            return func;
        }

        /// <summary>
        /// When implemented in a derived class it resolves a list of items that were not completed
        /// during a previous execution of the process.
        /// </summary>
        /// <returns></returns>
        internal protected override IEnumerable<Lookup> ResolveIncompleteItems()
        {
            var lookups = this._incomingOrderProvider.FetchIncomingOrderLookups();

            if (!this.FileLogger.IsExistingFile)
                return lookups;

            var logEntries = this.FileLogger.GetLog<TaskExecutionResult>(GetFileLogMappingFunction());

            if (logEntries == null || logEntries.Count <= 0)
                return lookups;

            var incompleted = from c in lookups
                              join l in logEntries on c.Id.Value equals l.Lookup.Id.Value into results
                              from i in results.DefaultIfEmpty()
                              where i == null
                              select c;

            return incompleted.ToList();
        }

        /// <summary>
        /// When implemented in a derived class it gets the Item Processing function.
        /// </summary>
        /// <param name="lookup">The lookup.</param>
        /// <returns></returns>
        internal protected override Func<TaskExecutionResult> GetItemProcessingFunction(Lookup lookup)
        {
            Func<TaskExecutionResult> func = () =>
            {
                try
                {

                    var order = _incomingOrderProvider.FindById(lookup.Id.Value);
                    if (order != null)
                    {
                        var parsed = _parser.Parse(order.OriginalXmlParsed);
                        if (null == parsed)
                        {
                            return new TaskExecutionResult(lookup, false, "Unable to parse xml into Incoming Order DTO object.");
                        }

                        var reparsedOrder = _mapperRegistry.Map<IncomingOrder>(parsed);
                        if (reparsedOrder == null)
                        {
                            return new TaskExecutionResult(lookup, false, "Unable to map into Incoming Order object.");
                        }
                        reparsedOrder.CompanyId = order.CompanyId;
                        reparsedOrder.CompanyName = order.CompanyName;
                       
                        _incomingOrderProvider.Update(reparsedOrder.Id.GetValueOrDefault(), reparsedOrder);
                            return new TaskExecutionResult(lookup, true);
                        
                        
                    }

                    return new TaskExecutionResult(lookup, false, "Incoming Order not found.");
                }
                catch (Exception ex)
                {
                    return new TaskExecutionResult(lookup, false, ex.Message);
                }

            };

            return func;
        }     

    }
}
