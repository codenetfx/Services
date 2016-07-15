using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto.Integration
{
    /// <summary>
    /// Defines field used for searching.
    /// </summary>
    [DataContract(Namespace = "http://portal.ul.com")]
    public enum FulfillmentProjectSearchField
    {
        /// <summary>
        /// The quote number
        /// </summary>
        [EnumMember(Value = "QuoteNumber")] QuoteNumber,

        /// <summary>
        /// The order number
        /// </summary>
        [EnumMember(Value = "OrderNumber")] OrderNumber,

        /// <summary>
        /// The project number
        /// </summary>
        [EnumMember(Value = "ProjectNumber")] ProjectNumber,

        /// <summary>
        /// The project identifier
        /// </summary>
        [EnumMember(Value = "ProjectId")] ProjectId,

        /// <summary>
        /// The product file number
        /// </summary>
        [EnumMember(Value = "FileNumber")] ProductFileNumber,

        /// <summary>
        /// The account number
        /// </summary>
        [EnumMember(Value = "AccountNumber")] AccountNumber ,

        /// <summary>
        /// The party site number
        /// </summary>
        [EnumMember(Value = "PartySiteNumber")] PartySiteNumber,
        /// <summary>
        /// The project status
        /// </summary>
        [EnumMember(Value = "ProjectStatus")] ProjectStatus,


    }
}