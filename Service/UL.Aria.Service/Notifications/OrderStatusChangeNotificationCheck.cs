using System;
using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class OrderStatusChangeNotificationCheck. This class cannot be inherited.
	/// </summary>
	public sealed class OrderStatusChangeNotificationCheck : IOrderNotificationCheck
	{
		/// <summary>
		/// Checks the specified project.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="incomingOrder">The incoming order.</param>
		/// <param name="notifications">The notifications.</param>
		/// <returns><c>true</c> if continue to check for more notifications, <c>false</c> otherwise.</returns>
		public bool Check(Project project, IncomingOrder incomingOrder, List<NotificationTypeDto> notifications)
		{
			//We just want to stop the notification checks here if no status change, 
            //so return true (continue) if the status' are different otherwise return false
            return String.Compare(project.Status, incomingOrder.Status, true) != 0;
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