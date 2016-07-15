﻿using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class OrderCanceledNotificationCheck. This class cannot be inherited.
	/// </summary>
	public sealed class OrderCanceledNotificationCheck : IOrderNotificationCheck
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
			var newStatus = incomingOrder.Status.ToLower();
            if (newStatus != "canceled" && newStatus != "cancelled") return true;
			notifications.Add(NotificationTypeDto.OrderCanceled);
			return false;
		}

		/// <summary>
		/// Gets the ordinal.
		/// </summary>
		/// <value>The ordinal.</value>
		public int Ordinal
		{
			get { return 400; }
		}
	}
}