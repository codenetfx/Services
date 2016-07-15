using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Defines attributes of an organization.
    /// </summary>
    [DataContract]
    public class CustomerOrganizationDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerOrganizationDto"/> class.
        /// </summary>
        public CustomerOrganizationDto()
        {
            Contacts= new List<IncomingOrderContactDto>();
            Locations = new List<IncomingOrderContactDto>();

        }
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>
        /// The customer.
        /// </value>
        [DataMember]
        public IncomingOrderCustomerDto Customer { get; set; }

        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        /// <value>
        /// The contacts.
        /// </value>
        [DataMember]
        public IList<IncomingOrderContactDto> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the locations.
        /// </summary>
        /// <value>
        /// The locations.
        /// </value>
        [DataMember]
        public IList<IncomingOrderContactDto> Locations { get; set; }
    }
}