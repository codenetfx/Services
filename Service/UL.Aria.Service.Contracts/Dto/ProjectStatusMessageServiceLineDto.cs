using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Status information pertaining to order service lines
    /// </summary>
    [DataContract]
    public class ProjectStatusMessageServiceLineDto
    {
        /// <summary>
        /// Gets or sets the project service lind id.
        /// </summary>
        /// <value>
        /// The project service lind id.
        /// </value>
        [DataMember]
        public Guid ProjectServiceLineId { get; set; }

        /// <summary>
        /// Gets or sets the service line action.
        /// </summary>
        /// <value>
        /// The service line action.
        /// </value>
        [DataMember]
        public ServiceLineAction ServiceLineAction { get; set; }

        /// <summary>
        /// Gets or sets the service line.
        /// </summary>
        /// <value>
        /// The service line.
        /// </value>
        [DataMember]
        public IncomingOrderServiceLineDto ServiceLine { get; set; }
    }
}