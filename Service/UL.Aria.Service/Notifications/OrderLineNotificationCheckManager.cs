using System.Collections.Generic;
using System.Linq;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class OrderLineNotificationCheckManager.
	/// </summary>
	public class OrderLineNotificationCheckManager : IOrderLineNotificationCheckManager
	{
		private readonly IEnumerable<IOrderLineNotificationCheck> _notificationChecks;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderLineNotificationCheckManager"/> class.
		/// </summary>
		/// <param name="notificationChecks">The notification checks.</param>
		public OrderLineNotificationCheckManager(IEnumerable<IOrderLineNotificationCheck> notificationChecks)
		{
			_notificationChecks = notificationChecks;
		}

		/// <summary>
		/// Gets the order line notifications.
		/// </summary>
		/// <param name="projectServiceLine">The project service line.</param>
		/// <param name="incomingOrderServiceLine">The incoming order service line.</param>
		/// <param name="notifications">The notifications.</param>
		public void GetOrderLineNotifications(IncomingOrderServiceLine projectServiceLine,
			IncomingOrderServiceLine incomingOrderServiceLine, List<NotificationTypeDto> notifications)
		{
			foreach (var notificationCheck in _notificationChecks.OrderBy(x => x.Ordinal))
			{
				//break if continue (return value from notification check) is false
				if (!notificationCheck.Check(projectServiceLine, incomingOrderServiceLine, notifications))
					break;
			}
		}
	}
}