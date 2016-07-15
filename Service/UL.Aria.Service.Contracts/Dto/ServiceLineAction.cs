using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Actions for services lines
    /// </summary>
    [DataContract]
    public enum ServiceLineAction
    {
        /// <summary>
        /// The no change action.
        /// </summary>
        [EnumMember(Value = "NoChange")]
        NoChange = 0,

        /// <summary>
        /// The add action.
        /// </summary>
        [EnumMember(Value = "Add")]
        Add = 1,

        /// <summary>
        /// The remove action.
        /// </summary>
        [EnumMember(Value = "Remove")]
        Remove = 2
    }
}