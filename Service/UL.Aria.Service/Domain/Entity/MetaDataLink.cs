using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class MetaDataLink.
    /// </summary>
    public class MetaDataLink
    {
        /// <summary>
        ///     Gets or sets the parent id.
        /// </summary>
        /// <value>The parent id.</value>
        public Guid ParentId { get; set; }

        /// <summary>
        ///     Gets or sets the asset id.
        /// </summary>
        /// <value>The asset id.</value>
        public Guid AssetId { get; set; }
    }
}