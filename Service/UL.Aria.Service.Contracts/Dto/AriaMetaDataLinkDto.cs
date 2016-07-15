using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class AriaMetaDataLinkDto.
    /// </summary>
    [DataContract]
    public class AriaMetaDataLinkDto
    {
        /// <summary>
        ///     Gets or sets the parent id.
        /// </summary>
        /// <value>The parent id.</value>
        [DataMember]
        public Guid ParentId { get; set; }

        /// <summary>
        ///     Gets or sets the asset id.
        /// </summary>
        /// <value>The asset id.</value>
        [DataMember]
        public Guid AssetId { get; set; }
    }
}