using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class Claim
    /// </summary>
    [DataContract]
    public class ClaimDto
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
        ///     Gets or sets the URI.
        /// </summary>
        /// <value>The URI.</value>
        [DataMember]
        public string Uri { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataMember]
        public string Description { get; set; }
    }
}