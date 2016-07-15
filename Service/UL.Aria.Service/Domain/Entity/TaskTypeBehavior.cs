using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Defines extended behaviors for <see cref="TaskType" /> when applied to a <see cref="Task" />.
    /// </summary>
	 [Serializable]
	public class TaskTypeBehavior : AuditableEntity, ISearchResult
    {
        private readonly IDictionary<string, string> _metadata = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the task type identifier.
        /// </summary>
        /// <value>
        /// The task type identifier.
        /// </value>
        public Guid TaskTypeId { get; set; }

        /// <summary>
        /// Gets or sets <see cref="TaskTypeAvailableBehavior"/>.
        /// </summary>
        /// <value>
        /// The task type available behavior.
        /// </value>
        public Guid TaskTypeAvailableBehaviorId { get; set; }

        /// <summary>
        /// Gets or sets the name of the behavior.
        /// </summary>
        /// <value>
        /// The name of the behavior.
        /// </value>
        public string BehaviorName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TaskTypeAvailableBehaviorField"/>.
        /// </summary>
        /// <value>
        /// The task type available behavior field.
        /// </value>
        public Guid? TaskTypeAvailableBehaviorFieldId { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return string.IsNullOrWhiteSpace(FieldName) ? BehaviorName : string.Format("{0}.{1}", BehaviorName, FieldName); }
        }

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
            get { return "TaskTypeBehavior"; }
        }
    }
}
