using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ProductFamilyDetailDto
    /// </summary>
    [DataContract]
    public class ProductFamilyDetailDto
    {
        /// <summary>
        ///     Gets or sets the product family.
        /// </summary>
        /// <value>The product family.</value>
        [DataMember]
        public ProductFamilyDto ProductFamily { get; set; }

        /// <summary>
        ///     Gets or sets the characteristics.
        /// </summary>
        /// <value>The characteristics.</value>
        [DataMember]
        public IList<ProductFamilyCharacteristicAssociationDto> Characteristics { get; set; }

        /// <summary>
        ///     Gets or sets the dependency mappings.
        /// </summary>
        /// <value>The dependency mappings.</value>
        [DataMember]
        public IList<ProductFamilyFeatureDependencyDto> Dependencies { get; set; }
    }
}