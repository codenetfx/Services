using System;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Claim.Domain
{
    /// <summary>
    /// User claim history entity.
    /// </summary>
    public class UserClaimHistory : ValueEntity
    {
        /// <summary>
        /// Gets or sets the claim id.
        /// </summary>
        /// <value>
        /// The claim id.
        /// </value>
        public Uri ClaimId { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the change by.
        /// </summary>
        /// <value>
        /// The change by.
        /// </value>
        public string ChangedBy { get; set; }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        public DateTime ChangeDate { get; set; }

        /// <summary>
        /// Gets or sets the login id.
        /// </summary>
        /// <value>
        /// The login id.
        /// </value>
        public string LoginId { get; set; }
    }
}