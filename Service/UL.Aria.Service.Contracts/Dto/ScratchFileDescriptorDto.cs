using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ScratchFileDescriptorDto
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        [DataMember]
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        [DataMember]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the creation time UTC.
        /// </summary>
        /// <value>
        /// The creation time UTC.
        /// </value>
        [DataMember]
        public DateTime CreationTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the last access time.
        /// </summary>
        /// <value>
        /// The last access time.
        /// </value>
        [DataMember]
        public DateTime LastAccessTime { get; set; }

        /// <summary>
        /// Gets or sets the last access time UTC.
        /// </summary>
        /// <value>
        /// The last access time UTC.
        /// </value>
        [DataMember]
        public DateTime LastAccessTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the last write time.
        /// </summary>
        /// <value>
        /// The last write time.
        /// </value>
        [DataMember]
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// Gets or sets the last write time UTC.
        /// </summary>
        /// <value>
        /// The last write time UTC.
        /// </value>
        [DataMember]
        public DateTime LastWriteTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        [DataMember]
        public long Size { get; set; }
 
    }
}