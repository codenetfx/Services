using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provides an interface for a Notification provider.
    /// </summary>
    public interface INotificationProvider
    {
        /// <summary>
        /// Fetches the notification matching the specified id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Notification FetchById(Guid id);

        /// <summary>
        /// Fetches all active notifications associated with the specified userId.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        IEnumerable<Notification> FetchNotificationsByUser(Guid userId);

        /// <summary>
        /// Fetches all active notifications associated with the specified entityId.
        /// </summary>
        /// <param name="entiyId">The entiy identifier.</param>
        /// <returns></returns>
        IEnumerable<Notification> FetchNotificationsByEntity(Guid entiyId);

        /// <summary>
        /// Deletes a notification with the specified notificationId.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(Guid id);

        /// <summary>
        /// Deletes the notifications for the specified entityId.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        void DeleteNotificationsForEntity(Guid entityId);

        /// <summary>
        /// Creates the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <returns></returns>
        Notification Create(Notification notification);

        /// <summary>
        /// Updates the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        void Update(Notification notification);

        /// <summary>
        /// Updates the list of notifications in the bulk for the specified entityId.
        /// </summary>
        /// <param name="notifications">The notifications.</param>
        /// <param name="entityId">The entity identifier.</param>
        void UpdateBulk(IEnumerable<Notification> notifications, Guid entityId);

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Delete(IEnumerable<Notification> entities);
    }
}
