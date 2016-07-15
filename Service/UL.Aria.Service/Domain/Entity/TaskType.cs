using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain
{
    /// <summary>
    /// Class TaskType.
    /// </summary>
    public class TaskType : AuditableEntity, ISearchResult 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskType"/> class.
        /// </summary>
        public TaskType()
        {
            TaskTypeBehaviors = new List<TaskTypeBehavior>();
        }

        private readonly IDictionary<string, string> _metadata = new Dictionary<string, string>();
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get { return this.Name; } }
        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        public DateTime ChangeDate { get { return UpdatedDateTime; } }
        /// <summary>
        /// Gets or sets the metadata dictionary.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        IDictionary<string, string> ISearchResult.Metadata { get { return _metadata; } }
        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public string EntityType { get { return EntityTypeEnumDto.TaskType.ToString(); } }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the business units.
        /// </summary>
        /// <value>The business units.</value>
        public List<BusinessUnit> BusinessUnits { get; set; }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>The links.</value>
        public List<Link> Links { get; set; }

        /// <summary>
        ///     Gets or sets the duration of the estimated.
        /// </summary>
        /// <value>The duration of the estimated.</value>
        public decimal? EstimatedDuration { get; set; }

        /// <summary>
        ///     Gets or sets the task owner.
        /// </summary>
        /// <value>The task owner.</value>
        public string TaskOwner { get; set; }

        /// <summary>
        /// Gets or sets the document templates.
        /// </summary>
        /// <value>
        /// The document templates.
        /// </value>
        public List<DocumentTemplate> DocumentTemplates { get; set; }

        /// <summary>
        /// Gets or sets the notifications.
        /// </summary>
        /// <value>
        /// The notifications.
        /// </value>
        public List<TaskTypeNotification> Notifications { get; set; }

        /// <summary>
        /// Gets or sets the created by login identifier.
        /// </summary>
        /// <value>
        /// The created by login identifier.
        /// </value>
        public string CreatedByLoginId { get; set; }

        /// <summary>
        /// Gets or sets the updated by login identifier.
        /// </summary>
        /// <value>
        /// The updated by login identifier.
        /// </value>
        public string UpdatedByLoginId { get; set; }

        /// <summary>
        /// Gets or sets the business unit codes.
        /// </summary>
        /// <value>
        /// The business unit codes.
        /// </value>
        public string BusinessUnitCodes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [prevent deletion].
		/// </summary>
		/// <value><c>true</c> if [prevent deletion]; otherwise, <c>false</c>.</value>
		public bool PreventDeletion { get; set; }

        /// <summary>
        /// Gets or sets the task type behaviors.
        /// </summary>
        /// <value>
        /// The task type behaviors.
        /// </value>
        public IEnumerable<TaskTypeBehavior> TaskTypeBehaviors { get; set; }
	}
}
