using System;

using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Class Claim
    /// </summary>
    public class Claim
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>
        ///     The id.
        /// </value>
        public Int64 Id { get; set; }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>The URI.</value>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
    }
}