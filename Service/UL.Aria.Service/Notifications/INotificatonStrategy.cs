using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Interface INotificationStrategy
	/// </summary>
	public interface INotificationStrategy
	{
		/// <summary>
		/// When implemented in derived classes, it creates 
		/// a notification using its particular strategy.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		void Run(DomainEntity entity);
	}

	/// <summary>
	/// Interface INotificationStrategy
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface INotificationStrategy<in T> : INotificationStrategy where T : DomainEntity
	{
		/// <summary>
		/// When implemented in derived classes, it creates 
		/// a notification using its particular strategy.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		void Run(T entity);
	}
}