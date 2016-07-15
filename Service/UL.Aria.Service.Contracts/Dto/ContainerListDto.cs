using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ContainerPermissionGrouping
    /// </summary>
    [DataContract]
    public class ContainerListDto
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
        /// Gets or sets the container id.
        /// </summary>
        /// <value>The container id.</value>
        [DataMember]
        public Guid ContainerId { get; set; }

        /// <summary>
        /// Gets or sets the type of the asset.
        /// </summary>
        /// <value>The type of the asset.</value>
        [DataMember]
        public string AssetType { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the claim permissions.
        /// </summary>
        /// <value>The claim permissions.</value>
        [DataMember]
        public IList<ContainerListPermissionDto> Permissions { get; set; }
    }
}