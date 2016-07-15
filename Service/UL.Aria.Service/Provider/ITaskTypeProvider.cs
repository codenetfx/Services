using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface ITaskTypeProvider
	/// </summary>
    public interface ITaskTypeProvider : ISearchProviderBase<TaskType>
	{

		/// <summary>
		/// Gets the lookups.
		/// </summary>
		IEnumerable<Lookup> GetLookups();

        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <returns></returns>
        IEnumerable<Lookup> GetLookups(bool includeDeleted);

	    /// <summary>
	    /// Fetches the active by identifier.
	    /// </summary>
	    /// <param name="id">The identifier.</param>
	    /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
	    /// <returns></returns>
        TaskType Fetch(Guid id, bool isDeleted);

		/// <summary>
		/// Fetches all.
		/// </summary>
		/// <returns></returns>
		IEnumerable<TaskType> FetchAll();
	}
}
