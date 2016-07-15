using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Interface for a 
    /// </summary>
    public interface IUserBusinessClaimProvider
    {
        /// <summary>
        /// Adds the specified user claim dto.
        /// </summary>
        /// <param name="userClaim">The user claim dto.</param>
        void Add(UserBusinessClaim userClaim);

        /// <summary>
        /// Removes the specified user claim.
        /// </summary>
        /// <param name="userClaimId">The user claim id.</param>
        void Remove(string userClaimId);

        /// <summary>
        /// Removes user's claim.
        /// </summary>
        /// <param name="loginId"></param>
        void RemoveAllUserClaims(string loginId);

        /// <summary>
        /// Gets the user claim values.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        IList<UserBusinessClaim> GetUserClaimValues(BusinessClaim claim, string loginId);

        /// <summary>
        /// Gets the user claims.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        IList<UserBusinessClaim> GetUserClaims(string userId);

        /// <summary>
        /// Gets the user claim history.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        IList<UserBusinessClaimHistory> GetUserClaimHistory(BusinessClaim claim, string userId);


        /// <summary>
        /// Finds the claims.
        /// </summary>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        IList<UserBusinessClaim> GetUserClaimsByValue(string claimValue);

        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        IList<UserBusinessClaim> GetUserClaimsByClaimAndValue(string claim, string claimValue);
    }
}
