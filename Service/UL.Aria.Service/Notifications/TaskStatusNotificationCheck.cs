using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class TaskStatusNotificationCheck. This class cannot be inherited.
	/// </summary>
	public sealed class TaskStatusNotificationCheck : ITaskNotificationCheck
	{
		/// <summary>
		/// Checks the specified original task.
		/// </summary>
		/// <param name="originalTask">The original task.</param>
		/// <param name="newTask">The new task.</param>
		/// <param name="notifications">The notifications.</param>
		/// <param name="notificationContext">The notification context.</param>
		/// <returns><c>true</c> if continue, <c>false</c> otherwise.</returns>
		public bool Check(Task originalTask, Task newTask, List<NotificationTypeDto> notifications,
			NotificationContext notificationContext)
		{
			var originalStatus = (originalTask != null) ? originalTask.Status : TaskStatusEnumDto.NotScheduled;
			var newStatus = newTask.Status;
			var taskOkForNotification = originalStatus != TaskStatusEnumDto.Completed
			                            && originalStatus != TaskStatusEnumDto.Canceled
			                            && newStatus != TaskStatusEnumDto.Completed
			                            && newStatus != TaskStatusEnumDto.Canceled;
			if (!taskOkForNotification)
			{
				notifications.Add(NotificationTypeDto.EntityCleanup);
			}
			return taskOkForNotification;
		}

		/// <summary>
		/// Gets the ordinal.
		/// </summary>
		/// <value>The ordinal.</value>
		public int Ordinal
		{
			get { return 100; }
		}
	}
}