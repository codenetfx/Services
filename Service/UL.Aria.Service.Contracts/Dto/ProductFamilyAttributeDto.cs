using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Defines attributes that describe product families.
    /// </summary>
    [DataContract]
    public class ProductFamilyAttributeDto : ProductFamilyCharacteristicDto
    {
        /// <summary>
        /// Gets or sets the data type id.
        /// </summary>
        /// <value>
        /// The data type id.
        /// </value>
        [DataMember]
        public ProductFamilyCharacteristicDataType DataTypeId { get; set; }
    }
}