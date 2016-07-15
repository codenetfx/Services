using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
	/// <summary>
	///     Interface IDepartmentCodeService
	/// </summary>
	[ServiceContract]
	public interface IDepartmentCodeService
	{
		/// <summary>
		///     Fetches all.
		/// </summary>
		/// <returns>IList{DepartmentCodeDto}.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/List", Method = "GET", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		IList<DepartmentCodeDto> FetchAll();

		/// <summary>
		///     Fetches the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>ServiceCodeDto.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		DepartmentCodeDto Fetch(string id);

		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>DepartmentCodeDto.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/ExternalId/{externalId}", Method = "GET", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		DepartmentCodeDto FetchByExternalId(string externalId);

		/// <summary>
		///     Creates the specified Department code.
		/// </summary>
		/// <param name="departmentCode">The Department code.</param>
		[OperationContract]
		[WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		void Create(DepartmentCodeDto departmentCode);

		/// <summary>
		///     Updates the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="departmentCode">The Department code.</param>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}", Method = "PUT", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		void Update(string id, DepartmentCodeDto departmentCode);

		/// <summary>
		///     Deletes the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		void Delete(string id);

		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/Search", Method = "POST",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		LookupCodeSearchResultSetDto Search(SearchCriteriaDto searchCriteria);
	}
}