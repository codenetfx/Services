using System;
using System.Collections.Generic;
using System.Globalization;

using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Claim.Data;
using UL.Aria.Service.Claim.Domain;
using UL.Aria.Service.Claim.Provider;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Business message user claim provider decorator to provide logging services.
    /// </summary>
    public class BusinessMessageUserClaimProviderDecorator : IUserClaimProvider
    {
        private readonly IBusinessMessageProvider _businessMessageProvider;
        private readonly IUserClaimProvider _instanceToWrap;
        private readonly IUserClaimRepository _userClaimRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BusinessMessageUserClaimProviderDecorator" /> class.
        /// </summary>
        /// <param name="instanceToWrap">The instance to wrap.</param>
        /// <param name="businessMessageProvider">The business message provider.</param>
        /// <param name="userClaimRepository">The user claim repository.</param>
        public BusinessMessageUserClaimProviderDecorator(IUserClaimProvider instanceToWrap,
            IBusinessMessageProvider businessMessageProvider, IUserClaimRepository userClaimRepository)
        {
            _instanceToWrap = instanceToWrap;
            _businessMessageProvider = businessMessageProvider;
            _userClaimRepository = userClaimRepository;
        }

        /// <summary>
        ///     Adds the specified user claim.
        /// </summary>
        /// <param name="userClaim">The user claim.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Add(UserClaim userClaim)
        {
            _instanceToWrap.Add(userClaim);

            _businessMessageProvider.Publish(AuditMessageIdEnumDto.AddUserClaim,
                string.Format(CultureInfo.InvariantCulture,
                    "Add user claim of {0} (value {1}) for user {2}.", userClaim.ClaimId, userClaim.ClaimValue,
                    userClaim.UserId),
                new Dictionary<string, string>
                {
                    {"claimId", userClaim.ClaimId.ToString()},
                    {"claimValue", userClaim.ClaimValue},
                    {"userId", userClaim.UserId.ToString()}
                });
        }

        /// <summary>
        ///     Removes the specified user claim id.
        /// </summary>
        /// <param name="userClaimId">The user claim id.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Remove(Guid userClaimId)
        {
            var userClaim = _userClaimRepository.FindById(userClaimId);

            _instanceToWrap.Remove(userClaimId);

            _businessMessageProvider.Publish(AuditMessageIdEnumDto.RemoveUserClaim,
                string.Format(CultureInfo.InvariantCulture,
                    "Remove user claim of {0} (value {1}) for user {2}.", userClaim.ClaimId, userClaim.ClaimValue,
                    userClaim.UserId),
                new Dictionary<string, string>
                {
                    {"claimId", userClaim.ClaimId.ToString()},
                    {"claimValue", userClaim.ClaimValue},
                    {"userId", userClaim.UserId.ToString()}
                });
        }

        /// <summary>
        ///     Gets the user claim values.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public IList<UserClaim> GetUserClaimValues(Uri claimId, string loginId)
        {
            return _instanceToWrap.GetUserClaimValues(claimId, loginId);
        }

        /// <summary>
        ///     Finds the user claim values.
        /// </summary>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        public IList<UserClaim> GetUserClaimsByValue(string claimValue)
        {
            return _instanceToWrap.GetUserClaimsByValue(claimValue);
        }

        /// <summary>
        ///     Finds the user claim values.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        public IList<UserClaim> GetUserClaimsByIdAndValue(string claimId, string claimValue)
        {
            return _instanceToWrap.GetUserClaimsByIdAndValue(claimId, claimValue);
        }

        /// <summary>
        ///     Gets the user claim history.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<UserClaimHistory> GetUserClaimHistory(Uri claimId, string loginId)
        {
            return _instanceToWrap.GetUserClaimHistory(claimId, loginId);
        }

        /// <summary>
        ///     Gets all the user claim values for a user.
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns>
        ///     All the user claims values for a particular user.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<UserClaim> GetUserClaimValues(string loginId)
        {
            return _instanceToWrap.GetUserClaimValues(loginId);
        }

        /// <summary>
        /// Removes the user claims.
        /// </summary>
        /// <param name="loginId"></param>
        public void RemoveUserClaims(string loginId)
        {
            _instanceToWrap.RemoveUserClaims(loginId);

            _businessMessageProvider.Publish(AuditMessageIdEnumDto.RemoveAllUserClaims,
                string.Format(CultureInfo.InvariantCulture,
                    "Remove all claims for user{0}.",loginId),
                new Dictionary<string, string>
                {
                    {"userId",loginId}
                });
            
        }
    }
}