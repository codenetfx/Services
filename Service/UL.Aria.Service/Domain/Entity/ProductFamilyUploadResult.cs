using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Results of a <see cref="ProductFamily"/> upload.
    /// </summary>
    public sealed class ProductFamilyUploadResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyUploadResult"/> class.
        /// </summary>
        public ProductFamilyUploadResult()
        {
            Messages = new List<ProductUploadMessage>();
            CharacteristicUploads = new List<ProductFamilyCharacteristicUpload>();
            DependencyUploads = new List<ProductFamilyFeatureAllowedValueDependencyUpload>();
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the product family.
        /// </summary>
        /// <value>
        /// The product family.
        /// </value>
        public ProductFamily ProductFamily { get; set; }

        /// <summary>
        /// Gets or sets the characteristic uploads.
        /// </summary>
        /// <value>
        /// The characteristic uploads.
        /// </value>
        public IEnumerable<ProductFamilyCharacteristicUpload> CharacteristicUploads { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public IList<ProductUploadMessage> Messages { get; set; }

        /// <summary>
        /// Gets or sets the dependency uploads.
        /// </summary>
        /// <value>
        /// The dependency uploads.
        /// </value>
        public IList<ProductFamilyFeatureAllowedValueDependencyUpload> DependencyUploads { get; set; }
    }
}