using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Enum ProductCharacteristicChildType
    /// </summary>
    [DataContract]
    public enum ProductCharacteristicChildType
    {
        /// <summary>
        ///     The multi value
        /// </summary>
        [EnumMember(Value = "MultiValue")]
        MultiValue=0,

        /// <summary>
        ///     The multi range value
        /// </summary>
        [EnumMember(Value = "MultiRangeValue")]
        MultiRangeValue=1,

        /// <summary>
        ///     The range value
        /// </summary>
        [EnumMember(Value = "RangeValue")]
        RangeValue=2
    }
}