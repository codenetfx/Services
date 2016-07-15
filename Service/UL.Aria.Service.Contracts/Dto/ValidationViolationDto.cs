using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Contains information about a validaiton violation.
    /// </summary>
    [DataContract]
    public class ValidationViolationDto
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        [DataMember]
        public ValidationLevelEnumDto Level { get; set; }


        /// <summary>
        /// Gets or sets the tag object. this should be used to provide additional data
        /// for the violation.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        [DataMember]
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        [DataMember]
        public string DataType { get; set; }

    }
}