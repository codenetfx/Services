using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Asset type enumeration.
    /// </summary>
    [DataContract]
    public enum AssetTypeEnumDto
    {
        /// <summary>
        /// The order asset types.
        /// </summary>
        [EnumMember(Value = "Container")]
        Container,

        /// <summary>
        /// The order asset types.
        /// </summary>
        [EnumMember(Value = "Order")]
        Order,

        /// <summary>
        /// The product asset types.
        /// </summary>
        [EnumMember(Value = "Product")]
        Product,

        /// <summary>
        /// The project asset types.
        /// </summary>
        [EnumMember(Value = "Project")]
        Project,

        /// <summary>
        /// The document asset types.
        /// </summary>
        [EnumMember(Value = "Document")]
        Document,

        /// <summary>
        /// The task
        /// </summary>
        [EnumMember(Value = "Task")]
        Task,

        /// <summary>
        /// The task
        /// </summary>
        [EnumMember(Value = "IncomingOrderServiceLine")]
        IncomingOrderServiceLine
    }
}