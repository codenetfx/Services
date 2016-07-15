using System;
using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class TaskNotificationStrategyBase : NotificationStrategyBase<Task>
	{
		private readonly IProjectManager _projectManager;
		private readonly IContainerManager _containerManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskNotificationStrategyBase" /> class.
		/// </summary>
		/// <param name="notificationManager">The notification manager.</param>
		/// <param name="profileManager">The profile manager.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="projectManager">The project manager.</param>
		/// <param name="containerManager">The container manager.</param>
		protected TaskNotificationStrategyBase(INotificationManager notificationManager,
			IProfileManager profileManager, IPrincipalResolver principalResolver, IProjectManager projectManager,
			IContainerManager containerManager)
			: base(notificationManager, profileManager, principalResolver)
		{
			_projectManager = projectManager;
			_containerManager = containerManager;
		}

		/// <summary>
		/// Gets the project.
		/// </summary>
		/// <param name="containerId"></param>
		/// <returns></returns>
		protected Project GetProject(Guid containerId)
		{
			var container = _containerManager.FindById(containerId);
			return _projectManager.GetProjectById(container.PrimarySearchEntityId);
		}

		/// <summary>
		/// Gets the notification body.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>System.String.</returns>
		protected internal override string GetNotificationBody(Task entity)
		{
			return GetNotificationBody(entity.Title);
		}

		/// <summary>
		/// Gets the container identifier.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>System.Nullable&lt;Guid&gt;.</returns>
		protected internal override Guid? GetContainerId(Task entity)
		{
			return entity.ContainerId;
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

			return users;
		}

		/// <summary>
		/// Gets the type of the entity.
		/// </summary>
		/// <value>The type of the entity.</value>
		protected internal override EntityTypeEnumDto EntityType
		{
			get { return EntityTypeEnumDto.Task; }
		}
	}
}