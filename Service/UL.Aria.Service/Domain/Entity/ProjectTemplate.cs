using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Defines a project template.
    /// </summary>
    public class ProjectTemplate : TrackedDomainEntity, ISearchResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectTemplate" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public ProjectTemplate(Guid? id) : base(id)
        {
            TaskTemplates = new List<TaskTemplate>();
            BusinessUnits = new List<BusinessUnit>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectTemplate" /> class.
        /// </summary>
        public ProjectTemplate()
            : this(null)
        {
        }

        /// <summary>
        /// Gets or sets the Correlation Id which indicates a connection to a Project Template that has been copied to different states, Draft/Publish/Deleted.
        /// </summary>
        /// <value>
        /// The CorrelationId.
        /// </value>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Draft status.
        /// </summary>
        /// <value>
        /// Is this in Draft status, else in Published status.
        /// </value>
        public bool IsDraft { get; set; }

        /// <summary>
        ///     Gets or sets the tasks.
        /// </summary>
        /// <value>The tasks.</value>
        public List<TaskTemplate> TaskTemplates { get; set; }

        /// <summary>
        /// Gets or sets the business unit.
        /// </summary>
        /// <value>
        /// The business unit.
        /// </value>
        public List<BusinessUnit> BusinessUnits { get; set; }

		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		/// <value>
		/// The version.
		/// </value>
	    public decimal Version { get; set; }


        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get
            {
                return this.Name;
            }
          
        }

        /// <summary>
        /// Gets or sets the entity type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public EntityTypeEnumDto? EntityType
        {
            get
            {
                return EntityTypeEnumDto.ProjectTemplate;
            }
          
        }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        public DateTime ChangeDate
        {
            get
            {
                return UpdatedDateTime;
            }
         
        }

        /// <summary>
        /// Gets or sets the metadata dictionary.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IDictionary<string, string> Metadata
        {
            get;
            set;
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
		/// Gets or sets the business unit codes.
		/// </summary>
		/// <value>
		/// The business unit codes.
		/// </value>
		public string BusinessUnitCodes { get; set; }


		/// <summary>
		/// Gets or sets a value indicating whether [automatic complete project].
		/// </summary>
		/// <value>
		/// <c>true</c> if [automatic complete project]; otherwise, <c>false</c>.
		/// </value>
		public bool AutoCompleteProject { get; set; }
    }
}