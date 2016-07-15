using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// ProductFamilyCharacteristicDataType
    /// </summary>
    [DataContract]
    public enum ProductFamilyCharacteristicDataType
    {
        /// <summary>
        /// The unknown
        /// </summary>
        [EnumMember(Value = "Unknown")]
        Unknown = 0,
        /// <summary>
        /// The string
        /// </summary>
        [EnumMember (Value = "String")]
        String =1,
        /// <summary>
        /// The number
        /// </summary>
        [EnumMember(Value = "Number")]
        Number = 2,

        /// <summary>
        /// The date
        /// </summary>
        [EnumMember (Value = "Date")]
        Date = 3,

        /// <summary>
        /// The document reference
        /// </summary>
        [EnumMember(Value = "DocumentReference")]
        DocumentReference = 4
    }
}