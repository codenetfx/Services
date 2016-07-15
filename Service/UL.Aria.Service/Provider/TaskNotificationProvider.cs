using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Task Notification Provider implementation
    /// </summary>
    public class TaskNotificationProvider : ITaskNotificationProvider
    {
        private readonly ITaskNotificationRepository _taskNotificationRepository;
        private readonly IPrincipalResolver _principalResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotificationProvider" /> class.
        /// </summary>
        /// <param name="taskNotificationRepository">The task notification repository.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public TaskNotificationProvider(ITaskNotificationRepository taskNotificationRepository, IPrincipalResolver principalResolver)
        {
            _taskNotificationRepository = taskNotificationRepository;
            _principalResolver = principalResolver;
        }

        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="taskNotificationId">The task notification identifier.</param>
        /// <returns></returns>
        public TaskNotification FetchById(Guid taskNotificationId)
        {
            return _taskNotificationRepository.Fetch(taskNotificationId);
        }

        /// <summary>
        /// Fetches task notifications by task id.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        public IEnumerable<TaskNotification> FetchByTaskId(Guid taskId)
        {
            return _taskNotificationRepository.FindByTaskId(taskId);
        }

        /// <summary>
        /// Deletes the specified task notification identifier.
        /// </summary>
        /// <param name="taskNotificationId">The task notification identifier.</param>
        public void Delete(Guid taskNotificationId)
        {
            _taskNotificationRepository.Delete(taskNotificationId);
        }

        /// <summary>
        /// Saves the specified task notifications.
        /// </summary>
        /// <param name="taskNotifications">The task notifications.</param>
        /// <param name="taskId">The task identifier.</param>
        public void Save(IEnumerable<TaskNotification> taskNotifications, Guid taskId)
        {
            // Remove duplicate emails.
            taskNotifications = taskNotifications.GroupBy(tn => tn.Email.ToLower()).Select(grp => grp.First()).ToList();

            SetupTaskNotification(_principalResolver, taskNotifications, taskId);
            _taskNotificationRepository.Save(taskNotifications, taskId);
        }


		internal static void SetupTaskNotification(IPrincipalResolver principalResolver, IEnumerable<TaskNotification> taskNotifications, Guid taskId)
		{
		    taskNotifications.ForEach(taskNotification =>
		    {
                if (!taskNotification.Id.HasValue)
                {
                    taskNotification.Id = Guid.NewGuid();
                }

		        taskNotification.TaskId = taskId;

                var now = DateTime.UtcNow;
                taskNotification.CreatedById = principalResolver.UserId;
                taskNotification.CreatedDateTime = now;
                taskNotification.UpdatedById = principalResolver.UserId;
                taskNotification.UpdatedDateTime = now;
		    });
		}
    }
}
