using System;
using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Lookup
{
	/// <summary>
	/// Class DocumentTemplate.
	/// </summary>
	public class DocumentTemplate : AuditableEntity, ISearchResult
	{
		private readonly IDictionary<string, string> _metadata = new Dictionary<string, string>();

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentTemplate"/> class.
		/// </summary>
		public DocumentTemplate() : this(null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TrackedDomainEntity" /> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public DocumentTemplate(Guid? id)
		{
			Id = id;
			BusinessUnits = new List<BusinessUnit>();
		}
        
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public string ContentType { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the document identifier.
		/// </summary>
		/// <value>The document identifier.</value>
		public Guid DocumentId { get; set; }

		/// <summary>
		/// Gets or sets the business units.
		/// </summary>
		/// <value>The business units.</value>
		public IEnumerable<BusinessUnit> BusinessUnits { get; set; }

		/// <summary>
		/// Gets or sets the business unit codes.
		/// </summary>
		/// <value>The business unit codes.</value>
		public string BusinessUnitCodes { get; set; }

		/// <summary>
		/// Gets or sets the entity type.
		/// </summary>
		/// <value>The type.</value>
		public EntityTypeEnumDto? EntityType
		{
			get { return EntityTypeEnumDto.DocumentTemplate; }
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title
		{
			get { return Name; }
		}

		/// <summary>
		/// Gets or sets the change date.
		/// </summary>
		/// <value>The change date.</value>
		public DateTime ChangeDate
		{
			get { return UpdatedDateTime; }
		}

		/// <summary>
		/// Gets or sets the metadata dictionary.
		/// </summary>
		/// <value>The metadata.</value>
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
		/// <exception cref="System.NotImplementedException"></exception>
		string ISearchResult.EntityType
		{
			get { return EntityType.ToString(); }

		}


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
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
		public bool IsDeleted { get; set; }


	
	}
}