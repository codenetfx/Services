using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ProductFamilyCreateUpdateRequestDto
    /// </summary>
    [DataContract]
    public class ProductFamilyCreateUpdateRequestDto
    {
        /// <summary>
        ///     Gets or sets the product family.
        /// </summary>
        /// <value>The product family.</value>
        [DataMember]
        public ProductFamilyDto ProductFamily { get; set; }

        /// <summary>
        /// Gets or sets the characteristic uploads.
        /// </summary>
        /// <value>The characteristic uploads.</value>
        [DataMember]
        public IList<ProductFamilyCharacteristicUploadDto> CharacteristicUploads { get; set; }

        /// <summary>
        /// Gets or sets the dependencies.
        /// </summary>
        /// <value>The dependencies.</value>
        [DataMember]
        public IList<ProductFamilyFeatureAllowedValueDependencyUploadDto> Dependencies { get; set; }
    }
}