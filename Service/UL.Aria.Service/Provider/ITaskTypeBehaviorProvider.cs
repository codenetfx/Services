using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Defines a provider for <see cref="TaskTypeBehavior"/> objects.
	/// </summary>
	public interface ITaskTypeBehaviorProvider : ISearchProviderBase<TaskTypeBehavior>
	{
		/// <summary>
		/// Finds the by task type identifier.
		/// </summary>
		/// <param name="taskTypeId">The task type identifier.</param>
		/// <returns></returns>
		IEnumerable<TaskTypeBehavior> FindByTaskTypeId(Guid taskTypeId);

		/// <summary>
		/// Saves the specified task type behaviors.
		/// </summary>
		/// <param name="taskTypeBehaviors">The task type behaviors.</param>
		/// <param name="taskTypeId">The value.</param>
		void Save(IEnumerable<TaskTypeBehavior> taskTypeBehaviors, Guid taskTypeId);

		/// <summary>
		/// Fetches the by multiple task type ids.
		/// </summary>
		/// <param name="taskTypeIds">The task type ids.</param>
		/// <returns></returns>
		IDictionary<Guid, IEnumerable<TaskTypeBehavior>> FetchByMultipleTaskTypeIds(IEnumerable<Guid> taskTypeIds);
	}
}