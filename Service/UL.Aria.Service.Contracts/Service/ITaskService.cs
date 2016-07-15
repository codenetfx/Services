using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Interface ITaskService
    /// </summary>
    [ServiceContract]
    public interface ITaskService
    {
        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <returns>IList{TaskDto}.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Container/{containerId}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<TaskDto> FetchAll(string containerId);

        /// <summary>
        /// Fetches the count of tasks in the specified container
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Container/{containerId}/count", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        int FetchCount(string containerId);

        /// <summary>
        /// Downloads the specified container id.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Container/Download", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Stream Download(SearchCriteriaDto searchCriteria);

        /// <summary>
        /// Downloads a doc ument for the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Container/DownloadSearch", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Stream DownloadSearch(SearchCriteriaDto searchCriteria);

        /// <summary>
        ///     Fetches the by id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="id">The id.</param>
        /// <returns>TaskDto.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}/Container/{containerId}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        TaskDto FetchById(string containerId, string id);

        /// <summary>
        /// Validates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "{id}/Container/{containerId}/Validate", Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<TaskValidationEnumDto> Validate(string id, string containerId, TaskDto task);

        /// <summary>
        ///     Creates the specified container id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="task">The task.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Container/{containerId}", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string Create(string containerId, TaskDto task);

        /// <summary>
        ///     Updates the specified task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="containerId">The container id.</param>
        /// <param name="task">The task.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "{id}/Container/{containerId}", Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Update(string id, string containerId, TaskDto task);

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="id">The id.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}/Container/{containerId}", Method = "DELETE",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Delete(string containerId, string id);

        /// <summary>
        ///     Bulks the create.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="tasks">The tasks.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Container/{containerId}/Tasks", Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void BulkCreate(string containerId, IList<TaskDto> tasks);

        /// <summary>
        ///     Fetches the history.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="taskId">The task id.</param>
        /// <returns>IList{TaskHistoryDto}.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Container/{containerId}/Tasks/{taskId}/History", Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<TaskHistoryDto> FetchHistory(string containerId, string taskId);

        /// <summary>
        /// Fetches the delta history.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="taskId">The task identifier.</param>
        /// <returns>IList{TaskDeltaDto}</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Container/{containerId}/Tasks/{taskId}/DeltaHistory", Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<TaskDeltaDto> FetchDeltaHistory(string containerId, string taskId);

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Container/Tasks/Search", Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        TaskSearchResultSetDto Search(SearchCriteriaDto searchCriteria);

        /// <summary>
        /// Fetches all with deleted.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Container/{containerId}/IncludeDeleted", Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<TaskDto> FetchAllWithDeleted(string containerId);

        /// <summary>
        /// Fetches status and status list by the task data.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="id">The id.</param>
        /// <param name="task">The task.</param>
        /// <returns>TaskStatusListDto.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}/Container/{containerId}/Tasks/StatusList", Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        TaskStatusListDto FetchStatusListByTaskData(string containerId, string id, TaskDto task);

        /// <summary>
        /// Updates the tasks.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="tasks">The tasks.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Container/{containerId}/UpdateTasks", Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        TaskSearchResultSetDto UpdateTasks(string containerId, IList<TaskDto> tasks);


        /// <summary>
        /// Validates the bulk.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="tasks">The tasks.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "Container/{containerId}/ValidateTasks", Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Dictionary<Guid, IList<TaskValidationEnumDto>> ValidateTasks(string containerId, IList<TaskDto> tasks);

        /// <summary>
        /// Creates the and link document.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="documentTemplateId">The document template identifier.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>The document identifier,System.String.</returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate =
                "/{taskId}?operation=CreateAndLinkDocument&containerId={containerId}&documentTemplateId={documentTemplateId}",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string CreateAndLinkDocument(string taskId, string containerId, string documentTemplateId,
            IDictionary<string, string> metaData);


        /// <summary>
        /// Fetches the project tasks.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Project/{projectId}", Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json)]
        TaskSearchResultSetDto FetchProjectTasks(string projectId, SearchCriteriaDto searchCriteria);


        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        [OperationContract]
		[WebInvoke(UriTemplate = "/Container/{containerId}/DeleteTasks", Method = "DELETE",
            BodyStyle= WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        TaskChangeResponseDto DeleteTasks(string containerId, List<string> ids);


	    /// <summary>
	    /// Validates the on delete tasks.
	    /// </summary>
	    /// <param name="containerId">The container identifier.</param>
	    /// <param name="taskIds">The task ids.</param>
	    /// <returns></returns>
	    [OperationContract]
		[WebInvoke(UriTemplate = "Container/{containerId}/ValidateOnDeleteTasks", Method = "POST",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		Dictionary<Guid, IList<TaskDeleteValidationDto>> ValidateOnDeleteTasks(string containerId, IList<string> taskIds);

    }
}