using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Interface IOrderNotificationCheckManager
	/// </summary>
	public interface IOrderNotificationCheckManager
	{
		/// <summary>
		/// Gets the order notifications.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="incomingOrder">The incoming order.</param>
		/// <returns>List&lt;NotificationTypeDto&gt;.</returns>
		List<NotificationTypeDto> GetOrderNotifications(Project project, IncomingOrder incomingOrder);
	}
}