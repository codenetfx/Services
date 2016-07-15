using System;
using System.Collections.Generic;
using System.Linq;
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

namespace UL.Aria.Service.Update.Managers
{
    /// <summary>
    /// Provides an implemenation of the IUpdateManager for the Project Entity.
    /// </summary>
    [Entity(EntityTypeEnumDto.Project)]
    public class ProjectUpdateManager : UpdateManagerBase
    {
        private readonly IProjectProvider _projectProvider;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IXmlParser _parser;  
        private const EntityTypeEnumDto EntityType = EntityTypeEnumDto.Project;


        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectUpdateManager"/> class.
        /// </summary>
        /// <param name="projectProvider">The project provider.</param>
        /// <param name="parserResolver">The parser resolver.</param>
        /// <param name="fileLogger">The file logger.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="mapperRegistry"></param>
        public ProjectUpdateManager(IProjectProvider projectProvider, IXmlParserResolver parserResolver, IFileLogger fileLogger, IUpdateConfigurationSource config, IMapperRegistry mapperRegistry)
            :base(fileLogger, config)
        {
            _projectProvider = projectProvider;
            _mapperRegistry = mapperRegistry;
            _parser = parserResolver.Resolve(EntityType.ToString());
        }
      

        /// <summary>
        /// Gets the file log mapping function.
        /// </summary>
        /// <returns></returns>
        internal Func<string, TaskExecutionResult>  GetFileLogMappingFunction()
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
        
        /// <summary>
        /// When implemented in a derived class it resolves a list of items that were not completed
        /// during a previous execution of the process.
        /// </summary>
        /// <returns></returns>
        internal protected override IEnumerable<Lookup> ResolveIncompleteItems()
        {
            var lookups = _projectProvider.FetchProjectLookups();

            if (!FileLogger.IsExistingFile)
                return lookups;

            var logEntries = FileLogger.GetLog(GetFileLogMappingFunction());

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
                   
                    var project = _projectProvider.FetchById(lookup.Id.Value);
                    if (project != null)
                    {
                        var parsed = _parser.Parse(project.OriginalXmlParsed);
                         if (null == parsed)
                        {
                            return new TaskExecutionResult(lookup, false, "Unable to parse xml into Incoming Order DTO object.");
                        }
                         var incomingOrder = _mapperRegistry.Map<IncomingOrder>(parsed);

                        var lower = incomingOrder.Status.ToLower();
                        if (lower == "canceled" || lower == "cancelled")
                             project.ProjectStatus = ProjectStatusEnumDto.Canceled;
                         var projectIncomingOrderContactId = project.IncomingOrderContact.Id;
                         var projectBillToContactId = project.BillToContact.Id;
                         var projectShipToContactId = project.ShipToContact.Id;
                         var projectIncomingOrderCustomerId = project.IncomingOrderCustomer.Id;
                         IncomingOrderProvider.MapIncomingOrderToProject(incomingOrder, project);
                         project.IncomingOrderContact.Id = projectIncomingOrderContactId;
                         project.BillToContact.Id = projectBillToContactId;
                         project.ShipToContact.Id = projectShipToContactId;
                         project.IncomingOrderCustomer.Id = projectIncomingOrderCustomerId;
                        foreach (var projectServiceLine in project.ServiceLines)
                        {
                            var line =
                                incomingOrder.ServiceLines.FirstOrDefault(
                                    l => l.ExternalId == projectServiceLine.ExternalId);
                            if (null == line)
                                continue;
                            projectServiceLine.IndustryCode = line.IndustryCode;
                            projectServiceLine.ServiceCode = line.ServiceCode;
                            projectServiceLine.LocationCode = line.LocationCode;
							projectServiceLine.LocationName = line.LocationName;
							projectServiceLine.LocationCodeLabel = line.LocationCodeLabel;
							projectServiceLine.ServiceCodeLabel = line.ServiceCodeLabel;
							projectServiceLine.IndustryCodeLabel = line.IndustryCodeLabel;
						}
                         _projectProvider.UpdateFromIncomingOrder(project);
                       
                            return new TaskExecutionResult(lookup, true);
                        //}
                       // return new TaskExecutionResult(lookup, false, "Unable to parse xml into Project object.");
                    }

                    return new TaskExecutionResult(lookup, false, "Project not found.");
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
