using System;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class TaskReminderStrategy.
	/// </summary>
	[NotificationType(NotificationTypeDto.TaskReminder)]
	public sealed class TaskReminderStrategy : TaskNotificationStrategyBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TaskReminderStrategy" /> class.
		/// </summary>
		/// <param name="notificationManager">The notification manager.</param>
		/// <param name="profileManager">The profile manager.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="projectManager">The project manager.</param>
		/// <param name="containerManager">The container manager.</param>
		public TaskReminderStrategy(INotificationManager notificationManager,
			IProfileManager profileManager, IPrincipalResolver principalResolver, IProjectManager projectManager,
			IContainerManager containerManager)
			: base(notificationManager, profileManager, principalResolver, projectManager, containerManager)
		{
		}

		/// <summary>
		/// Gets the start date.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>System.Nullable&lt;DateTime&gt;.</returns>
		protected override DateTime? GetStartDate(Task entity)
		{
			return (entity.ReminderDate != null) ? entity.ReminderDate.Value.Date : entity.ReminderDate;
		}
	}
}