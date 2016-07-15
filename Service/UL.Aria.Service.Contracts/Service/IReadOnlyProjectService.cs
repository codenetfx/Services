using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Dto.Integration;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Defined operations for read only project services.
    /// </summary>
    [ServiceContract(Namespace = "http://portal.ul.com", Name = "ReadOnlyProjectService")]
    public interface IReadOnlyProjectService
    {
        /// <summary>
        /// Gets the project by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The matching project</returns>
        /// <exception cref="ArgumentException"><paramref name="id"/> was null, empty or not parseable as a <see cref="Guid"/>.</exception>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{id}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        [FaultContract(typeof(ArgumentException))]
        [FaultContract(typeof(ArgumentNullException))]
        [FaultContract(typeof(CommunicationException))]
        FulfillmentProject GetProjectById(string id);

        /// <summary>
        /// Searches the projects.
        /// </summary>
        /// <param name="fulfillmentProjectSearchCriteria">The fulfillment project search criteria.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><see cref="FulfillmentProjectSearchCriteria"/> argument not provided.</exception>
        /// <exception cref="ArgumentException"><see cref="FulfillmentProjectSearchCriteria.SearchValue"/> property was empty or null.</exception>
       
        [OperationContract]
        [WebInvoke(
            Method = "POST",
             UriTemplate = "/Search",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        [FaultContract(typeof(ArgumentException))]
        [FaultContract(typeof(ArgumentNullException))]
        [FaultContract(typeof(EndpointNotFoundException))]
        List<FulfillmentProject> SearchProjects(FulfillmentProjectSearchCriteria fulfillmentProjectSearchCriteria);

        /// <summary>
        /// Searches the projects.
        /// </summary>
        /// <param name="fulfillmentOrderProjectSearchRequest"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            Method = "POST",
             UriTemplate = "/SearchOrderProjects",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        [FaultContract(typeof(ArgumentException))]
        [FaultContract(typeof(ArgumentNullException))]
        [FaultContract(typeof(EndpointNotFoundException))]
        FulfillmentOrderProjectSearchResponse SearchOrderProjects(FulfillmentOrderProjectSearchRequest fulfillmentOrderProjectSearchRequest);

        /// <summary>
        /// Searches the fulfillment summaries.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            Method = "POST",
             UriTemplate = "/SearchFulfillmentSummaries",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        [FaultContract(typeof(ArgumentException))]
        [FaultContract(typeof(ArgumentNullException))]
        [FaultContract(typeof(EndpointNotFoundException))]
        OrderFulfillmentSummarySearchResponse SearchFulfillmentSummaries(OrderFulfillmentSummarySearchRequest orderFulfillmentSummarySearchRequest);


        /// <summary>
        /// Gets the documents linked to the document with the matching projectId.
        /// </summary>
        /// <param name="projectId">The identifier.</param>
        /// <returns>
        /// The List of Project Documents
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="projectId"/> was null, empty or not parseable as a <see cref="Guid"/>.</exception>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{projectId}/Documents",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        [FaultContract(typeof(ArgumentException))]
        [FaultContract(typeof(ArgumentNullException))]
        [FaultContract(typeof(EndpointNotFoundException))]
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
		[FaultContract(typeof(ArgumentException))]
		[FaultContract(typeof(ArgumentNullException))]
		[FaultContract(typeof(EndpointNotFoundException))]
		bool Ping();
    }
}