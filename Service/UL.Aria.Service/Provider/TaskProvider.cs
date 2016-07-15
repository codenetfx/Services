using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Microsoft.Practices.ObjectBuilder2;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider.EntityHistoryStrategy;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	///     Class TaskProvider
	/// </summary>
	public sealed class TaskProvider : ITaskProvider
	{
		private readonly ITaskRepository _taskRepository;
		private readonly IContainerRepository _containerRepository;
		private readonly IPrincipalResolver _principalResolver;
		private readonly IHistoryProvider _historyProvider;
		private readonly IMapperRegistry _mapperRegistry;
		private readonly IEntityHistoryStrategyResolver _entityHistoryStrategyResolver;
        private readonly IProjectRepository _projectRepository;
	    private readonly ITaskNotificationProvider _taskNotificationProvider;
        private readonly ITransactionFactory _transactionFactory;
	    private readonly ITaskTypeBehaviorProvider _taskTypeBehaviorProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskProvider" /> class.
        /// </summary>
        /// <param name="taskRepository">The task repository.</param>
        /// <param name="containerRepository">The container repository.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="historyProvider">The history provider.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="entityHistoryStrategyResolver">The entity history strategy resolver.</param>
        /// <param name="projectRepository">The project repository.</param>
        /// <param name="taskNotificationProvider">The task notification provider.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="taskTypeBehaviorProvider">The task type behavior provider.</param>
		public TaskProvider(ITaskRepository taskRepository, IContainerRepository containerRepository,
			IPrincipalResolver principalResolver, IHistoryProvider historyProvider, IMapperRegistry mapperRegistry,
			IEntityHistoryStrategyResolver entityHistoryStrategyResolver, IProjectRepository projectRepository,
            ITaskNotificationProvider taskNotificationProvider, ITransactionFactory transactionFactory, ITaskTypeBehaviorProvider taskTypeBehaviorProvider)
		{
			_taskRepository = taskRepository;
			_containerRepository = containerRepository;
			_principalResolver = principalResolver;
			_historyProvider = historyProvider;
			_mapperRegistry = mapperRegistry;
			_entityHistoryStrategyResolver = entityHistoryStrategyResolver;
            _projectRepository = projectRepository;
            _taskNotificationProvider = taskNotificationProvider;
            _transactionFactory = transactionFactory;
	        _taskTypeBehaviorProvider = taskTypeBehaviorProvider;
		}


		/// <summary>
		/// Fetches the project by id.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>Project.</returns>
		public Project FetchByProject(Guid projectId)
		{
            Project project = _projectRepository.FetchById(projectId);
            var tasks = _taskRepository.FetchByProject(projectId).OrderBy(x=> x.TaskNumber).ToList();
            var predecessors = _taskRepository.FetchPredecessorsByProject(projectId);
		    
            var unquieTaskTypeIds = tasks.Where(x => x.TaskTypeId.HasValue).Select(x => x.TaskTypeId.Value).Distinct().ToList();
		    var taskTypeBehaviorDict = _taskTypeBehaviorProvider.FetchByMultipleTaskTypeIds(unquieTaskTypeIds);
           

            var taskIdIndex = tasks.ToDictionary(x => x.Id);
            var predecessorDict = predecessors.GroupBy(x=> x.SuccessorId).ToDictionary(x => x.Key);
            var orderedList = new List<Task>();

            tasks.ForEach(task =>
            {
                task.ContainerId = project.ContainerId;
                task.Project = project;
                var taskId = task.Id.GetValueOrDefault();
                if (predecessorDict.ContainsKey(taskId))
                    task.Predecessors.AddRange(predecessorDict[taskId]);

				if (task.ParentId != Guid.Empty)
                {
                    var parent = taskIdIndex[task.ParentId];
                    task.Parent = parent;
                    task.ParentTaskNumber = parent.TaskNumber;
                    parent.SubTasks.Add(task);
					parent.ChildTaskNumbers.Add(task.TaskNumber);
                }

               task.Predecessors.ForEach(pTask =>
               {
                   var predecessor = taskIdIndex[pTask.TaskId];
                   predecessor.SuccessorRefs.Add(task);
                   task.PredecessorRefs.Add(predecessor);
               });
			    UpdateTaskProgress(task);
                
                if (task.TaskTypeId.GetValueOrDefault() != Guid.Empty && taskTypeBehaviorDict.ContainsKey(task.TaskTypeId.Value))
                {
                    task.TaskTypeBehaviors.AddRange(taskTypeBehaviorDict[task.TaskTypeId.Value]);
                }

            });

            //force tree parent/child ordering
            orderedList.AddRange(tasks.Where(x => x.Parent == null));
            for(var i = 0; i < orderedList.Count; i++)
            {
                if (orderedList[i].HasChildren)
                    orderedList.InsertRange(i + 1, orderedList[i].SubTasks);
            }

            project.Tasks = orderedList;
            return project;
        }

		/// <summary>
		///     Fetches all.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <returns>IList{Task}.</returns>
		public IList<Task> FetchAll(Guid containerId)
		{
			var container = GetContainer(containerId);
			var tasks = _taskRepository.FindByPrimarySearchEntityId(container.PrimarySearchEntityId);
			if (null == tasks)
			{
				return null;
			}

			GetStatusAndCompletionHistories(containerId, tasks);

			return tasks;
		}

		/// <summary>
		///     Fetches all keeping in flat list format, not tree with SubTasks.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <returns>IList{Task}.</returns>
		public IList<Task> FetchAllFlatList(Guid containerId)
		{
			var container = GetContainer(containerId);
			var tasks = _taskRepository.FindByPrimarySearchEntityId(container.PrimarySearchEntityId, false, true);
			if (null == tasks)
			{
				return null;
			}

			GetStatusAndCompletionHistories(containerId, tasks);

			return tasks;
		}

		internal void GetStatusAndCompletionHistories(Guid containerId, IEnumerable<Task> tasks)
		{
			//TODO:need specialized sproc added to history repo to get history for multiple entities
			foreach (var task in tasks)
			{
				var statusHistories = new List<TaskStatusHistory>();
				var completionHistories = new List<TaskCompletionHistory>();
				var tasksHistories = FetchHistory(containerId, task.Id.GetValueOrDefault());
				foreach (var taskHistory in tasksHistories)
				{
					var taskStatusHistory = new TaskStatusHistory
					{
						CreatedBy = taskHistory.CreatedBy,
						CreatedDate = taskHistory.CreatedDate,
						Status = taskHistory.Task.Status
					};
					statusHistories.Add(taskStatusHistory);
					var taskCompletionHistory = new TaskCompletionHistory
					{
						CreatedBy = taskHistory.CreatedBy,
						CreatedDate = taskHistory.CreatedDate,
						Completion = taskHistory.Task.PercentComplete.ToString(CultureInfo.InvariantCulture)
					};
					completionHistories.Add(taskCompletionHistory);
				}
				task.StatusHistories = RemoveDuplicateHistory(statusHistories);
				task.CompletionHistories = RemoveDuplicateHistory(completionHistories);
				UpdateTaskProgress(task);
			}
		}

		internal Container GetContainer(Guid containerId)
		{
			var container = _containerRepository.FindById(containerId);
			if (null == container)
			{
				throw new DatabaseItemNotFoundException();
			}

			return container;
		}

		/// <summary>
		/// Determines if the container has pending tasks.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool PendingTasks(Guid containerId)
		{
			var container = GetContainer(containerId);
			return _taskRepository.PendingTasks(container.PrimarySearchEntityId);
		}

		/// <summary>
		/// Fetches the task lookup list for the specified container.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
		public IList<Lookup> FetchLookups(Guid containerId)
		{
			var container = GetContainer(containerId);
			var tasks = _taskRepository.FindIdsByPrimarySearchEntityId(container.PrimarySearchEntityId);
			if (tasks.Count == 0)
				return null;

			return tasks.Select(x => new Lookup
			{
				ContainerId = containerId,
				Id = x.Id,
				Name = string.Empty
			}).ToList();
		}


		/// <summary>
		/// Fetches all, including deleted tasks.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <returns>
		/// IList{Task}.
		/// </returns>
		public IList<Task> FetchAllWithDeleted(Guid containerId)
		{
			var container = GetContainer(containerId);
			var tasks = _taskRepository.FindByPrimarySearchEntityId(container.PrimarySearchEntityId, true, true);
			if (null == tasks)
				return null;

			GetStatusAndCompletionHistories(containerId, tasks);

			return tasks;
		}

		/// <summary>
		///     Finds the by id.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="id">The id.</param>
		/// <returns>Task.</returns>
		public Task FetchById(Guid containerId, Guid id)
		{
			GetContainer(containerId);
			var task = _taskRepository.FindById(id);

			if (task == null)
				return null;

			task.ContainerId = containerId;
			UpdateTaskProgress(task);
			return task;
		}

		/// <summary>
		/// Fetches the task only.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="id">The identifier.</param>
		/// <returns>Task.</returns>
		public Task FetchTaskOnly(Guid containerId, Guid id)
		{
			GetContainer(containerId);
			var task = _taskRepository.FindByIdTaskOnly(id);

			if (task == null)
				return null;

			task.ContainerId = containerId;
			UpdateTaskProgress(task);
			return task;
		}

		/// <summary>
		///     Creates the specified container id.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="task">The task.</param>
		/// <returns>Guid.</returns>
		public Guid Create(Guid containerId, Task task)
		{
		    using (var scope = _transactionFactory.Create())
		    {
                var container = GetContainer(containerId);
                SetupTask(_principalResolver, container, task);
                task.Created = DateTime.UtcNow;
                var taskId = _taskRepository.Create(task);
                _taskNotificationProvider.Save(task.Notifications, taskId);

		        scope.Complete();

		        return taskId;
		    }
		}

		/// <summary>
		///     Updates the specified container id.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="task">The task.</param>
		public void Update(Guid containerId, Task task)
		{
		    using (var scope = _transactionFactory.Create())
		    {
		        var container = GetContainer(containerId);
		        SetupTask(_principalResolver, container, task);
		        task.Modified = DateTime.UtcNow;
		        _taskRepository.Update(task);
		        _taskNotificationProvider.Save(task.Notifications, task.Id.GetValueOrDefault());

		        scope.Complete();
		    }
		}

        /// <summary>
        /// Updates a list of tasks.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="tasks">The tasks.</param>
	    public void BulkUpdate(Guid containerId, IList<Task> tasks)
        {
            using (var scope = _transactionFactory.Create())
            {

                var container = GetContainer(containerId);
                var flattenedTasks = new List<Task>();
                var taskCreationOrder = 0;
                tasks.Aggregate(0,
                    (current, task) =>
                        SetupBulkTasks(_principalResolver, container, task, current, flattenedTasks,
                            ref taskCreationOrder));
                _taskRepository.BulkUpdate(flattenedTasks);
                flattenedTasks.ForEach(task =>
                {
                    _taskNotificationProvider.Save(task.Notifications, task.Id.GetValueOrDefault());
                });

                scope.Complete();
            }
        }

		/// <summary>
		///     Deletes the specified container id.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="id">The id.</param>
		public void Delete(Guid containerId, Guid id)
		{
		    using (var scope = _transactionFactory.Create())
		    {
		        GetContainer(containerId);
		        _taskRepository.Remove(id, _principalResolver.UserId);
                _taskNotificationProvider.Save(new List<TaskNotification>(), id);

		        scope.Complete();
		    }
		}

		/// <summary>
		///     Bulks the create.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="tasks">The tasks.</param>
		public void BulkCreate(Guid containerId, IList<Task> tasks)
		{
		    using (var scope = _transactionFactory.Create())
		    {
		        var container = GetContainer(containerId);
		        var flattenedTasks = new List<Task>();
		        var taskCreationOrder = 0;
		        tasks.Aggregate(0,
		            (current, task) =>
		                SetupBulkTasks(_principalResolver, container, task, current, flattenedTasks, ref taskCreationOrder));
		        _taskRepository.BulkCreate(flattenedTasks);
		        tasks.ForEach(task =>
		        {
		            _taskNotificationProvider.Save(task.Notifications, task.Id.GetValueOrDefault());
		        });

		        scope.Complete();
		    }
		}

		internal static int SetupBulkTasks(IPrincipalResolver principalResolver, Container container, Task task, int lastTaskNumber, List<Task> flattenedTasks,
			ref int taskCreationOrder)
		{
			SetupTask(principalResolver, container, task);
			//task.TaskNumber = 
			++lastTaskNumber;
			flattenedTasks.Add(task);

			if (task.SubTasks != null && task.SubTasks.Count > 0)
			{
				foreach (var subTask in task.SubTasks)
				{
					lastTaskNumber = SetupBulkTasks(principalResolver, container, subTask, lastTaskNumber, flattenedTasks, ref taskCreationOrder);
					task.ChildTaskNumbers.Add(subTask.TaskNumber);
				}
			}

			task.CreationOrder = ++taskCreationOrder;
			return lastTaskNumber;
		}

		internal static void SetupTask(IPrincipalResolver principalResolver, Container container, Task task)
		{
			if (!task.Id.HasValue)
			{
				task.Id = Guid.NewGuid();
			}
			task.PrimarySearchEntityId = container.PrimarySearchEntityId;
			task.PrimarySearchEntityType = container.PrimarySearchEntityType;
			task.ContainerId = container.Id.GetValueOrDefault();
			var currentDateTime = DateTime.UtcNow;
			task.CreatedById = principalResolver.UserId;
			task.CreatedDateTime = currentDateTime;
			task.UpdatedById = principalResolver.UserId;
			task.UpdatedDateTime = currentDateTime;
		}

		/// <summary>
		///     Fetches the history.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="taskId">The task id.</param>
		/// <returns>IList{TaskHistory}.</returns>
		public IList<TaskHistory> FetchHistory(Guid containerId, Guid taskId)
		{
			GetContainer(containerId);
			var taskHistories = new List<TaskHistory>();
			var histories = _historyProvider.FetchHistoryByEntityId(taskId);
			foreach (var history in histories)
			{
				var entityHistoryStrategy = _entityHistoryStrategyResolver.Resolve(history.ActionDetailEntityType);
				var entityHistoryContext = new EntityHistoryContext(entityHistoryStrategy);
				var entityHistory = entityHistoryContext.CreateEntityHistory(history);
				var entityTaskHistory = entityHistory as EntityTaskHistory;
				if (entityTaskHistory != null)
				{
					var taskHistory = _mapperRegistry.Map<TaskHistory>(entityTaskHistory);
					taskHistories.Add(taskHistory);
				}
			}
			return taskHistories;
		}

		private TaskHistory CreateTaskHistory(History history)
		{
			var historyBytes = Encoding.ASCII.GetBytes(history.ActionDetail);
			var reader = XmlDictionaryReader.CreateTextReader(historyBytes, new XmlDictionaryReaderQuotas());
			var serializer = new DataContractSerializer(typeof(TaskDto));
			var taskDto = (TaskDto)serializer.ReadObject(reader, true);
			var task = _mapperRegistry.Map<Task>(taskDto);
			return new TaskHistory { Task = task, CreatedBy = task.ModifiedBy, CreatedDate = task.Modified };
		}

		/// <summary>
		/// Fetches the history.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="taskId">The task identifier.</param>
		/// <returns></returns>
		public IList<TaskDelta> FetchDeltaHistory(Guid containerId, Guid taskId)
		{
			var taskDeltas = new List<TaskDelta>();
			EntityHistory entityHistoryPrevious = new EntityTaskHistory { Task = new Task() };
			GetContainer(containerId);
			var histories = _historyProvider.FetchHistoryByEntityId(taskId).OrderBy(x => x.ActionDate);
			foreach (var history in histories)
			{
				var entityHistoryStrategy = _entityHistoryStrategyResolver.Resolve(history.ActionDetailEntityType);
				var entityHistoryContext = new EntityHistoryContext(entityHistoryStrategy);
				entityHistoryPrevious = entityHistoryContext.Process(taskDeltas, history, entityHistoryPrevious);
			}
			return taskDeltas;
		}

		/// <summary>
		/// Fetches the count of tasks for the container.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
		public int FetchCount(Guid containerId)
		{
			var container = GetContainer(containerId);
			var count = _taskRepository.FetchCountByPrimarySearchEntityId(container.PrimarySearchEntityId);
			return count;
		}

		/// <summary>
		///     Fetches the multiple container tasks.
		/// </summary>
		/// <param name="containerIds">The container ids.</param>
		/// <returns>IList{TaskContainer}.</returns>
		public IList<TaskContainer> FetchMultipleContainerTasks(IEnumerable<Guid> containerIds)
		{
			//ToDo: improve performance by adding _containerRepository.FindByIds
			var containers = containerIds.Select(GetContainer).ToList();
			var primarySearchEntityIds = containers.Select(x => x.PrimarySearchEntityId).ToList();
			var tasks = _taskRepository.FindByMultiplePrimarySearchEntityIds(primarySearchEntityIds);
			var taskContainers = new List<TaskContainer>();

			foreach (var container in containers)
			{
				// ReSharper disable once AccessToForEachVariableInClosure
				var containerTasks = tasks.Where(x => x.PrimarySearchEntityId == container.PrimarySearchEntityId);
				// ReSharper disable once PossibleNullReferenceException
				var taskContainer = new TaskContainer { ContainerId = container.Id.GetValueOrDefault(), Tasks = containerTasks };
				taskContainers.Add(taskContainer);
			}

			return taskContainers;
		}

		internal static IList<T> RemoveDuplicateHistory<T>(IEnumerable<T> sourceList) where T : TaskHistoryBase, new()
		{
			var newList = new List<T>();
			var lastItem = new T();
			var i = 0;
			foreach (var item in sourceList.OrderBy(x => x.CreatedDate))
			{
				if (i++ == 0 || !item.Value.Equals(lastItem.Value))
				{
					newList.Add(item);
				}
				lastItem = item;
			}
			return newList;
		}

		/// <summary>
		/// Fetches the predecessors by project.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		/// <returns></returns>
		public IList<TaskPredecessor> FetchPredecessorsByProject(Guid entityId)
		{
			return _taskRepository.FetchPredecessorsByProject(entityId);
		}

		internal void UpdateTaskProgress(Task task)
		{
			var today = DateTime.Today;

			if (task.Status == TaskStatusEnumDto.Completed)
			{
				task.Progress = TaskProgressEnumDto.Completed;
			}
			else if (task.Status == TaskStatusEnumDto.Canceled)
			{
				task.Progress = TaskProgressEnumDto.Canceled;
			}
			else if (task.DueDate.HasValue && task.DueDate.Value.Date < today)
			{
				task.Progress = TaskProgressEnumDto.InTrouble;
			}

			else if (task.ReminderDate.HasValue && task.ReminderDate.Value.Date < today)
			{
				task.Progress = TaskProgressEnumDto.Slipping;
			}
			else if ((task.StartDate.HasValue && task.StartDate.Value.Date > today) ||
					 task.StartDate.GetValueOrDefault() == default(DateTime))
			{
				task.Progress = TaskProgressEnumDto.Waiting;
			}
			else
			{
				task.Progress = TaskProgressEnumDto.OnTrack;
			}

			if (task.SubTasks.Any())
			{
				foreach (var subTask in task.SubTasks)
				{
					UpdateTaskProgress(subTask);
				}
			}
		}
	}
}