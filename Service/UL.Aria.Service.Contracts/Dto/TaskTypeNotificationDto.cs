using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Class TaskTypeNotificationDto
    /// </summary>
    [DataContract]
    public class TaskTypeNotificationDto : TrackedEntityDto
    {
        /// <summary>
        /// Gets or sets the task type identifier.
        /// </summary>
        /// <value>
        /// The task type identifier.
        /// </value>
        [DataMember]
        public Guid TaskTypeId { get; set; }

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
