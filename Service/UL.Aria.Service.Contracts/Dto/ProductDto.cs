using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Product data transfer object.
    /// </summary>
    [DataContract]
    public class ProductDto : TrackedEntityDto
    {
        /// <summary>
        /// Gets or sets the company id.
        /// </summary>
        /// <value>The company id.</value>
        [DataMember]
		public Guid CompanyId { get; set; }

		/// <summary>
		/// Gets or sets the container id.
		/// </summary>
		/// <value>
		/// The container id.
		/// </value>
		[DataMember]
		public Guid? ContainerId { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the family id.
        /// </summary>
        /// <value>
        ///     The family id.
        /// </value>
        [DataMember]
        public Guid ProductFamilyId { get; set; }

        /// <summary>
        ///     Gets or sets the characteristics.
        /// </summary>
        /// <value>
        ///     The characteristics.
        /// </value>
        [DataMember]
        public IList<ProductCharacteristicDto> Characteristics { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can delete.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can delete; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool CanDelete { get; set; }

        /// <summary>
        /// Gets or sets the product status.
        /// </summary>
        /// <value>
        /// The product status.
        /// </value>
        [DataMember]
		public ProductStatus Status { get; set; }

		/// <summary>
		/// Gets or sets the date the product was submitted - if it has been.
		/// </summary>
		/// <value>
		/// The submitted date.
		/// </value>
		[DataMember]
		public DateTime? SubmittedDateTime { get; set; }
        
    }
}