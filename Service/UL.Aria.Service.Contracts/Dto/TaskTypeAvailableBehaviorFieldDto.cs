using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Data Contract for Task Type Behaviors
    /// </summary>
    [DataContract]
    public class TaskTypeAvailableBehaviorFieldDto : TrackedEntityDto
    {
        /// <summary>
        /// The restricted to project handler behavior
        /// </summary>
        public const string RestrictedToProjectHandlerBehavior = "00000000-0000-0000-0000-100000000001";

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the task type available behavior identifier.
        /// </summary>
        /// <value>
        /// The task type available behavior identifier.
        /// </value>
        [DataMember]
        public Guid TaskTypeAvailableBehaviorId { get; set; }
    }
}