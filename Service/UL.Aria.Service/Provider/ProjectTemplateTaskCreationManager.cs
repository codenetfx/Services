using System;
using System.Collections.Generic;
using System.Linq;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Linq;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Implements operations for creating <see cref="Task"/> entities for <see cref="Domain.Entity.Project"/> entities based on
    /// <see cref="Domain.Entity.ProjectTemplate"/> entities.
    /// </summary>
    public class ProjectTemplateTaskCreationManager : IProjectTemplateTaskCreationManager
    {
        private readonly IProjectTemplateManager _projectTemplateManager;
        private readonly ITaskManager _taskManager;
        private readonly ITaskTypeProvider _taskTypeProvider;
        private readonly IMapperRegistry _mapperRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTemplateTaskCreationManager" /> class.
        /// </summary>
        /// <param name="projectTemplateManager">The project template manager.</param>
        /// <param name="taskManager">The task manager.</param>
        /// <param name="taskTypeProvider">The task type provider.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public ProjectTemplateTaskCreationManager(IProjectTemplateManager projectTemplateManager, ITaskManager taskManager, ITaskTypeProvider taskTypeProvider, IMapperRegistry mapperRegistry)
        {
            _projectTemplateManager = projectTemplateManager;
            _taskManager = taskManager;
            _taskTypeProvider = taskTypeProvider;
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        /// Creates the project template tasks for update.
        /// </summary>
        /// <param name="projectTemplateId">The project template identifier.</param>
        /// <param name="containerId">The container identifier.</param>
        public void CreateProjectTemplateTasksForUpdate(Guid? projectTemplateId, Guid containerId)
        {
            if (projectTemplateId.GetValueOrDefault() == default (Guid))
                return;
            var max = Flatten(_taskManager.FetchAll(containerId)).Max(y => y.TaskNumber);
            CreateProjectTemplateTasks(projectTemplateId.Value, containerId, max);
        }

        /// <summary>
        /// Creates the project template tasks.
        /// </summary>
        /// <param name="projectTemplateId">The project template identifier.</param>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="taskNumberSeed">The task number seed.</param>
        public void CreateProjectTemplateTasks (Guid projectTemplateId, Guid containerId, int taskNumberSeed = 0)
        {
            var projectTemplate = _projectTemplateManager.FindById(projectTemplateId);

            if (projectTemplate != null)
            {

                var tasks = _mapperRegistry.Map<List<Task>>(projectTemplate.TaskTemplates);
                if (tasks != null && tasks.Any())
                {
                    // TODO : Optimize FetchAll, perhaps "Fetch all template task types" method.
                    var taskTypeDict = _taskTypeProvider.FetchAll().ToDictionary(x => x.Id);
                    Flatten(tasks).ToList().ForEach(x =>
                    {
                        x.TaskNumber += taskNumberSeed;
                        if (x.TaskTypeId != null && taskTypeDict.ContainsKey(x.TaskTypeId))
                        {
                            var taskType = taskTypeDict[x.TaskTypeId];
                            x.TaskOwner = taskType.TaskOwner;
                            var temp = Convert.ToDouble(taskType.EstimatedDuration.GetValueOrDefault());
                            x.EstimatedDuration = (temp == 0) ? null : (double?)temp;
                        }

                    });

                    _taskManager.BulkCreate(containerId, tasks);
                }
            }
        }

        private IEnumerable<Task> Flatten(IEnumerable<Task> tasks)
        {
            return tasks.SelectMany(c => Flatten(c.SubTasks)).Concat(tasks);
        }
    }
}