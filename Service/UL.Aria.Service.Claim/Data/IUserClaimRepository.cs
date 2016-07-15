using System;
using System.Collections.Generic;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Claim.Domain;

namespace UL.Aria.Service.Claim.Data
{
    /// <summary>
    /// User claim repository interface.
    /// </summary>
    public interface IUserClaimRepository : IRepositoryBase<UserClaim>
    {
        /// <summary>
        /// Gets the user claim values.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        IList<UserClaim> FindUserClaimValues(Uri claimId, string loginId);

        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        IList<UserClaim> FindUserClaimValues(string loginId);

        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        IList<UserClaim> FindUserClaimsByValue(string claimValue);

        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        IList<UserClaim> FindUserClaimsByIdAndValue(string claimId, string claimValue);

        /// <summary>
        /// Finds the user claim history.
        /// </summary>
        /// <param name="claimId">The expected claim id.</param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        IList<UserClaimHistory> FindUserClaimHistory(Uri claimId, string loginId);

        /// <summary>
        /// Removes the user claims.
        /// </summary>
        /// <param name="loginId"></param>
        void RemoveUserClaims(string loginId);
    }
}