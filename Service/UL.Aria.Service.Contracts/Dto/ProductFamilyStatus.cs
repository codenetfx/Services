using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Describes the product family status
    /// </summary>
    [DataContract]
    public enum ProductFamilyStatus
    {
        /// <summary>
        /// The draft
        /// </summary>
        [EnumMember(Value = "Draft")]
        Draft,
        /// <summary>
        /// The active
        /// </summary>
        [EnumMember(Value = "Active")]
        Active,
        /// <summary>
        /// The inactive
        /// </summary>
        [EnumMember(Value = "Inactive")]
        Inactive
    }
}