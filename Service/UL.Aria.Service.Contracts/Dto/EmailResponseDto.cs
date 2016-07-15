using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// signifies context and error info regarding the sending of an e-mail
    /// </summary>
    [DataContract]
    public class EmailResponseDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailResponseDto"/> class.
        /// </summary>
        public EmailResponseDto()
        {
            Error = false;
            Message = string.Empty;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="EmailResponseDto"/> is error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if error; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Error { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember]
        public string Message { get; set; }
    }
}