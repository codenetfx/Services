using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ProductFamilyFeatureAllowedValueDependencyMappingDto
    /// </summary>
    [DataContract]
    public class ProductFamilyFeatureAllowedValueDependencyMappingDto
    {
        /// <summary>
        ///     Gets or sets the parent assocation.
        /// </summary>
        /// <value>
        ///     The parent assocation.
        /// </value>
        [DataMember]
        public ProductFamilyFeatureAssociationDto ParentAssocation { get; set; }

        /// <summary>
        ///     Gets or sets the parent values.
        /// </summary>
        /// <value>
        ///     The parent values.
        /// </value>
        [DataMember]
        public IList<ProductFamilyFeatureAllowedValueDto> ParentValues { get; set; }

        /// <summary>
        ///     Gets or sets the child assocation.
        /// </summary>
        /// <value>
        ///     The parent assocation.
        /// </value>
        [DataMember]
        public ProductFamilyFeatureAssociationDto ChildAssocation { get; set; }

        /// <summary>
        ///     Gets or sets the child values.
        /// </summary>
        /// <value>
        ///     The parent values.
        /// </value>
        [DataMember]
        public IList<ProductFamilyFeatureAllowedValueDto> ChildValues { get; set; }
    }
}