using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Profile manager interface.
    /// </summary>
    public interface IProfileManager
    {
        /// <summary>
        /// Gets the profile by id.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <returns></returns>
		ProfileBo FetchById(Guid id);

        /// <summary>
        /// Gets the name of the profile by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        ProfileBo FetchByUserName(string userName);

        /// <summary>
        /// Gets the profiles by company id.
        /// </summary>
        /// <param name="companyId">The company id.</param>
        /// <returns></returns>
        IList<ProfileBo> FetchAllByCompanyId(Guid companyId);

        /// <summary>
        /// Gets the profile image by id.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <returns></returns>
		ProfileImageBo FetchImageById(Guid id);

        /// <summary>
        /// Adds the company user.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="companyId">The company id.</param>
        /// <returns></returns>
        Guid Create(ProfileEditBasicBo profile, Guid companyId);

        /// <summary>
        /// Removes the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        void Delete(Guid userId);

	    /// <summary>
	    /// Searches the specified search specification.
	    /// </summary>
	    /// <param name="searchSpecification">The search specification.</param>
	    /// <returns></returns>
	    IEnumerable<ProfileBo> Search(ProfileSearchSpecification searchSpecification);

        /// <summary>
        /// Updates the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        void Update(ProfileEditBasicBo profile);

        /// <summary>
        /// Gets the terms and conditions.
        /// </summary>
        /// <param name="userId"></param>
        IEnumerable<TermsAndConditions> GetTermsAndConditions(Guid userId);

        /// <summary>
        /// Accepts the terms and conditions.
        /// </summary>
        /// <param name="termsAndConditions">The terms and conditions.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="hasAccepted">if set to <c>true</c> [has accepted].</param>
        void AcceptTermsAndConditions(TermsAndConditions termsAndConditions, Guid userId, bool hasAccepted);

        /// <summary>
        /// Fetches the saved searches by user id.
        /// </summary>
        /// <returns>Saved search listing.</returns>
        IEnumerable<FavoriteSearch> FetchFavoriteSearches();

        /// <summary>
        /// Fetches the saved searches by user id and location uri.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>Saved search listing.</returns>
        IEnumerable<FavoriteSearch> FetchFavoriteSearchesByLocation(string location);

        /// <summary>
        /// Creates the saved search.
        /// </summary>
        /// <param name="favoriteSearch">The saved search.</param>
        /// <returns>Created saved search entity.</returns>
        FavoriteSearch CreateFavoriteSearch(FavoriteSearch favoriteSearch);

        /// <summary>
        /// Updates the saved search.
        /// </summary>
        /// <param name="favoriteSearch">The saved search.</param>
        /// <returns>Updated saved search entity.</returns>
        FavoriteSearch UpdateFavoriteSearch(FavoriteSearch favoriteSearch);

        /// <summary>
        /// Deletes the saved search.
        /// </summary>
        /// <param name="favoriteSearchId">The saved search id.</param>
        void DeleteFavoriteSearch(Guid favoriteSearchId);

        /// <summary>
        /// Searches the favorite searches.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>Matching favorite search search result listing.</returns>
        FavoriteSearchSearchResult SearchFavoriteSearches(SearchCriteria searchCriteria);

        /// <summary>
        /// Fetches the favorite search by id.
        /// </summary>
        /// <param name="favoriteSearchId">The favorite search id.</param>
        /// <returns>Favorite search with matching id.</returns>
        FavoriteSearch FetchFavoriteSearchById(Guid favoriteSearchId);

        /// <summary>
        /// Partials the update favorite search.
        /// </summary>
        /// <param name="partialUpdateFavoriteSearch">The partial update favorite search.</param>
        /// <returns>Updated favorite search.</returns>
        FavoriteSearch PartialUpdateFavoriteSearch(PartialUpdateFavoriteSearch partialUpdateFavoriteSearch);

        /// <summary>
        /// Fetches the active favorite searches by location.
        /// </summary>        
        /// <param name="location">The location.</param>
        /// <returns></returns>
        FavoriteSearch FetchActiveFavoriteSearchesByLocation(string location);

        /// <summary>
        /// Fetches the available favorite searches by location.
        /// </summary>        
        /// <param name="location">The location.</param>
        /// <returns></returns>
        IEnumerable<FavoriteSearch> FetchAvailableFavoriteSearchesByLocation(string location);

	    /// <summary>
	    /// Fetches the list of user whom belong to the specified team.
	    /// </summary>
	    /// <param name="userTeamId">The user team identifier.</param>
	    /// <param name="includeTeamMemberTeams">if set to <c>true</c> Includes the Team members of teams owned by team members recursively.</param>
	    /// <param name="maxDepth">The maximum depth of recursion.</param>
	    /// <returns></returns>
	    IList<ProfileBo> FetchUsersByTeamId(Guid userTeamId, bool includeTeamMemberTeams = false, int maxDepth = 2);
    }
}