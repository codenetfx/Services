using System.Collections.Generic;

using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Interface IOrderLineNotificationCheck
	/// </summary>
	public interface IOrderLineNotificationCheck : IOrderable
	{
		/// <summary>
		/// Checks the specified project service line.
		/// </summary>
		/// <param name="projectServiceLine">The project service line.</param>
		/// <param name="incomingOrderServiceLine">The incoming order service line.</param>
		/// <param name="notifications">The notifications.</param>
		/// <returns><c>true</c> if continue to check for more notifications, <c>false</c> otherwise.</returns>
		bool Check(IncomingOrderServiceLine projectServiceLine, IncomingOrderServiceLine incomingOrderServiceLine,
			List<NotificationTypeDto> notifications);
	}
}