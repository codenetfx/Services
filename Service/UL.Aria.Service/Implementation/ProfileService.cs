using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Transactions;
using UL.Aria.Common.Authorization;
using UL.Aria.Service.Logging;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Configuration;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Provider;
using UL.Aria.Common;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    /// Profile service class.
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class ProfileService : IProfileService
    {
        private readonly IUserBusinessClaimProvider _claimProvider;
        private readonly ISmtpClientManager _smtpClientManager;
        private readonly IServiceConfiguration _serviceConfiguration;
        private readonly ILogManager _logManager;
        private readonly IProfileManager _profileManager;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService" /> class.
        /// </summary>
        /// <param name="profileManager">The profileManager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="claimProvider">The claim provider.</param>
        /// <param name="smtpClientManager">The SMTP client manager.</param>
        /// <param name="serviceConfiguration">The service configuration.</param>
        /// <param name="logManager"></param>
        public ProfileService(IProfileManager profileManager, IMapperRegistry mapperRegistry,
            ITransactionFactory transactionFactory, IUserBusinessClaimProvider claimProvider,
            ISmtpClientManager smtpClientManager, IServiceConfiguration serviceConfiguration, ILogManager logManager)
        {
            _profileManager = profileManager;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
            _claimProvider = claimProvider;
            _smtpClientManager = smtpClientManager;
            _serviceConfiguration = serviceConfiguration;
            _logManager = logManager;
        }


        /// <summary>
        ///     Gets the profile by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///     ProfileDto
        /// </returns>
        public ProfileDto FetchByIdOrUserName(string id)
        {
            Guid userId;
            if (Guid.TryParse(id, out userId))
            {
                ProfileBo bo = _profileManager.FetchById(userId);
                if (null == bo)
                    return null;
                ProfileDto dto = MapProfileToDto(bo);
                return dto;
            }

            ProfileBo bo1 = _profileManager.FetchByUserName(id);
            if (null == bo1)
                return null;
            ProfileDto dto1 = MapProfileToDto(bo1);
            return dto1;
        }


        /// <summary>
        ///     Gets the profiles by company id.
        /// </summary>
        /// <param name="companyId">The company id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<ProfileDto> FetchAllByCompanyId(string companyId)
        {
            IList<ProfileBo> profiles = _profileManager.FetchAllByCompanyId(new Guid(companyId));

            var list = _mapperRegistry.Map<List<ProfileDto>>(profiles);
            foreach (ProfileDto profileDto in list)
            {
                IList<UserBusinessClaim> userBusinessClaims = _claimProvider.GetUserClaims(profileDto.LoginId);
                if (null != userBusinessClaims)
                {
                    List<BusinessClaim> claims = userBusinessClaims.Select(x => x.Claim).ToList();

                    if (null != claims)
                        profileDto.Claims = _mapperRegistry.Map<List<BusinessClaimDto>>(claims);
                }
            }

            return list;
        }

        /// <summary>
        ///     Gets the profile by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///     ProfileDto
        /// </returns>
        public ProfileImageDto FetchImageById(string id)
        {
            Guid userId;
            Guid.TryParse(id, out userId);

            ProfileImageBo bo = _profileManager.FetchImageById(userId);

            return _mapperRegistry.Map<ProfileImageDto>(bo);
        }


        /// <summary>
        ///     Adds the company user.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="companyId">The company id.</param>
        /// <returns></returns>
        public ProfileDto Create(ProfileBasicDto profile, Guid companyId)
        {
            var bo = _mapperRegistry.Map<ProfileEditBasicBo>(profile);
            ProfileBo returnBo = null;
            using (TransactionScope transactionScope = _transactionFactory.Create())
            {
                Guid id = _profileManager.Create(bo, companyId);
                profile.ProfileId = id;
                AddEmployeeId(profile);
                returnBo = _profileManager.FetchById(id);
                transactionScope.Complete();
            }

            if (companyId == _serviceConfiguration.UlCompanyId)
            {
                try
                {
                    _smtpClientManager.SendAccountCreated(returnBo);
                }
                catch (Exception ex)
                {
                    _logManager.Log(new LogMessage(MessageIds.UnableToSendAccountCreatedEmail, LogPriority.Critical,
                        TraceEventType.Error,
                        "Unable to send account created email: " + ex, LogCategory.User));
                }
            }

            return _mapperRegistry.Map<ProfileDto>(returnBo);
        }

        /// <summary>
        ///     Updates the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ProfileDto Update(ProfileBasicDto profile, string id)
        {
            var bo = _mapperRegistry.Map<ProfileEditBasicBo>(profile);
            ProfileBo returnBo = null;
            using (TransactionScope transactionScope = _transactionFactory.Create())
            {
                _profileManager.Update(bo);
                RemoveEmployeeId(profile);
                AddEmployeeId(profile);
                returnBo = _profileManager.FetchById(bo.ProfileId);
                transactionScope.Complete();
            }

            return _mapperRegistry.Map<ProfileDto>(returnBo);
        }

        private void RemoveEmployeeId(ProfileBasicDto profile)
        {
            IList<UserBusinessClaim> claims = null;
           
                claims = _claimProvider.GetUserClaims(profile.LoginId);
           
            if (null != claims)
            {
                foreach (
                    var claim in claims.Where(x => x.Claim != null && x.Claim.EntityClaim == SecuredClaims.EmployeeId))
                {
                    _claimProvider.Remove(claim.Id.ToString());
                }
            }
        }

        private void AddEmployeeId(ProfileBasicDto profile)
        {
            if (string.IsNullOrWhiteSpace(profile.EmployeeId))
                return;
            var claim = new UserBusinessClaim
            {
                Claim = new BusinessClaim {EntityClaim = SecuredClaims.EmployeeId, Value = profile.EmployeeId},
                LoginId = profile.LoginId,
                UserId = profile.ProfileId
            };
            _claimProvider.Add(claim);
        }

        /// <summary>
        /// Removes the User
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            using (TransactionScope transactionScope = _transactionFactory.Create())
            {
                var userId = new Guid(id);
                var profile = _profileManager.FetchById(userId);

                _profileManager.Delete(userId);
                _claimProvider.RemoveAllUserClaims(profile.LoginId);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Searches the specified profile search specification dto.
        /// </summary>
        /// <param name="searchSpecification">The search specification.</param>
        /// <returns></returns>
        public IEnumerable<ProfileDto> Search(ProfileSearchSpecificationDto searchSpecification)
        {
            var specification = _mapperRegistry.Map<ProfileSearchSpecification>(searchSpecification);

            IEnumerable<ProfileBo> result = _profileManager.Search(specification);

            return _mapperRegistry.Map<List<ProfileDto>>(result);
        }

        /// <summary>
        /// Gets the terms and conditions.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public IEnumerable<TermsAndConditionsDto> GetTermsAndConditions(string userId)
        {
            Guard.IsNotNullOrEmpty(userId, "userId");

            IEnumerable<TermsAndConditions> termsAndConditions = _profileManager.GetTermsAndConditions(new Guid(userId));
            return termsAndConditions.Select(i => _mapperRegistry.Map<TermsAndConditionsDto>(i));
        }

        /// <summary>
        /// Accepts the terms and conditions.
        /// </summary>
        /// <param name="termsAndConditionsDto">The terms and conditions dto.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="hasAccepted">if set to <c>true</c> [has accepted].</param>
        public void AcceptTermsAndConditions(TermsAndConditionsDto termsAndConditionsDto, Guid userId, bool hasAccepted)
        {
            Guard.IsNotEmptyGuid(userId, "userId");

            TermsAndConditions termsAndConditions = _mapperRegistry.Map<TermsAndConditions>(termsAndConditionsDto);
            _profileManager.AcceptTermsAndConditions(termsAndConditions, userId, hasAccepted);
        }

        /// <summary>
        /// Search based on the provided company id and criteria.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public CompanyUserSearchModelDto SearchCompanyUsers(string companyId, SearchCriteriaDto searchCriteria)
        {
            //
            // This method (SQL based) is almost ready - except it does not provide total count, so we cannot
            // accurately page with out doing a list all....which is what we are doing below.
            //
            //var profileSearchSpec = new ProfileSearchSpecification() {
            //	CompanyId = companyId.ToGuid(),
            //	Keyword = searchCriteria.Keyword,
            //	StartIndex = searchCriteria.StartIndex,
            //	EndIndex = searchCriteria.EndIndex
            //};
            //var results = _profileManager.Search(profileSearchSpec);

            var companyUserSearchModelDto = new CompanyUserSearchModelDto();
            var results = FetchAllByCompanyId(companyId);

            if (!string.IsNullOrEmpty(searchCriteria.Keyword))
            {
                results = results.Where(c =>
                    c.DisplayName.IndexOf(searchCriteria.Keyword, StringComparison.OrdinalIgnoreCase) != -1 ||
                    c.LoginId.IndexOf(searchCriteria.Keyword, StringComparison.OrdinalIgnoreCase) != -1 ||
                    (c.AboutMe != null &&
                     c.AboutMe.IndexOf(searchCriteria.Keyword, StringComparison.OrdinalIgnoreCase) != -1) ||
                    (c.Title != null &&
                     c.Title.IndexOf(searchCriteria.Keyword, StringComparison.OrdinalIgnoreCase) != -1))
                    .ToList();
            }

            companyUserSearchModelDto.Summary = new SearchSummaryDto();
            companyUserSearchModelDto.Summary.TotalResults = results.Count();

            var pageSize = ((searchCriteria.EndIndex - searchCriteria.StartIndex) + 1);
            if (results.Count > pageSize)
            {
                results = results
                    .Skip((int) searchCriteria.StartIndex)
                    .Take((int) pageSize)
                    .ToList();
            }

            companyUserSearchModelDto.CompanyId = new Guid(companyId);
            companyUserSearchModelDto.SearchCriteria = searchCriteria;
            companyUserSearchModelDto.Users = results.Select(x =>
            {
                var item = _mapperRegistry.Map<ProfileDto>(x);
                return item;
            });

            return companyUserSearchModelDto;
        }


        private ProfileDto MapProfileToDto(ProfileBo bo)
        {
            IList<UserBusinessClaim> userBusinessClaims = _claimProvider.GetUserClaims(bo.LoginId);
            if (null != userBusinessClaims)
                bo.Claims = userBusinessClaims.Select(x => x.Claim).ToList();

            var dto = _mapperRegistry.Map<ProfileDto>(bo);
            if (null != bo.Claims)
            {
                dto.Claims = _mapperRegistry.Map<List<BusinessClaimDto>>(bo.Claims);
                var employeeIdClaim = bo.Claims.FirstOrDefault(x => x.EntityClaim == SecuredClaims.EmployeeId);
                if (null != employeeIdClaim)
                {
                    dto.EmployeeId = employeeIdClaim.Value;
                }
            }

            return dto;
        }


        /// <summary>
        /// Fetches the saved searches by user id.
        /// </summary>
        /// <returns>
        /// List of save search data transfer objects.
        /// </returns>
        public IEnumerable<FavoriteSearchDto> FetchFavoriteSearches()
        {
            var savedSearches = _profileManager.FetchFavoriteSearches();

            var fetchSavedSearchesByUserId = _mapperRegistry.Map<IList<FavoriteSearchDto>>(savedSearches);
            return fetchSavedSearchesByUserId;
        }

        /// <summary>
        /// Fetches the saved searches by user id and location.
        /// </summary>
        /// <param name="location">The location URI.</param>
        /// <returns>
        /// List of save search data transfer objects.
        /// </returns>
        public IEnumerable<FavoriteSearchDto> FetchFavoriteSearchesByLocation(string location)
        {
            Guard.IsNotNullOrEmptyTrimmed(location, "location");

            var savedSearches = _profileManager.FetchFavoriteSearchesByLocation(location);

            return _mapperRegistry.Map<IList<FavoriteSearchDto>>(savedSearches);
        }

        /// <summary>
        /// Fetches the active favorite searches by location.
        /// </summary>        
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public FavoriteSearchDto FetchActiveFavoriteSearchesByLocation(string location)
        {
            Guard.IsNotNullOrEmptyTrimmed(location, "location");

            var savedSearches = _profileManager.FetchActiveFavoriteSearchesByLocation(location.DecodeFrom64());

            return _mapperRegistry.Map<FavoriteSearchDto>(savedSearches);
        }

        /// <summary>
        /// Fetches the available favorite searches by location.
        /// </summary>        
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public IEnumerable<FavoriteSearchDto> FetchAvailableFavoriteSearchesByLocation(string location)
        {
            Guard.IsNotNullOrEmptyTrimmed(location, "location");

            var savedSearches = _profileManager.FetchAvailableFavoriteSearchesByLocation(location.DecodeFrom64());

            return _mapperRegistry.Map<IList<FavoriteSearchDto>>(savedSearches);
        }

        /// <summary>
        /// Creates the saved search.
        /// </summary>
        /// <param name="favoriteSearchDto">The saved search dto.</param>
        /// <returns>
        /// Created saved search.
        /// </returns>
        public FavoriteSearchDto CreateFavoriteSearch(FavoriteSearchDto favoriteSearchDto)
        {
            Guard.IsNotNull(favoriteSearchDto, "FavoriteSearchDto");

            var incomingSavedSearchDto = _mapperRegistry.Map<FavoriteSearch>(favoriteSearchDto);
            FavoriteSearch result;
            using (var transaction = _transactionFactory.Create())
            {
                result = _profileManager.CreateFavoriteSearch(incomingSavedSearchDto);
                transaction.Complete();
            }

            return _mapperRegistry.Map<FavoriteSearchDto>(result);
        }

        /// <summary>
        /// Updates the saved search.
        /// </summary>
        /// <param name="favoriteSearchId"></param>
        /// <param name="favoriteSearchDto">The saved search dto.</param>
        /// <returns>
        /// Updated saved search.
        /// </returns>
        public FavoriteSearchDto UpdateFavoriteSearch(string favoriteSearchId, FavoriteSearchDto favoriteSearchDto)
        {
            Guard.IsNotNull(favoriteSearchDto, "FavoriteSearchDto");
            Guard.AreEqual(favoriteSearchId.ToGuid(), favoriteSearchDto.Id, "favoriteSearchId");

            var incomingSavedSearchDto = _mapperRegistry.Map<FavoriteSearch>(favoriteSearchDto);
            FavoriteSearch result;
            using (var transaction = _transactionFactory.Create())
            {
                result = _profileManager.UpdateFavoriteSearch(incomingSavedSearchDto);
                transaction.Complete();
            }

            return _mapperRegistry.Map<FavoriteSearchDto>(result);
        }

        /// <summary>
        /// Deletes the saved search.
        /// </summary>
        /// <param name="favoriteSearchId">The saved search id.</param>
        public void DeleteFavoriteSearch(string favoriteSearchId)
        {
            _profileManager.DeleteFavoriteSearch(favoriteSearchId.ToGuid());
        }

        /// <summary>
        /// Searches the favorite searches.
        /// </summary>
        /// <param name="searchCriteriaDto">The search criteria dto.</param>
        /// <returns>
        /// Matching search result for favorite search criteris specified.
        /// </returns>
        public FavoriteSearchSearchModelDto SearchFavoriteSearches(SearchCriteriaDto searchCriteriaDto)
        {
            Guard.IsNotNull(searchCriteriaDto, "searchCriteriaDto");
            //Guard against potentially invalid use of search criteria (solely covers the basic invalid uses)
            Guard.IsNull(searchCriteriaDto.UserId, "searchCriteria.UserId");
            Guard.IsNull(searchCriteriaDto.SortBy, "searchCriteria.SortBy");

            var searchCriteria = _mapperRegistry.Map<SearchCriteria>(searchCriteriaDto);

            var result = _profileManager.SearchFavoriteSearches(searchCriteria);

            return _mapperRegistry.Map<FavoriteSearchSearchModelDto>(result);
        }

        /// <summary>
        /// Fetches the favorite searches by id.
        /// </summary>
        /// <param name="favoriteSearchId">The favorite search id.</param>
        /// <returns>
        /// Favorite search with matching id.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public FavoriteSearchDto FetchFavoriteSearchById(string favoriteSearchId)
        {
            var result = _profileManager.FetchFavoriteSearchById(favoriteSearchId.ToGuid());

            return _mapperRegistry.Map<FavoriteSearchDto>(result);
        }

        /// <summary>
        /// Partials the update favorite search.
        /// </summary>
        /// <param name="favoriteSearchId">The favorite search id.</param>
        /// <param name="favoriteSearchDto">The favorite search dto.</param>
        /// <returns>
        /// Updated favorite search.
        /// </returns>
        /// <remarks>
        /// This operation is a partial update, so not all values contained in the favorite search DTO will be respected.  Attempts to update an unsupported member will throw an exception.
        /// </remarks>
        public FavoriteSearchDto PartialUpdateFavoriteSearch(string favoriteSearchId,
            FavoriteSearchDto favoriteSearchDto)
        {
            Guard.IsNotNull(favoriteSearchDto, "FavoriteSearchDto");
            Guard.AreEqual(favoriteSearchId.ToGuid(), favoriteSearchDto.Id, "favoriteSearchId");
            Guard.AreEqual(favoriteSearchDto.CreatedDateTime, DateTime.MinValue, "favoriteSearchDto.CreatedDateTime");
            Guard.AreEqual(favoriteSearchDto.CreatedById, Guid.Empty, "favoriteSearchDto.CreatedById");

            var incomingSavedSearchDto = _mapperRegistry.Map<PartialUpdateFavoriteSearch>(favoriteSearchDto);
            FavoriteSearch result;
            using (var transaction = _transactionFactory.Create())
            {
                result = _profileManager.PartialUpdateFavoriteSearch(incomingSavedSearchDto);
                transaction.Complete();
            }

            return _mapperRegistry.Map<FavoriteSearchDto>(result);
        }

		/// <summary>
		/// Fetches the list of user whom belong to the specified team.
		/// </summary>
		/// <param name="userTeamId">The user team identifier.</param>
		/// <param name="includeTeamMemberTeams">if set to <c>true</c> Includes the Team members of teams owned by team members recursively.</param>
		/// <param name="maxDepth">The maximum depth of recursion.</param>
		/// <returns></returns>
	   public IList<ProfileDto> FetchUsersByTeamId(string userTeamId, string includeTeamMemberTeams  , string maxDepth )
	    {
			Guard.IsNotNull(userTeamId, "userTeamId");
			Guid id;
			Guid.TryParse(userTeamId, out id);
			Guard.IsNotNull(id, "userTeamId");

			bool includeTeam = false;
			bool.TryParse(includeTeamMemberTeams, out includeTeam);
			int depth = 2;
			int.TryParse(maxDepth, out depth);

			IList<ProfileBo> profiles = _profileManager.FetchUsersByTeamId(id, includeTeam, depth);
			var list = _mapperRegistry.Map<List<ProfileDto>>(profiles);
			return list;
	    }
    }
}