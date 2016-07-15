using System;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Messages from importing <see cref="Product" />s.
    /// </summary>
    public class ProductUploadMessage : TrackedDomainEntity
    {
        /// <summary>
        /// Gets or sets the product upload result id.
        /// </summary>
        /// <value>The product upload result id.</value>
        public Guid ProductUploadResultId { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the details.
        /// </summary>
        /// <value>
        ///     The details.
        /// </value>
        public string Detail { get; set; }

        /// <summary>
        ///     Gets or sets the type of the message.
        /// </summary>
        /// <value>The type of the message.</value>
        public ProductUploadMessageTypeEnumDto MessageType { get; set; }

        /// <summary>
        ///     Gets or sets the characteristic id.
        /// </summary>
        /// <value>
        ///     The characteristic id.
        /// </value>
        public Guid? ProductFamilyCharacteristicId { get; set; }
    }
}