using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Defines attributes of an organization.
    /// </summary>
    public class CustomerOrganization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerOrganization"/> class.
        /// </summary>
        public CustomerOrganization()
        {
            Contacts = new List<IncomingOrderContact>();
            Locations = new List<IncomingOrderContact>();

        }
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>
        /// The customer.
        /// </value>
        public IncomingOrderCustomer Customer { get; set; }

        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        /// <value>
        /// The contacts.
        /// </value>
        public IList<IncomingOrderContact> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the locations.
        /// </summary>
        /// <value>
        /// The locations.
        /// </value>
        public IList<IncomingOrderContact> Locations { get; set; }
    }
}