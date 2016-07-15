using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// A history for a business claim for a user.
    /// </summary>
    public class UserBusinessClaimHistory
    {
        /// <summary>
        /// Gets or sets the claim.
        /// </summary>
        /// <value>
        /// The claim id.
        /// </value>
        public BusinessClaim Claim { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the changed by.
        /// </summary>
        /// <value>
        /// The changed by.
        /// </value>
        public string ChangedBy { get; set; }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        public DateTime ChangeDate { get; set; }
    }
}