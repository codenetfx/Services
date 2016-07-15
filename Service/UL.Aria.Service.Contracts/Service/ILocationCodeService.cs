using System.ServiceModel;
using System.Collections.Generic;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Interface ILocationCodeService
    /// </summary>
    [ServiceContract]
    public interface ILocationCodeService
    {
        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{LocationCodeDto}.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/List", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<LocationCodeDto> FetchAll();

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>LocationCodeDto.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        LocationCodeDto Fetch(string id);

		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>LocationCodeDto.</returns>
	    [OperationContract]
		[WebInvoke(UriTemplate = "/ExternalId/{externalId}", Method = "GET", RequestFormat = WebMessageFormat.Json,
		    ResponseFormat = WebMessageFormat.Json)]
	    LocationCodeDto FetchByExternalId(string externalId);

        /// <summary>
        ///     Creates the specified Location code.
        /// </summary>
        /// <param name="locationCode">The location code.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Create(LocationCodeDto locationCode);

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="locationCode">The location code.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Update(string id, LocationCodeDto locationCode);

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Delete(string id);
    }
}