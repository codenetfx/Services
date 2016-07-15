using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Available product family characteristics
    /// </summary>
    [DataContract]
    public enum ProductFamilyCharacteristicTypeDto
    {
        /// <summary>
        /// The attribute
        /// </summary>
        [EnumMember]
        Attribute,

        /// <summary>
        /// The feature
        /// </summary>
        [EnumMember]
        Feature
    }
}