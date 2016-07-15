using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Class TaskTypeDto.
	/// </summary>
	[DataContract]
	public class TaskTypeDto : TrackedEntityDto
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTypeDto"/> class.
        /// </summary>
	    public TaskTypeDto()
        {
            BusinessUnits = new List<BusinessUnitDto>();
            Links = new List<LinkDto>();
            DocumentTemplates = new List<DocumentTemplateDto>();
            TaskTypeBehaviors = new List<TaskTypeBehaviorDto>();
        }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		[DataMember]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		[DataMember]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the business unit.
		/// </summary>
		/// <value>The business unit.</value>
		[DataMember]
		public List<BusinessUnitDto> BusinessUnits { get; set; }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        [DataMember]
        public List<LinkDto> Links { get; set; }

        /// <summary>
        ///     Gets or sets the duration of the estimated.
        /// </summary>
        /// <value>The duration of the estimated.</value>
        [DataMember]
        public decimal? EstimatedDuration { get; set; }

        /// <summary>
        ///     Gets or sets the task owner.
        /// </summary>
        /// <value>The task owner.</value>
        [DataMember]
        public string TaskOwner { get; set; }

        /// <summary>
        /// Gets or sets the document templates.
        /// </summary>
        /// <value>
        /// The document templates.
        /// </value>
        [DataMember]
        public List<DocumentTemplateDto> DocumentTemplates { get; set; }

        /// <summary>
        /// Gets or sets the notifications.
        /// </summary>
        /// <value>
        /// The notifications.
        /// </value>
        [DataMember]
        public List<TaskTypeNotificationDto> Notifications { get; set; }

            /// <summary>
        /// Gets or sets the created by login identifier.
        /// </summary>
        /// <value>
        /// The created by login identifier.
        /// </value>
        [DataMember]
        public string CreatedByLoginId { get; set; }

        /// <summary>
        /// Gets or sets the updated by login identifier.
        /// </summary>
        /// <value>
        /// The updated by login identifier.
        /// </value>
        [DataMember]
        public string UpdatedByLoginId { get; set; }

        /// <summary>
        /// Gets or sets the business unit codes.
        /// </summary>
        /// <value>
        /// The business unit codes.
        /// </value>
        [DataMember]
        public string BusinessUnitCodes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsDeleted { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [prevent deletion].
		/// </summary>
		/// <value><c>true</c> if [prevent deletion]; otherwise, <c>false</c>.</value>
		[DataMember]
		public bool PreventDeletion { get; set; }

        /// <summary>
        /// Gets or sets the task type behaviors.
        /// </summary>
        /// <value>
        /// The task type behaviors.
        /// </value>
        [DataMember]
        public List<TaskTypeBehaviorDto> TaskTypeBehaviors { get; set; }
	}
}
