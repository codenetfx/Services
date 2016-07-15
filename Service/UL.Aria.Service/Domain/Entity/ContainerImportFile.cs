using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Tracking for importing files from other containers.
    /// </summary>
    public class ContainerImportFile
    {
        /// <summary>
        /// Gets or sets the original id.
        /// </summary>
        /// <value>
        /// The external id.
        /// </value>
        public Guid OriginalId { get; set; }

        /// <summary>
        /// Gets or sets the original container.
        /// </summary>
        /// <value>
        /// The original container.
        /// </value>
        public Guid OriginalContainerId { get; set; }


        /// <summary>
        /// Gets or sets the new id.
        /// </summary>
        /// <value>
        /// The new id.
        /// </value>
        public Guid NewId { get; set; }

    }
}
