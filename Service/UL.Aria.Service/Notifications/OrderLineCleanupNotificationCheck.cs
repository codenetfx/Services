using System.Collections.Generic;
using System.Globalization;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class OrderLineCleanupNotificationCheck. This class cannot be inherited.
	/// </summary>
	public sealed class OrderLineCleanupNotificationCheck : IOrderLineNotificationCheck
	{
		/// <summary>
		/// Checks the specified project service line.
		/// </summary>
		/// <param name="projectServiceLine">The project service line.</param>
		/// <param name="incomingOrderServiceLine">The incoming order service line.</param>
		/// <param name="notifications">The notifications.</param>
		/// <returns>System.Boolean.</returns>
		public bool Check(IncomingOrderServiceLine projectServiceLine, IncomingOrderServiceLine incomingOrderServiceLine,
			List<NotificationTypeDto> notifications)
		{
			var newStatus = incomingOrderServiceLine.Status.ToLower();
			var hold = incomingOrderServiceLine.Hold;

            if (newStatus == "canceled" || newStatus == "cancelled" || !string.IsNullOrEmpty(hold) && hold.ToLower() == "y") return true;

			if (notifications.Count == 0)
			{
				notifications.Add(NotificationTypeDto.OrderLineCleanup);
			}
			return false;
		}

		/// <summary>
		/// Gets the ordinal.
		/// </summary>
		/// <value>The ordinal.</value>
		public int Ordinal
		{
			get { return 200; }
		}
	}
}