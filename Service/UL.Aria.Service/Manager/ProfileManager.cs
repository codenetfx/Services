using System;
using System.Collections.Generic;

using UL.Aria.Common.Authorization;
using UL.Aria.Common.BusinessMessage;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Managees the Profile info from the repository and possibly applies business rules and validation
    /// </summary>
    public class ProfileManager : IProfileManager
    {
        private readonly IAcceptanceClaimManager _acceptanceClaimManager;
        private readonly IBusinessMessageProvider _businessMessageProvider;
        private readonly ICompanyManager _companyManager;
        private readonly IFavoriteSearchProvider _favoriteSearchProvider;
        private readonly IMapperRegistry _mapper;
        private readonly IPrincipalResolver _principalResolver;

        /// <summary>
        ///     The _profile repository
        /// </summary>
        private readonly IProfileRepository _profileRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProfileManager" /> class.
        /// </summary>
        /// <param name="profileRepository">The profile repository.</param>
        /// <param name="companyManager">The company manager.</param>
        /// <param name="businessMessageProvider">The business message provider.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="acceptanceClaimManager">The acceptance claim manager.</param>
        /// <param name="favoriteSearchProvider">The saved search provider.</param>
        public ProfileManager(IProfileRepository profileRepository, ICompanyManager companyManager,
            IBusinessMessageProvider businessMessageProvider, IMapperRegistry mapper,
            IPrincipalResolver principalResolver, IAcceptanceClaimManager acceptanceClaimManager,
            IFavoriteSearchProvider favoriteSearchProvider)
        {
            _profileRepository = profileRepository;
            _companyManager = companyManager;
            _businessMessageProvider = businessMessageProvider;
            _mapper = mapper;
            _principalResolver = principalResolver;
            _acceptanceClaimManager = acceptanceClaimManager;
            _favoriteSearchProvider = favoriteSearchProvider;
        }

        /// <summary>
        ///     Gets the profile by id.
        /// </summary>
        /// <param name="profileId">The profile id.</param>
        /// <returns></returns>
        public ProfileBo FetchById(Guid profileId)
        {
            ProfileBo profileBo = _profileRepository.FetchById(profileId);
            SetCompanyProperties(profileBo);
            return profileBo;
        }

        /// <summary>
        ///     Gets the name of the profile by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public ProfileBo FetchByUserName(string userName)
        {
            ProfileBo profileBo = _profileRepository.FetchByUserName(userName);
            SetCompanyProperties(profileBo);
            return profileBo;
        }

        /// <summary>
        ///     Gets the profiles by company id.
        /// </summary>
        /// <param name="companyId">The company id.</param>
        /// <returns></returns>
        public IList<ProfileBo> FetchAllByCompanyId(Guid companyId)
        {
            //throw new NotImplementedException("Ttoups to revisit");
            IList<ProfileBo> profileBos = _profileRepository.FetchAllByCompanyId(companyId);
            foreach (ProfileBo profileBo in profileBos)
            {
                SetCompanyProperties(profileBo);
            }
            return profileBos;
        }

        /// <summary>
        ///     Gets the profile image by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ProfileImageBo FetchImageById(Guid id)
        {
            return new ProfileImageBo {Image = new byte[0]};
        }

        /// <summary>
        ///     Adds the company user.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="companyId">The company id.</param>
        /// <returns></returns>
        public Guid Create(ProfileEditBasicBo profile, Guid companyId)
        {
            ProfileBo existingUser = _profileRepository.FetchByUserName(profile.LoginId);
            if (existingUser != null)
            {
                //user already exists, cannot create user with same login id
                throw new DatabaseItemNotFoundException(
                    string.Format("Unable to create user '{0}', a user with that login name already exists.",
                        profile.LoginId));
            }

            var profileBo = _mapper.Map<ProfileBo>(profile);
            profileBo.CompanyId = companyId;
            Guid modifyingUser = profile.ModifyingUser;
            profileBo.CreatedById = modifyingUser;
            profileBo.CreatedDateTime = profileBo.UpdatedDateTime = DateTime.UtcNow;
            Guid id = _profileRepository.Create(profileBo);
            _businessMessageProvider.Publish(AuditMessageIdEnumDto.CreateUser,
                String.Format(
                    "A user with an Id of {0} has been added to the system by {1}.", id,
                    profile.ModifyingUser));
            return id;
        }

        /// <summary>
        ///     Updates the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void Update(ProfileEditBasicBo profile)
        {
            //return _profileRepository.AddCompanyUser(profile, companyId);
            var profileBo = _mapper.Map<ProfileBo>(profile);
            Guid modifyingUser = profile.ModifyingUser;
            profileBo.CreatedById = modifyingUser;
            profileBo.CreatedDateTime = profileBo.UpdatedDateTime = DateTime.UtcNow;
            SetCompanyId(profile, profileBo);
            _profileRepository.Update(profileBo);
            _businessMessageProvider.Publish(AuditMessageIdEnumDto.UpdateUser,
                String.Format(
                    "A user with an Id of {0} has been updated in the system by {1}.",
                    profile.ProfileId, profile.ModifyingUser));
        }

        /// <summary>
        ///     Removes the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public void Delete(Guid userId)
        {
            var modifyingUser = _principalResolver.UserId;
            _profileRepository.Remove(userId, modifyingUser);
            _businessMessageProvider.Publish(AuditMessageIdEnumDto.DeleteUser,
                String.Format(
                    "A user with an Id of {0} has been removed from the system by {1}.",
                    userId, modifyingUser));
        }

        /// <summary>
        ///     Searches the specified search specification.
        /// </summary>
        /// <param name="searchSpecification">The search specification.</param>
        /// <returns></returns>
        public IEnumerable<ProfileBo> Search(ProfileSearchSpecification searchSpecification)
        {
            IList<ProfileBo> profileBos = _profileRepository.Search(searchSpecification);
            foreach (ProfileBo profileBo in profileBos)
            {
                SetCompanyProperties(profileBo);
            }
            return profileBos;
        }


        /// <summary>
        ///     Gets the terms and conditions.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<TermsAndConditions> GetTermsAndConditions(Guid userId)
        {
            return _acceptanceClaimManager.GetTermsAndConditions(userId);
        }

        /// <summary>
        ///     Accepts the terms and conditions.
        /// </summary>
        /// <param name="termsAndConditions">The terms and conditions.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="hasAccepted">if set to <c>true</c> [has accepted].</param>
        public void AcceptTermsAndConditions(TermsAndConditions termsAndConditions, Guid userId, bool hasAccepted)
        {
            _acceptanceClaimManager.AcceptTermsAndConditions(termsAndConditions, userId, hasAccepted);
            _businessMessageProvider.Publish(AuditMessageIdEnumDto.AcceptTermsAndConditions,
                String.Format("User {0} has {1} the terms and conditions {2} ", userId,
                    hasAccepted ? "Accepted" : "Declined", termsAndConditions.Id));
        }

        /// <summary>
        ///     Fetches the saved searches by user id.
        /// </summary>
        /// <returns>Saved search listing.</returns>
        public IEnumerable<FavoriteSearch> FetchFavoriteSearches()
        {
            return _favoriteSearchProvider.FetchAll();
        }

        /// <summary>
        ///     Fetches the saved searches by user id and location uri.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>
        ///     Saved search listing.
        /// </returns>
        public IEnumerable<FavoriteSearch> FetchFavoriteSearchesByLocation(string location)
        {
            return _favoriteSearchProvider.FetchByLocation(location);
        }

        /// <summary>
        /// Fetches the active favorite searches by location.
        /// </summary>        
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public FavoriteSearch FetchActiveFavoriteSearchesByLocation(string location)
        {
            return _favoriteSearchProvider.FetchActiveByLocation(location);
        }

        /// <summary>
        /// Fetches the available favorite searches by location.
        /// </summary>        
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public IEnumerable<FavoriteSearch> FetchAvailableFavoriteSearchesByLocation(string location)
        {
            return _favoriteSearchProvider.FetchAvailableByLocation(location);
        }

        /// <summary>
        ///     Creates the saved search.
        /// </summary>
        /// <param name="favoriteSearch">The saved search.</param>
        /// <returns>
        ///     Created saved search entity.
        /// </returns>
        public FavoriteSearch CreateFavoriteSearch(FavoriteSearch favoriteSearch)
        {
            return _favoriteSearchProvider.Create(favoriteSearch);
        }

        /// <summary>
        ///     Updates the saved search.
        /// </summary>
        /// <param name="favoriteSearch">The saved search.</param>
        /// <returns>
        ///     Updated saved search entity.
        /// </returns>
        public FavoriteSearch UpdateFavoriteSearch(FavoriteSearch favoriteSearch)
        {
            return _favoriteSearchProvider.Update(favoriteSearch);
        }

        /// <summary>
        ///     Deletes the saved search.
        /// </summary>
        /// <param name="favoriteSearchId">The saved search id.</param>
        public void DeleteFavoriteSearch(Guid favoriteSearchId)
        {
            _favoriteSearchProvider.Delete(favoriteSearchId);
        }

        /// <summary>
        ///     Searches the favorite searches.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>
        ///     Matching favorite search search result listing.
        /// </returns>
        public FavoriteSearchSearchResult SearchFavoriteSearches(SearchCriteria searchCriteria)
        {
            return _favoriteSearchProvider.Search(searchCriteria);
        }

        /// <summary>
        ///     Fetches the favorite search by id.
        /// </summary>
        /// <param name="favoriteSearchId">The favorite search id.</param>
        /// <returns>
        ///     Favorite search with matching id.
        /// </returns>
        public FavoriteSearch FetchFavoriteSearchById(Guid favoriteSearchId)
        {
            return _favoriteSearchProvider.FetchById(favoriteSearchId);
        }

        /// <summary>
        ///     Partials the update favorite search.
        /// </summary>
        /// <param name="partialUpdateFavoriteSearch">The partial update favorite search.</param>
        /// <returns>
        ///     Updated favorite search.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public FavoriteSearch PartialUpdateFavoriteSearch(PartialUpdateFavoriteSearch partialUpdateFavoriteSearch)
        {
            return _favoriteSearchProvider.PartialUpdate(partialUpdateFavoriteSearch);
        }

        private void SetCompanyProperties(ProfileBo profileBo)
        {
            if (null != profileBo && profileBo.CompanyId != Guid.Empty)
            {
                Company company = _companyManager.FetchById(profileBo.CompanyId);
                if (null != company)
                {
                    if (company.ExternalIds.Count > 0)
                        profileBo.CompanyExternalId = company.ExternalIds[0];
                    profileBo.CompanyName = company.Name;
                }
            }
        }

        private void SetCompanyId(ProfileEditBasicBo profile, ProfileBo profileBo)
        {
            if (!string.IsNullOrWhiteSpace(profile.CompanyExternalId))
            {
                Company company = _companyManager.FetchByExternalId(profile.CompanyExternalId);
                if (null != company)
                    profileBo.CompanyId = company.Id.Value;
            }
        }


	    /// <summary>
	    /// Fetches the list of user whom belong to the specified team.
	    /// </summary>
	    /// <param name="userTeamId">The user team identifier.</param>
	    /// <param name="includeTeamMemberTeams">if set to <c>true</c> Includes the Team members of teams owned by team members recursively.</param>
	    /// <param name="maxDepth">The maximum depth of recursion.</param>
	    /// <returns></returns>
	  public  IList<ProfileBo> FetchUsersByTeamId(Guid userTeamId, bool includeTeamMemberTeams = false, int maxDepth = 2)
	    {
		    return _profileRepository.FetchByTeam(userTeamId, includeTeamMemberTeams, maxDepth);
	    }
    }
}