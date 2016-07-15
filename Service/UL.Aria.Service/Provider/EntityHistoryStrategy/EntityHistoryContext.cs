using System.Collections.Generic;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider.EntityHistoryStrategy
{
	/// <summary>
	/// Class EntityHistoryContext.
	/// </summary>
	public class EntityHistoryContext
	{
		private readonly IEntityHistoryStrategy _entityHistoryStrategy;

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityHistoryContext"/> class.
		/// </summary>
		/// <param name="entityHistoryStrategy">The entity history strategy.</param>
		public EntityHistoryContext(IEntityHistoryStrategy entityHistoryStrategy)
		{
			_entityHistoryStrategy = entityHistoryStrategy;
		}

		/// <summary>
		/// Creates the entity history.
		/// </summary>
		/// <param name="history">The history.</param>
		/// <returns>EntityHistory.</returns>
		public EntityHistory CreateEntityHistory(History history)
		{
			return _entityHistoryStrategy.CreateEntityHistory(history);
		}

		/// <summary>
		/// Derives the tracked information.
		/// </summary>
		/// <param name="history">The history.</param>
		/// <returns>List&lt;NameValuePair&gt;.</returns>
		public List<NameValuePair> DeriveTrackedInfo(History history)
		{
			return _entityHistoryStrategy.DeriveTrackedInfo(history);
		}

		/// <summary>
		/// Processes the specified history deltas.
		/// </summary>
		/// <param name="historyDeltas">The history deltas.</param>
		/// <param name="history">The history.</param>
		/// <param name="entityHistoryPrevious">The entity history previous.</param>
		/// <returns>EntityHistory.</returns>
		public EntityHistory Process(ICollection<TaskDelta> historyDeltas,
			History history, EntityHistory entityHistoryPrevious)
		{
			return _entityHistoryStrategy.Process(history, historyDeltas, entityHistoryPrevious);
		}

		/// <summary>
		/// Builds the action.
		/// </summary>
		/// <param name="entityType">Type of the entity.</param>
		/// <param name="action">The action.</param>
		/// <returns>System.String.</returns>
		public string BuildAction(string entityType, string action)
		{
			return _entityHistoryStrategy.BuildAction(entityType, action);
		}
	}
}