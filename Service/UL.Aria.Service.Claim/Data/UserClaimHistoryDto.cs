using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Claim.Data
{
    /// <summary>
    /// User claim history data transfer class.
    /// </summary>
    [DataContract]
    public class UserClaimHistoryDto
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