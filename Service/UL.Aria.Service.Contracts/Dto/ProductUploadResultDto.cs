using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ProductUploadResultDto
    /// </summary>
    [DataContract]
    public class ProductUploadResultDto
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        ///     Gets or sets the product upload id.
        /// </summary>
        /// <value>The product upload id.</value>
        [DataMember]
        public Guid ProductUploadId { get; set; }

        /// <summary>
        ///     Gets or sets the product.
        /// </summary>
        /// <value>
        ///     The product.
        /// </value>
        [DataMember]
        public ProductDto Product { get; set; }

        /// <summary>
        ///     Gets or sets the errors.
        /// </summary>
        /// <value>
        ///     The errors.
        /// </value>
        [DataMember]
        public IEnumerable<ProductUploadMessageDto> Messages { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsValid { get; set; }

        /// <summary>
        ///     Gets or sets the created on.
        /// </summary>
        /// <value>The created on.</value>
        [DataMember]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        ///     Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        [DataMember]
        public Guid CreatedBy { get; set; }

        /// <summary>
        ///     Gets or sets the updated on.
        /// </summary>
        /// <value>The updated on.</value>
        [DataMember]
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        ///     Gets or sets the updated by.
        /// </summary>
        /// <value>The updated by.</value>
        [DataMember]
        public Guid UpdatedBy { get; set; }

        /// <summary>
        ///     Gets or sets the created by user login id.
        /// </summary>
        /// <value>The created by user login id.</value>
        [DataMember]
        public string CreatedByUserLoginId { get; set; }
    }
}