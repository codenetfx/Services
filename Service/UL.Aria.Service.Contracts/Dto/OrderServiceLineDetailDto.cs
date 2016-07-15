using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class OrderServiceLineDetailDto.
    /// </summary>
    [DataContract]
    public class OrderServiceLineDetailDto
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        ///     Gets or sets the order number.
        /// </summary>
        /// <value>The order number.</value>
        [DataMember]
        public string OrderNumber { get; set; }

        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        [DataMember]
        public Guid? OrderId { get; set; }

        /// <summary>
        ///     Gets or sets the line identifier.
        /// </summary>
        /// <value>The line identifier.</value>
        [DataMember]
        public string LineId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the sender.
        /// </summary>
        /// <value>The name of the sender.</value>
        [DataMember]
        public string SenderName { get; set; }

        /// <summary>
        ///     Gets or sets the name of the group.
        /// </summary>
        /// <value>The name of the group.</value>
        [DataMember]
        public string GroupName { get; set; }

        /// <summary>
        ///     Gets or sets the original XML.
        /// </summary>
        /// <value>The original XML.</value>
        [DataMember]
        public string OriginalXml { get; set; }

        /// <summary>
        ///     Gets or sets the line XML.
        /// </summary>
        /// <value>The line XML.</value>
        [DataMember]
        public string LineXml { get; set; }

        /// <summary>
        ///     Gets or sets the sender identifier.
        /// </summary>
        /// <value>The sender identifier.</value>
        [DataMember]
        public Int16? SenderId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has custom.
        /// </summary>
        /// <value><c>true</c> if this instance has custom; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool HasCustom { get; set; }

        /// <summary>
        ///     Gets or sets the created by id.
        /// </summary>
        /// <value>The created by id.</value>
        [DataMember]
        public Guid CreatedById { get; set; }

        /// <summary>
        ///     Gets or sets the Aria auditing updated user id.
        /// </summary>
        /// <value>The updated by id.</value>
        [DataMember]
        public Guid UpdatedById { get; set; }

        /// <summary>
        ///     Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        [DataMember]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the Aria auditing updated date time.
        /// </summary>
        /// <value>The updated date time.</value>
        [DataMember]
        public DateTime UpdatedDateTime { get; set; }
    }
}