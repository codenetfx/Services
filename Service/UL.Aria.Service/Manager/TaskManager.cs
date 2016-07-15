using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using UL.Aria.Service.Configuration;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Domain.View;
using UL.Aria.Service.Logging;
using UL.Aria.Service.Manager.DataRule;
using UL.Aria.Service.Manager.DataRule.Task;
using UL.Aria.Service.Manager.Validation;
using UL.Aria.Service.Notifications;
using UL.Aria.Service.Provider;
using UL.Aria.Service.TaskStatus;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using System.Transactions;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Class TaskManager
    /// </summary>
    public sealed class TaskManager : ITaskManager
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IContainerManager _containerManager;
        private readonly ILogManager _logManager;
        private readonly IPrincipalResolver _principalResolver;
        private readonly IProfileManager _profileManager;
        private readonly IProjectProvider _projectProvider;
        private readonly ISearchProvider _searchProvider;
        private readonly ISmtpClientManager _smtpClientManager;
        private readonly IMultiProjectDocumentBuilder _taskDocumentBuilder;
        private readonly ITaskProvider _taskProvider;
        private readonly INotificationManager _notificationManager;
        private readonly ITaskValidationManager _taskValidationManager;
        private readonly ITaskNotificationCheckManager _taskNotificationCheckManager;
        private readonly ITransactionFactory _transactionFactory;
        private readonly ITaskFetchStatusStrategyFactory _taskFetchStatusStrategyFactory;
        private readonly ITaskFetchStatusListStrategyFactory _taskFetchStatusListStrategyFactory;
        private readonly ITaskDeleteValidationManager _taskDeleteValidationManager;
        private readonly IDocumentTemplateManager _documentTemplateManager;
        private readonly IDocumentManagementManager _documentManagementManager;
        private readonly IMapperRegistry _mapper;
        private readonly IWorkflowManagerFactory _workflowManagerFactory;
        private readonly IWorkflowDataContextFactory _workflowDataContextFactory;
        private readonly ITaskTypeNotificationProvider _taskTypeNotificationProvider;
        private readonly IServiceConfiguration _serviceConfiguration;
        private readonly IProjectCoreManager _projectCoreManager;
	    private readonly ITaskTypeBehaviorProvider _taskTypeBehaviorProvider;
        private readonly ITaskNotificationProvider _taskNotificationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskManager" /> class.
        /// </summary>
        /// <param name="taskProvider">The task provider.</param>
        /// <param name="projectProvider">The project provider.</param>
        /// <param name="assetProvider">The asset provider.</param>
        /// <param name="taskDocumentBuilder">The task document builder.</param>
        /// <param name="searchProvider">The search provider.</param>
        /// <param name="smtpClientManager">The SMTP client manager.</param>
        /// <param name="logManager">The log manager.</param>
        /// <param name="profileManager">The profile manager.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="containerManager">The container manager.</param>
        /// <param name="notificationManager">The notification manager.</param>
        /// <param name="taskValidationManager">The task validation manager.</param>
        /// <param name="taskNotificationCheckManager">The task notification check manager.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="taskFetchStatusStrategyFactory">The task fetch status strategy factory.</param>
        /// <param name="taskFetchStatusListStrategyFactory">The task fetch status list strategy factory.</param>
        /// <param name="taskDeleteValidationManager">The task delete validation manager.</param>
        /// <param name="documentTemplateManager">The document template manager.</param>
        /// <param name="documentManagementManager">The document management manager.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="workflowManagerFactory">The workflow manager factory.</param>
        /// <param name="workflowDataContextFactory">The workflow data context factory.</param>
        /// <param name="taskTypeNotificationProvider">The task type notification provider.</param>
        /// <param name="serviceConfiguration">The service configuration.</param>
        /// <param name="projectCoreManager">The project core manager.</param>
        /// <param name="taskTypeBehaviorProvider">The task type behavior provider.</param>
        /// <param name="taskNotificationProvider">The task notification provider.</param>
	    public TaskManager(ITaskProvider taskProvider, IProjectProvider projectProvider, IAssetProvider assetProvider,
            IMultiProjectDocumentBuilder taskDocumentBuilder, ISearchProvider searchProvider,
            ISmtpClientManager smtpClientManager, ILogManager logManager, IProfileManager profileManager,
            IPrincipalResolver principalResolver, IContainerManager containerManager, INotificationManager notificationManager,
            ITaskValidationManager taskValidationManager,
            ITaskNotificationCheckManager taskNotificationCheckManager,
            ITransactionFactory transactionFactory,
            ITaskFetchStatusStrategyFactory taskFetchStatusStrategyFactory,
            ITaskFetchStatusListStrategyFactory taskFetchStatusListStrategyFactory,
            ITaskDeleteValidationManager taskDeleteValidationManager,
            IDocumentTemplateManager documentTemplateManager,
            IDocumentManagementManager documentManagementManager,
            IMapperRegistry mapper,
            IWorkflowManagerFactory workflowManagerFactory,
            IWorkflowDataContextFactory workflowDataContextFactory,
            ITaskTypeNotificationProvider taskTypeNotificationProvider,
            IServiceConfiguration serviceConfiguration,
            IProjectCoreManager projectCoreManager,
			ITaskTypeBehaviorProvider taskTypeBehaviorProvider,
            ITaskNotificationProvider taskNotificationProvider)
        {
            _taskProvider = taskProvider;
            _projectProvider = projectProvider;
            _assetProvider = assetProvider;
            _taskDocumentBuilder = taskDocumentBuilder;
            _searchProvider = searchProvider;
            _smtpClientManager = smtpClientManager;
            _logManager = logManager;
            _profileManager = profileManager;
            _principalResolver = principalResolver;
            _containerManager = containerManager;
            _notificationManager = notificationManager;
            _taskValidationManager = taskValidationManager;
            _taskNotificationCheckManager = taskNotificationCheckManager;
            _transactionFactory = transactionFactory;
            _taskFetchStatusStrategyFactory = taskFetchStatusStrategyFactory;
            _taskFetchStatusListStrategyFactory = taskFetchStatusListStrategyFactory;
            _taskDeleteValidationManager = taskDeleteValidationManager;
            _documentTemplateManager = documentTemplateManager;
            _documentManagementManager = documentManagementManager;
            _mapper = mapper;
            _workflowManagerFactory = workflowManagerFactory;
            _workflowDataContextFactory = workflowDataContextFactory;
            _taskTypeNotificationProvider = taskTypeNotificationProvider;
            _serviceConfiguration = serviceConfiguration;
            _projectCoreManager = projectCoreManager;
	        _taskTypeBehaviorProvider = taskTypeBehaviorProvider;
            _taskNotificationProvider = taskNotificationProvider;
        }


        /// <summary>
        /// Updates multiple tasks.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="updatedTasks">The tasks.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Tasks with associated projects that are Completed or Canceled cannot be updated.</exception>
        public TaskChangeResult UpdateBulk(Guid containerId, List<Task> updatedTasks)
        {
            IEnumerable<Task> tasks = null;
            List<Task> deletedTasks = null;

            using (var scope = _transactionFactory.Create())
            {
                Project project = GetEditableProject(containerId);

                var context = _workflowDataContextFactory.Create(project, updatedTasks);

                // Perform validations.
                if (
                    ValidateTasks(context.UserAlteredTaskIds, context.WorkingTasks,
                        context.OriginalTasks).Any())
                {
                    throw new ArgumentException(
                        "Validation errors exist.  Please call ValidateBulk before calling UpdateBulk.");
                }

                var workflowManager = this._workflowManagerFactory.Create<Project, Task>(Workflow.TaskModification);
                workflowManager.Process(context);
                var wfUpdatedTasks = context.GetUpdatedEntities();

                // Update tasks in the database.

                _taskProvider.BulkUpdate(containerId, wfUpdatedTasks);
                _assetProvider.SaveAssets(wfUpdatedTasks);

	            var updatedProject = context.GetUpdatedParent();
                var minTaskDueDate = wfUpdatedTasks.Min(x => x.DueDate);
                var minDueDateTask = wfUpdatedTasks.FirstOrDefault(x => x.DueDate == minTaskDueDate);
				UpdateProjectTaskMinimumDueDate(updatedProject, wfUpdatedTasks, minDueDateTask);
				if (updatedProject.TaskMinimumDueDate != context.OriginalProject.TaskMinimumDueDate || updatedProject.MinimumDueDateTaskId != context.OriginalProject.MinimumDueDateTaskId)
	            {
		            context.IsActiveProjectDirty = true;
	            }

				// This is potentially set in rules too
				if (context.IsActiveProjectDirty)
				{
					_projectCoreManager.Update(updatedProject.Id.GetValueOrDefault(), updatedProject, true);
				}

                // Process notifications.
                CheckAndCreateTaskNotifications(wfUpdatedTasks, context);
                SendChangeNotifications(wfUpdatedTasks, context.OriginalTasks, containerId);

                // Fetch the updated tasks from the database.
                var actualUpdatedTaskIds = new HashSet<Guid?>(wfUpdatedTasks.Select(x => x.Id));
                var tempProject = _taskProvider.FetchByProject(project.Id.GetValueOrDefault());
                tasks = tempProject.Tasks.Where(task => actualUpdatedTaskIds.Contains(task.Id));
                LoadTaskChildData(tasks, project);

                deletedTasks = context.GetDeletedEntities();

                scope.Complete();
            }

            return new TaskChangeResult()
            {
                UpdatedTasks = tasks.ToList(),
                DeletedTasks = deletedTasks
            };
        }


        /// <summary>
        /// Deletes the bulk.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="deletedTaskIds">The deleted task ids.</param>
        /// <returns></returns>
        public TaskChangeResult DeleteBulk(Guid containerId, List<Guid> deletedTaskIds)
        {
            IEnumerable<Task> tasks = null;
            List<Task> allDeletedTasks = null;

            using (var scope = _transactionFactory.Create())
            {
                Project project = GetEditableProject(containerId);
                var deletedTasksIdHash = new HashSet<Guid>(deletedTaskIds);
                var deletedTasks = project.Tasks.Where(x => deletedTasksIdHash.Contains(x.Id.Value)).ToList();

                if (deletedTasks != null && deletedTasks.Count > 0)
                {
                    deletedTasks = Common.Framework.Guard.Clone(deletedTasks);
                    var context = _workflowDataContextFactory.Create(project, deletedTasks, true);

                    deletedTasks.ForEach(task =>
                    {
                        var errorList = _taskDeleteValidationManager.Validate(context.OriginalProject, task);
                        if (null != errorList && errorList.Any())
                        {
                            throw new InvalidOperationException("Task was not valid");
                        }
                    });

                    var workflowManager = this._workflowManagerFactory.Create<Project, Task>(Workflow.TaskDeletion);
                    workflowManager.Process(context);
                    var tasksToUpdate = context.GetUpdatedEntities();
                    var taskChangeResult = this.UpdateBulk(containerId, tasksToUpdate);
                    var wfUpdatedTasks = taskChangeResult.UpdatedTasks;

                    var taskToDelete = context.GetDeletedEntities();

                    taskToDelete = taskChangeResult.DeletedTasks
                        .Where(x => !taskToDelete.Exists(y => y.Id == x.Id))
                        .Union(taskToDelete).ToList();

                    if (taskToDelete != null && taskToDelete.Count > 0)
                    {
                        DeleteTasks(containerId, taskToDelete);

                        taskToDelete.ForEach(x =>
                        {
                            _notificationManager.DeleteNotificationsForEntity(x.Id.GetValueOrDefault());
                        });
                    }

	                var updatedProject = context.GetUpdatedParent();

					if (taskToDelete.Exists(x => x.Id == updatedProject.MinimumDueDateTaskId))
					{
						context.IsActiveProjectDirty = true;
                        var reducedTasksList = context.WorkingTasks.Values.Where(x => !x.IsDeleted);
                        var minTaskDueDate = reducedTasksList.Min(x => x.DueDate);
                        var minDueDateTask = reducedTasksList.FirstOrDefault(x => x.DueDate == minTaskDueDate);
						UpdateProjectTaskMinimumDueDate(updatedProject, reducedTasksList.ToList(), minDueDateTask);
                    }

					if (context.IsActiveProjectDirty)
					{
						_projectCoreManager.Update(updatedProject.Id.GetValueOrDefault(), updatedProject, true);
					}

                    // Fetch the updated tasks from the database.
                    var actualUpdatedTaskIds = new HashSet<Guid?>(wfUpdatedTasks.Select(x => x.Id).ToList());
                    var tempProject = _taskProvider.FetchByProject(project.Id.GetValueOrDefault());
                    tasks = tempProject.Tasks.Where(task => actualUpdatedTaskIds.Contains(task.Id));
                    LoadTaskChildData(tasks, project);
                    allDeletedTasks = taskToDelete;

                    scope.Complete();
                }
            }

            return new TaskChangeResult()
            {
                UpdatedTasks = tasks.ToList(),
                DeletedTasks = allDeletedTasks
            };

        }

        /// <summary>
        ///     Deletes the specified task in the specified container.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="taskId">The task id.</param>
        public void Delete(Guid containerId, Guid taskId)
        {
            var taskIds = new List<Guid>() { taskId };
            this.DeleteBulk(containerId, taskIds);
        }

        internal void CheckAndCreateTaskNotifications(IEnumerable<Task> updatedTasks, IDataRuleContext<Project, Task> context)
        {
            var originalTasks = context.OriginalTasks;
            var workingTasks = context.WorkingTasks.Values;        
            var updatedIds = new HashSet<Guid>(updatedTasks.Select(x=> x.Id.Value));
            var updatedWorkingTasks = workingTasks.Where(x=> updatedIds.Contains(x.Id.Value));
            
            updatedWorkingTasks.ForEach(task =>
            {   
               //run normal notification check process 
               var notificationsToApply = _taskNotificationCheckManager.GetTaskNotifications(originalTasks.ContainsKey(task.Id) ? originalTasks[task.Id] : null, 
                   task, new NotificationContext { });

                //Need to figure out how to incorporate this as a notification clean up strategy
               if (notificationsToApply.Any(x => x == NotificationTypeDto.TaskSuccessorStatusSet))
               {
                    //special case to delete existing start date/status notification
                    //delete any TaskSuccessorStatusSet notifications for any of the updated entities
                   _notificationManager.FetchNotificationsByEntity(task.Id.Value)
                       .Where(x => x.NotificationType == NotificationTypeDto.TaskSuccessorStatusSet)
                       .ForEach(x =>
                       {
                           _notificationManager.Delete(x.Id.Value);
                       });
               }

                //process notifications determined from the check process for this task.
               _notificationManager.ProcessNotifications(notificationsToApply, task);
                 
            });
        }

        private void SendChangeNotifications(IEnumerable<Task> updatedTasks, Dictionary<Guid?, Task> originalTasks, Guid containerId)
        {
            updatedTasks.ForEach(task =>
            {
                //Defect - Task completed email should have last  posted comment
                Task originalTask = null;
                if (string.IsNullOrEmpty(task.Comment))
                {
                    originalTask = (originalTasks.ContainsKey(task.Id)) ? originalTasks[task.Id] : null;
                    if (originalTask != null && originalTask.Comments.Any())
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        task.Comment = originalTask.Comments.OrderByDescending(x => x.CreatedDate).FirstOrDefault().Comment;
                    }
                }

                ChangeNotifications(containerId, task, originalTask);
            });
        }

        private void LoadTaskChildData(IEnumerable<Task> tasks, Project project)
        {
            //load docs
            var projDocuments = _assetProvider.FetchAllDocuments(project.ContainerId.GetValueOrDefault())
                .Results.OrderBy(x => x.Id);

	        var taskList = tasks as IList<Task> ?? tasks.ToList();
			var taskIds = taskList.Select(x => x.Id.GetValueOrDefault());

            var ids = _assetProvider.FetchMultipleParentAssetLinks(taskIds)
                .GroupBy(x => x.ParentId)
                .ToDictionary(x => x.Key, y => y.OrderBy(z => z.AssetId));

			var taskTypeIds = taskList.Select(x => x.TaskTypeId.GetValueOrDefault()).Distinct();

			var taskTypeBehaviours = _taskTypeBehaviorProvider.FetchByMultipleTaskTypeIds(taskTypeIds);

			taskList.ForEach(x =>
            {
                // add documents to each task
                var key = x.Id.GetValueOrDefault();
                if (ids.ContainsKey(key))
                {
                    var docRefList = ids[key];
                    x.Documents = (from doc in projDocuments
                                   join link in docRefList
                                        on doc.Id equals link.AssetId
                                   select doc).ToList();
                }

                // Get status list
                x.StatusList = this.FetchStatusListByTaskData(x.ContainerId.GetValueOrDefault(), x.Id.GetValueOrDefault(), x);


				if (taskTypeBehaviours.ContainsKey(x.TaskTypeId.GetValueOrDefault()))
					x.TaskTypeBehaviors = taskTypeBehaviours[x.TaskTypeId.GetValueOrDefault()].ToList();

				

            });
        }


        /// <summary>
        /// Validates the bulk.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="updatedTasks">The updated tasks.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Tasks with associated projects that are Completed or Canceled cannot be updated.</exception>
        public Dictionary<Guid, IList<TaskValidationEnumDto>> ValidateTasks(Guid containerId, IList<Task> updatedTasks)
        {
            Project project = GetEditableProject(containerId); ;
            var context = _workflowDataContextFactory.Create(project, updatedTasks.ToList());

            // Perform validations.
            var errors = ValidateTasks(context.UserAlteredTaskIds, context.WorkingTasks, context.OriginalTasks);
            context.SyncErrors.ForEach(x =>
            {
                if (errors.ContainsKey(x.Key))
                {
                    x.Value.ForEach(y =>
                    {
                        errors[x.Key].Add(y);

                    });
                }
                else
                {
                    errors[x.Key] = x.Value;
                }
            });

            return errors;
        }

        private Project GetEditableProject(Guid containerId)
        {
            var container = _containerManager.FindById(containerId);
            var project = _taskProvider.FetchByProject(container.PrimarySearchEntityId);
            if ((project.ProjectStatus == ProjectStatusEnumDto.Completed ||
                 project.ProjectStatus == ProjectStatusEnumDto.Canceled))
            {
                throw new InvalidOperationException(
                    "Tasks with associated projects that are Completed or Canceled cannot be updated.");
            }

            return project;
        }


        /// <summary>
        ///     Fetches all tasks in a container.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <returns>IList{Task}.</returns>
        public IList<Task> FetchAll(Guid containerId)
        {
            return _taskProvider.FetchAll(containerId);
        }

        /// <summary>
        ///     Fetches all tasks in a container keeping flat, tasks next to subtasks.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <returns>IList{Task}.</returns>
        public IList<Task> FetchAllFlatList(Guid containerId)
        {
            return _taskProvider.FetchAllFlatList(containerId);
        }

        /// <summary>
        /// Fetches all with deleted, including all child tasks to N level deep.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <returns></returns>
        public IList<Task> FetchAllWithDeleted(Guid containerId)
        {
            return _taskProvider.FetchAllWithDeleted(containerId);
        }

        /// <summary>
        ///     Fetches the task in the specified container by id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="taskId">The task id.</param>
        /// <returns>UL.Aria.Service.Domain.Entity.Task.</returns>
        public Task FetchById(Guid containerId, Guid taskId)
        {
            return _taskProvider.FetchById(containerId, taskId);
        }

        /// <summary>
        /// Validates the specified container identifier.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        public IList<TaskValidationEnumDto> Validate(Guid containerId, Task task)
        {
            task.ContainerId = containerId;
            var container = _containerManager.FindById(containerId);
            var project = _taskProvider.FetchByProject(container.PrimarySearchEntityId);
            Task originalTask = project.Tasks.FirstOrDefault(x => x.Id == task.Id);
            if (null != originalTask)
            {
                originalTask.Notifications = _taskNotificationProvider.FetchByTaskId(originalTask.Id.Value).ToList() ;
            }
           
            return _taskValidationManager.Validate(new TaskValidationContext { Entity = task, OriginalEntity = originalTask, Project = project });
        }


        private Dictionary<Guid, IList<TaskValidationEnumDto>> ValidateTasks(IEnumerable<Guid> UpdateTaskIds, IDictionary<Guid?, Task> workingTaskDict,
            IDictionary<Guid?, Task> originalTasks)
        {
            var validationErrors = new Dictionary<Guid, IList<TaskValidationEnumDto>>();
            UpdateTaskIds.ForEach(taskId =>
            {
                var taskToValidate = workingTaskDict[taskId];
                Task originalTask = null;
                originalTasks.TryGetValue(taskId, out originalTask);
                var taskValidationErrors = _taskValidationManager.Validate(new TaskValidationContext
                {
                    Entity = taskToValidate,
                    OriginalEntity = originalTask,
                    Project = taskToValidate.Project
                });

                if (taskValidationErrors != null && taskValidationErrors.Any())
                {
                    validationErrors.Add(taskToValidate.Id.GetValueOrDefault(), taskValidationErrors);
                }
            });

            return validationErrors;
        }


        /// <summary>
        ///     Creates the task in specified container.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="task">The task.</param>
        /// <returns>System.Guid.</returns>
        public Guid Create(Guid containerId, Task task)
        {
            if (task.Id.GetValueOrDefault() == Guid.Empty)
                task.Id = Guid.NewGuid();

            var tasks = new List<Task>() { task };
            BulkCreate(containerId, tasks);
            return task.Id.GetValueOrDefault();
        }

        internal void UpdateProjectTaskMinimumDueDate(Project project, List<Task> workingTasks, Task task)
        {
            if (task == null)
                return;
            if (task.Status == TaskStatusEnumDto.Canceled || task.Status == TaskStatusEnumDto.Completed)
            {
                if (project.MinimumDueDateTaskId.HasValue
                    && task.Id == project.MinimumDueDateTaskId.GetValueOrDefault())
                {
					var taskFound = workingTasks.Where(x => x.Status != TaskStatusEnumDto.Canceled
                        && x.Status != TaskStatusEnumDto.Completed && x.DueDate.HasValue
                        && x.Id != task.Id).OrderBy(x => x.DueDate).FirstOrDefault();

                    if (taskFound == null)
                    {
                        project.TaskMinimumDueDate = null;
                        project.MinimumDueDateTaskId = null;
                    }
                    else
                    {
                        project.TaskMinimumDueDate = taskFound.DueDate;
                        project.MinimumDueDateTaskId = taskFound.Id;
                    }
                }
            }
            else
            {
                if (task.DueDate.HasValue &&
                    (!project.TaskMinimumDueDate.HasValue ||
                     task.DueDate.GetValueOrDefault() < project.TaskMinimumDueDate.GetValueOrDefault()))
                {
                    project.TaskMinimumDueDate = task.DueDate;
                    project.MinimumDueDateTaskId = task.Id;
                }
            }
        }

        /// <summary>
        ///     Updates the specified task in the specified container.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="task">The task.</param>
        public void Update(Guid containerId, Task task)
        {

            try
            {
                task.ContainerId = containerId;
                var tasks = new List<Task>() { task };
                //TODO: returns all tasks that have been altered.  May want to change signature to return all as well.
                UpdateBulk(containerId, tasks);
            }
            catch (ArgumentException)
            {
                // if excption is thrown from bulk, this is the orginal logic needed for original callers to this method.

                var container = _containerManager.FindById(containerId);
                var project = _taskProvider.FetchByProject(container.PrimarySearchEntityId);
                var originalTask = project.Tasks.FirstOrDefault(x => x.Id == task.Id);
                var errorList = _taskValidationManager.Validate(new TaskValidationContext { Entity = task, OriginalEntity = originalTask, Project = project });

                if (null != errorList && errorList.Any())
                {
                    if (errorList.Contains(TaskValidationEnumDto.TaskClosedBecauseProjectClosed))
                    {
                        throw new InvalidOperationException("Tasks with associated projects that are Completed or Canceled cannot be updated.");
                    }

                    if (errorList.Contains(TaskValidationEnumDto.ProjectHandlerCanUpdateStatusWhenClosed))
                    {
                        throw new InvalidOperationException("Tasks is already Completed or Canceled cannot be updated.");
                    }

                    throw;
                }
            }
        }


        /// <summary>
        ///     Creates the tasks in specified container.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="tasks">The tasks.</param>

        public void BulkCreate(Guid containerId, IList<Task> tasks)
        {
            using (var scope = _transactionFactory.Create())
            {
                var project = GetEditableProject(containerId);
                var flatList = Flatten(tasks).ToList();
                var ruleContext = _workflowDataContextFactory.Create(project, flatList);
                var workflowManager = this._workflowManagerFactory.Create<Project, Task>(Workflow.TaskModification);
                workflowManager.Process(ruleContext);
                var tasksToCreate = ruleContext.GetUpdatedEntities(true);
                var hierarchicalList = ruleContext.GetUpdatedEntities(false);

                _taskProvider.BulkCreate(containerId, hierarchicalList);
                _assetProvider.SaveAssets(tasksToCreate);

                scope.Complete();
            }

        }

        private IEnumerable<Task> Flatten(IEnumerable<Task> tasks)
        {
            var taskList = tasks as IList<Task> ?? tasks.ToList();
            return taskList.SelectMany(c => Flatten(c.SubTasks)).Concat(taskList);
        }

        /// <summary>
        ///     Fetches the history.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="taskId">The task id.</param>
        /// <returns>System.Collections.Generic.IList{UL.Aria.Service.Domain.Entity.TaskHistory}.</returns>
        public IList<TaskHistory> FetchHistory(Guid containerId, Guid taskId)
        {
            return _taskProvider.FetchHistory(containerId, taskId);
        }

        /// <summary>
        /// Fetches the delta history.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        public IList<TaskDelta> FetchDeltaHistory(Guid containerId, Guid taskId)
        {
            return _taskProvider.FetchDeltaHistory(containerId, taskId);
        }

        /// <summary>
        /// Fetches the count.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <returns></returns>
        public int FetchCount(Guid containerId)
        {
            return _taskProvider.FetchCount(containerId);
        }

        /// <summary>
        /// Fetches the status and status list by task data.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="task">The task.</param>
        /// <returns>TaskStatusList.</returns>
        public TaskStatusList FetchStatusListByTaskData(Guid containerId, Guid id, Task task)
        {
            var taskStatusList = new TaskStatusList
            {
                Status = _taskFetchStatusStrategyFactory.GetStrategy(task.Status).FetchTaskStatus(task)
            };
            taskStatusList.StatusList = _taskFetchStatusListStrategyFactory.GetStrategy(taskStatusList.Status).FetchTaskStatusList();
            return taskStatusList;
        }

        /// <summary>
        ///     Downloads the tasks for a given container.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        public Stream DownloadByContainer(SearchCriteria searchCriteria)
        {
            return Download(searchCriteria);
        }

        /// <summary>
        ///     Downloads the tasks for the specified container id.
        /// </summary>
        /// <param name="searchCriteria">The container id.</param>
        /// <returns></returns>
        public Stream Download(SearchCriteria searchCriteria)
        {
            var searchResult = _searchProvider.Search(searchCriteria);
            var tasks = searchResult.Results.Where(x => x.EntityType == EntityTypeEnumDto.Task).ToList();
            var foundProjects = new Dictionary<Guid, ProjectDetail>();
            var projectIds = new List<Guid>();
            var containerIds = new List<Guid>();

            foreach (var result in tasks)
            {
                var projectId = new Guid(result.Metadata[AssetFieldNames.AriaProjectId]);
                ProjectDetail projectDetail;
                if (foundProjects.TryGetValue(projectId, out projectDetail)) continue;
                foundProjects.Add(projectId, null);
                projectIds.Add(projectId);
                var containerId = new Guid(result.Metadata[AssetFieldNames.AriaContainerId]);
                containerIds.Add(containerId);
            }

            var projects = _projectProvider.FetchProjects(projectIds);
            var containersTasks = _taskProvider.FetchMultipleContainerTasks(containerIds);
            var flattenedTasks = new List<Task>();
            foreach (var containerTask in containersTasks)
                FlattenTasks(flattenedTasks, containerTask.Tasks.ToList());

            foreach (var project in projects)
                foundProjects[project.Id.GetValueOrDefault()] = new ProjectDetail { Project = project };

            foreach (var result in tasks)
            {
                var projectId = new Guid(result.Metadata[AssetFieldNames.AriaProjectId]);
                var taskId = new Guid(result.Metadata[AssetFieldNames.AriaTaskId]);

                var task = flattenedTasks.FirstOrDefault(x => x.Id.GetValueOrDefault() == taskId);
                //skip tasks that cant be found, usually occurs if they are deleted and SP index is stale
                if (task == null)
                    continue;

                var projectDetail = foundProjects[projectId];
                projectDetail.Tasks.Add(task);
                if (task.SubTasks != null && task.SubTasks.Any())
                {
                    projectDetail.ParentTasks.Add(task.Id.GetValueOrDefault(), task);
                }
            }

            var projectProductsAssetLinks = _assetProvider.FetchMultipleParentAssetLinks(projectIds).ToList();

            foreach (var taskProjectMapping in foundProjects.Values)
            {
                var products = (from projectProductsAssetLink in projectProductsAssetLinks
                                where projectProductsAssetLink.ParentId == taskProjectMapping.Project.Id
                                select projectProductsAssetLink.AssetId).ToList();
                taskProjectMapping.ProductIds.AddRange(products);

                foreach (var task in taskProjectMapping.Tasks)
                {
                    if (task.ParentId == default(Guid)) continue;
                    if (taskProjectMapping.ParentTasks.ContainsKey(task.ParentId)) continue;
                    var parent = flattenedTasks.FirstOrDefault(x => x.Id.GetValueOrDefault() == task.ParentId);
                    //skip tasks that cant be found, usually occurs if they are deleted and SP index is stale
                    if (parent == null)
                        continue;
                    taskProjectMapping.ParentTasks.Add(parent.Id.GetValueOrDefault(), parent);
                }
            }

            return _taskDocumentBuilder.Build(foundProjects.Values);
        }




        private List<Task> FilterTasks(SearchCriteria searchCriteria, out Dictionary<string, List<RefinementItemDto>> refinerResults, IEnumerable<Task> taskResult)
        {
            var projectShouldBillingTriggerCount = taskResult.Count(x => x.ShouldTriggerBilling && x.Status != TaskStatusEnumDto.Completed && x.Status != TaskStatusEnumDto.Canceled);
            //
            // filter in memory
            //
            if (!string.IsNullOrEmpty(searchCriteria.Keyword))
            {
                taskResult = taskResult.Where(x =>
                {
                    var keyword = searchCriteria.Keyword.ToLower();
                    return
                        (x.Title != null && x.Title.ToLower().Contains(keyword))
                        || x.TaskNumber.ToString(CultureInfo.InvariantCulture).ToLower().Contains(keyword)
                        || (x.TaskOwner != null && x.TaskOwner.ToLower().Contains(keyword))
                        || (x.Body != null && x.Body.ToLower().Contains(keyword))
                        ||
                        x.Status
                            .GetDisplayName()
                            .ToString(CultureInfo.InvariantCulture)
                            .ToLower()
                            .Contains(keyword)
                        ||
                        x.Progress
                            .GetDisplayName()
                            .ToString(CultureInfo.InvariantCulture)
                            .ToLower()
                            .Contains(keyword)
                        || (x.Category != null && x.Category.ToLower().Contains(keyword))
                        || (x.Group != null && x.Group.ToLower().Contains(keyword))
                        || (x.StartDate != null && x.StartDate.ToString().ToLower().Contains(keyword))
                        || (x.DueDate != null && x.StartDate.ToString().ToLower().Contains(keyword))
                        || x.PercentComplete.ToString(CultureInfo.InvariantCulture).ToLower().Contains(keyword)
                        || (x.ActualDuration != null && x.ActualDuration.ToString().ToLower().Contains(keyword))
                        || (x.EstimatedDuration != null && x.EstimatedDuration.ToString().ToLower().Contains(keyword))
                        || (x.Description != null && x.Description.ToString().ToLower().Contains(keyword))
                        || (x.ClientBarrierHours != null && x.ClientBarrierHours.ToString().ToLower().Contains(keyword))
                        ||
                        (x.EstimatedStartDayNumber != null &&
                         x.EstimatedStartDayNumber.ToString().ToLower().Contains(keyword))
                        ||
                        (x.Comments != null &&
                         x.Comments.Any(y => y.Comment != null && y.Comment.ToLower().Contains(keyword)));
                });
            }

            List<string> filter;
            if (searchCriteria.Filters.TryGetValue(AssetFieldNames.AriaTaskPhase, out filter))
            {
                var status = filter;
                taskResult = taskResult.Where(x => status.Any(y => y == x.StatusSearchValue));
            }

            if (searchCriteria.Filters.TryGetValue(AssetFieldNames.AriaTaskProgress, out filter))
            {
                var progress = filter;
                taskResult = taskResult.Where(x => progress.Any(y => y == x.ProgressSearchValue));
            }

            if (searchCriteria.Filters.TryGetValue(AssetFieldNames.AriaTaskOwner, out filter))
            {
                var owner = filter;

                taskResult = taskResult.Where(x => owner.Any(y =>
                    y == x.TaskOwner
                    || (y == "Assigned" &&
                        !string.IsNullOrWhiteSpace(x.TaskOwner)
                        && x.TaskOwner.ToLower() != "unassigned"
                        && x.Status != TaskStatusEnumDto.Canceled
                        && x.Status != TaskStatusEnumDto.Completed
                        )));
            }
            if (searchCriteria.Filters.TryGetValue(AssetFieldNames.AriaTaskDueDate, out filter))
            {
                if (filter.Contains("OverDue"))
                {
                    var dateTime = DateTime.Today;

                    taskResult = taskResult.Where(x =>
                        x.DueDate.HasValue
                        && x.DueDate != default(DateTime)
                        && x.DueDate < dateTime
                        && x.Status != TaskStatusEnumDto.Canceled
                        && x.Status != TaskStatusEnumDto.Completed
                        );
                }
            }


            //
            // evaluate all LINQ queries and turn to list
            //
            var results = taskResult.ToList();
            refinerResults = new Dictionary<string, List<RefinementItemDto>>();

            results.Where(x => x.ShouldTriggerBilling).ForEach(x => x.ProjectTaskShouldTriggerBillingCount = projectShouldBillingTriggerCount);
            // 
            // build refiners
            //
            foreach (var refiner in searchCriteria.Refiners)
            {
                if (AssetFieldNames.AriaTaskPhase == refiner)
                {
                    refinerResults.Add(refiner,
                        results.GroupBy(x => x.Status)
                            .Select(
                                x =>
                                    new RefinementItemDto
                                    {
                                        Name = x.Key.ToSharePointString(),
                                        Count = x.Count(),
                                        Value = x.Key.ToSharePointString()
                                    })
                            .ToList());
                }
                else if (AssetFieldNames.AriaTaskProgress == refiner)
                {
                    refinerResults.Add(refiner,
                        results.GroupBy(x => x.Progress)
                            .Select(
                                x =>
                                    new RefinementItemDto
                                    {
                                        Name = x.Key.ToSharePointString(),
                                        Count = x.Count(),
                                        Value = x.Key.ToSharePointString()
                                    })
                            .ToList());
                }
                else if (AssetFieldNames.AriaTaskOwner == refiner)
                {
                    refinerResults.Add(refiner,
                        results.GroupBy(x => x.TaskOwner)
                            .Select(
                                x => new RefinementItemDto { Name = x.Key + "", Count = x.Count(), Value = x.Key + "" })
                            .ToList());
                }
            }

            return results;
        }



        /// <summary>
        ///     Searches the tasks for the project specified in the search criteria.
        /// </summary>
        /// <param name="searchCriteria">
        ///     The search criteria.  This must have the container id specified in the AriaContainerID
        ///     filter.
        /// </param>
        /// <param name="refinerResults">The refiner results1.</param>
        /// <returns>IList{Task}.</returns>
        public IList<Task> Search(SearchCriteria searchCriteria,
            out Dictionary<string, List<RefinementItemDto>> refinerResults)
        {
            var containerId = searchCriteria.Filters[AssetFieldNames.AriaContainerId].FirstOrDefault();
            Guard.IsNotNullOrEmpty(containerId, "containerId");

            //
            // start with everything
            //
            var taskResultOrig = FetchAll(containerId.ToGuid()); //.AsEnumerable();
            var taskResultFlatten = new List<Task>();
            FlattenTasks(taskResultFlatten, taskResultOrig);
            var taskResult = taskResultFlatten.AsEnumerable();

            var results = FilterTasks(searchCriteria, out refinerResults, taskResult);

            var projDocuments = _assetProvider.FetchAllDocuments(containerId.ToGuid())
                .Results.OrderBy(x => x.Id);

            var TaskIds = results.Select(x => x.Id.GetValueOrDefault());

            var ids = _assetProvider.FetchMultipleParentAssetLinks(TaskIds)
                .GroupBy(x => x.ParentId)
                .ToDictionary(x => x.Key, y => y.OrderBy(z => z.AssetId));

            var container = _containerManager.FindById(Guid.Parse(containerId));
            var predecessors = _taskProvider.FetchPredecessorsByProject(container.PrimarySearchEntityId);

            // add documents to each task
            results.ForEach(x =>
            {
                var key = x.Id.GetValueOrDefault();
                if (ids.ContainsKey(key))
                {
                    var docRefList = ids[key];
                    x.Documents = (from doc in projDocuments
                                   join link in docRefList
                                        on doc.Id equals link.AssetId
                                   select doc).ToList();
                }

                x.Predecessors = predecessors.Where(y => y.SuccessorId == key).ToList();
            });

            if (searchCriteria.Sorts.Count == 0)
            {
                ReHydrateFromFlatten(taskResultOrig, results);
            }
            else if (searchCriteria.Sorts[0].FieldName == AssetFieldNames.AriaTaskNumber)
            {
                ReHydrateFromFlatten(taskResultOrig, results);
                taskResultOrig = SortReHydrated(taskResultOrig, searchCriteria.Sorts[0].Order);
            }
            else
            {
                taskResultOrig = SortFlat(results, searchCriteria);
            }

            return taskResultOrig;
        }

        /// <summary>
        /// Fetches the project tasks.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="refinerResults">The refiner results.</param>
        /// <returns></returns>
        public List<Task> FetchProjectTasks(Guid projectId, SearchCriteria searchCriteria, out  Dictionary<string, List<RefinementItemDto>> refinerResults)
        {
            var project = _taskProvider.FetchByProject(projectId);
            var taskResults = FilterTasks(searchCriteria, out refinerResults, project.Tasks);
            var projDocuments = _assetProvider.FetchAllDocuments(project.ContainerId.GetValueOrDefault())
                .Results.OrderBy(x => x.Id);

            var taskIds = taskResults.Select(x => x.Id.GetValueOrDefault());

            var ids = _assetProvider.FetchMultipleParentAssetLinks(taskIds)
                .GroupBy(x => x.ParentId)
                .ToDictionary(x => x.Key, y => y.OrderBy(z => z.AssetId));

			var taskTypeIds = taskResults.Select(x => x.TaskTypeId.GetValueOrDefault()).Distinct();

			var taskTypeBehaviours = _taskTypeBehaviorProvider.FetchByMultipleTaskTypeIds(taskTypeIds);

            taskResults.ForEach(x =>
            {
                // add documents to each task
                var key = x.Id.GetValueOrDefault();
                if (ids.ContainsKey(key))
                {
                    var docRefList = ids[key];
                    x.Documents = (from doc in projDocuments
                                   join link in docRefList
                                        on doc.Id equals link.AssetId
                                   select doc).ToList();

                }

                // Get Status List
                x.StatusList = this.FetchStatusListByTaskData(x.ContainerId.GetValueOrDefault(), x.Id.GetValueOrDefault(), x);

	            if (taskTypeBehaviours.ContainsKey(x.TaskTypeId.GetValueOrDefault()))
		            x.TaskTypeBehaviors = taskTypeBehaviours[x.TaskTypeId.GetValueOrDefault()].ToList();
            });

            return project.Tasks;
        }

        /// <summary>
        /// Creates the document from the document Template and links document to the task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="documentTemplateId">The document template identifier.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>Guid.</returns>
        public Guid CreateAndLinkDocument(Guid taskId, Guid containerId, Guid documentTemplateId, IDictionary<string, string> metaData)
        {
            Guid documentId;

            using (var transactionScope = _transactionFactory.Create())
            {
                var documentTemplate = _documentTemplateManager.Fetch(documentTemplateId);

                if (metaData.ContainsKey(AssetFieldNames.AriaContentType))
                {
                    metaData[AssetFieldNames.AriaContentType] = documentTemplate.ContentType;
                }
                else
                {
                    metaData.Add(AssetFieldNames.AriaContentType, documentTemplate.ContentType);
                }

                if (metaData.ContainsKey(AssetFieldNames.AriaName))
                {
                    metaData[AssetFieldNames.AriaName] = documentTemplate.FileName;
                }
                else
                {
                    metaData.Add(AssetFieldNames.AriaName, documentTemplate.FileName);
                }

                string lastDoc = metaData.ContainsKey(AssetFieldNames.AriaTitle) ? metaData[AssetFieldNames.AriaTitle] : documentTemplate.FileName;

                documentId = _documentManagementManager.CreateAndLink(documentTemplate.DocumentId, containerId, taskId, metaData);

                var task = FetchById(containerId, taskId);
                task.LastDocumentAdded = lastDoc;
                Update(containerId, task);

                transactionScope.Complete();
            }

            return documentId;
        }

		/// <summary>
		/// Validates the on delete tasks.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="taskGuidIds">The task unique identifier ids.</param>
		/// <returns></returns>
		public Dictionary<Guid, IList<TaskDeleteValidationDto>> ValidateOnDeleteTasks(Guid containerId,
			   List<Guid> taskGuidIds)
		{

			var result = new Dictionary<Guid, IList<TaskDeleteValidationDto>>();

			Project project = GetEditableProject(containerId);
			var deletedTasksIdHash = new HashSet<Guid>(taskGuidIds);
			var deletedTasks = project.Tasks.Where(x => deletedTasksIdHash.Contains(x.Id.Value)).ToList();
			if (deletedTasks.Count > 0)
			{
				deletedTasks.ForEach(task =>
				{
					var errorList = _taskDeleteValidationManager.Validate(project, task);
					if (errorList.Any())
					{
						var errorResult = new List<TaskDeleteValidationDto>();
						errorList.ForEach(
							x => errorResult.Add(new TaskDeleteValidationDto() { TaskName = task.Title, TaskDeleteValidationEnum = x }));
						result.Add(task.Id.Value, errorResult);
					}
				});
			}
			return result;
		}

        internal void DeleteTasks(Guid containerId, IList<Task> tasks)
        {
            foreach (var task in tasks)
            {
                if (!task.ContainerId.HasValue)
                    task.ContainerId = containerId;
                _taskProvider.Delete(containerId, task.Id.GetValueOrDefault());
                try
                {
                    _assetProvider.Delete(task.Id.GetValueOrDefault());
                }
                catch (EndpointNotFoundException ex)
                {
                    // sometimes happens if there were previous errors, ensure we can keep mving if the search asset is not found.
                    _logManager.Log(ex.ToLogMessage(MessageIds.TaskManagerDeleteTaskAssetNotFoundException, LogCategory.Project, LogPriority.Low, TraceEventType.Error));
                }
                DeleteNotification(task);
            }
        }

        internal void DeleteNotification(Task task)
        {
            if (task == null || string.IsNullOrEmpty(task.TaskOwner))
                return;

            var taskEmail = BuildTaskEmail(task, task.TaskOwner);

            if (taskEmail.RecipientName != null)
                LogExceptionAndContinue(() => _smtpClientManager.SendTaskDelete(task.TaskOwner, taskEmail));
        }

        internal TaskEmail BuildTaskEmail(Task task, string recipientLoginId)
        {
            var actorProfile = _profileManager.FetchById(_principalResolver.UserId);
            var recipientProfile = _profileManager.FetchByUserName(recipientLoginId);
            var container = _containerManager.FindById(task.ContainerId.GetValueOrDefault());
            var project = _projectProvider.FetchById(container.PrimarySearchEntityId);
            return new TaskEmail
            {
                ProjectId = project.Id.GetValueOrDefault(),
                TaskId = task.Id.GetValueOrDefault(),
                RecipientName = recipientProfile == null ? null : recipientProfile.DisplayName,
                ActorName = actorProfile.DisplayName,
                ActorLoginId = actorProfile.LoginId,
                ProjectName = project.Name,
                TaskName = task.Title,
                EndDate = task.DueDate
            };
        }

        internal void LogExceptionAndContinue(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                var logMessage = ex.ToLogMessage(MessageIds.TaskManagerException, LogCategory.Project, LogPriority.High,
                    TraceEventType.Error);
                _logManager.Log(logMessage);
            }
        }

        internal void ChangeNotifications(Guid containerId, Task task, Task originalTask)
        {
            SendTaskOwnerChangeEmail(task, originalTask);
            SendTaskCompletedEmail(containerId, task, originalTask);
        }

        private void SendTaskOwnerChangeEmail(Task task, Task originalTask)
        {
            if ((originalTask == null || task.TaskOwner != originalTask.TaskOwner) &&
                !string.IsNullOrEmpty(task.TaskOwner))
            {
                //
                // Notify new task owner
                //
                var taskEmail = BuildTaskEmail(task, task.TaskOwner);

                if (taskEmail.RecipientName != null)
                    LogExceptionAndContinue(() => _smtpClientManager.SendTaskAssigned(task.TaskOwner, taskEmail));
            }

            if (originalTask == null || string.IsNullOrEmpty(originalTask.TaskOwner) ||
                task.TaskOwner == originalTask.TaskOwner)
                return;

            //
            // Notify old task owner
            //
            var taskEmailReAssign = BuildTaskEmail(task, originalTask.TaskOwner);
            if (taskEmailReAssign.RecipientName != null)
                LogExceptionAndContinue(
                    () => _smtpClientManager.SendTaskReassigned(originalTask.TaskOwner, taskEmailReAssign));
        }

        private void SendTaskCompletedEmail(Guid containerId, Task task, Task originalTask)
        {
            if (originalTask != null && originalTask.Status == TaskStatusEnumDto.Completed)
                return;

            if (task.Status == TaskStatusEnumDto.Completed)
            {
                var container = _containerManager.FindById(containerId);
                var project = _projectProvider.FetchById(container.PrimarySearchEntityId);

                var taskCompletedEmail = new TaskCompletedEmail
                {
                    DueDate = task.DueDate,
                    ProjectId = project.Id.GetValueOrDefault(),
                    ProjectName = project.Name,
                    StartDate = task.StartDate,
                    TaskId = task.Id.GetValueOrDefault(),
                    TaskName = task.Title,
                    TaskPhase = task.StatusLabel,
                    TaskProgress = task.ProgressLabel,
                    LastPostedComment = task.Comment
                };
                LogExceptionAndContinue(() => _smtpClientManager.SendTaskCompleted(project.ProjectHandler, taskCompletedEmail));

                // Notify users associated with the freeform task.
                if (task.TaskTypeId == _serviceConfiguration.FreeformTaskTypeId)
                {
                    task.Notifications.ForEach(notification =>
                    {
                        LogExceptionAndContinue(() => _smtpClientManager.SendTaskCompleted(notification.Email, taskCompletedEmail));
                    });
                }
                // Notify users associated with the (non-freeform) task type.
                else
                {
                    var taskTypeNotifications =
                        _taskTypeNotificationProvider.FetchByTaskTypeId(task.TaskTypeId.GetValueOrDefault());
                    taskTypeNotifications.ForEach(notification =>
                    {
                        LogExceptionAndContinue(() => _smtpClientManager.SendTaskCompleted(notification.Email, taskCompletedEmail));
                    });
                }
            }
        }


        internal static IList<Task> SortReHydrated(IList<Task> taskList, SortDirection order)
        {
            foreach (var task in taskList)
                task.SubTasks = SortReHydrated(task.SubTasks, order);

            return
                (order == SortDirection.Ascending
                    ? taskList.OrderBy(x => x.TaskNumber)
                    : taskList.OrderByDescending(x => x.TaskNumber)).ToList();
        }

        internal static IList<Task> SortFlat(IEnumerable<Task> taskResult, SearchCriteria criteria)
        {
            for (var i = criteria.Sorts.Count - 1; i >= 0; i--)
            {
                var sort = criteria.Sorts[i];

                switch (sort.FieldName)
                {
                    case AssetFieldNames.AriaTaskTitle:
                        taskResult = sort.Order == SortDirection.Ascending
                            ? taskResult.OrderBy(x => x.Title)
                            : taskResult.OrderByDescending(x => x.Title);
                        break;
                    case AssetFieldNames.AriaTaskPhase:
                        taskResult = sort.Order == SortDirection.Ascending
                            ? taskResult.OrderBy(x => x.Status.GetDisplayName())
                            : taskResult.OrderByDescending(x => x.Status.GetDisplayName());
                        break;
                    case AssetFieldNames.AriaTaskProgress:
                        taskResult = sort.Order == SortDirection.Ascending
                            ? taskResult.OrderBy(x => x.Progress.GetDisplayName())
                            : taskResult.OrderByDescending(x => x.Progress.GetDisplayName());
                        break;
                    case AssetFieldNames.AriaTaskDueDate:
                        taskResult = sort.Order == SortDirection.Ascending
                            ? taskResult.OrderBy(x => x.DueDate)
                            : taskResult.OrderByDescending(x => x.DueDate);
                        break;
                    case AssetFieldNames.AriaTaskDuration:
                        taskResult = sort.Order == SortDirection.Ascending
                            ? taskResult.OrderBy(x => x.ActualDuration)
                            : taskResult.OrderByDescending(x => x.ActualDuration);
                        break;
                    case AssetFieldNames.AriaTaskOwner:
                        taskResult = sort.Order == SortDirection.Ascending
                            ? taskResult.OrderBy(x => x.TaskOwner)
                            : taskResult.OrderByDescending(x => x.TaskOwner);
                        break;
                    case AssetFieldNames.ariaTaskType:
                        taskResult = sort.Order == SortDirection.Ascending
                            ? taskResult.OrderBy(x => x.TaskTypeName)
                            : taskResult.OrderByDescending(x => x.TaskTypeName);
                        break;
                }
            }

            return taskResult.ToList();
        }

        internal void AddTaskSearchData(Guid containerId, Task task, Task matchedCreatedTask, IList<Task> flattenedTasks, Project project)
        {
            matchedCreatedTask.ContainerId = containerId;
            flattenedTasks.Add(matchedCreatedTask);
            EnrichTaskWithCompanyandOrder(project, matchedCreatedTask);
            if (task.SubTasks != null && task.SubTasks.Any())
            {
                foreach (var subtask in task.SubTasks)
                {
                    var matchedCreatedSubTask =
                        matchedCreatedTask.SubTasks.FirstOrDefault(
                            createdSubTask => subtask.Title == createdSubTask.Title);
                    AddTaskSearchData(containerId, subtask, matchedCreatedSubTask, flattenedTasks, project);
                }
                matchedCreatedTask.SubTasks = null;
            }
        }

        internal void ReHydrateFromFlatten(IList<Task> taskList, IList<Task> taskResult)
        {
            for (var i = taskList.Count - 1; i >= 0; i--)
            {
                var task = taskList[i];
                ReHydrateFromFlatten(task.SubTasks, taskResult);
                var foundTask = taskResult.FirstOrDefault(x => x.Id == task.Id);

                if (foundTask == null && task.SubTasks.Count == 0)
                    taskList.Remove(task);
            }
        }

        internal static void FlattenTasksHelper(IList<Task> taskList, IList<Task> tasksToAdd)
        {
            foreach (var task in tasksToAdd)
            {
                if (string.IsNullOrEmpty(task.TaskOwner))
                    task.TaskOwner = "Unassigned";
                taskList.Add(task);
                FlattenTasksHelper(taskList, task.SubTasks);
            }
        }

        internal void FlattenTasks(IList<Task> taskList, IList<Task> tasksToAdd)
        {
            FlattenTasksHelper(taskList, tasksToAdd);
        }


        internal void EnrichTaskWithCompanyandOrder(Project project, Task task)
        {
            task.OrderNumber = project.OrderNumber;
            task.CompanyId = project.CompanyId;
            task.CompanyName = project.CompanyName;
        }
    }
}