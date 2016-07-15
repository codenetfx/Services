using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Class DocumentTemplateDto.
	/// </summary>
	[DataContract]
	public class DocumentTemplateDto
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[DataMember]
		public Guid? Id { get; set; }


        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        [DataMember]
        public string ContentType { get; set; }

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
		/// Gets or sets the document identifier.
		/// </summary>
		/// <value>The document identifier.</value>
		[DataMember]
		public Guid DocumentId { get; set; }

		/// <summary>
		/// Gets or sets the business units.
		/// </summary>
		/// <value>The business units.</value>
		[DataMember]
		public List<BusinessUnitDto> BusinessUnits { get; set; }

		/// <summary>
		/// Gets or sets the business unit codes.
		/// </summary>
		/// <value>The business unit codes.</value>
		[DataMember]
		public string BusinessUnitCodes { get; set; }

		/// <summary>
		/// Gets or sets the created by identifier.
		/// </summary>
		/// <value>The created by identifier.</value>
		[DataMember]
		public Guid CreatedById { get; set; }

		/// <summary>
		/// Gets or sets the created date time.
		/// </summary>
		/// <value>The created date time.</value>
		[DataMember]
		public DateTime CreatedDateTime { get; set; }

		/// <summary>
		/// Gets or sets the updated by identifier.
		/// </summary>
		/// <value>The updated by identifier.</value>
		[DataMember]
		public Guid UpdatedById { get; set; }

		/// <summary>
		/// Gets or sets the updated date time.
		/// </summary>
		/// <value>The updated date time.</value>
		[DataMember]
		public DateTime UpdatedDateTime { get; set; }



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
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
		[DataMember]
		public bool IsDeleted { get; set; }



	}
}