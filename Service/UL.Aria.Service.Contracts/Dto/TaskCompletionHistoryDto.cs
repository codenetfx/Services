using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Class TaskCompletionHistoryDto
    /// </summary>
    [DataContract]
    public class TaskCompletionHistoryDto
    {
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        [DataMember]
        public string Completion { get; set; }
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