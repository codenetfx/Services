using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Interface IOrderLineNotificationCheckManager
	/// </summary>
	public interface IOrderLineNotificationCheckManager
	{
		/// <summary>
		/// Gets the order line notifications.
		/// </summary>
		/// <param name="projectServiceLine">The project service line.</param>
		/// <param name="incomingOrderServiceLine">The incoming order service line.</param>
		/// <param name="notifications">The notifications.</param>
		void GetOrderLineNotifications(IncomingOrderServiceLine projectServiceLine,
			IncomingOrderServiceLine incomingOrderServiceLine, List<NotificationTypeDto> notifications);
	}
}