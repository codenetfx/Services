using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Framework;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Interface ITaskNotificationCheck
	/// </summary>
	public interface ITaskNotificationCheck : IOrderable
	{
		/// <summary>
		/// Checks the specified original task.
		/// </summary>
		/// <param name="originalTask">The original task.</param>
		/// <param name="newTask">The new task.</param>
		/// <param name="notifications">The notifications.</param>
		/// <param name="notificationContext">The notification context.</param>
		/// <returns><c>true</c> if continue, <c>false</c> otherwise.</returns>
		bool Check(Task originalTask, Task newTask, List<NotificationTypeDto> notifications,
			NotificationContext notificationContext);
	}
}