using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Service contract defining queueing operations for Order Messages
    /// </summary>
    [ServiceContract]
    public interface IOrderMessageService
    {
        /// <summary>
        ///     Enqueues the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/enqueue", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Enqueue(OrderMessageDto message);

        /// <summary>
        ///     Dequeues the top message.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/dequeue", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        OrderMessageDto Dequeue();

        /// <summary>
        ///     Pings the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/ping/{message}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string Ping(string message);
    }
}