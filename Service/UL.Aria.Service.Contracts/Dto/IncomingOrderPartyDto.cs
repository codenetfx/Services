using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// DTO to encapsulate contact and organization for an order party.
    /// </summary>
    [DataContract]
    public class IncomingOrderPartyDto
    {
        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>
        /// The contact.
        /// </value>
        [DataMember]
        public IncomingOrderContactDto Contact { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        /// <value>
        /// The organization.
        /// </value>
        [DataMember]
        public IncomingOrderCustomerDto Organization { get; set; }

        /// <summary>
        /// Gets or sets the party site number.
        /// </summary>
        /// <value>
        /// The party site number.
        /// </value>
        [DataMember]
        public string PartySiteNumber { get; set; }

        /// <summary>
        /// Gets or sets the party site identifier.
        /// </summary>
        /// <value>
        /// The party site identifier.
        /// </value>
        [DataMember]
        public string PartySiteId { get; set; }
       
        /// <summary>
        /// Gets or sets the account number.
        /// </summary>
        /// <value>
        /// The account number.
        /// </value>
        [DataMember]
        public string AccountNumber { get; set; }
        
        /// <summary>
        /// Gets or sets the party identifier.
        /// </summary>
        /// <value>
        /// The party identifier.
        /// </value>
        [DataMember]
        public string PartyId { get; set; }
    }
}