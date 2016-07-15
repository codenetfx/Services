using System;

using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class ContainerListPermission
    /// </summary>
    public class ContainerListPermission
    {
        /// <summary>
        /// Gets or sets the container list id.
        /// </summary>
        /// <value>The container list id.</value>
        public Int64 ContainerListId { get; set; }

        /// <summary>
        /// Gets or sets the container available claim id.
        /// </summary>
        /// <value>The container available claim id.</value>
        public Int64 ContainerAvailableClaimId { get; set; }

        /// <summary>
        /// Gets or sets the claim.
        /// </summary>
        /// <value>The claim.</value>
        public System.Security.Claims.Claim Claim { get; set; }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>The permission.</value>
        public string Permission { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>The name of the group.</value>
        public string GroupName { get; set; }
    }
}