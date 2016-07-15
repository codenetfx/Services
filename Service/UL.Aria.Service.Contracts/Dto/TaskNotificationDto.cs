using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Class TaskNotificationDto
    /// </summary>
    [DataContract]
    public class TaskNotificationDto : TrackedEntityDto
    {
        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        [DataMember]
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [DataMember]
        public string Email { get; set; }
    }
}
