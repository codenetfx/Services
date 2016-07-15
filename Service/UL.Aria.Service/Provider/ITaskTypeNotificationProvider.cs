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
    /// Provides an interface for a Task Type Notification Provider.
    /// </summary>
    public interface ITaskTypeNotificationProvider
    {
        /// <summary>
        /// Fetches the task type notification by identifier.
        /// </summary>
        /// <param name="taskTypeNotificationId">The task type notification identifier.</param>
        /// <returns></returns>
        TaskTypeNotification FetchById(Guid taskTypeNotificationId);

        /// <summary>
        /// Fetches task type notifications by task type identifier.
        /// </summary>
        /// <param name="taskTypeId">The task type identifier.</param>
        /// <returns></returns>
        IEnumerable<TaskTypeNotification> FetchByTaskTypeId(Guid taskTypeId);

        /// <summary>
        /// Deletes the specified task type notification by identifier.
        /// </summary>
        /// <param name="taskTypeNotificationId">The task type notification identifier.</param>
        void Delete(Guid taskTypeNotificationId);

        /// <summary>
        /// Saves the specified task type notifications.
        /// </summary>
        /// <param name="taskTypeNotifications">The task type notifications.</param>
        /// <param name="taskTypeId">The task type identifier.</param>
        void Save(IEnumerable<TaskTypeNotification> taskTypeNotifications, Guid taskTypeId);
    }
}
