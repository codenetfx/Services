using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Interface ITaskTypeRepository
	/// </summary>
    public interface ITaskTypeRepository : IPrimaryEntityRepository<TaskType>
	{
        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <returns></returns>
        IEnumerable<Lookup> GetLookups(bool includeDeleted =  false);

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
        IList<TaskType> FetchAll();
	}
}
