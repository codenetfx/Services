using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.InboundOrderProcessing.Resolver;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Update.Configuration;
using UL.Aria.Service.Update.Results;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.Update.Managers
{

    /// <summary>
    /// Provides a process to update Order search metadata.
    /// </summary>
    [Entity(EntityTypeEnumDto.Order)]
    public class OrderUpdateManager : UpdateManagerBase, IUpdateManager
    {
        private IOrderProvider _orderProvider;
        private IAssetProvider _assetProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderUpdateManager" /> class.
        /// </summary>
        /// <param name="orderProvider">The order provider.</param>
        /// <param name="fileLogger">The file logger.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="assetProvider">The asset provider.</param>
        public OrderUpdateManager(IOrderProvider orderProvider, IFileLogger fileLogger, IUpdateConfigurationSource config, IAssetProvider assetProvider)
            : base(fileLogger, config)
        {
            _orderProvider = orderProvider;
            _assetProvider = assetProvider;
        }

        /// <summary>
        /// When implemented in a derived class it gets the Item Processing function.
        /// </summary>
        /// <param name="lookup">The lookup.</param>
        /// <returns></returns>
        protected internal override Func<TaskExecutionResult> GetItemProcessingFunction(Lookup lookup)
        {
            Func<TaskExecutionResult> func = () =>
            {
                try
                {
                    var order = _orderProvider.FindById(lookup.Id.Value);
                    if (order != null && order.Id.GetValueOrDefault() != Guid.Empty)
                    {
                        _assetProvider.Update(order.Id.Value, order);
                        return new TaskExecutionResult(lookup, true);
                    }
                    else
                    {
                        return new TaskExecutionResult(lookup, false, "Order not found.");
                    }
                }
                catch (Exception ex)
                {
                    return new TaskExecutionResult(lookup, false, ex.Message);
                }

            };

            return func;
        }

        /// <summary>
        /// When implemented in a derived class it resolves a list of items that were not completed
        /// during a previous execution of the process.
        /// </summary>
        /// <returns></returns>
        protected internal override IEnumerable<Domain.Entity.Lookup> ResolveIncompleteItems()
        {
            var lookups = _orderProvider.FindOrderLookups();

            if (!FileLogger.IsExistingFile)
                return lookups;

            var logEntries = FileLogger.GetLog(GetFileLogMappingFunction());

            if (logEntries == null || logEntries.Count <= 0)
                return lookups;

            var incompleted = from c in lookups
                              join l in logEntries on c.Id.Value equals l.Lookup.Id.GetValueOrDefault() into results
                              from i in results.DefaultIfEmpty()
                              where i == null 
                              select c;

            return incompleted.ToList();
        }


        /// <summary>
        /// Gets the file log mapping function.
        /// </summary>
        /// <returns></returns>
        internal Func<string, TaskExecutionResult> GetFileLogMappingFunction()
        {
            Func<string, TaskExecutionResult> func = stringToMap =>
            {
                TaskExecutionResult result = null;
                var values = stringToMap.Split('\t');
                if (values.Length == 4)
                {
                    result = new TaskExecutionResult(new Lookup
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
    }
}
