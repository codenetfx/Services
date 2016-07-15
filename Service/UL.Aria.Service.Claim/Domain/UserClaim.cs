using System;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Claim.Domain
{
    /// <summary>
    /// User claim class.
    /// </summary>
    public class UserClaim : DomainEntity
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the claim id.
        /// </summary>
        /// <value>
        /// The claim id.
        /// </value>
        public Uri ClaimId { get; set; }

        /// <summary>
        /// Gets or sets the claim value.
        /// </summary>
        /// <value>
        /// The claim value.
        /// </value>
        public string ClaimValue { get; set; }

        /// <summary>
        /// Gets or sets the login id of the user.
        /// </summary>
        /// <value>
        /// The login id.
        /// </value>
        public string LoginId { get; set; }
    }
}