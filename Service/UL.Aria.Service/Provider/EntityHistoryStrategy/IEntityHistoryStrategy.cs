using System.Collections.Generic;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider.EntityHistoryStrategy
{
	/// <summary>
	/// Interface IEntityHistoryStrategy
	/// </summary>
	public interface IEntityHistoryStrategy
	{
		/// <summary>
		/// Creates the entity history.
		/// </summary>
		/// <param name="history">The history.</param>
		/// <returns>EntityHistory.</returns>
		EntityHistory CreateEntityHistory(History history);

		/// <summary>
		/// Derives the tracked information.
		/// </summary>
		/// <param name="history">The history.</param>
		/// <returns>List&lt;NameValuePair&gt;.</returns>
		List<NameValuePair> DeriveTrackedInfo(History history);

		/// <summary>
		/// Processes the specified history.
		/// </summary>
		/// <param name="history">The history.</param>
		/// <param name="historyDeltas">The history deltas.</param>
		/// <param name="entityHistoryPrevious">The entity history previous.</param>
		/// <returns>EntityHistory.</returns>
		EntityHistory Process(History history, ICollection<TaskDelta> historyDeltas, EntityHistory entityHistoryPrevious);

		/// <summary>
		/// Builds the action.
		/// </summary>
		/// <param name="entityType">Type of the entity.</param>
		/// <param name="action">The action.</param>
		/// <returns>System.String.</returns>
		string BuildAction(string entityType, string action);
	}
}