using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Data transfer object for history for business claims
    /// </summary>
    [DataContract]
    public class UserBusinessClaimHistoryDto
    {
        /// <summary>
        /// Gets or sets the claim.
        /// </summary>
        /// <value>
        /// The claim id.
        /// </value>
        [DataMember]
        public BusinessClaimDto Claim { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the changed by.
        /// </summary>
        /// <value>
        /// The changed by.
        /// </value>
        [DataMember]
        public string ChangedBy { get; set; }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        [DataMember]
        public DateTime ChangeDate { get; set; }
    }
}