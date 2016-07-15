using System;
using System.Collections.Generic;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Saved search repository interface definition.
    /// </summary>
    public interface IFavoriteSearchRepository :  IRepositoryBase<FavoriteSearch>
    {
        /// <summary>
        /// Fetches the by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>User's saved searches.</returns>
        IEnumerable<FavoriteSearch> FindByUserId(Guid userId);

        /// <summary>
        /// Fetches the by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="location">The location.</param>
        /// <returns>User's saved searches for a location.</returns>
        IEnumerable<FavoriteSearch> FindByUserIdAndLocation(Guid userId, string location);

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>User's favorite searches matching keyword.</returns>
        FavoriteSearchSearchResult Search(SearchCriteria searchCriteria, Guid userId);

        /// <summary>
        /// Finds the active by user identifier and location.
        /// </summary>        
        /// <param name="userId">The user identifier.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        FavoriteSearch FindActiveByUserIdAndLocation(Guid userId, string location);

        /// <summary>
        /// Finds the default by user identifier and location.
        /// </summary>        
        /// <param name="userId">The user identifier.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        IEnumerable<FavoriteSearch> FindAvailableByUserIdAndLocation(Guid userId, string location);
    }
}