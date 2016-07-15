using System;
using System.Runtime.Serialization;


namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Provides a data object for Notification information.
    /// </summary>
    [DataContract]
    public class NotificationDto:TrackedEntityDto
    {

       
        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        [DataMember]
        public EntityTypeEnumDto EntityType { get; set; }

        /// <summary>
        /// Gets or sets the type of the notification.
        /// </summary>
        /// <value>
        /// The type of the notification.
        /// </value>
        [DataMember]
        public NotificationTypeDto NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        /// <value>
        /// The entity identifier.
        /// </value>
        [DataMember]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [DataMember]
        public Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the notification body.
        /// </summary>
        /// <value>
        /// The notification body.
        /// </value>
        [DataMember]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [DataMember]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [DataMember]
        public DateTime? EndDate { get; set; }


        /// <summary>
        /// Gets or sets the container identifier, this is only used when the Enity Type 
        /// is managed in SharePoint.
        /// </summary>
        /// <value>
        /// The container identifier.
        /// </value>
        [DataMember]
        public Guid? ContainerId { get; set; }

    }
}
