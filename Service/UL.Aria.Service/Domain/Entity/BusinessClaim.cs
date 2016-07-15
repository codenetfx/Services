using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Specific Claim details for an entity.
    /// </summary>
    public class BusinessClaim
    {
        /// <summary>
        /// Gets or sets the entity id. (e.g. a Product id)
        /// </summary>
        /// <value>
        /// The entity id.
        /// </value>
        public Guid? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity claim. (e.g. Read)
        /// </summary>
        /// <value>
        /// The entity claim.
        /// </value>
        public string EntityClaim { get; set; }

        /// <summary>
        /// Gets or sets the claim. (e.g. Allow)
        /// </summary>
        /// <value>
        /// The claim.
        /// </value>
        public string Value { get; set; }
    }
}