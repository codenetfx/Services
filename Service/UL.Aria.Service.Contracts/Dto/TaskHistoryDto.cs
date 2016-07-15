using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class TaskHistoryDto
    /// </summary>
    [DataContract]
    public class TaskHistoryDto
    {
        /// <summary>
        ///     Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        [DataMember]
        public string CreatedBy { get; set; }

        /// <summary>
        ///     Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     Gets or sets the task.
        /// </summary>
        /// <value>The task.</value>
        [DataMember]
        public TaskDto Task { get; set; }
    }
}