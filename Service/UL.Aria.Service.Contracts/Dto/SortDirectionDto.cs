using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Direction to be used by Sorts.
    /// </summary>
    [DataContract]
    public enum SortDirectionDto
    {
        /// <summary>
        /// The ascending
        /// </summary>
        [EnumMember]
        Ascending,
        /// <summary>
        /// The descending
        /// </summary>
        [EnumMember]
        Descending
    }
}