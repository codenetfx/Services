using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ContainerPermissionGroupingClaim
    /// </summary>
    [DataContract]
    public class ContainerListPermissionDto
    {
        /// <summary>
        /// Gets or sets the container list id.
        /// </summary>
        /// <value>The container list id.</value>
        [DataMember]
        public Int64 ContainerListId { get; set; }

        /// <summary>
        /// Gets or sets the container available claim id.
        /// </summary>
        /// <value>The container available claim id.</value>
        [DataMember]
        public Int64 ContainerAvailableClaimId { get; set; }

        /// <summary>
        /// Gets or sets the claim.
        /// </summary>
        /// <value>The claim.</value>
        [DataMember]
        public System.Security.Claims.Claim Claim { get; set; }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>The permission.</value>
        [DataMember]
        public string Permission { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>The name of the group.</value>
        [DataMember]
        public string GroupName { get; set; }
    }
}