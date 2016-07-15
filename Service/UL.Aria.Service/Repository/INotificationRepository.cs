using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Provides an interface for a Notification Repository.
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// Finds all notifications in the database.
        /// </summary>
        /// <returns></returns>
        IList<Notification> FindAll();

        /// <summary>
        /// Fetches the notification matching the specified id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Notification FindById(Guid id);

        /// <summary>
        /// Adds the specified notification.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(Notification entity);

        /// <summary>
        /// Updates the specified notification.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        int Update(Notification entity);

        /// <summary>
        /// Removes a notification with the specified notificationId from the database.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        void Remove(Guid entityId);

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
        /// Removes the notifications for the specified entityId from the database.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        void RemoveNotificationsForEntity(Guid entityId);

        /// <summary>
        /// Updates the list of notifications in the bulk for the specified entityId.
        /// </summary>
        /// <param name="notifications">The notifications.</param>
        /// <param name="entityId">The entity identifier.</param>
        void UpdateBulk(IEnumerable<Notification> notifications, Guid entityId);
        
        /// <summary>
        /// Deletes the specified list of notifications.
        /// </summary>
        /// <param name="entities">The entities.</param>
        int Delete(IEnumerable<Notification> entities);
    }
}
