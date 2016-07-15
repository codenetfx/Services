using System.Collections.Generic;
using System.Linq;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class OrderNotificationCheckManager.
	/// </summary>
	public class OrderNotificationCheckManager : IOrderNotificationCheckManager
	{

		private readonly IEnumerable<IOrderNotificationCheck> _notificationChecks;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderNotificationCheckManager"/> class.
		/// </summary>
		/// <param name="notificationChecks">The notification checks.</param>
		public OrderNotificationCheckManager(IEnumerable<IOrderNotificationCheck> notificationChecks)
		{
			_notificationChecks = notificationChecks;
		}

		/// <summary>
		/// Gets the order notifications.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="incomingOrder">The incoming order.</param>
		/// <returns>List&lt;NotificationTypeDto&gt;.</returns>
		public List<NotificationTypeDto> GetOrderNotifications(Project project, IncomingOrder incomingOrder)
		{
			var notifications = new List<NotificationTypeDto>();

			foreach (var notificationCheck in _notificationChecks.OrderBy(x => x.Ordinal))
			{
				//break if continue (return value from notification check) is false
				if (!notificationCheck.Check(project, incomingOrder, notifications))
					break;
			}

			return notifications;
		}
	}
}
