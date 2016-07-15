using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Defines service operations for publshing messages.
    /// </summary>
    [ServiceContract]
    public interface IMessageService
    {
        /// <summary>
        /// Publishes the specified unique identifier.
        /// </summary>
        /// <param name="message">The message.</param>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/Project/Status",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void PublishProjectStatusMessage(ProjectStatusMessageDto message);

        /// <summary>
        ///     Pings the specified message.
        /// </summary>
        /// <returns>System.String.</returns>
        [OperationContract(Action = "ping", ReplyAction = "pingresponse")]
        [WebInvoke(UriTemplate = "/Ping", Method = "GET",
            BodyStyle=  WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json)]
        [return : MessageParameter(Name = "pingreply")]
        string Ping();
    }
}