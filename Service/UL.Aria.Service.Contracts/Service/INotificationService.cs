using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Provides a Service interface for manipulating Notifications objects and records.
    /// </summary>
    [ServiceContract]
    public interface INotificationService
    {

        /// <summary>
        /// Fetches the notification matching the specified id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        NotificationDto FetchById(string id);

        /// <summary>
        /// Fetches all active notifications associated with the specified userId.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/User/{userId}", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<NotificationDto> FetchNotificationsByUser(string userId);

        /// <summary>
        /// Fetches all active notifications associated with the specified entityId.
        /// </summary>
        /// <param name="entityId">The owner entity identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Entity/{entityId}", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<NotificationDto> FetchNotificationsByEntity(string entityId);

        /// <summary>
        /// Deletes a notification with the specified notificationId.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/{notificationId}", Method = "DELETE", RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Delete(string notificationId);


        /// <summary>
        /// Deletes the notifications associated with the specified entityId.
        /// </summary>
        /// <param name="entityId">The owner entity identifier.</param>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/Entity/{entityId}", Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void DeleteNotificationsForEntity(string entityId);


        /// <summary>
        /// Creates the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        NotificationDto Create(NotificationDto notification);

        /// <summary>
        /// Updates the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="notificationId">The notification identifier.</param>
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/{notificationId}", Method = "PUT", RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Update(NotificationDto notification, string notificationId);


        /// <summary>
        /// Updates the list of notifications in the bulk for the specified entityId.
        /// </summary>
        /// <param name="notifications">The notifications.</param>
        /// <param name="entityId">The owner entity identifier.</param>
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/Entity/{entityId}", Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void UpdateBulk(IEnumerable<NotificationDto> notifications, string entityId);

    }
}
