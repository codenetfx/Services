using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Interface IIncomingOrderService
    /// </summary>
    [ServiceContract]
    public interface IIncomingOrderService
    {
        /// <summary>
        ///     Creates the specified new order.
        /// </summary>
        /// <param name="incomingOrder">The new order.</param>
        /// <returns>Guid.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/CreateIncomingOrder", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string Create(IncomingOrderDto incomingOrder);

        /// <summary>
        ///     Updates the specified incoming order id.
        /// </summary>
        /// <param name="incomingOrderId">The incoming order id.</param>
        /// <param name="incomingOrder">The incoming order.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateIncomingOrder/{incomingOrderId}", Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Update(string incomingOrderId, IncomingOrderDto incomingOrder);

        /// <summary>
        ///     Search based on the provided criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Search", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IncomingOrderSearchResultSetDto Search(SearchCriteriaDto searchCriteria);

        /// <summary>
        ///     Creates the specified new order.
        /// </summary>
        /// <param name="request">The new order.</param>
        /// <returns>Container Id.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/ProjectCreationRequest", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string PublishProjectCreationRequest(ProjectCreationRequestDto request);

        /// <summary>
        ///     Fetches the specified <see cref="IncomingOrderDto" />.
        /// </summary>
        /// <param name="idOrOrderNumber">The id or order number.</param>
        /// <returns>IncomingOrderDto.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{idOrOrderNumber}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedResponse)]
        IncomingOrderDto Fetch(string idOrOrderNumber);

        /// <summary>
        ///     Finds the project by service line.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/ServiceLine/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedResponse)]
        IncomingOrderDto FetchByServiceLine(string id);
    }
}