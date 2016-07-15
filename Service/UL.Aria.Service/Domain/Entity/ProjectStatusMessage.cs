using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Status messages pertaining to projects
    /// </summary>
    [DataContract]
    public class ProjectStatusMessage : TrackedDomainEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectStatusMessage" /> class.
        /// </summary>
        public ProjectStatusMessage() : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectStatusMessage" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public ProjectStatusMessage(Guid? id) : base(id)
        {
            ProjectServiceLineStatuses = new List<ProjectStatusMessageServiceLine>();
        }

        /// <summary>
        ///     Gets or sets the project id.
        /// </summary>
        /// <value>
        ///     The project id.
        /// </value>
        [DataMember]
        public Guid ProjectId { get; set; }

        /// <summary>
        ///     Gets or sets the new status.
        /// </summary>
        /// <value>
        ///     The new status.
        /// </value>
        [DataMember]
        public ProjectStatusEnumDto NewStatus { get; set; }

        /// <summary>
        ///     Gets or sets the old status.
        /// </summary>
        /// <value>
        ///     The old status.
        /// </value>
        [DataMember]
        public ProjectStatusEnumDto OldStatus { get; set; }

        /// <summary>
        ///     Gets or sets the name of the project.
        /// </summary>
        /// <value>
        ///     The name of the project.
        /// </value>
        [DataMember]
        public string ProjectName { get; set; }

        /// <summary>
        ///     Gets or sets the project number.
        /// </summary>
        /// <value>
        ///     The project number.
        /// </value>
        [DataMember]
        public string ProjectNumber { get; set; }

        /// <summary>
        ///     Gets or sets the order number.
        /// </summary>
        /// <value>
        ///     The order number.
        /// </value>
        [DataMember]
        public string OrderNumber { get; set; }

        /// <summary>
        ///     Gets or sets the external project id.
        /// </summary>
        /// <value>
        ///     The external project id.
        /// </value>
        [DataMember]
        public string ExternalProjectId { get; set; }

        /// <summary>
        ///     Gets or sets the order service line statuses.
        /// </summary>
        /// <value>
        ///     The order service line statuses.
        /// </value>
        [DataMember]
        public IList<ProjectStatusMessageServiceLine> ProjectServiceLineStatuses { get; set; }

        /// <summary>
        ///     Gets or sets the event id.
        /// </summary>
        /// <value>
        ///     The event id.
        /// </value>
        [DataMember]
        public short EventId { get; set; }

        /// <summary>
        ///     Gets or sets the work order business component id.
        /// </summary>
        /// <value>The work order business component id.</value>
        [DataMember]
        public string WorkOrderBusinessComponentId { get; set; }

        /// <summary>
        ///     Gets or sets the work order id.
        /// </summary>
        /// <value>The work order id.</value>
        [DataMember]
        public string WorkOrderId { get; set; }

        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        [DataMember]
        public Guid CorrelationId { get; set; }
    }
}