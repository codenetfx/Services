using System.Collections.Generic;
using System.Linq;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class TaskSuccessorStartDateChangedNotificationCheck. This class cannot be inherited.
	/// </summary>
	public sealed class TaskSuccessorStartDateChangedNotificationCheck : ITaskNotificationCheck
	{
		/// <summary>
		/// Gets the ordinal.
		/// </summary>
		/// <value>The ordinal.</value>
		public int Ordinal
		{
			get { return 600; }
		}

		/// <summary>
		/// Checks the specified original task.
		/// </summary>
		/// <param name="originalTask">The original task.</param>
		/// <param name="newTask">The new task.</param>
		/// <param name="notifications">The notifications.</param>
		/// <param name="notificationContext">The notification context.</param>
		/// <returns><c>true</c> if continue, <c>false</c> otherwise.</returns>
		public bool Check(Task originalTask, Task newTask, List<NotificationTypeDto> notifications, NotificationContext notificationContext)
		{
            var hasStartDateChange = ((originalTask != null && originalTask.StartDate != newTask.StartDate)
                  || (newTask.StartDate.HasValue && originalTask == null)); 

            var hasPredecessorsThatAreAllClosed = (newTask.PredecessorRefs.Any() && newTask.PredecessorRefs.All(x => x.IsClosed));
            
            if (hasStartDateChange && hasPredecessorsThatAreAllClosed)
            {
                notifications.Add(NotificationTypeDto.TaskSuccessorStatusSet);
            }

			return true;
		}
	}
}