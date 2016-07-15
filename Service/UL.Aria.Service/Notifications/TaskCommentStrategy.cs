using System;
using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class TaskCommentStrategy.
	/// </summary>
	[NotificationType(NotificationTypeDto.TaskComment)]
	public sealed class TaskCommentStrategy : TaskNotificationStrategyBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TaskCommentStrategy" /> class.
		/// </summary>
		/// <param name="notificationManager">The notification manager.</param>
		/// <param name="profileManager">The profile manager.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="projectManager">The project manager.</param>
		/// <param name="containerManager">The container manager.</param>
		public TaskCommentStrategy(INotificationManager notificationManager,
			IProfileManager profileManager, IPrincipalResolver principalResolver, IProjectManager projectManager,
			IContainerManager containerManager)
			: base(notificationManager, profileManager, principalResolver, projectManager, containerManager)
		{
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
                if (_currentUserId != taskOwner.Id)
                    users.Add(taskOwner.Id.GetValueOrDefault());
            }

            if (!string.IsNullOrWhiteSpace(entity.Project.ProjectHandler)
                && !string.Equals(entity.Project.ProjectHandler, entity.TaskOwner, StringComparison.InvariantCultureIgnoreCase))
            {
                var projectHandler = _profileManager.FetchByUserName(entity.Project.ProjectHandler);
                if (_currentUserId != projectHandler.Id)
                    users.Add(projectHandler.Id.GetValueOrDefault());
            }

            return users;
		}
	}
}