using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Data transfer object for order messages.
    /// </summary>
    [DataContract]
    public class OrderMessageDto
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderMessageDto"/> class.
        /// </summary>
        public OrderMessageDto()
        {
            Properties=new List<KeyValuePair<string, object>>();
        }

        /// <summary>
        ///     Gets or sets the message body.
        /// </summary>
        /// <value>
        ///     The message body.
        /// </value>
        [DataMember]
        public string Body { get; set; }

        /// <summary>
        ///     Gets or sets the message originator.
        /// </summary>
        /// <value>
        ///     The originator.
        /// </value>
        [DataMember]
        public string Originator { get; set; }

        /// <summary>
        ///     Gets or sets the receiver.
        /// </summary>
        /// <value>
        ///     The receiver.
        /// </value>
        [DataMember]
        public string Receiver { get; set; }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>
        ///     The id.
        /// </value>
        [DataMember]
        public string ExternalMessageId { get; set; }

        /// <summary>
        ///     The properties as a list of key value pairs.
        /// </summary>
        /// <remarks>
        ///     This is <em>not</em> intended to be a dictionary as there may be more than one instance of a given property.
        /// </remarks>
        [DataMember]
        public List<KeyValuePair<string, object>> Properties { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return !(string.IsNullOrWhiteSpace(Body)) ? string.Format("{0}:{1}", ExternalMessageId, Body) : base.ToString();
        }
    }
}
