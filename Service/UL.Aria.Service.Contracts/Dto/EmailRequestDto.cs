using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     represents contextual information
    /// </summary>
    [DataContract]
    public class EmailRequestDto
    {
        /// <summary>
        ///     Gets the address to.
        /// </summary>
        /// <value>
        ///     The address to.
        /// </value>
        [DataMember]
        public string AddressTo { get; set; }

        /// <summary>
        ///     Gets the address from.
        /// </summary>
        /// <value>
        ///     The address from.
        /// </value>
        [DataMember]
        public string AddressFrom { get; set; }

        /// <summary>
        ///     Gets the subject.
        /// </summary>
        /// <value>
        ///     The subject.
        /// </value>
        [DataMember]
        public string Subject { get; set; }

        /// <summary>
        ///     Gets the text.
        /// </summary>
        /// <value>
        ///     The text.
        /// </value>
        [DataMember]
        public string MessageBody { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is message body HTML.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is message body HTML; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsMessageBodyHtml { get; set; }

        /// <summary>
        /// Gets or sets the address CC.
        /// </summary>
        /// <value>
        /// The address CC.
        /// </value>
        [DataMember]
        public string AddressCC { get; set; }

        /// <summary>
        /// Gets or sets the address BCC.
        /// </summary>
        /// <value>
        /// The address BCC.
        /// </value>
        [DataMember]
        public string AddressBcc { get; set; }

    }
}