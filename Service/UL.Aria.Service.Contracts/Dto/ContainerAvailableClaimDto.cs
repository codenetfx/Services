using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ContainerAvailableClaims
    /// </summary>
    [DataContract]
    public class ContainerAvailableClaimDto
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>
        ///     The id.
        /// </value>
        [DataMember]
        public Int64 Id { get; set; }

        /// <summary>
        /// Gets or sets the container id.
        /// </summary>
        /// <value>The container id.</value>
        [DataMember]
        public Guid ContainerId { get; set; }

        /// <summary>
        ///     Gets or sets the claim.
        /// </summary>
        /// <value>The claim.</value>
        [DataMember]
        public ClaimDto Claim { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [DataMember]
        public string Value { get; set; }
    }
}