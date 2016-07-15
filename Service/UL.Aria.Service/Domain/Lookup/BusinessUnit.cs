using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Lookup
{
    /// <summary>
    /// Business Unit Domain Model
    /// </summary>
    public class BusinessUnit : AuditableEntity, ISearchResult
    {
        private readonly IDictionary<string, string> _metadata = new Dictionary<string, string>();
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessUnit"/> class.
        /// </summary>
        public BusinessUnit() : this(null)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessUnit"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public BusinessUnit(Guid? id)
        {
            Id = id;
        }
        /// <summary>
        /// Gets or sets the name of the business unit.
        /// </summary>
        /// <value>
        /// The name of the business unit.
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
            get { return this.Code; }
        }

        /// <summary>
        /// Gets or sets the entity type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public EntityTypeEnumDto? EntityType
        {
            get { return EntityTypeEnumDto.BusinessUnit; }
        }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
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
        /// BusinessUnitCode
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets the note.
        /// </summary>
        /// <value>
        /// The note.
        /// </value>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        string ISearchResult.EntityType
        {
            get
            {
                return this.EntityType.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the updated by login identifier.
        /// </summary>
        /// <value>
        /// The updated by login identifier.
        /// </value>
        public string UpdatedByLoginId { get; set; }

        /// <summary>
        /// Gets or sets the created by login identifier.
        /// </summary>
        /// <value>
        /// The created by login identifier.
        /// </value>
        public string CreatedByLoginId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is delete prevented.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is delete prevented; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeletePrevented { get; set; }
    }
}
