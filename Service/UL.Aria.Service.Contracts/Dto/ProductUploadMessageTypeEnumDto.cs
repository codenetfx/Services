using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Enum ProductUploadMessageTypeEnumDto
    /// </summary>
    [DataContract]
    public enum ProductUploadMessageTypeEnumDto
    {
        /// <summary>
        /// The error
        /// </summary>
        [EnumMember(Value = "Error")]
        Error = 0
    }
}