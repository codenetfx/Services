using Microsoft.Practices.Unity;

namespace UL.Aria.Service.Provider.EntityHistoryStrategy
{
	/// <summary>
	/// Class EntityHistoryStrategyResolver.
	/// </summary>
	public sealed class EntityHistoryStrategyResolver : IEntityHistoryStrategyResolver
	{
		private readonly IUnityContainer _container;

		/// <summary>
		///     Initializes a new instance of the <see cref="EntityHistoryStrategyResolver" /> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public EntityHistoryStrategyResolver(IUnityContainer container)
		{
			_container = container;
		}

		/// <summary>
		/// Resolves the specified action detail entity type.
		/// </summary>
		/// <param name="actionDetailEntityType">Type of the action detail entity.</param>
		/// <returns>IEntityHistoryStrategy.</returns>
		public IEntityHistoryStrategy Resolve(string actionDetailEntityType)
		{
			return _container.Resolve<IEntityHistoryStrategy>(actionDetailEntityType);
		}
	}
}