﻿using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class TaskCommentNotificationCheck. This class cannot be inherited.
	/// </summary>
	public sealed class TaskCommentNotificationCheck : ITaskNotificationCheck
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
			if (((originalTask != null && (originalTask.Comments.Count < newTask.Comments.Count
			                               ||
			                               (!string.IsNullOrWhiteSpace(newTask.Comment) &&
			                                newTask.Comment != originalTask.Comment)))
			     || (originalTask == null || newTask.Comments.Count > 0 || (!string.IsNullOrWhiteSpace(newTask.Comment)))))
			{
				notifications.Add(NotificationTypeDto.TaskComment);
			}

			return true;
		}

		/// <summary>
		/// Gets the ordinal.
		/// </summary>
		/// <value>The ordinal.</value>
		public int Ordinal
		{
			get { return 400; }
		}
	}
}