using System.ServiceModel;
using System.Collections.Generic;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Interface IIndustryCodeService
    /// </summary>
    [ServiceContract]
    public interface IIndustryCodeService
    {
        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{IndustryCodeDto}.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/List", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<IndustryCodeDto> FetchAll();

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>IndustryCodeDto.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IndustryCodeDto Fetch(string id);

		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>IndustryCodeDto.</returns>
	    [OperationContract]
		[WebInvoke(UriTemplate = "/ExternalId/{externalId}", Method = "GET", RequestFormat = WebMessageFormat.Json,
		    ResponseFormat = WebMessageFormat.Json)]
	    IndustryCodeDto FetchByExternalId(string externalId);

        /// <summary>
        ///     Creates the specified industry code.
        /// </summary>
        /// <param name="industryCode">The industry code.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Create(IndustryCodeDto industryCode);

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="industryCode">The industry code.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Update(string id, IndustryCodeDto industryCode);

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