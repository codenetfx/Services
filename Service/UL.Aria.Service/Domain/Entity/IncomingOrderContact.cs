using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Class NewProjectContact
    /// </summary>
    [Serializable]
    public class IncomingOrderContact : TrackedDomainEntity
    {
        /// <summary>
        ///     Gets or sets the id of the parent entity (e.g. <see cref="IncomingOrder"/>, <see cref="Project"/>, <see cref="Order"/>.
        /// </summary>
        /// <value>The incoming order id.</value>
        public Guid IncomingOrderId { get; set; }

        /// <summary>
        ///     Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName { get; set; }

        /// <summary>
        ///     Gets or sets the contact role identifier.
        /// </summary>
        /// <value>The contact role identifier.</value>
        public ContactRoleEnum ContactRoleId { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the email.
        /// </summary>
        /// <value>
        ///     The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        ///     Gets or sets the address.
        /// </summary>
        /// <value>
        ///     The address.
        /// </value>
        public string Address { get; set; }

        /// <summary>
        ///     Gets or sets the phone.
        /// </summary>
        /// <value>
        ///     The phone.
        /// </value>
        public string Phone { get; set; }

        /// <summary>
        ///     Gets or sets the state.
        /// </summary>
        /// <value>
        ///     The state.
        /// </value>
        public string State { get; set; }

        /// <summary>
        ///     Gets or sets the country.
        /// </summary>
        /// <value>
        ///     The country.
        /// </value>
        public string Country { get; set; }

        /// <summary>
        ///     Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City { get; set; }

        /// <summary>
        ///     Gets or sets the province.
        /// </summary>
        /// <value>The province.</value>
        public string Province { get; set; }

        /// <summary>
        ///     Gets or sets the postal code.
        /// </summary>
        /// <value>The postal code.</value>
        public string PostalCode { get; set; }


        /// <summary>
        /// Gets or sets the external identifier.
        /// </summary>
        /// <value>
        /// The external identifier.
        /// </value>
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the subscriber number.
        /// </summary>
        /// <value>
        /// The subscriber number.
        /// </value>
        public string SubscriberNumber { get; set; }

        /// <summary>
        /// Gets or sets the party site number.
        /// </summary>
        /// <value>
        /// The party site number.
        /// </value>
        public string PartySiteNumber { get; set; }

        /// <summary>
        ///     Maps the incoming order contact.
        /// </summary>
        /// <param name="incomingOrderContact">The incoming order contact.</param>
        /// <returns>IncomingOrderContact.</returns>
        public static IncomingOrderContact MapIncomingOrderContact(IncomingOrderContact incomingOrderContact)
        {
            return new IncomingOrderContact
            {
                Address = incomingOrderContact.Address,
                City = incomingOrderContact.City,
                ContactRoleId = incomingOrderContact.ContactRoleId,
                Country = incomingOrderContact.Country,
                Email = incomingOrderContact.Email,
                FullName = incomingOrderContact.FullName,
                IncomingOrderId = incomingOrderContact.IncomingOrderId,
                Phone = incomingOrderContact.Phone,
                PostalCode = incomingOrderContact.PostalCode,
                Province = incomingOrderContact.Province,
                State = incomingOrderContact.State,
                Title = incomingOrderContact.Title,
                ExternalId = incomingOrderContact.ExternalId,
                PartySiteNumber = incomingOrderContact.PartySiteNumber,
                SubscriberNumber = incomingOrderContact.SubscriberNumber
            };
        }
    }
}