using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Data transfer object for business claims.
    /// </summary>
    [DataContract]
    public class BusinessClaimDto
    {
        /// <summary>
        /// Gets or sets the entity id. (e.g. a Product id)
        /// </summary>
        /// <value>
        /// The entity id.
        /// </value>
        [DataMember]
        public Guid? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity claim. (e.g. Read)
        /// </summary>
        /// <value>
        /// The entity claim.
        /// </value>
        [DataMember]
        public string EntityClaim { get; set; }

        /// <summary>
        /// Gets or sets the claim. (e.g. Allow)
        /// </summary>
        /// <value>
        /// The claim.
        /// </value>
        [DataMember] 
        public string Value { get; set; }

    }
}