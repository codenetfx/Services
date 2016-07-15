using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Repository;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Defines an extensible property for a <see cref="Task" /> entity
    /// </summary>
    public class TaskProperty : AuditableEntity, ISearchResult
    {
        private readonly IDictionary<string, string> _metadata = new Dictionary<string, string>();
        private readonly IList<TaskProperty> _children = new List<TaskProperty>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="TaskProperty" /> class.
        /// </summary>
        public TaskProperty() : this(null)
        {
            TaskPropertyType = new TaskPropertyType();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TrackedDomainEntity" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public TaskProperty(Guid? id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get { return TaskPropertyType.Name; }}

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
            get { return EntityTypeEnumDto.TaskProperty.ToString(); }
        }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public Guid? ParentTaskPropertyId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the task property type identifier.
        /// </summary>
        /// <value>
        /// The task property type identifier.
        /// </value>
        public Guid TaskPropertyTypeId { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public TaskProperty Parent { get; set; }

        /// <summary>
        /// Gets or sets the type of the task property.
        /// </summary>
        /// <value>
        /// The type of the task property.
        /// </value>
        public TaskPropertyType TaskPropertyType { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public virtual IList<TaskProperty> Children { get { return _children; } }

        /// <summary>
        /// Gets the child property value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected string GetChildPropertyValue([CallerMemberName]string propertyName = null)
        {
            var childTaskProperty = _children.FirstOrDefault(x => x.TaskPropertyType.Id.ToString() == GetTaskPropertyTypeAttribute(propertyName).Id);
            if (null == childTaskProperty)
                return null;
            return  childTaskProperty.Value;
        }

        /// <summary>
        /// Sets the child property value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        protected void SetChildPropertyValue(string value, [CallerMemberName]string propertyName = null)
        {
            var childTaskProperty = _children.FirstOrDefault(x => x.TaskPropertyType.Name == GetTaskPropertyTypeAttribute(propertyName).Name);
            if (null == childTaskProperty)
            {
                childTaskProperty = GetTaskPropertyTypeForConstruction(propertyName);
                childTaskProperty.TaskPropertyTypeId = childTaskProperty.TaskPropertyType.Id.Value;
                childTaskProperty.Parent = this;
                childTaskProperty.ParentTaskPropertyId = this.Id;
                _children.Add(childTaskProperty);
            }
            childTaskProperty.Value = value;
        }



        /// <summary>
        /// Gets the task property type for construction.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected virtual TaskProperty GetTaskPropertyTypeForConstruction(string propertyName)
        {
            var attribute = GetTaskPropertyTypeAttribute(propertyName);
            var uif = new TaskProperty {TaskPropertyType = new TaskPropertyType{Id = new Guid(attribute.Id), Name = attribute.Name}};
            uif.TaskPropertyTypeId = uif.TaskPropertyType.Id.Value;
            return uif;
        }

        /// <summary>
        /// Gets the task property type attribute.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected TaskPropertyTypeAttribute GetTaskPropertyTypeAttribute(string propertyName)
        {
            var property = this.GetType().GetProperty(propertyName);
            var attribute = (TaskPropertyTypeAttribute) property.GetCustomAttribute(typeof (TaskPropertyTypeAttribute));
            return attribute;
        }
    }
}