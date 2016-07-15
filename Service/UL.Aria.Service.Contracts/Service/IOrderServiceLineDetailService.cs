using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Interface IOrderServiceLineDetailService
    /// </summary>
    [ServiceContract]
    public interface IOrderServiceLineDetailService
    {
        /// <summary>
        ///     Creates the specified order service line detail.
        /// </summary>
        /// <param name="orderServiceLineDetail">The order service line detail.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Create", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Create(OrderServiceLineDetailDto orderServiceLineDetail);

        /// <summary>
        ///     Fetches all order service line details.
        /// </summary>
        /// <returns>IList{OrderServiceLineDetailDto}.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/List", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<OrderServiceLineDetailDto> FetchAll();

        /// <summary>
        ///     Fetches a order service line detail by the specified order identifier and line identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="lineId">The line identifier.</param>
        /// <returns>OrderServiceLineDetailDto.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{orderId}/{lineId}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        OrderServiceLineDetailDto Fetch(string orderId, string lineId);

        /// <summary>
        /// Searches for order service line details by the specified search criteria.
        /// Note: Keyword should be a valid order id, sorting is fixed and not supported via the SearchCrieriaDto
        /// and refiners are not supported.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>
        /// OrderServiceLineDetailSearchResultSetDto.
        /// </returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Search", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        OrderServiceLineDetailSearchResultSetDto Search(SearchCriteriaDto searchCriteria);

        /// <summary>
        /// Updates the specified order service line detail.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <param name="orderServiceLineDetail">The order service line detail.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Update/{id}", Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Update(string id, OrderServiceLineDetailDto orderServiceLineDetail);

        /// <summary>
        ///     Deletes the order service line detail by the specified order identifier and line identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="lineId">The line identifier.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{orderId}/{lineId}", Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Delete(string orderId, string lineId);
    }
}