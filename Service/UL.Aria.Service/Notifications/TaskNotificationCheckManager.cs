using System.Collections.Generic;
using System.Linq;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class TaskNotificationCheckManager.
	/// </summary>
	public class TaskNotificationCheckManager : ITaskNotificationCheckManager
	{
		private readonly IEnumerable<ITaskNotificationCheck> _notificationChecks;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderNotificationCheckManager"/> class.
		/// </summary>
		/// <param name="notificationChecks">The notification checks.</param>
		public TaskNotificationCheckManager(IEnumerable<ITaskNotificationCheck> notificationChecks)
		{
			_notificationChecks = notificationChecks;
		}

		/// <summary>
		/// Gets the task notifications.
		/// </summary>
		/// <param name="originalTask">The original task.</param>
		/// <param name="newTask">The new task.</param>
		/// <param name="notificationContext">The notification context.</param>
		/// <returns>List&lt;NotificationTypeDto&gt;.</returns>
		public List<NotificationTypeDto> GetTaskNotifications(Task originalTask, Task newTask,
			NotificationContext notificationContext)
		{
			var notifications = new List<NotificationTypeDto>();

			foreach (var notificationCheck in _notificationChecks.OrderBy(x => x.Ordinal))
			{
				//break if continue (return value from notification check) is false
				if (!notificationCheck.Check(originalTask, newTask, notifications, notificationContext))
					break;
			}

			return notifications;
		}
	}
}