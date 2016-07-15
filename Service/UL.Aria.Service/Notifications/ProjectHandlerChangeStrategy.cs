using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Framework;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class OrderOnHoldStrategy.
	/// </summary>
	[NotificationType(NotificationTypeDto.ProjectHandlerChange)]
	public sealed class ProjectHandlerChangeStrategy : OrderNotificationStrategyBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OrderOnHoldStrategy" /> class.
		/// </summary>
		/// <param name="notificationManager">The notification manager.</param>
		/// <param name="profileManager">The profile manager.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		public ProjectHandlerChangeStrategy(INotificationManager notificationManager,
			IProfileManager profileManager, IPrincipalResolver principalResolver)
			: base(notificationManager, profileManager, principalResolver)
		{
		}



		/// <summary>
		/// Runs the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void Run(Project entity)
		{
			var allNotifications = _notificationManager.FetchNotificationsByEntity(entity.Id.GetValueOrDefault());
			var projectHandler = this._profileManager.FetchByUserName(entity.ProjectHandler);
			if(projectHandler == null) return;

			var notifications = allNotifications as IList<Notification> ?? allNotifications.ToList();
			notifications.ForEach(x =>
			{
				x.UserId = projectHandler.Id;
				//_notificationManager.Update(x);
				
			});
			_notificationManager.UpdateBulk(notifications, entity.Id.GetValueOrDefault());
		}
	}
}