using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Linq;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
	/// <summary>
	///     Class TaskService
	/// </summary>
	[AutoRegisterRestService]
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
		InstanceContextMode = InstanceContextMode.PerCall)]
	public class TaskService : ITaskService
	{
		private readonly IMapperRegistry _mapperRegistry;
		private readonly ITaskManager _taskManager;

		/// <summary>
		///     Initializes a new instance of the <see cref="TaskService" /> class.
		/// </summary>
		/// <param name="taskManager">The task manager.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		public TaskService(ITaskManager taskManager, IMapperRegistry mapperRegistry)
		{
			_taskManager = taskManager;
			_mapperRegistry = mapperRegistry;
		}

		/// <summary>
		///     Fetches all.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <returns>IList{TaskDto}.</returns>
		public IList<TaskDto> FetchAll(string containerId)
		{
			var containerIdGuid = ExtractContainerId(containerId);
			var tasks = _taskManager.FetchAll(containerIdGuid);
			return _mapperRegistry.Map<IList<TaskDto>>(tasks);
		}

		/// <summary>
		/// Fetches the count of tasks in the specified container
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
		public int FetchCount(string containerId)
		{
			var containerIdGuid = ExtractContainerId(containerId);
			return _taskManager.FetchCount(containerIdGuid);
		}

		/// <summary>
		/// Fetches all with deleted.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
		public IList<TaskDto> FetchAllWithDeleted(string containerId)
		{
			var containerIdGuid = ExtractContainerId(containerId);
			var tasks = _taskManager.FetchAllWithDeleted(containerIdGuid);
			if (null == tasks)
				return new List<TaskDto>();
			return _mapperRegistry.Map<IList<TaskDto>>(tasks);
		}

		/// <summary>
		/// Fetches status and status list by the task data.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="id">The id.</param>
		/// <param name="task">The task.</param>
		/// <returns>TaskStatusListDto.</returns>
		public TaskStatusListDto FetchStatusListByTaskData(string containerId, string id, TaskDto task)
		{
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var containerIdGuid = containerId.ToGuid();
			Guard.IsNotEmptyGuid(containerIdGuid, "containerId");
			Guard.IsNotNullOrEmpty(id, "id");
			var idGuid = id.ToGuid();
			Guard.IsNotEmptyGuid(idGuid, "id");
			Guard.IsNotNull(task, "task");
			var taskBo = _mapperRegistry.Map<Task>(task);
			var taskStatus = _taskManager.FetchStatusListByTaskData(containerIdGuid, idGuid, taskBo);
			return _mapperRegistry.Map<TaskStatusListDto>(taskStatus);
		}

		/// <summary>
		/// Downloads the tasks for the specified container id.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		public Stream Download(SearchCriteriaDto searchCriteria)
		{
			Guard.IsNotNull(searchCriteria, "searchCriteria");
			Guard.IsNotNull(searchCriteria.Filters, "searchCriteria.Filters");
			var containerId = Common.Framework.Guard.FilterIsNotNull(searchCriteria, AssetFieldNames.AriaContainerId);

			var document = _taskManager.DownloadByContainer(_mapperRegistry.Map<SearchCriteria>(searchCriteria));
			AddHeaders(containerId, document);
			return document;
		}

		/// <summary>
		/// Downloads a doc ument for the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		public Stream DownloadSearch(SearchCriteriaDto searchCriteria)
		{
			Guard.IsNotNull(searchCriteria, "searchCriteria");
			if (EntityTypeEnumDto.Task != searchCriteria.EntityType)
				throw new ArgumentException("This operation only supports searching for Tasks.", "searchCriteria");
			var document = _taskManager.Download(_mapperRegistry.Map<SearchCriteria>(searchCriteria));
			AddHeaders(EntityTypeEnumDto.Task.ToString(), document);
			return document;
		}

		/// <summary>
		///     Fetches the by id.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="id">The id.</param>
		/// <returns>TaskDto.</returns>
		public TaskDto FetchById(string containerId, string id)
		{
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var containerIdGuid = containerId.ToGuid();
			Guard.IsNotEmptyGuid(containerIdGuid, "containerId");
			Guard.IsNotNullOrEmpty(id, "id");

			Task task;
			Guid idGuid;
			int idInt;

			if (Guid.TryParse(id, out idGuid))
				task = FetchById(containerIdGuid, idGuid);
			else if (int.TryParse(id, out idInt))
				task = FetchByNumber(containerIdGuid, idInt);
			else
				throw new ArgumentException("Argument is not a valid Guid or Integer", "id");

			return _mapperRegistry.Map<TaskDto>(task);
		}

		/// <summary>
		/// Validates the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="task">The task.</param>
		/// <returns></returns>
		public IList<TaskValidationEnumDto> Validate(string id, string containerId, TaskDto task)
		{
			Task taskBo;
			var containerIdGuid = VerifyParametersAndMap(id, containerId, task, out taskBo);
			return _taskManager.Validate(containerIdGuid, taskBo);
		}

		/// <summary>
		///     Creates the specified task.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="task">The task.</param>
		/// <returns>System.String.</returns>
		public string Create(string containerId, TaskDto task)
		{
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var containerIdGuid = containerId.ToGuid();
			Guard.IsNotEmptyGuid(containerIdGuid, "containerId");
			Guard.IsNotNull(task, "task");
			var taskBo = _mapperRegistry.Map<Task>(task);
			return _taskManager.Create(containerIdGuid, taskBo).ToString();
		}

		/// <summary>
		///     Updates the specified task.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="containerId">The container id.</param>
		/// <param name="task">The task.</param>
		public void Update(string id, string containerId, TaskDto task)
		{
			Task taskBo;
			var containerIdGuid = VerifyParametersAndMap(id, containerId, task, out taskBo);
			_taskManager.Update(containerIdGuid, taskBo);
		}

		/// <summary>
		///     Deletes the specified id.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="id">The id.</param>
		public void Delete(string containerId, string id)
		{
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var containerIdGuid = containerId.ToGuid();
			Guard.IsNotEmptyGuid(containerIdGuid, "containerId");
			Guard.IsNotNullOrEmpty(id, "id");
			var idGuid = id.ToGuid();
			Guard.IsNotEmptyGuid(idGuid, "id");
			_taskManager.Delete(containerIdGuid, idGuid);
		}

		/// <summary>
		///     Bulks the create.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="tasks">The tasks.</param>
		public void BulkCreate(string containerId, IList<TaskDto> tasks)
		{
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var containerIdGuid = containerId.ToGuid();
			Guard.IsNotEmptyGuid(containerIdGuid, "containerId");
			Guard.IsNotNull(tasks, "tasks");
			var tasksBo = _mapperRegistry.Map<IList<Task>>(tasks);
			_taskManager.BulkCreate(containerIdGuid, tasksBo);
		}

		/// <summary>
		///     Fetches the history.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="taskId">The task id.</param>
		/// <returns>IList{TaskHistoryDto}.</returns>
		public IList<TaskHistoryDto> FetchHistory(string containerId, string taskId)
		{
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var containerIdGuid = containerId.ToGuid();
			Guard.IsNotEmptyGuid(containerIdGuid, "containerId");
			Guard.IsNotNullOrEmpty(taskId, "taskId");
			var taskIdGuid = taskId.ToGuid();
			Guard.IsNotEmptyGuid(taskIdGuid, "taskId");
			var taskHistories = _taskManager.FetchHistory(containerIdGuid, taskIdGuid);
			return _mapperRegistry.Map<IList<TaskHistoryDto>>(taskHistories);
		}

		/// <summary>
		///     Fetches the history.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="taskId">The task id.</param>
		/// <returns>IList{TaskHistoryDto}.</returns>
		public IList<TaskDeltaDto> FetchDeltaHistory(string containerId, string taskId)
		{
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var containerIdGuid = containerId.ToGuid();
			Guard.IsNotEmptyGuid(containerIdGuid, "containerId");
			Guard.IsNotNullOrEmpty(taskId, "taskId");
			var taskIdGuid = taskId.ToGuid();
			Guard.IsNotEmptyGuid(taskIdGuid, "taskId");
			var taskDeltaHistory = _taskManager.FetchDeltaHistory(containerIdGuid, taskIdGuid);
			return _mapperRegistry.Map<IList<TaskDeltaDto>>(taskDeltaHistory);
		}

		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		public TaskSearchResultSetDto Search(SearchCriteriaDto searchCriteria)
		{
			Guard.IsNotNull(searchCriteria, "searchCriteria");

			Dictionary<string, List<RefinementItemDto>> refinerResults;
			var taskResult = _taskManager.Search(_mapperRegistry.Map<SearchCriteria>(searchCriteria), out refinerResults);

			// convert to dto
			var results = taskResult.Select(x => _mapperRegistry.Map<TaskSearchResultDto>(x)).ToList();

			//
			// build search result set
			//
			var taskSearchResultSetDto = new TaskSearchResultSetDto
			{
				SearchCriteria = searchCriteria,
				Results = results,
				Summary = new SearchSummaryDto { TotalResults = results.Count(), EndIndex = results.Count() - 1 },
				RefinerResults = refinerResults
			};

			return taskSearchResultSetDto;
		}

		/// <summary>
		/// Fetches the project tasks.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		public TaskSearchResultSetDto FetchProjectTasks(string projectId, SearchCriteriaDto searchCriteria)
		{
			Dictionary<string, List<RefinementItemDto>> refinerResults;
			Guard.IsNotNullOrEmpty(projectId, "projectId");
			var projectIdGruid = projectId.ToGuid();
			var criteria = _mapperRegistry.Map<SearchCriteria>(searchCriteria);
			var taskResult = _taskManager.FetchProjectTasks(projectIdGruid, criteria, out refinerResults);


			// convert to dto
			var results = taskResult.Select(x => _mapperRegistry.Map<TaskSearchResultDto>(x)).ToList();

			//
			// build search result set
			//
			var taskSearchResultSetDto = new TaskSearchResultSetDto
			{
				SearchCriteria = searchCriteria,
				Results = results,
				Summary = new SearchSummaryDto { TotalResults = results.Count(), EndIndex = results.Count() - 1 },
				RefinerResults = refinerResults
			};

			return taskSearchResultSetDto;


		}


		/// <summary>
		///     Fetches all keeping flat, tasks next to subtasks.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <returns>IList{TaskDto}.</returns>
		public IList<TaskDto> FetchAllFlatList(string containerId)
		{
			var containerIdGuid = ExtractContainerId(containerId);
			var tasks = _taskManager.FetchAllFlatList(containerIdGuid);
			return _mapperRegistry.Map<IList<TaskDto>>(tasks);
		}

		private static Guid ExtractContainerId(string containerId)
		{
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var containerIdGuid = containerId.ToGuid();
			Guard.IsNotEmptyGuid(containerIdGuid, "containerId");
			return containerIdGuid;
		}

		private static void AddHeaders(string containerId, Stream document)
		{
			var context = WebOperationContext.Current;
			if (null != context)
			{
				context.OutgoingResponse.Headers["Content-Disposition"] = "attachment; filename=" + containerId + ".xlsx";
				context.OutgoingResponse.ContentLength = document.Length;
				context.OutgoingResponse.ContentType =
					"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			}
		}

		private Task FetchById(Guid containerId, Guid id)
		{
			Guard.IsNotEmptyGuid(id, "id");

			return _taskManager.FetchById(containerId, id);
		}

		private Task FetchByNumber(Guid containerId, int number)
		{
			Guard.IsGreaterThan(0, number, "number");

			var tasks = _taskManager.FetchAll(containerId);
			var task = tasks.Flatten(t => t.SubTasks).FirstOrDefault(x => x.TaskNumber == number);
			if (task == null)
				throw new DatabaseItemNotFoundException();

			return task;
		}

		private Guid VerifyParametersAndMap(string id, string containerId, TaskDto task, out Task taskBo)
		{
			Guard.IsNotNullOrEmpty(id, "id");
			var idGuid = id.ToGuid();
			Guard.IsNotEmptyGuid(idGuid, "id");
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var containerIdGuid = containerId.ToGuid();
			Guard.IsNotEmptyGuid(containerIdGuid, "containerId");
			Guard.IsNotNull(task, "task");
			taskBo = _mapperRegistry.Map<Task>(task);
			return containerIdGuid;
		}

		/// <summary>
		/// Updates the tasks.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="tasks">The tasks.</param>
		/// <returns></returns>	
		public TaskSearchResultSetDto UpdateTasks(string containerId, IList<TaskDto> tasks)
		{

			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var containerIdGuid = containerId.ToGuid();
			Guard.IsNotEmptyGuid(containerIdGuid, "containerId");
			Guard.IsNotNull(tasks, "tasks");
			Guard.IsGreaterThan(0, tasks.Count, "tasks");

			var tasksBo = _mapperRegistry.Map<IList<Task>>(tasks);
			var taskResult = _taskManager.UpdateBulk(containerIdGuid, tasksBo.ToList());

			// convert to dto
			var results = taskResult.UpdatedTasks.Select(x => _mapperRegistry.Map<TaskSearchResultDto>(x)).ToList();

			//
			// build search result set
			//
			var taskSearchResultSetDto = new TaskSearchResultSetDto
			{
				SearchCriteria = null,
				Results = results,
				Summary = new SearchSummaryDto { TotalResults = results.Count(), EndIndex = results.Count() - 1 },
				RefinerResults = null
			};

			return taskSearchResultSetDto;
		}


		/// <summary>
		/// Validates the bulk.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="tasks">The tasks.</param>
		/// <returns></returns>		
		public Dictionary<Guid, IList<TaskValidationEnumDto>> ValidateTasks(string containerId, IList<TaskDto> tasks)
		{
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var containerIdGuid = containerId.ToGuid();
			Guard.IsNotEmptyGuid(containerIdGuid, "containerId");
			Guard.IsNotNull(tasks, "tasks");
			var tasksBo = _mapperRegistry.Map<IList<Task>>(tasks);
			return _taskManager.ValidateTasks(containerIdGuid, tasksBo);
		}

		/// <summary>
		/// Creates the and link document.
		/// </summary>
		/// <param name="taskId">The task identifier.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="documentTemplateId">The document template identifier.</param>
		/// <param name="metaData">The meta data.</param>
		/// <returns>The document identifier,System.String.</returns>
		public string CreateAndLinkDocument(string taskId, string containerId, string documentTemplateId,
			IDictionary<string, string> metaData)
		{
			Guard.IsNotNullOrEmpty(taskId, "taskId");
			var convertedTaskId = taskId.ToGuid();
			Guard.IsNotEmptyGuid(convertedTaskId, "taskId");
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var convertedContainerId = containerId.ToGuid();
			Guard.IsNotEmptyGuid(convertedContainerId, "containerId");
			Guard.IsNotNullOrEmpty(documentTemplateId, "documentTemplateId");
			var convertedDocumentTemplateId = documentTemplateId.ToGuid();
			Guard.IsNotEmptyGuid(convertedDocumentTemplateId, "documentTemplateId");
			Guard.IsNotNull(metaData, "metaData");

			var documentId = _taskManager.CreateAndLinkDocument(convertedTaskId, convertedContainerId, convertedDocumentTemplateId, metaData);

			return documentId.ToString();
		}

		/// <summary>
		/// Deletes the tasks.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="taskIds">The task ids.</param>
		/// <returns></returns>
		public TaskChangeResponseDto DeleteTasks(string containerId, List<string> taskIds)
		{
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var convertedContainerId = containerId.ToGuid();
			Guard.IsNotEmptyGuid(convertedContainerId, "containerId");
			Guard.IsNotNull(taskIds, "taskIds");
			Guard.IsGreaterThan(0, taskIds.Count, "taskIds");
			List<Guid> taskGuidIds = new List<Guid>();
			taskIds.ForEach(id =>
			{
				var tempId = id.ToGuid();
				Guard.IsNotEmptyGuid(tempId, "taskIds");
				taskGuidIds.Add(tempId);
			});

			var result = _taskManager.DeleteBulk(convertedContainerId, taskGuidIds);

			var taskSearchResultSetDto = new TaskSearchResultSetDto
			{
				SearchCriteria = null,
				Results = _mapperRegistry.Map<List<TaskSearchResultDto>>(result.UpdatedTasks),
				Summary = new SearchSummaryDto { TotalResults = result.UpdatedTasks.Count(), EndIndex = result.UpdatedTasks.Count() - 1 },
				RefinerResults = null
			};

			return new TaskChangeResponseDto()
				{
					UpdatedTasksSearchResult = taskSearchResultSetDto,
					DeletedEntityIDs = result.DeletedTasks.Select(x => x.Id.Value).ToList()
				};
		}

		/// <summary>
		/// Validates the on delete tasks.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="taskIds">The task ids.</param>
		/// <returns></returns>
		public Dictionary<Guid, IList<TaskDeleteValidationDto>> ValidateOnDeleteTasks(string containerId, IList<string> taskIds)
		{
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var convertedContainerId = containerId.ToGuid();
			Guard.IsNotEmptyGuid(convertedContainerId, "containerId");
			Guard.IsNotNull(taskIds, "taskIds");
			Guard.IsGreaterThan(0, taskIds.Count, "taskIds");
			var taskGuidIds = new List<Guid>();
			taskIds.ToList().ForEach(id =>
			{
				var tempId = id.ToGuid();
				Guard.IsNotEmptyGuid(tempId, "taskIds");
				taskGuidIds.Add(tempId);
			});

			return _taskManager.ValidateOnDeleteTasks(convertedContainerId, taskGuidIds);
		}
	}
}