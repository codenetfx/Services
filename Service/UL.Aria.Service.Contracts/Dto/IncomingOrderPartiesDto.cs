using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// DTO to encapsulate contacts and organization for an order.
    /// </summary>
    [DataContract]
    public class IncomingOrderPartiesDto
    {
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>
        /// The customer.
        /// </value>
        [DataMember]
        public IncomingOrderPartyDto Customer { get; set; }

        /// <summary>
        /// Gets or sets the agent.
        /// </summary>
        /// <value>
        /// The agent.
        /// </value>
        [DataMember]
        public IncomingOrderPartyDto Agent { get; set; }
    }
}