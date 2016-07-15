using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Defines available extended behaviors for <see cref="TaskType"/> when applied to a <see cref="Task"/>.
    /// </summary>
    public class TaskTypeAvailableBehaviorField : AuditableEntity, ISearchResult
    {
        private readonly IDictionary<string, string> _metadata = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the task type available behavior identifier.
        /// </summary>
        /// <value>
        /// The task type available behavior identifier.
        /// </value>
        public Guid TaskTypeAvailableBehaviorId { get; set; }


        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get { return Name; }
        }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        public DateTime ChangeDate
        {
            get { return UpdatedDateTime; }
        }

        /// <summary>
        /// Gets or sets the metadata dictionary.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IDictionary<string, string> Metadata
        {
            get { return _metadata; }
        }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public string EntityType
        {
            get { return "TaskTypeAvailableBehaviorField"; }
        }
    }
}