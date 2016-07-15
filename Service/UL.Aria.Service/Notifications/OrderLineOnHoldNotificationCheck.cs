using System.Collections.Generic;
using System.Linq;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class OrderLineOnHoldNotificationCheck. This class cannot be inherited.
	/// </summary>
	public sealed class OrderLineOnHoldNotificationCheck : IOrderLineNotificationCheck
	{
		/// <summary>
		/// Checks the specified project service line.
		/// </summary>
		/// <param name="projectServiceLine">The project service line.</param>
		/// <param name="incomingOrderServiceLine">The incoming order service line.</param>
		/// <param name="notifications">The notifications.</param>
		/// <returns><c>true</c> if continue to check for more notifications, <c>false</c> otherwise.</returns>
		public bool Check(IncomingOrderServiceLine projectServiceLine, IncomingOrderServiceLine incomingOrderServiceLine,
			List<NotificationTypeDto> notifications)
		{
			var hold = incomingOrderServiceLine.Hold;
			if (string.IsNullOrEmpty(hold) || hold.ToLower() != "y") return true;
			if (notifications.Any(x => x == NotificationTypeDto.OrderLineOnHold)) return false;
			notifications.Add(NotificationTypeDto.OrderLineOnHold);
			notifications.RemoveAll(x => x == NotificationTypeDto.OrderLineCleanup);
			return false;
		}

		/// <summary>
		/// Gets the ordinal.
		/// </summary>
		/// <value>The ordinal.</value>
		public int Ordinal
		{
			get { return 300; }
		}
	}
}