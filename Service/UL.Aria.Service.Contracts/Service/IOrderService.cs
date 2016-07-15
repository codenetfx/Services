using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Service interface contract for Order entities.
    /// </summary>
    [ServiceContract]
    public interface IOrderService
    {
        /// <summary>
        ///     Gets the order by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Order Dto</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IncomingOrderDto FetchById(string id);

        /// <summary>
        ///     Gets the order by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>Order.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/OrderNumber/{orderNumber}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IncomingOrderDto FetchByOrderNumber(string orderNumber);

		/// <summary>
		/// Creates the order.
		/// </summary>
		/// <param name="orderXml">The order.</param>
		/// <returns>The created order</returns>
        [OperationContract]
		[WebInvoke(UriTemplate = "", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string Create(string orderXml);

		/// <summary>
		/// Updates the order.
		/// </summary>
		/// <param name="orderXml">The order.</param>
        [OperationContract]
		[WebInvoke(UriTemplate = "", Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
		void Update(string orderXml);

        /// <summary>
        ///     Deletes the order by id.
        /// </summary>
        /// <param name="id">The id.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Delete(string id);
    }
}