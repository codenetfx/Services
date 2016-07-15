using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Enum ProductUploadStatusEnumDto
    /// </summary>
    [DataContract]
    public enum ProductUploadStatusEnumDto
    {
        /// <summary>
        /// The ready to process
        /// </summary>
        [EnumMember(Value = "ReadyToProcess")]
        ReadyToProcess = 0,

        /// <summary>
        /// The processing
        /// </summary>
        [EnumMember(Value = "Processing")]
        Processing = 1,

        /// <summary>
        /// The processed successfully
        /// </summary>
        [EnumMember(Value = "ProcessedSuccessfully")]
        ProcessedSuccessfully = 65,

        /// <summary>
        /// The processed with errors
        /// </summary>
        [EnumMember(Value = "ProcessedWithErrors")]
        ProcessedWithErrors = 67
    }
}