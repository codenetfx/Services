using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto.Integration
{
    /// <summary>
    /// Defines fields used for Refiners.
    /// </summary>
    [DataContract]
    public enum FulfillmentProjectRefinerField
    {
        /// <summary>
        /// The quote number
        /// </summary>
        [EnumMember(Value = "ProjectStatus")] ProjectStatus,

        /// <summary>
        /// The order number
        /// </summary>
        [EnumMember(Value = "OrderNumber")] OrderNumber,
    }
}