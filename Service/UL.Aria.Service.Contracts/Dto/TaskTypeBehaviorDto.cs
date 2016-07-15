using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Data Contract for Task Type Behaviors
    /// </summary>
    [DataContract]
    public class TaskTypeBehaviorDto : TrackedEntityDto
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
        /// Gets or sets Available Behavior Id.
        /// </summary>
        /// <value>
        /// The task type available behavior.
        /// </value>
        [DataMember]
        public Guid TaskTypeAvailableBehaviorId { get; set; }

        /// <summary>
        /// Gets or sets the name of the behavior.
        /// </summary>
        /// <value>
        /// The name of the behavior.
        /// </value>
        [DataMember]
        public string BehaviorName { get; set; }

        /// <summary>
        /// Gets or sets the Field Id.
        /// </summary>
        /// <value>
        /// The task type available behavior field.
        /// </value>
        [DataMember]
        public Guid? TaskTypeAvailableBehaviorFieldId { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        [DataMember]
        public string FieldName { get; set; }
    }
}
