using System.Collections.Generic;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.InboundOrderProcessing.Domain
{
    /// <summary>
    ///     A class defining data for order messages received from other systems.
    /// </summary>
    public class OrderMessage :DomainEntity
    {
        private readonly List<KeyValuePair<string, object>> _properties = new List<KeyValuePair<string, object>>();

		/// <summary>
		/// Gets or sets the message identifier.
		/// </summary>
		/// <value>The message identifier.</value>
	    public string MessageId { get; set; }

        /// <summary>
        ///     Gets or sets the message body.
        /// </summary>
        /// <value>
        ///     The message body.
        /// </value>
        public string Body { get; set; }

        /// <summary>
        ///     Gets or sets the message originator.
        /// </summary>
        /// <value>
        ///     The originator.
        /// </value>
        public string Originator { get; set; }

        /// <summary>
        ///     Gets or sets the receiver.
        /// </summary>
        /// <value>
        ///     The receiver.
        /// </value>
        public string Receiver { get; set; }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>
        ///     The id.
        /// </value>
        public string ExternalMessageId { get; set; }

        /// <summary>
        ///     The properties as a list of key value pairs.
        /// </summary>
        /// <remarks>
        ///     This is <em>not</em> intended to be a dictionary as there may be more than one instance of a given property.
        /// </remarks>
        public List<KeyValuePair<string, object>> Properties
        {
            get { return _properties; }
        }
    }
}