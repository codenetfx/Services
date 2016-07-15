using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Claim.Contract
{
    /// <summary>
    /// Claim definition data transfer class.
    /// </summary>
    [DataContract]
    public class ClaimDefinitionDto
    {
        /// <summary>
        /// Gets or sets the claim id.
        /// </summary>
        /// <value>
        /// The claim id.
        /// </value>
        [DataMember]
        public Uri ClaimId { get; set; }

        /// <summary>
        /// Gets or sets the domain values.
        /// </summary>
        /// <value>
        /// The domain values.
        /// </value>
        [DataMember]
        public IList<string> ClaimDomainValues { get; set; }
    }
}