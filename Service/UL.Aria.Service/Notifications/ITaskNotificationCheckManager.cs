using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Interface ITaskNotificationCheckManager
	/// </summary>
	public interface ITaskNotificationCheckManager
	{
		/// <summary>
		/// Gets the task notifications.
		/// </summary>
		/// <param name="originalTask">The original task.</param>
		/// <param name="newTask">The new task.</param>
		/// <param name="notificationContext">The notification context.</param>
		/// <returns>List&lt;NotificationTypeDto&gt;.</returns>
		List<NotificationTypeDto> GetTaskNotifications(Task originalTask, Task newTask,
			NotificationContext notificationContext);
	}
}