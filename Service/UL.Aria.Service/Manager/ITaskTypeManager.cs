using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	/// Provides an interface for a Task Type Manager.
	/// </summary>
    public interface ITaskTypeManager: IManagerBase<TaskType>, ISearchManagerBase<TaskType>
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
		/// Fetches all.
		/// </summary>
		/// <returns></returns>
		IEnumerable<TaskType> FetchAll();
      
	}
}