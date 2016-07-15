using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Provides a dictionary of the <see cref="ProductFamilyDto" /> keyed by their Id.
    /// </summary>
    [DataContract]
    public class ProductFamiliesDto
    {
        /// <summary>
        ///     Gets or sets the product families.
        /// </summary>
        /// <value>
        ///     The product families.
        /// </value>
        [DataMember]
        public IEnumerable<ProductFamilyIdValuePairDto> ProductFamilies { get; set; }
    }
}