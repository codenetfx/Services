using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Domain;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Notification Domain Enity.
    /// </summary>
    public class Notification : TrackedDomainEntity
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        public Notification() { this.Id = null; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public Notification(Guid id) : base(new Guid?(id)) { }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public EntityTypeEnumDto EntityType { get; set; }

        /// <summary>
        /// Gets or sets the type of the notification.
        /// </summary>
        /// <value>
        /// The type of the notification.
        /// </value>
        public NotificationTypeDto NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        /// <value>
        /// The entity identifier.
        /// </value>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the notification body.
        /// </summary>
        /// <value>
        /// The notification body.
        /// </value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>       
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the container identifier, this is only used when the Enity Type 
        /// is managed in SharePoint.
        /// </summary>
        /// <value>
        /// The container identifier.
        /// </value>    
        public Guid? ContainerId { get; set; }

    }
}
