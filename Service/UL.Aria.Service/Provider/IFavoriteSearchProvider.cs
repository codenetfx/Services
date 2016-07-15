using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Saved search provider interface definition.
    /// </summary>
    public interface IFavoriteSearchProvider
    {
        /// <summary>
        /// Fetches the saved searches by user id.
        /// </summary>
        /// <returns>User's saved searches.</returns>
        IEnumerable<FavoriteSearch> FetchAll();

        /// <summary>
        /// Fetches the by user id and location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>User's saved searches for a location</returns>
        IEnumerable<FavoriteSearch> FetchByLocation(string location);

        /// <summary>
        /// Creates the specified saved search.
        /// </summary>
        /// <param name="favoriteSearch">The saved search.</param>
        /// <returns></returns>
        FavoriteSearch Create(FavoriteSearch favoriteSearch);

        /// <summary>
        /// Updates the specified saved search.
        /// </summary>
        /// <param name="favoriteSearch">To saved search.</param>
        /// <returns></returns>
        FavoriteSearch Update(FavoriteSearch favoriteSearch);

        /// <summary>
        /// Deletes the specified favorite search id.
        /// </summary>
        /// <param name="favoriteSearchId">The favorite search id.</param>
        void Delete(Guid favoriteSearchId);

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>Matching favorite search results.</returns>
        FavoriteSearchSearchResult Search(SearchCriteria searchCriteria);

        /// <summary>
        /// Fetches the by id.
        /// </summary>
        /// <param name="favoriteSearchId">The favorite search id.</param>
        /// <returns>Matching favorite search.</returns>
        FavoriteSearch FetchById(Guid favoriteSearchId);

        /// <summary>
        /// Partials the update.
        /// </summary>
        /// <param name="partialUpdateFavoriteSearch">The partial update favorite search.</param>
        /// <returns>Updated favorite search.</returns>
        FavoriteSearch PartialUpdate(PartialUpdateFavoriteSearch partialUpdateFavoriteSearch);

        /// <summary>
        /// Fetches the active by location.
        /// </summary>        
        /// <param name="location">The location.</param>
        /// <returns></returns>
        FavoriteSearch FetchActiveByLocation(string location);

        /// <summary>
        /// Fetches the available by location.
        /// </summary>        
        /// <param name="location">The location.</param>
        /// <returns></returns>
        IEnumerable<FavoriteSearch> FetchAvailableByLocation(string location);
    }
}