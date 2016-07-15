using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
	/// <summary>
	/// 
	/// </summary>
	[ServiceContract]
	public interface ITaskCategoryService
	{

		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/Search", Method = "POST", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		TaskCategorySearchModelDto Search(SearchCriteriaDto searchCriteria);

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		TaskCategoryDto FetchById(string id);

		/// <summary>
		/// Creates the specified task template.
		/// </summary>
		/// <param name="taskTemplate">The task template.</param>
		/// <returns></returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		TaskCategoryDto Create(TaskCategoryDto taskTemplate);


		/// <summary>
		/// Updates the task template.
		/// </summary>
		/// <param name="taskCategoryId"></param>
		/// <param name="taskCategory"></param>
		/// <returns></returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{taskCategoryId}", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		TaskCategoryDto Update(string taskCategoryId, TaskCategoryDto taskCategory);


		/// <summary>
		/// Deletes the task template by id.
		/// </summary>
		/// <param name="id">The id.</param>
		[OperationContract]
		[FaultContract(typeof(InvalidOperationException))]
		[WebInvoke(UriTemplate = "/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		void Delete(string id);

		/// <summary>
		/// Fetches all count.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/Count", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		int FetchAllCount();

	}
}
