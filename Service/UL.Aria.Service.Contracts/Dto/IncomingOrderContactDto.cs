using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class NewProjectContactDto
    /// </summary>
    [DataContract]
    public class IncomingOrderContactDto
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        ///     Gets or sets the incoming order id.
        /// </summary>
        /// <value>The incoming order id.</value>
        [DataMember]
        public Guid IncomingOrderId { get; set; }

        /// <summary>
        ///     Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        ///     Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        [DataMember]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the created by identifier.
        /// </summary>
        /// <value>The created by identifier.</value>
        [DataMember]
        public Guid CreatedById { get; set; }

        /// <summary>
        ///     Gets or sets the updated date time.
        /// </summary>
        /// <value>The updated date time.</value>
        [DataMember]
        public DateTime UpdatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the updated by identifier.
        /// </summary>
        /// <value>The updated by identifier.</value>
        [DataMember]
        public Guid UpdatedById { get; set; }

        /// <summary>
        ///     Gets or sets the email.
        /// </summary>
        /// <value>
        ///     The email.
        /// </value>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the address.
        /// </summary>
        /// <value>
        ///     The address.
        /// </value>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        ///     Gets or sets the phone.
        /// </summary>
        /// <value>
        ///     The phone.
        /// </value>
        [DataMember]
        public string Phone { get; set; }

        /// <summary>
        ///     Gets or sets the state.
        /// </summary>
        /// <value>
        ///     The state.
        /// </value>
        [DataMember]
        public string State { get; set; }

        /// <summary>
        ///     Gets or sets the country.
        /// </summary>
        /// <value>
        ///     The country.
        /// </value>
        [DataMember]
        public string Country { get; set; }

        /// <summary>
        ///     Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        [DataMember]
        public string City { get; set; }

        /// <summary>
        ///     Gets or sets the province.
        /// </summary>
        /// <value>The province.</value>
        [DataMember]
        public string Province { get; set; }

        /// <summary>
        ///     Gets or sets the postal code.
        /// </summary>
        /// <value>The postal code.</value>
        [DataMember]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the external identifier.
        /// </summary>
        /// <value>
        /// The external identifier.
        /// </value>
        [DataMember]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the subscriber number.
        /// </summary>
        /// <value>
        /// The subscriber number.
        /// </value>
        [DataMember]
        public string SubscriberNumber { get; set; }

        /// <summary>
        /// Gets or sets the party site number.
        /// </summary>
        /// <value>
        /// The party site number.
        /// </value>
        [DataMember]
        public string PartySiteNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        [DataMember]
        public string CompanyName { get; set; }
    }
}