using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class OrderServiceLineDetail.
    /// </summary>
    public class OrderServiceLineDetail : TrackedDomainEntity
    {
        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        public Guid? OrderId { get; set; }

        /// <summary>
        ///     Gets or sets the line identifier.
        /// </summary>
        /// <value>The line identifier.</value>
        public string LineId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the sender.
        /// </summary>
        /// <value>The name of the sender.</value>
        public string SenderName { get; set; }

        /// <summary>
        ///     Gets or sets the name of the group.
        /// </summary>
        /// <value>The name of the group.</value>
        public string GroupName { get; set; }

        /// <summary>
        ///     Gets or sets the original XML.
        /// </summary>
        /// <value>The original XML.</value>
        public string OriginalXml { get; set; }

        /// <summary>
        ///     Gets or sets the line XML.
        /// </summary>
        /// <value>The line XML.</value>
        public string LineXml { get; set; }

        /// <summary>
        ///     Gets or sets the sender identifier.
        /// </summary>
        /// <value>The sender identifier.</value>
        public Int16? SenderId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has custom.
        /// </summary>
        /// <value><c>true</c> if this instance has custom; otherwise, <c>false</c>.</value>
        public bool HasCustom { get; set; }
    }
}