using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class OrderLineStatusChangeNotificationCheck. This class cannot be inherited.
	/// </summary>
	public sealed class OrderLineStatusChangeNotificationCheck : IOrderLineNotificationCheck
	{
		/// <summary>
		/// Checks the specified project service line.
		/// </summary>
		/// <param name="projectServiceLine">The project service line.</param>
		/// <param name="incomingOrderServiceLine">The incoming order service line.</param>
		/// <param name="notifications">The notifications.</param>
		/// <returns><c>true</c> if continue to check for more notifications, <c>false</c> otherwise.</returns>
		public bool Check(IncomingOrderServiceLine projectServiceLine, IncomingOrderServiceLine incomingOrderServiceLine, List<NotificationTypeDto> notifications)
		{
			var originalStatus = projectServiceLine.Status.ToLower();
			var newStatus = incomingOrderServiceLine.Status.ToLower();
			var orginalHold = projectServiceLine.Hold;
			var newHold = incomingOrderServiceLine.Hold;
			//We just want to stop the notification checks here if no status change, so return true (continue) if the status' are different otherwise return false
			return originalStatus != newStatus || orginalHold !=newHold;
		}

		/// <summary>
		/// Gets the ordinal.
		/// </summary>
		/// <value>The ordinal.</value>
		public int Ordinal
		{
			get { return 100; }
		}
	}
}