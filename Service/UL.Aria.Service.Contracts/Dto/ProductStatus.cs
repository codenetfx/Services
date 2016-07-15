using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// ProductStatus
    /// </summary>
    [DataContract]
    public enum ProductStatus
    {
        /// <summary>
        /// The unknown
        /// </summary>
        [EnumMember(Value = "Unknown")]
        Unknown = 0,
        /// <summary>
        /// The draft
        /// </summary>
        [EnumMember(Value = "Draft")]
        Draft = 1,
        /// <summary>
        /// The submitted
        /// </summary>
        [EnumMember(Value = "Submitted")]
        Submitted = 2
    }
}