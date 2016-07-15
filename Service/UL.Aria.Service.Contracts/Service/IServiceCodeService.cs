using System.ServiceModel;
using System.Collections.Generic;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Interface IServiceCodeService
    /// </summary>
    [ServiceContract]
    public interface IServiceCodeService
    {
        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{ServiceCodeDto}.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/List", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<ServiceCodeDto> FetchAll();

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ServiceCodeDto.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ServiceCodeDto Fetch(string id);

		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>ServiceCodeDto.</returns>
	    [OperationContract]
		[WebInvoke(UriTemplate = "/ExternalId/{externalId}", Method = "GET", RequestFormat = WebMessageFormat.Json,
		    ResponseFormat = WebMessageFormat.Json)]
	    ServiceCodeDto FetchByExternalId(string externalId);

        /// <summary>
        ///     Creates the specified Service code.
        /// </summary>
        /// <param name="serviceCode">The Service code.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Create(ServiceCodeDto serviceCode);

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="serviceCode">The Service code.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Update(string id, ServiceCodeDto serviceCode);

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
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