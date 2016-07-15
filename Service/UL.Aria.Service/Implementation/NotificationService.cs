using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Contracts.Dto;
using System.ServiceModel;
using UL.Aria.Common;
using UL.Aria.Common.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{

    /// <summary>
    /// Provides a Service Implemenation of the INotificationService interface.
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
       InstanceContextMode = InstanceContextMode.PerCall)]
    public class NotificationService : INotificationService
    {
        private const string NotificationMissMatchedExceptionMsg = "All Notifications must be owned by the same entity instnace in order to be bulk updated.";

        private readonly INotificationManager _notificationManager;
        private readonly IMapperRegistry _mapperRegistry;
       

        // private readonly IAuthorizationManager _authorizationManager;
        //  private readonly IPrincipalResolver _principalResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService" /> class.
        /// </summary>
        /// <param name="notificationManager">The notification manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public NotificationService(INotificationManager notificationManager, IMapperRegistry mapperRegistry)
        {
            _notificationManager = notificationManager;
            _mapperRegistry = mapperRegistry;
          
            // _authorizationManager = authorizationManager;
            // _principalResolver = principalResolver;
        }

        internal Guid AssureGuidId(string id, string parameterName)
        {
            Guard.IsNotNullOrEmptyTrimmed(id, parameterName);
            Guid guidID = id.ParseOrDefault<Guid>(Guid.Empty);
            Guard.IsNotEmptyGuid(guidID, parameterName);
            return guidID;
        }

        internal void AssureNotificationsOfEntityOwner(IEnumerable<NotificationDto> notifications, Guid entityOwnerId, string execptionMsg)
        {
            var AllOwnedByEntity = notifications.All(x => x.EntityId == entityOwnerId);
            if (!AllOwnedByEntity)
                throw new InvalidOperationException(execptionMsg);
        }



        /// <summary>
        /// Fetches the notification matching the specified id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public NotificationDto FetchById(string id)
        {
            var guidId = AssureGuidId(id, "id");
            var result = _notificationManager.FetchById(guidId);
            return _mapperRegistry.Map<NotificationDto>(result);
        }

        /// <summary>
        /// Fetches all active notifications associated with the specified userId.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public IEnumerable<NotificationDto> FetchNotificationsByUser(string userId)
        {
            var guidId = AssureGuidId(userId, "userId");
            var results = _notificationManager.FetchNotificationsByUser(guidId);
            return _mapperRegistry.Map<IEnumerable<NotificationDto>>(results);
        }


        /// <summary>
        /// Fetches all active notifications associated with the specified entityId.
        /// </summary>
        /// <param name="entityId">The owner entity identifier.</param>
        /// <returns></returns>
        public IEnumerable<NotificationDto> FetchNotificationsByEntity(string entityId)
        {
            var guidId = AssureGuidId(entityId, "entityId");
            var results = _notificationManager.FetchNotificationsByEntity(guidId);
            return _mapperRegistry.Map<IEnumerable<NotificationDto>>(results);
        }

        /// <summary>
        /// Deletes a notification with the specified notificationId.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        public void Delete(string notificationId)
        {
            var guidId = AssureGuidId(notificationId, "notificationId");
            _notificationManager.Delete(guidId);
        }
        
        /// <summary>
        /// Deletes the notifications associated with the specified entityId.
        /// </summary>
        /// <param name="entityId">The owner entity identifier.</param>
        public void DeleteNotificationsForEntity(string entityId)
        {
            var guidId = AssureGuidId(entityId, "entityId");
            _notificationManager.DeleteNotificationsForEntity(guidId);
        }

        /// <summary>
        /// Creates the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <returns></returns>
        public NotificationDto Create(NotificationDto notification)
        {
            Guard.IsNotNull(notification, "notification");
            var result = _notificationManager.Create(_mapperRegistry.Map<Notification>(notification));
            return _mapperRegistry.Map<NotificationDto>(result);
        }

        /// <summary>
        /// Updates the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="notificationId"></param>
        public void Update(NotificationDto notification, string notificationId)
        {
            var guidId = AssureGuidId(notificationId, "notificationId");
            Guard.IsNotNull(notification, "notifications");
            Guard.AreEqual(notification.Id.GetValueOrDefault(), guidId, "notificationId");
            _notificationManager.Update(_mapperRegistry.Map<Notification>(notification));
        }

        /// <summary>
        /// Updates the list of notifications in the bulk for the specified entityId.
        /// </summary>
        /// <param name="notifications">The notifications.</param>
        /// <param name="entityId">The owner entity identifier.</param>
        public void UpdateBulk(IEnumerable<NotificationDto> notifications, string entityId)
        {
            var guidId = AssureGuidId(entityId, "entityId");
            Guard.IsNotNull(notifications, "notifications");
            AssureNotificationsOfEntityOwner(notifications, guidId, NotificationMissMatchedExceptionMsg);
            _notificationManager.UpdateBulk(_mapperRegistry.Map<IEnumerable<Notification>>(notifications), guidId);
        }
    }
}