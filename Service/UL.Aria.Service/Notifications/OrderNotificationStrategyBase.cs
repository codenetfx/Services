using System;
using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class OrderNotificationStrategyBase.
	/// </summary>
	public abstract class OrderNotificationStrategyBase : NotificationStrategyBase<Project>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OrderNotificationStrategyBase" /> class.
		/// </summary>
		/// <param name="notificationManager">The notification manager.</param>
		/// <param name="profileManager">The profile manager.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		protected OrderNotificationStrategyBase(INotificationManager notificationManager,
			IProfileManager profileManager, IPrincipalResolver principalResolver)
			: base(notificationManager, profileManager, principalResolver)
		{
		}

		/// <summary>
		/// Gets the user list.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>IList&lt;Guid&gt;.</returns>
		protected internal override IList<Guid> GetUserList(Project entity)
		{
			var users = new List<Guid>();
			if (!string.IsNullOrWhiteSpace(entity.ProjectHandler))
			{
				var projectHandler = _profileManager.FetchByUserName(entity.ProjectHandler);
				users.Add(projectHandler.Id.GetValueOrDefault());
			}

			return users;
		}

		/// <summary>
		/// Gets the notification body.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>System.String.</returns>
		protected internal override string GetNotificationBody(Project entity)
		{
			return GetNotificationBody(entity.Name);
		}

		/// <summary>
		/// Gets the container identifier.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>System.Nullable&lt;Guid&gt;.</returns>
		protected internal override Guid? GetContainerId(Project entity)
		{
			return entity.ContainerId;
		}

		/// <summary>
		/// Gets the type of the entity.
		/// </summary>
		/// <value>The type of the entity.</value>
		protected internal override EntityTypeEnumDto EntityType
		{
			get { return EntityTypeEnumDto.Project; }
		}
	}
}