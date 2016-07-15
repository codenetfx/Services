namespace UL.Aria.Service.Provider.EntityHistoryStrategy
{
	/// <summary>
	/// Interface IEntityHistoryStrategyResolver
	/// </summary>
	public interface IEntityHistoryStrategyResolver
	{
		/// <summary>
		/// Resolves the specified action detail entity type.
		/// </summary>
		/// <param name="actionDetailEntityType">Type of the action detail entity.</param>
		/// <returns>IEntityHistoryStrategy.</returns>
		IEntityHistoryStrategy Resolve(string actionDetailEntityType);
	}
}