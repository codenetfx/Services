using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class AriaMetaDataItem
    /// </summary>
    [DataContract]
    public class AriaMetaDataItem
    {
        /// <summary>
        ///     Gets or sets the asset id.
        /// </summary>
        /// <value>The asset id.</value>
        [DataMember]
        public Guid AssetId { get; set; }

        /// <summary>
        ///     Gets or sets the meta data.
        /// </summary>
        /// <value>The meta data.</value>
        [DataMember]
        public string MetaData { get; set; }
    }
}