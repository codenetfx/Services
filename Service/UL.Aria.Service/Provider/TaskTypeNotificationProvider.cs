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
    /// Task Type Notification Provider implementation
    /// </summary>
    public class TaskTypeNotificationProvider : ITaskTypeNotificationProvider
    {
        private readonly ITaskTypeNotificationRepository _taskTypeNotificationRepository;
        private readonly IPrincipalResolver _principalResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTypeNotificationProvider" /> class.
        /// </summary>
        /// <param name="taskTypeNotificationRepository">The task type notification repository.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public TaskTypeNotificationProvider(ITaskTypeNotificationRepository taskTypeNotificationRepository, IPrincipalResolver principalResolver)
        {
            _taskTypeNotificationRepository = taskTypeNotificationRepository;
            _principalResolver = principalResolver;
        }

        /// <summary>
        /// Fetches the task type notification by identifier.
        /// </summary>
        /// <param name="taskTypeNotificationId">The task type notification identifier.</param>
        /// <returns></returns>
        public TaskTypeNotification FetchById(Guid taskTypeNotificationId)
        {
            return _taskTypeNotificationRepository.Fetch(taskTypeNotificationId);
        }

        /// <summary>
        /// Fetches task type notifications by task type id.
        /// </summary>
        /// <param name="taskTypeId">The task type identifier.</param>
        /// <returns></returns>
        public IEnumerable<TaskTypeNotification> FetchByTaskTypeId(Guid taskTypeId)
        {
            return _taskTypeNotificationRepository.FindByTaskTypeId(taskTypeId);
        }

        /// <summary>
        /// Deletes the specified task type notification by identifier.
        /// </summary>
        /// <param name="taskTypeNotificationId">The task type notification identifier.</param>
        public void Delete(Guid taskTypeNotificationId)
        {
            _taskTypeNotificationRepository.Delete(taskTypeNotificationId);
        }

        /// <summary>
        /// Saves the specified task type notifications.
        /// </summary>
        /// <param name="taskTypeNotifications">The task type notifications.</param>
        /// <param name="taskTypeId">The task type identifier.</param>
        public void Save(IEnumerable<TaskTypeNotification> taskTypeNotifications, Guid taskTypeId)
        {
            // Remove duplicate emails.
            taskTypeNotifications = taskTypeNotifications.GroupBy(ttn => ttn.Email.ToLower()).Select(grp => grp.First()).ToList();

            SetupTaskTypeNotification(_principalResolver, taskTypeNotifications, taskTypeId);
            _taskTypeNotificationRepository.Save(taskTypeNotifications, taskTypeId);
        }


		internal static void SetupTaskTypeNotification(IPrincipalResolver principalResolver, 
            IEnumerable<TaskTypeNotification> taskTypeNotifications, Guid taskTypeId)
		{
		    taskTypeNotifications.ForEach(taskTypeNotification =>
		    {
                if (!taskTypeNotification.Id.HasValue)
                {
                    taskTypeNotification.Id = Guid.NewGuid();
                }

		        taskTypeNotification.TaskTypeId = taskTypeId;

                var now = DateTime.UtcNow;
                taskTypeNotification.CreatedById = principalResolver.UserId;
                taskTypeNotification.CreatedDateTime = now;
                taskTypeNotification.UpdatedById = principalResolver.UserId;
                taskTypeNotification.UpdatedDateTime = now;
		    });
		}
    }
}
