using System;
using System.Collections.Generic;
using UL.Aria.Service.Claim.Data;
using UL.Aria.Service.Claim.Domain;

namespace UL.Aria.Service.Claim.Provider
{
    /// <summary>
    /// User claim provider class.
    /// </summary>
    public class UserClaimProvider : IUserClaimProvider
    {
        private readonly IUserClaimRepository _userClaimRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaimProvider" /> class.
        /// </summary>
        /// <param name="userClaimRepository">The user claim repository.</param>
        public UserClaimProvider(IUserClaimRepository userClaimRepository)
        {
            _userClaimRepository = userClaimRepository;
        }

        /// <summary>
        /// Adds the specified user claim.
        /// </summary>
        /// <param name="userClaim">The user claim.</param>
        public void Add(UserClaim userClaim)
        {
            _userClaimRepository.Add(userClaim);

        }

        /// <summary>
        /// Removes the specified user claim id.
        /// </summary>
        /// <param name="userClaimId">The user claim id.</param>
        public void Remove(Guid userClaimId)
        {
            _userClaimRepository.Remove(userClaimId);
        }

        /// <summary>
        /// Gets the user claim values.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<UserClaim> GetUserClaimValues(Uri claimId, string loginId)
        {
            return _userClaimRepository.FindUserClaimValues(claimId, loginId);
        }

        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        public IList<UserClaim> GetUserClaimsByValue(string claimValue)
        {
            return _userClaimRepository.FindUserClaimsByValue(claimValue);
        }

        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        public IList<UserClaim> GetUserClaimsByIdAndValue(string claimId, string claimValue)
        {
            return _userClaimRepository.FindUserClaimsByIdAndValue(claimId, claimValue);
        }

        /// <summary>
        /// Gets the user claim history.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<UserClaimHistory> GetUserClaimHistory(Uri claimId, string loginId)
        {
            return _userClaimRepository.FindUserClaimHistory(claimId, loginId);
        }

        /// <summary>
        /// Gets all the user claim values for a user.
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns>
        /// All the user claims values for a particular user.
        /// </returns>
        public IList<UserClaim> GetUserClaimValues(string loginId)
        {
            return _userClaimRepository.FindUserClaimValues(loginId);
        }

        /// <summary>
        /// Removes the user claims.
        /// </summary>
        /// <param name="loginId"></param>
        public void RemoveUserClaims(string loginId)
        {
            _userClaimRepository.RemoveUserClaims(loginId);
        }
    }
}