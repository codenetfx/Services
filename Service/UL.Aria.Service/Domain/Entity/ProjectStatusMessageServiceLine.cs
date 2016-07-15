using System;
using System.Runtime.Serialization;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Status information pertaining to order service lines
    /// </summary>
    [DataContract]
    public class ProjectStatusMessageServiceLine
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
        public IncomingOrderServiceLine ServiceLine { get; set; }
    }
}