using System.Collections.Generic;

using Microsoft.Practices.Unity;

using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// 
	/// </summary>
	public class NotificationStrategyFactory : Disposable, INotificationStrategyFactory
	{
		private IUnityContainer _container;


		/// <summary>
		/// Initializes a new instance of the <see cref="NotificationStrategyFactory"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public NotificationStrategyFactory(IUnityContainer container)
		{
			_container = container;
		}

		/// <summary>
		/// Gets the strategy.
		/// </summary>
		/// <param name="notificationType">Type of the notification.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public INotificationStrategy GetStrategy(NotificationTypeDto notificationType)
		{
			return _container.Resolve<INotificationStrategy>(notificationType.ToString());
		}

		/// <summary>
		/// Gets a list of strategies objects determined by the specified notification list.
		/// </summary>
		/// <param name="notifications">The notifications.</param>
		/// <returns></returns>
		public List<INotificationStrategy> GetStrategies(List<NotificationTypeDto> notifications)
		{
			var strategies = new List<INotificationStrategy>();

// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (var notificationType in notifications)
			{
				if (notificationType != NotificationTypeDto.Undefined)
					strategies.Add(GetStrategy(notificationType));
			}

			return strategies;
		}


		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			{
				//we are only removing the reference to the container
				//if we call dispose it will tear down the 
				//IOC container for the whole app.
				_container = null;
			}
		}
	}
}