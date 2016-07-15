using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Claim.Contract
{
    /// <summary>
    /// User role DTO class.
    /// </summary>
    [DataContract]
    public class UserClaimDto
    {

        /// <summary>
        /// Gets or sets the user claim id.
        /// </summary>
        /// <value>
        /// The user claim id.
        /// </value>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        [DataMember]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the login id.
        /// </summary>
        /// <value>
        /// The login id.
        /// </value>
        [DataMember]
        public string LoginId { get; set; }
        
        /// <summary>
        /// Gets or sets the claim id.
        /// </summary>
        /// <value>
        /// The claim id.
        /// </value>
        [DataMember]        
        public Uri ClaimId { get;  set; }

        /// <summary>
        /// Gets or sets the claim value.
        /// </summary>
        /// <value>
        /// The claim value.
        /// </value>
        [DataMember]
        public string ClaimValue { get; set; }
    }
}