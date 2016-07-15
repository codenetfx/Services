using System;
using System.Collections.Generic;
using System.IO;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	///     Interface ITaskManager
	/// </summary>
	public interface ITaskManager
	{



		/// <summary>
		/// Bulk updates tasks.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="tasks">The tasks.</param>
		/// <returns></returns>
		TaskChangeResult UpdateBulk(Guid containerId, List<Task> tasks);

		/// <summary>
		///     Fetches all.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <returns>IList{Task}.</returns>
		IList<Task> FetchAll(Guid containerId);

		/// <summary>
		///     Fetches all keeping flat, tasks next to subtasks.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <returns>IList{Task}.</returns>
		IList<Task> FetchAllFlatList(Guid containerId);

		/// <summary>
		///     Finds the by id.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="taskId">The task id.</param>
		/// <returns>Container.</returns>
		Task FetchById(Guid containerId, Guid taskId);

		/// <summary>
		///     Updates the specified task.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="task">The task.</param>
		void Update(Guid containerId, Task task);

		/// <summary>
		/// Validates the specified container identifier.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="task">The task.</param>
		/// <returns></returns>
		IList<TaskValidationEnumDto> Validate(Guid containerId, Task task);

		/// <summary>
		///     Creates the specified task.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="task">The task.</param>
		/// <returns>Guid.</returns>
		Guid Create(Guid containerId, Task task);

		/// <summary>
		///     Deletes the specified task id.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="taskId">The task id.</param>
		void Delete(Guid containerId, Guid taskId);

		/// <summary>
		///     Bulks the create.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="tasks">The tasks.</param>
		void BulkCreate(Guid containerId, IList<Task> tasks);

		/// <summary>
		///     Fetches the history.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="taskId">The task id.</param>
		/// <returns>IList{TaskHistory}.</returns>
		IList<TaskHistory> FetchHistory(Guid containerId, Guid taskId);

		/// <summary>
		///     Downloads the tasks for a given container.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		Stream DownloadByContainer(SearchCriteria searchCriteria);

		/// <summary>
		///     Downloads the tasks for the specified container id.
		/// </summary>
		/// <param name="searchCriteria">The container id.</param>
		/// <returns></returns>
		Stream Download(SearchCriteria searchCriteria);

		/// <summary>
		///     Searches the tasks for the project specified in the search crtieria.
		/// </summary>
		/// <param name="searchCriteria">
		///     The search criteria.  This must have the contiaenr id specified in the AriaContainerID
		///     filter.
		/// </param>
		/// <param name="refinerResults"></param>
		/// <returns></returns>
		IList<Task> Search(SearchCriteria searchCriteria, out Dictionary<string, List<RefinementItemDto>> refinerResults);

		/// <summary>
		/// Fetches all with deleted.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
		IList<Task> FetchAllWithDeleted(Guid containerId);

		/// <summary>
		/// Fetches the delta history.
		/// </summary>
		/// <param name="containerIdGuid">The container identifier unique identifier.</param>
		/// <param name="taskIdGuid">The task identifier unique identifier.</param>
		/// <returns></returns>
		IList<TaskDelta> FetchDeltaHistory(Guid containerIdGuid, Guid taskIdGuid);

		/// <summary>
		/// Fetches the count.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		int FetchCount(Guid containerId);

		/// <summary>
		/// Fetches the status list by task data.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="id">The identifier.</param>
		/// <param name="task">The task.</param>
		/// <returns>TaskStatusList.</returns>
		TaskStatusList FetchStatusListByTaskData(Guid containerId, Guid id, Task task);

		/// <summary>
		/// Validates the bulk.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="tasksToValidate">The tasks to validate.</param>
		/// <returns></returns>
		Dictionary<Guid, IList<TaskValidationEnumDto>> ValidateTasks(Guid containerId, IList<Task> tasksToValidate);

		/// <summary>
		/// Creates the document from the document Template and links document to the task.
		/// </summary>
		/// <param name="taskId">The task identifier.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="documentTemplateId">The document template identifier.</param>
		/// <param name="metaData">The meta data.</param>
		/// <returns>Guid.</returns>
		Guid CreateAndLinkDocument(Guid taskId, Guid containerId, Guid documentTemplateId,
			IDictionary<string, string> metaData);

		/// <summary>
		/// Fetches the project tasks.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <param name="refinerResults">The refiner results.</param>
		/// <returns></returns>
		List<Task> FetchProjectTasks(Guid projectId, SearchCriteria searchCriteria, out  Dictionary<string, List<RefinementItemDto>> refinerResults);

		/// <summary>
		/// Deletes the bulk.
		/// </summary>
		/// <param name="convertedContainerId">The converted container identifier.</param>
		/// <param name="taskGuidIds">The task unique identifier ids.</param>
		/// <returns></returns>
		TaskChangeResult DeleteBulk(Guid convertedContainerId, List<Guid> taskGuidIds);

		

		/// <summary>
		/// Validates the on delete tasks.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="taskGuidIds">The task unique identifier ids.</param>
		/// <returns></returns>
		Dictionary<Guid, IList<TaskDeleteValidationDto>> ValidateOnDeleteTasks(Guid containerId, List<Guid> taskGuidIds);
	}
}