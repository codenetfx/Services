using System;
using System.Collections.Generic;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Claim.Domain
{
    /// <summary>
    /// Claim definition class.
    /// </summary>
    public class ClaimDefinition : DomainEntity
    {
        private readonly IList<string> _claimDomainValues = new List<string>();

        /// <summary>
        /// Gets or sets the claim id.
        /// </summary>
        /// <value>
        /// The claim id.
        /// </value>
        public Uri ClaimId { get; set; }

        /// <summary>
        /// Gets or sets the claim domain values.
        /// </summary>
        /// <value>
        /// The claim domain values.
        /// </value>
        public IList<string> ClaimDomainValues
        {
            get { return _claimDomainValues; }
        }
    }
}