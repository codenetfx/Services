using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Class TaskStatusHistoryDto
    /// </summary>
    [DataContract]
    public class TaskStatusHistoryDto
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        [DataMember]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        [DataMember]
        public DateTime CreatedDate { get; set; }
    }
}