using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Dto.Integration;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Defines service contract for <see cref="ProjectDto"/> relays.
    /// </summary>
    [ServiceContract(Namespace = "http://portal.ul.com")]
    public interface IRelayProjectService
    {
        /// <summary>
        ///     Gets the product by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        FulfillmentProject GetProjectById(string id);

        /// <summary>
        /// Searches the projects.
        /// </summary>
        /// <param name="fulfillmentProjectSearchRequest">The search criteria.</param>
        /// <param name="includeDetail"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/Search",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        FulfillmentOrderProjectSearchResponse SearchProjects(FulfillmentOrderProjectSearchRequest fulfillmentProjectSearchRequest, bool includeDetail);

        /// <summary>
        /// Gets the project documents.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{projectId}/Documents",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<FulfillmentDocumentMetaData> GetAllProjectDocuments(string projectId);

		/// <summary>
		/// Pings this instance.
		/// </summary>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		[OperationContract]
		[WebInvoke(
			UriTemplate = "/Ping",
			Method = "POST",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		bool Ping();
	}
}