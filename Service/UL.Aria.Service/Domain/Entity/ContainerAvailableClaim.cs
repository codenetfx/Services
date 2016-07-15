using System;
using System.Collections.Generic;

using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class ContainerAvailableClaims
    /// </summary>
    public class ContainerAvailableClaim
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public Int64 Id { get; set; }

        /// <summary>
        /// Gets or sets the container id.
        /// </summary>
        /// <value>The container id.</value>
        public Guid ContainerId { get; set; }

        /// <summary>
        ///     Gets or sets the claim.
        /// </summary>
        /// <value>The claim.</value>
        public Claim Claim { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }
}