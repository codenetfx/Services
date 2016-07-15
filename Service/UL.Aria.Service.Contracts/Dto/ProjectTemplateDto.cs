using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Defines a project template.
    /// </summary>
    [DataContract]
    public class ProjectTemplateDto
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTemplateDto"/> class.
        /// </summary>
        public ProjectTemplateDto()
        {
            TaskTemplates = new List<TaskTemplateDto>();
            BusinessUnits = new List<BusinessUnitDto>();
           
        }
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Draft status.
        /// </summary>
        /// <value>
        /// Is this in Draft status, else in Published status.
        /// </value>
        [DataMember]
        public bool IsDraft { get; set; }

        /// <summary>
        ///     Gets or sets the tasks.
        /// </summary>
        /// <value>The tasks.</value>
        [DataMember]
        public IList<TaskTemplateDto> TaskTemplates { get; set; }

        /// <summary>
        /// Gets or sets the business unit.
        /// </summary>
        /// <value>
        /// The business unit.
        /// </value>
        [DataMember]
        public IList<BusinessUnitDto> BusinessUnits { get; set; }

        /// <summary>
        ///     Gets or sets the created by id.
        /// </summary>
        /// <value>The created by id.</value>
        [DataMember]
        public Guid CreatedById { get; set; }

        /// <summary>
        ///     Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        [DataMember]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the updated by id.
        /// </summary>
        /// <value>The updated by id.</value>
        [DataMember]
        public Guid UpdatedById { get; set; }

        /// <summary>
        ///     Gets or sets the updated date time.
        /// </summary>
        /// <value>The updated date time.</value>
        [DataMember]
        public DateTime UpdatedDateTime { get; set; }


        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        [DataMember]
        public Guid CorrelationId { get; set; }


		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		/// <value>
		/// The version.
		/// </value>
		[DataMember]
		public decimal Version { get; set; }


		/// <summary>
		/// Gets or sets the updated by login identifier.
		/// </summary>
		/// <value>
		/// The updated by login identifier.
		/// </value>
		[DataMember]
		public string UpdatedByLoginId { get; set; }
		/// <summary>
		/// Gets or sets the created by login identifier.
		/// </summary>
		/// <value>
		/// The created by login identifier.
		/// </value>
		[DataMember]
		public string CreatedByLoginId { get; set; }


		/// <summary>
		/// Gets or sets the business unit codes.
		/// </summary>
		/// <value>
		/// The business unit codes.
		/// </value>
		[DataMember]
		public string BusinessUnitCodes { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [automatic complete project].
		/// </summary>
		/// <value>
		/// <c>true</c> if [automatic complete project]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool AutoCompleteProject { get; set; }
    }
}