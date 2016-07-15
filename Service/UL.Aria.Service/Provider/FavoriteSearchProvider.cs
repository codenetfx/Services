using System;
using System.Collections.Generic;
using System.Diagnostics;
using UL.Aria.Common.Authorization;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Domain;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Saved search provider class.
    /// </summary>
    public class FavoriteSearchProvider : IFavoriteSearchProvider
    {
        private readonly IFavoriteSearchRepository _favoriteSearchRepository;
        private readonly IPrincipalResolver _principalResolver;
        private readonly IValidator<FavoriteSearch> _favoriteSearchValidator;
        private readonly IMapperRegistry _mapperRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteSearchProvider" /> class.
        /// </summary>
        /// <param name="favoriteSearchRepository">The saved search repository.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="favoriteSearchValidator">The favorite search validator.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public FavoriteSearchProvider(IFavoriteSearchRepository favoriteSearchRepository, IPrincipalResolver principalResolver, IValidator<FavoriteSearch> favoriteSearchValidator, IMapperRegistry mapperRegistry)
        {
            _favoriteSearchRepository = favoriteSearchRepository;
            _principalResolver = principalResolver;
            _favoriteSearchValidator = favoriteSearchValidator;
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        /// Fetches the saved searches by user id.
        /// </summary>
        /// <returns>
        /// User's saved searches.
        /// </returns>
        public IEnumerable<FavoriteSearch> FetchAll()
        {           
            return _favoriteSearchRepository.FindByUserId(_principalResolver.UserId);
        }

        /// <summary>
        /// Fetches the by user id and location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>
        /// User's saved searches for a location
        /// </returns>
        public IEnumerable<FavoriteSearch> FetchByLocation(string location)
        {
            return _favoriteSearchRepository.FindByUserIdAndLocation(_principalResolver.UserId, location);
        }

        /// <summary>
        /// Fetches the active by location.
        /// </summary>        
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public FavoriteSearch FetchActiveByLocation(string location)
        {
            return _favoriteSearchRepository.FindActiveByUserIdAndLocation(_principalResolver.UserId, location);
        }

        /// <summary>
        /// Fetches the available by location.
        /// </summary>        
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public IEnumerable<FavoriteSearch> FetchAvailableByLocation(string location)
        {
            return _favoriteSearchRepository.FindAvailableByUserIdAndLocation(_principalResolver.UserId, location);
        }

        /// <summary>
        /// Creates the specified saved search.
        /// </summary>
        /// <param name="favoriteSearch">The saved search.</param>
        /// <returns>Created favorite search.</returns>
        public FavoriteSearch Create(FavoriteSearch favoriteSearch)
        {
            //Set the creator info (until this logic is migrated to the repository)
            favoriteSearch.CreatedById = _principalResolver.UserId;
            favoriteSearch.CreatedDateTime = DateTime.UtcNow;
            
            _favoriteSearchValidator.AssertIsValid(favoriteSearch);

            _favoriteSearchRepository.Add(favoriteSearch);

            return _favoriteSearchRepository.FindById(favoriteSearch.Id.Value);
        }

        /// <summary>
        /// Updates the specified saved search.
        /// </summary>
        /// <param name="favoriteSearch">To saved search.</param>
        /// <returns>Updated favorite search.</returns>
        public FavoriteSearch Update(FavoriteSearch favoriteSearch)
        {            
            //Set the creator info (until this logic is migrated to the repository)
            favoriteSearch.UpdatedById = _principalResolver.UserId;
            favoriteSearch.UpdatedDateTime = DateTime.UtcNow;

            _favoriteSearchValidator.AssertIsValid(favoriteSearch);

            _favoriteSearchRepository.Update(favoriteSearch);

            return _favoriteSearchRepository.FindById(favoriteSearch.Id.Value);
        }

        /// <summary>
        /// Deletes the specified favorite search id.
        /// </summary>
        /// <param name="favoriteSearchId">The favorite search id.</param>
        public void Delete(Guid favoriteSearchId)
        {
            _favoriteSearchRepository.Remove(favoriteSearchId);
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>
        /// Matching favorite search results.
        /// </returns>
        public FavoriteSearchSearchResult Search(SearchCriteria searchCriteria)
        {
            return _favoriteSearchRepository.Search(searchCriteria, _principalResolver.UserId);
        }

        /// <summary>
        /// Fetches the by id.
        /// </summary>
        /// <param name="favoriteSearchId">The favorite search id.</param>
        /// <returns>
        /// Matching favorite search.
        /// </returns>
        public FavoriteSearch FetchById(Guid favoriteSearchId)
        {
            return _favoriteSearchRepository.FindById(favoriteSearchId);
        }

        /// <summary>
        /// Partials the update.
        /// </summary>
        /// <param name="partialUpdateFavoriteSearch">The partial update favorite search.</param>
        /// <returns>
        /// Updated favorite search.
        /// </returns>
        public FavoriteSearch PartialUpdate(PartialUpdateFavoriteSearch partialUpdateFavoriteSearch)
        {
            //Retrieve the existing search
            var toBeUpdatedFavoriteSearch = FetchById(partialUpdateFavoriteSearch.Id);

            toBeUpdatedFavoriteSearch.ActiveDefault = partialUpdateFavoriteSearch.ActiveDefault;
            toBeUpdatedFavoriteSearch.AvailableDefault = partialUpdateFavoriteSearch.AvailableDefault;

            //Couldn't figure out how to get automapper to do this...
            if(!string.IsNullOrWhiteSpace(partialUpdateFavoriteSearch.Name))
            {
                toBeUpdatedFavoriteSearch.Name = partialUpdateFavoriteSearch.Name;
            }

            if(!string.IsNullOrWhiteSpace(partialUpdateFavoriteSearch.Location))
            {
                toBeUpdatedFavoriteSearch.Location = partialUpdateFavoriteSearch.Location;
            }

            if(Guid.Empty != partialUpdateFavoriteSearch.UserId)
            {
                toBeUpdatedFavoriteSearch.UserId = partialUpdateFavoriteSearch.UserId;
            }

            if (null != partialUpdateFavoriteSearch.SearchCriteria)
            {
                toBeUpdatedFavoriteSearch.SearchCriteria = _mapperRegistry.Map<SearchCriteria>(partialUpdateFavoriteSearch.SearchCriteria);
            }

            //Invoke update
            return Update(toBeUpdatedFavoriteSearch);
        }
    }
}