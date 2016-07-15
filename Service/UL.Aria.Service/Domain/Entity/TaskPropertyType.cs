using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Defines an extensible property definition for a <see cref="Task" /> entity
    /// </summary>
    public class TaskPropertyType : AuditableEntity, ISearchResult
    {
        private readonly IDictionary<string, string> _metadata = new Dictionary<string, string>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="TaskProperty" /> class.
        /// </summary>
        public TaskPropertyType()
            : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TrackedDomainEntity" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public TaskPropertyType(Guid? id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

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
            get { return EntityTypeEnumDto.TaskPropertyType.ToString(); }
        }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public Guid? ParentTaskPropertyTypeId { get; set; }

        /// <summary>
        /// Gets or sets the data type identifier.
        /// </summary>
        /// <value>
        /// The data type identifier.
        /// </value>
        public int DataTypeId{ get; set; }

       
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public TaskPropertyType ParentTaskPropertyType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow multiple].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow multiple]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowMultiple { get; set; }
    }
}