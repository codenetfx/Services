using System.Collections.Generic;
using System.Linq;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class OrderOnHoldStrategy.
	/// </summary>
	[NotificationType(NotificationTypeDto.OrderOnHold)]
	public sealed class OrderOnHoldStrategy : OrderNotificationStrategyBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OrderOnHoldStrategy" /> class.
		/// </summary>
		/// <param name="notificationManager">The notification manager.</param>
		/// <param name="profileManager">The profile manager.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		public OrderOnHoldStrategy(INotificationManager notificationManager,
			IProfileManager profileManager, IPrincipalResolver principalResolver)
			: base(notificationManager, profileManager, principalResolver)
		{
		}

		/// <summary>
		/// Deletes the notifications.
		/// </summary>
		/// <param name="allNotifications">All notifications.</param>
		/// <param name="notificationType">Type of the notification.</param>
		protected override void DeleteNotifications(IEnumerable<Notification> allNotifications,
			NotificationTypeDto notificationType)
		{
			var notificationsToDelete =
				allNotifications.Where(
					x => x.NotificationType == notificationType || x.NotificationType == NotificationTypeDto.OrderCanceled || x.NotificationType == NotificationTypeDto.OrderLineOnHold || x.NotificationType == NotificationTypeDto.OrderLineCanceled);
			_notificationManager.Delete(notificationsToDelete);
		}
	}
}