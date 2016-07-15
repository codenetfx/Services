using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class NotificationContext.
	/// </summary>
	public class NotificationContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NotificationContext"/> class.
		/// </summary>
		public NotificationContext()
		{
			TasksToNotifySuccessorStartDateChanged = new List<Task>();
		}

		/// <summary>
		/// Gets or sets the tasks to notify successor start date changed.
		/// </summary>
		/// <value>The tasks to notify successor start date changed.</value>
		public List<Task> TasksToNotifySuccessorStartDateChanged { get; set; }
	}
}