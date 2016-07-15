using System.Collections.Generic;

using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;


namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Interface IOrderNotificationCheck
	/// </summary>
	public interface IOrderNotificationCheck : IOrderable
    {
		/// <summary>
		/// Checks the specified project.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="incomingOrder">The incoming order.</param>
		/// <param name="notifications">The notifications.</param>
		/// <returns><c>true</c> if continue to check for more notifications, <c>false</c> otherwise.</returns>
		bool Check(Project project, IncomingOrder incomingOrder, List<NotificationTypeDto> notifications);
    }
}
