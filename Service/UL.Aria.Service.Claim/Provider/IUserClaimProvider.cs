using System;
using System.Collections.Generic;
using UL.Aria.Service.Claim.Domain;

namespace UL.Aria.Service.Claim.Provider
{
    /// <summary>
    /// User claim provider class.
    /// </summary>
    public interface IUserClaimProvider
    {
        /// <summary>
        /// Adds the specified user claim.
        /// </summary>
        /// <param name="userClaim">The user claim.</param>
        void Add(UserClaim userClaim);

        /// <summary>
        /// Removes the specified user claim id.
        /// </summary>
        /// <param name="userClaimId">The user claim id.</param>
        void Remove(Guid userClaimId);

        /// <summary>
        /// Gets the user claim values.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="loginId"></param>
        /// <returns>All the user claims values for a particular claim id and user.</returns>
        IList<UserClaim> GetUserClaimValues(Uri claimId, string loginId);


        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        IList<UserClaim> GetUserClaimsByValue(string claimValue);

        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        IList<UserClaim> GetUserClaimsByIdAndValue(string claimId, string claimValue);


        /// <summary>
        /// Gets the user claim history.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="loginId">The login id.</param>
        /// <returns>
        /// All the user claims history for a particular claim id and user.
        /// </returns>
        IList<UserClaimHistory> GetUserClaimHistory(Uri claimId, string loginId);

        /// <summary>
        /// Gets all the user claim values for a user.
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns>All the user claims values for a particular user.</returns>
        IList<UserClaim> GetUserClaimValues(string loginId);

        /// <summary>
        /// Removes the user claims.
        /// </summary>
        /// <param name="loginId"></param>
        void RemoveUserClaims(string loginId);
    }

}