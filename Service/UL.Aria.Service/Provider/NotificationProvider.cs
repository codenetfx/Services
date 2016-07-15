using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;


namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provides an implemenation of the Notificaiton provider interface.
    /// </summary>
    public class NotificationProvider:INotificationProvider
    {
        private readonly INotificationRepository _notificationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationProvider"/> class.
        /// </summary>
        /// <param name="notificationRepository">The notification repository.</param>
        public NotificationProvider(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        /// <summary>
        /// Fetches the notification matching the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Notification FetchById(Guid id)
        {
            return _notificationRepository.FindById(id);
        }

        /// <summary>
        /// Fetches all active notifications associated with the specified userId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<Notification> FetchNotificationsByUser(Guid userId)
        {
            return _notificationRepository.FetchNotificationsByUser(userId);
        }



        /// <summary>
        /// Fetches the notifications by entity.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns></returns>
        public IEnumerable<Notification> FetchNotificationsByEntity(Guid entityId)
        {
            return _notificationRepository.FetchNotificationsByEntity(entityId);
        }

        /// <summary>
        /// Deletes a notification with the specified notificationId.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(Guid id)
        {
            _notificationRepository.Remove(id);
        }


        /// <summary>
        /// Deletes the notifications for the specified entityId.
        /// </summary>
        /// <param name="entityId"></param>
        public void DeleteNotificationsForEntity(Guid entityId)
        {
            _notificationRepository.RemoveNotificationsForEntity(entityId);
        }

        /// <summary>
        /// Creates the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Notification Create(Notification notification)
        {
            if (!notification.Id.HasValue)
                notification.Id = Guid.NewGuid();
             _notificationRepository.Add(notification);
             return notification;
        }

        /// <summary>
        /// Updates the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Update(Notification notification)
        {
            _notificationRepository.Update(notification);
        }


        /// <summary>
        /// Updates the list of notifications in the bulk for the specified entityId.
        /// </summary>
        /// <param name="notifications">The notifications.</param>
        /// <param name="entityId">The entity identifier.</param>
        public void UpdateBulk(IEnumerable<Notification> notifications, Guid entityId)
        {
            _notificationRepository.UpdateBulk(notifications, entityId);
        }


        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(IEnumerable<Notification> entities)
        {
            _notificationRepository.Delete(entities);
        }
    }
}
