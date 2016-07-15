using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{

	/// <summary>
	/// Interface ITaskTypeService
	/// </summary>
    [ServiceContract]
    public interface ITaskTypeService
    {
		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>TaskTypeDto.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		TaskTypeDto FetchById(string id);

		/// <summary>
		/// Deletes the specified task type identifier.
		/// </summary>
		/// <param name="taskTypeId">The task type identifier.</param>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/{taskTypeId}", Method = "DELETE", RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Delete(string taskTypeId);

		/// <summary>
		/// Creates the specified task type.
		/// </summary>
		/// <param name="taskTypeDto">Type of the task.</param>
		/// <returns>TaskTypeDto.</returns>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        TaskTypeDto Create(TaskTypeDto taskTypeDto);

		/// <summary>
		/// Updates the specified task type identifier.
		/// </summary>
		/// <param name="taskTypeId">The task type identifier.</param>
		/// <param name="taskTypeDto">The task type dto.</param>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/{taskTypeId}", Method = "PUT", RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Update(string taskTypeId, TaskTypeDto taskTypeDto);

		/// <summary>
		/// Searches the specified search criteria dto.
		/// </summary>
		/// <param name="searchCriteriaDto">The search criteria dto.</param>
        [OperationContract]
		[FaultContract(typeof(InvalidOperationException))]
		[WebInvoke(UriTemplate = "/Search", Method = "POST", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		TaskTypeSearchModelDto Search(SearchCriteriaDto searchCriteriaDto);

		/// <summary>
		/// Gets the lookups.
		/// </summary>
		/// <returns>List&lt;LookupDto&gt;.</returns>
        [OperationContract]
		[FaultContract(typeof(InvalidOperationException))]
		[WebInvoke(UriTemplate = "/Lookup", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<LookupDto> GetLookups();

        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <returns></returns>
        [OperationContract(Name = "LookupEx")]
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/LookupEx/{includeDeleted}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<LookupDto> GetLookups(string includeDeleted);

		/// <summary>
		/// Fetches all.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		[FaultContract(typeof(InvalidOperationException))]
		[WebInvoke(UriTemplate = "/FetchAll", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		IEnumerable<TaskTypeDto> FetchAll();

        /// <summary>
        /// Validates the specified task type.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Validate", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<ValidationViolationDto> Validate(TaskTypeDto entity);
    }
}
