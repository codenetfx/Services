using System;
using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Provides an interface for the Notification Strategy Factory.
	/// </summary>
	public interface INotificationStrategyFactory : IDisposable
	{
		/// <summary>
		/// Gets the strategy.
		/// </summary>
		/// <param name="notificationType">Type of the notification.</param>
		/// <returns></returns>
		INotificationStrategy GetStrategy(NotificationTypeDto notificationType);


		/// <summary>
		/// Gets a list of strategies objects determined by the specified notification list.
		/// </summary>
		/// <param name="notifications">The notifications.</param>
		/// <returns></returns>
		List<INotificationStrategy> GetStrategies(List<NotificationTypeDto> notifications);
	}
}