using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Profile service interface.
    /// </summary>
    [ServiceContract]
    public interface IProfileService
    {
        /// <summary>
        /// Gets the profile by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// ProfileDto
        /// </returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ProfileDto FetchByIdOrUserName(string id);
        /// <summary>
        /// Gets the profiles by company id.
        /// </summary>
        /// <param name="companyId">The company id.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Company/{companyId}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<ProfileDto> FetchAllByCompanyId(string companyId);

        /// <summary>
        /// Gets the profile by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// ProfileDto
        /// </returns>
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/{id}/Image", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ProfileImageDto FetchImageById(string id);

        /// <summary>
        /// Adds the company user.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="companyId">The company id.</param>
        /// <returns></returns>
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        ProfileDto Create(ProfileBasicDto profile, Guid companyId);

        /// <summary>
        /// Updates the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="id"></param>
        /// <returns></returns>
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/{id}", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        ProfileDto Update(ProfileBasicDto profile, string id);

        /// <summary>
        /// Removes a user.
        /// </summary>
        /// <param name="id"></param>
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Delete(string id);

        /// <summary>
        /// Searches the specified profile search specification dto.
        /// </summary>
        /// <param name="searchSpecification">The search specification.</param>
        /// <returns></returns>
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/Search", Method = "POST", RequestFormat = WebMessageFormat
            .Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        IEnumerable<ProfileDto> Search(ProfileSearchSpecificationDto searchSpecification);

        /// <summary>
        /// Gets the terms and conditions.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}/Terms", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<TermsAndConditionsDto> GetTermsAndConditions(string id);

        /// <summary>
        /// Accepts the terms and conditions.
        /// </summary>
        /// <param name="termsAndConditionsDto">The terms and conditions dto.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="hasAccepted">if set to <c>true</c> [has accepted].</param>
        [WebInvoke(UriTemplate = "/Accept", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void AcceptTermsAndConditions(TermsAndConditionsDto termsAndConditionsDto, Guid userId, bool hasAccepted);

        /// <summary>
        ///     Search based on the provided company id and criteria.
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/SearchCompanyUsers", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        CompanyUserSearchModelDto SearchCompanyUsers(string companyId, SearchCriteriaDto searchCriteria);

	    /// <summary>
        /// Fetches the saved searches by user id.
        /// </summary>
        /// <returns>List of save search data transfer objects.</returns>
        [WebInvoke(UriTemplate = "/FavoriteSearches", Method = "GET", RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        IEnumerable<FavoriteSearchDto> FetchFavoriteSearches();

        /// <summary>
        /// Fetches the saved searches by user id and location.
        /// </summary>
        /// <param name="location">The location URI.</param>
        /// <returns>
        /// List of save search data transfer objects.
        /// </returns>
        [WebInvoke(UriTemplate = "/FavoriteSearches/Location/{location}", Method = "GET", RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        IEnumerable<FavoriteSearchDto> FetchFavoriteSearchesByLocation(string location);

        /// <summary>
        /// Fetches the active favorite searches by location.
        /// </summary>        
        /// <param name="location">The location.</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/FavoriteSearches/Location/{location}/Default", Method = "GET", RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        FavoriteSearchDto FetchActiveFavoriteSearchesByLocation(string location);

        /// <summary>
        /// Fetches the available favorite searches by location.
        /// </summary>        
        /// <param name="location">The location.</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/FavoriteSearches/Location/{location}/Defaults", Method = "GET", RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        IEnumerable<FavoriteSearchDto> FetchAvailableFavoriteSearchesByLocation(string location);

        /// <summary>
        /// Creates the saved search.
        /// </summary>
        /// <param name="favoriteSearchDto">The saved search dto.</param>
        /// <returns>Created saved search.</returns>
        [WebInvoke(UriTemplate = "/FavoriteSearches", Method = "POST", RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        FavoriteSearchDto CreateFavoriteSearch(FavoriteSearchDto favoriteSearchDto);

        /// <summary>
        /// Updates the saved search.
        /// </summary>
        /// <param name="favoriteSearchId"></param>
        /// <param name="favoriteSearchDto">The saved search dto.</param>
        /// <returns>Updated favorite search dto.</returns>
        [WebInvoke(UriTemplate = "/FavoriteSearches/{favoriteSearchId}", Method = "PUT", RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        FavoriteSearchDto UpdateFavoriteSearch(string favoriteSearchId, FavoriteSearchDto favoriteSearchDto);

        /// <summary>
        /// Deletes the saved search.
        /// </summary>
        /// <param name="favoriteSearchId">The saved search id.</param>
        [WebInvoke(UriTemplate = "/FavoriteSearches/{favoriteSearchId}", Method = "DELETE", RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void DeleteFavoriteSearch(string favoriteSearchId);

        /// <summary>
        /// Searches the favorite searches.
        /// </summary>
        /// <param name="searchCriteriaDto">The search criteria dto.</param>
        /// <returns>Matching search result for favorite search criteris specified.</returns>
        [WebInvoke(UriTemplate = "/FavoriteSearches/Search", Method = "POST", RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        FavoriteSearchSearchModelDto SearchFavoriteSearches(SearchCriteriaDto searchCriteriaDto);

        /// <summary>
        /// Fetches the favorite searches by id.
        /// </summary>
        /// <param name="favoriteSearchId">The favorite search id.</param>
        /// <returns>Favorite search with matching id.</returns>
        [WebInvoke(UriTemplate = "/FavoriteSearches/{favoriteSearchId}", Method = "GET", RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        FavoriteSearchDto FetchFavoriteSearchById(string favoriteSearchId);

        /// <summary>
        /// Partials the update favorite search.
        /// </summary>
        /// <param name="favoriteSearchId">The favorite search id.</param>
        /// <param name="favoriteSearchDto">The favorite search dto.</param>
        /// <returns>Updated favorite search.</returns>
        /// <remarks>This operation is a partial update, so not all values contained in the favorite search DTO will be respected.  Attempts to update an unsupported member will throw an exception.</remarks>
        [WebInvoke(UriTemplate = "/FavoriteSearches/{favoriteSearchId}", Method = "PATCH", RequestFormat =        WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        FavoriteSearchDto PartialUpdateFavoriteSearch(string favoriteSearchId, FavoriteSearchDto favoriteSearchDto);

		/// <summary>
		/// Fetches the list of user whom belong to the specified team.
		/// </summary>
		/// <param name="userTeamId">The user team identifier.</param>
		/// <param name="includeTeamMemberTeams">if set to <c>true</c> Includes the Team members of teams owned by team members recursively.</param>
		/// <param name="maxDepth">The maximum depth of recursion.</param>
		/// <returns></returns>
	
	    [OperationContract]
		[WebInvoke(UriTemplate = "/MyTeam/{userTeamId}/{includeTeamMemberTeams}/{maxDepth}", Method = "GET", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
	    IList<ProfileDto> FetchUsersByTeamId(string userTeamId, string includeTeamMemberTeams, string maxDepth);


    }
}
