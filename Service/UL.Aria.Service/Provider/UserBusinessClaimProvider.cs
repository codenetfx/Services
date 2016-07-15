using System;
using System.Collections.Generic;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Claim.Contract;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provider UserBusinessClaim
    /// </summary>
    public class UserBusinessClaimProvider : IUserBusinessClaimProvider
    {
        private readonly IUserClaimService _userClaimService;
        private readonly IMapperRegistry _mapperRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBusinessClaimProvider" /> class.
        /// </summary>
        /// <param name="userClaimService">The user claim service.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public UserBusinessClaimProvider(IUserClaimService userClaimService, IMapperRegistry mapperRegistry )
        {
            _userClaimService = userClaimService;
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        /// Adds the specified user claim dto.
        /// </summary>
        /// <param name="userClaim">The user claim dto.</param>
        public void Add(UserBusinessClaim userClaim)
        {
            if (!userClaim.Id.HasValue || userClaim.Id == default (Guid))
                userClaim.Id = Guid.NewGuid();

            var dtoClaim = _mapperRegistry.Map<UserClaimDto>(userClaim);
            _userClaimService.Add(dtoClaim);
        }

        /// <summary>
        /// Removes the specified user claim.
        /// </summary>
        /// <param name="userClaimId">The user claim id.</param>
        public void Remove(string userClaimId)
        {
            _userClaimService.Remove(userClaimId);
        }

        /// <summary>
        /// Removes user's claim.
        /// </summary>
        /// <param name="loginId"></param>
        public void RemoveAllUserClaims(string loginId)
        {
            _userClaimService.RemoveUserClaims(loginId);
        }

        /// <summary>
        /// Gets the user claim values.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public IList<UserBusinessClaim> GetUserClaimValues(BusinessClaim claim, string loginId)
        {
            var serviceDtoList = _userClaimService.GetUserClaimValues( GetClaimId(claim).EncodeTo64(), loginId);
            return _mapperRegistry.Map<List<UserBusinessClaim>>(serviceDtoList);
        }

        /// <summary>
        /// Gets the user claims.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public IList<UserBusinessClaim> GetUserClaims(string userId)
        {
            var serviceDtoList = _userClaimService.GetUserClaims(userId);
            return _mapperRegistry.Map<List<UserBusinessClaim>>(serviceDtoList);
        }

        /// <summary>
        /// Gets the user claim history.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public IList<UserBusinessClaimHistory> GetUserClaimHistory(BusinessClaim claim, string userId)
        {
            var serviceDtoList = _userClaimService.GetUserClaimHistory(GetClaimId(claim).EncodeTo64(), userId);

            return _mapperRegistry.Map<List<UserBusinessClaimHistory>>(serviceDtoList);
        }

        /// <summary>
        /// Finds the claims.
        /// </summary>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        public IList<UserBusinessClaim> GetUserClaimsByValue(string claimValue)
        {
            var serviceDtoList = _userClaimService.GetUserClaimsByValue(claimValue);
            return _mapperRegistry.Map<List<UserBusinessClaim>>(serviceDtoList);
        }

        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        public IList<UserBusinessClaim> GetUserClaimsByClaimAndValue(string claim, string claimValue)
        {
            var serviceDtoList = _userClaimService.GetUserClaimsByIdAndValue(claim.EncodeTo64(), claimValue);
            return _mapperRegistry.Map<List<UserBusinessClaim>>(serviceDtoList);
        }

        private static string GetClaimId(BusinessClaim claim)
        {
            return new Uri(claim.EntityClaim).ToString();
        }
    }
}