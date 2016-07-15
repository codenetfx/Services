using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provides an interface for a Task Notification Provider.
    /// </summary>
    public interface ITaskNotificationProvider
    {
        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="taskNotificationId">The task notification identifier.</param>
        /// <returns></returns>
        TaskNotification FetchById(Guid taskNotificationId);

        /// <summary>
        /// Fetches the by task identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        IEnumerable<TaskNotification> FetchByTaskId(Guid taskId);

        /// <summary>
        /// Deletes the specified task notification identifier.
        /// </summary>
        /// <param name="taskNotificationId">The task notification identifier.</param>
        void Delete(Guid taskNotificationId);

        /// <summary>
        /// Saves the specified task notifications.
        /// </summary>
        /// <param name="taskNotifications">The task notifications.</param>
        /// <param name="taskId">The task identifier.</param>
        void Save(IEnumerable<TaskNotification> taskNotifications, Guid taskId);
    }
}
