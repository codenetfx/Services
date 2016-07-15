using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// A Business claim for a user.
    /// </summary>
    public class UserBusinessClaim
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the login id.
        /// </summary>
        /// <value>
        /// The login id.
        /// </value>
        public string LoginId { get; set; }

        /// <summary>
        /// Gets or sets the claim id.
        /// </summary>
        /// <value>
        /// The claim id.
        /// </value>
        public BusinessClaim Claim { get; set; }
    }
}
