using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class TaskDueDateNotificationCheck. This class cannot be inherited.
	/// </summary>
	public sealed class TaskDueDateNotificationCheck : ITaskNotificationCheck
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
			if (newTask.DueDate != null)
			{
				notifications.Add(NotificationTypeDto.TaskDueDate);
			}

			return true;
		}

		/// <summary>
		/// Gets the ordinal.
		/// </summary>
		/// <value>The ordinal.</value>
		public int Ordinal
		{
			get { return 200; }
		}
	}
}