using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ProductUploadMessageDto
    /// </summary>
    [DataContract]
    public class ProductUploadMessageDto
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets the product upload result id.
        /// </summary>
        /// <value>The product upload result id.</value>
        [DataMember]
        public Guid ProductUploadResultId { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the details.
        /// </summary>
        /// <value>
        ///     The details.
        /// </value>
        [DataMember]
        public string Detail { get; set; }

        /// <summary>
        ///     Gets or sets the type of the message.
        /// </summary>
        /// <value>The type of the message.</value>
        [DataMember]
        public ProductUploadMessageTypeEnumDto MessageType { get; set; }

        /// <summary>
        ///     Gets or sets the characteristic id.
        /// </summary>
        /// <value>
        ///     The characteristic id.
        /// </value>
        [DataMember]
        public Guid? ProductFamilyCharacteristicId { get; set; }

        /// <summary>
        ///     Gets or sets the created on.
        /// </summary>
        /// <value>The created on.</value>
        [DataMember]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        ///     Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        [DataMember]
        public Guid CreatedBy { get; set; }

        /// <summary>
        ///     Gets or sets the updated on.
        /// </summary>
        /// <value>The updated on.</value>
        [DataMember]
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        ///     Gets or sets the updated by.
        /// </summary>
        /// <value>The updated by.</value>
        [DataMember]
        public Guid UpdatedBy { get; set; }
    }
}