using System;

namespace UL.Aria.Service.Domain
{
    /// <summary>
    /// represents meta data of the file maintained in the scratch space
    /// </summary>
    public class ScratchFileInfo
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the creation time UTC.
        /// </summary>
        /// <value>
        /// The creation time UTC.
        /// </value>
        public DateTime CreationTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the last access time.
        /// </summary>
        /// <value>
        /// The last access time.
        /// </value>
        public DateTime LastAccessTime { get; set; }

        /// <summary>
        /// Gets or sets the last access time UTC.
        /// </summary>
        /// <value>
        /// The last access time UTC.
        /// </value>
        public DateTime LastAccessTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the last write time.
        /// </summary>
        /// <value>
        /// The last write time.
        /// </value>
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// Gets or sets the last write time UTC.
        /// </summary>
        /// <value>
        /// The last write time UTC.
        /// </value>
        public DateTime LastWriteTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public long Size { get; set; }
 
    }
}