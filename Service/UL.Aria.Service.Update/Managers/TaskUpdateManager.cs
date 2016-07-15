using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Parser;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Update.Configuration;
using UL.Aria.Service.Update.Results;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Update.Managers
{
    /// <summary>
    /// Provides a Update manager class for updating Tasks in bulk
    /// </summary>
    [Entity(EntityTypeEnumDto.Task)]
    public class TaskUpdateManager : UpdateManagerBase, IUpdateManager
    {
        private const EntityTypeEnumDto EntityType = EntityTypeEnumDto.Task;

        private readonly ITaskProvider _taskProvider;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IProjectProvider _projectProvider;
        private readonly IAssetProvider _assetProvider;


        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectUpdateManager" /> class.
        /// </summary>
        /// <param name="projectProvider">The project provider.</param>
        /// <param name="taskProvider">The project provider.</param>
        /// <param name="assetProvider">The asset provider.</param>
        /// <param name="fileLogger">The file logger.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public TaskUpdateManager(IProjectProvider projectProvider, ITaskProvider taskProvider, IAssetProvider assetProvider, IFileLogger fileLogger, IUpdateConfigurationSource config, IMapperRegistry mapperRegistry)
            : base(fileLogger, config)
        {
            this._projectProvider = projectProvider;
            this._taskProvider = taskProvider;
            _mapperRegistry = mapperRegistry;
            _assetProvider = assetProvider;
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
            var incompleted = new List<Lookup>();
            var logEntryMapper = GetFileLogMappingFunction();
            var lookups = this._projectProvider.FetchProjectLookups();
            foreach (var projLookup in lookups)
            {
                var containerId = projLookup.ContainerId.GetValueOrDefault();
                if (containerId != Guid.Empty)
                {
                    var taskLookups = _taskProvider.FetchLookups(containerId);
                    if (null == taskLookups)
                        continue;
                    var logEntries = this.FileLogger.GetLog<TaskExecutionResult>(logEntryMapper);

                    if (logEntries == null || logEntries.Count <= 0)
                        incompleted.AddRange(taskLookups);
                    else
                    {
                        incompleted.AddRange(from c in taskLookups
                                             join l in logEntries on c.Id.Value equals l.Lookup.Id.Value into results
                                             from i in results.DefaultIfEmpty()
                                             where i == null
                                             select c);
                    }
                }
            }

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
                    var task = _taskProvider.FetchById(lookup.ContainerId.Value, lookup.Id.Value);

                    if (task != null)
                    {
                        _assetProvider.SaveAssets(new List<PrimarySearchEntityBase>() { task });
                        return new TaskExecutionResult(lookup, true);
                    }

                    return new TaskExecutionResult(lookup, false, "Task not found.");
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
