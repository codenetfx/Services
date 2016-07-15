using System;
using System.IO;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class AriaAsset.
    /// </summary>
    [DataContract]
    public class AriaAsset
    {
        /// <summary>
        ///     Gets or sets the container identifier.
        /// </summary>
        /// <value>The container identifier.</value>
        [DataMember]
        public Guid ContainerId { get; set; }

        /// <summary>
        ///     Gets or sets the asset identifier.
        /// </summary>
        /// <value>The asset identifier.</value>
        [DataMember]
        public Guid AssetId { get; set; }

        /// <summary>
        ///     Gets or sets the type of the stream.
        /// </summary>
        /// <value>The type of the stream.</value>
        [DataMember]
        public string StreamType { get; set; }

        /// <summary>
        ///     Gets or sets the stream.
        /// </summary>
        /// <value>The stream.</value>
        [DataMember]
        public byte[] Stream { get; set; }
    }
}