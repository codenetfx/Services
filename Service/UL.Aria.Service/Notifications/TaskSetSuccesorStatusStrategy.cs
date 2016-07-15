using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Authorization;

using Ntype = UL.Aria.Service.Contracts.Dto.NotificationTypeDto;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class TaskSetSuccessorStatusStrategy. This class cannot be inherited.
	/// </summary>
	[NotificationType(Ntype.TaskSuccessorStatusSet)]
	public sealed class TaskSetSuccessorStatusStrategy : TaskNotificationStrategyBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TaskNotificationStrategyBase" /> class.
		/// </summary>
		/// <param name="notificationManager">The notification manager.</param>
		/// <param name="profileManager">The profile manager.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="projectManager">The project manager.</param>
		/// <param name="containerManager">The container manager.</param>
		public TaskSetSuccessorStatusStrategy(INotificationManager notificationManager,
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
			return entity.StartDate;
		}

		/// <summary>
		/// Gets the user list.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>IList&lt;Guid&gt;.</returns>
		protected internal override IList<Guid> GetUserList(Task entity)
		{
			var users = new List<Guid>();
			if (!string.IsNullOrWhiteSpace(entity.TaskOwner))
			{
				var taskOwner = _profileManager.FetchByUserName(entity.TaskOwner);
				if (taskOwner != null)
					users.Add(taskOwner.Id.GetValueOrDefault());
			}

			var project = GetProject(entity.ContainerId.GetValueOrDefault());
			var projectHandler = _profileManager.FetchByUserName(project.ProjectHandler);

			if (projectHandler != null)
				users.Add(projectHandler.Id.GetValueOrDefault());

			return users;
		}
	}
}