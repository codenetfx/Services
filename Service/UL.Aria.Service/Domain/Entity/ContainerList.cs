using System;
using System.Collections.Generic;

using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class ContainerList
    /// </summary>
    public class ContainerList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerList"/> class.
        /// </summary>
        public ContainerList()
        {
            Permissions = new List<ContainerListPermission>();
        }

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
        /// Gets or sets the type of the asset.
        /// </summary>
        /// <value>The type of the asset.</value>
        public string AssetType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the claim permissions.
        /// </summary>
        /// <value>The claim permissions.</value>
        public IList<ContainerListPermission> Permissions { get; set; }

    }
}