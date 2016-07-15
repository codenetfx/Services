using System;

namespace UL.Aria.Service.Domain
{
    /// <summary>
    /// Class for metadata about a scratchfile document that is ready to be permanently uploaded into the system.
    /// </summary>
    public class ScratchFileUpload
    {
        /// <summary>
        /// Gets or sets the scratch file info.
        /// </summary>
        /// <value>
        /// The scratch file info.
        /// </value>
        public ScratchFileInfo ScratchFileInfo { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid? Id { get; set; }
    }
}