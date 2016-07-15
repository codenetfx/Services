using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class IncomingOrderDto
    /// </summary>
    [DataContract]
    public class IncomingOrderDto
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IncomingOrderDto" /> class.
        /// </summary>
        public IncomingOrderDto()
        {
            IncomingOrderContact = new IncomingOrderContactDto();
            BillToContact = new IncomingOrderContactDto();
            ShipToContact = new IncomingOrderContactDto();
            IncomingOrderCustomer = new IncomingOrderCustomerDto();
            ServiceLines = new List<IncomingOrderServiceLineDto>();
        }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        ///     Gets or sets the original XML parsed.
        /// </summary>
        /// <value>The original XML parsed.</value>
        [DataMember]
        public string OriginalXmlParsed { get; set; }

        /// <summary>
        ///     Gets or sets the task organization.
        /// </summary>
        /// <value>The task organization.</value>
        [DataMember]
        public string BusinessUnit { get; set; }

        /// <summary>
        ///     Gets or sets the project header status.
        /// </summary>
        /// <value>The project header status.</value>
        [DataMember]
        public string ProjectHeaderStatus { get; set; }

        /// <summary>
        ///     Gets or sets the creation date.
        /// </summary>
        /// <value>The creation date.</value>
        [DataMember]
        public DateTime CreationDate { get; set; }

        /// <summary>
        ///     Gets or sets the customer requested date.
        /// </summary>
        /// <value>The customer requested date.</value>
        [DataMember]
        public DateTime? CustomerRequestedDate { get; set; }

        /// <summary>
        ///     Gets or sets the date booked.
        /// </summary>
        /// <value>The date booked.</value>
        [DataMember]
        public DateTime? DateBooked { get; set; }

        /// <summary>
        ///     Gets or sets the date ordered.
        /// </summary>
        /// <value>The date ordered.</value>
        [DataMember]
        public DateTime? DateOrdered { get; set; }

        /// <summary>
        ///     Gets or sets the Buy/Pay last updated date.  This should not be updated by Aria.
        /// </summary>
        /// <value>The last update date.</value>
        [DataMember]
        public DateTime? LastUpdateDate { get; set; }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember]
        public string ExternalProjectId { get; set; }

        /// <summary>
        ///     Gets or sets the incoming order customer.
        /// </summary>
        /// <value>The incoming order customer.</value>
        [DataMember]
        public IncomingOrderCustomerDto IncomingOrderCustomer { get; set; }

        /// <summary>
        ///     Gets or sets the incoming order contact.
        /// </summary>
        /// <value>The incoming order contact.</value>
        [DataMember]
        public IncomingOrderContactDto IncomingOrderContact { get; set; }

        /// <summary>
        ///     Gets or sets the bill to contact.
        /// </summary>
        /// <value>The bill to contact.</value>
        [DataMember]
        public IncomingOrderContactDto BillToContact { get; set; }

        /// <summary>
        ///     Gets or sets the ship to contact.
        /// </summary>
        /// <value>The ship to contact.</value>
        [DataMember]
        public IncomingOrderContactDto ShipToContact { get; set; }

        /// <summary>
        ///     Gets or sets the project number.
        /// </summary>
        /// <value>The project number.</value>
        [DataMember]
        public string ProjectNumber { get; set; }

        /// <summary>
        ///     Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        [DataMember]
        public string ProjectName { get; set; }

        /// <summary>
        ///     Gets or sets the order number.
        /// </summary>
        /// <value>The order number.</value>
        [DataMember]
        public string OrderNumber { get; set; }

        /// <summary>
        ///     Gets or sets the type of the order.
        /// </summary>
        /// <value>The type of the order.</value>
        [DataMember]
        public string OrderType { get; set; }

        /// <summary>
        ///     Gets or sets the service lines.
        /// </summary>
        /// <value>The service lines.</value>
        [DataMember]
        public IList<IncomingOrderServiceLineDto> ServiceLines { get; set; }

        /// <summary>
        ///     Gets or sets the customer PO.
        /// </summary>
        /// <value>The customer PO.</value>
        [DataMember]
        public string CustomerPo { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        ///     Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        [DataMember]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the created by id.
        /// </summary>
        /// <value>The created by id.</value>
        [DataMember]
        public Guid CreatedById { get; set; }

        /// <summary>
        ///     Gets or sets the Aria auditing updated date time.
        /// </summary>
        /// <value>The updated date time.</value>
        [DataMember]
        public DateTime UpdatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the Aria auditing updated user id.
        /// </summary>
        /// <value>The updated by id.</value>
        [DataMember]
        public Guid UpdatedById { get; set; }

        /// <summary>
        ///     Gets or sets the company id.
        /// </summary>
        /// <value>
        ///     The company id.
        /// </value>
        [DataMember]
        public Guid? CompanyId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the company.
        /// </summary>
        /// <value>
        ///     The name of the company.
        /// </value>
        [DataMember]
        public string CompanyName { get; set; }

        /// <summary>
        ///     Gets or sets the work order business component id.
        /// </summary>
        /// <value>The work order business component id.</value>
        [DataMember]
        public string WorkOrderBusinessComponentId { get; set; }

        /// <summary>
        ///     Gets or sets the work order id.
        /// </summary>
        /// <value>The work order id.</value>
        [DataMember]
        public string WorkOrderId { get; set; }

        /// <summary>
        ///     Gets or sets the container id.
        /// </summary>
        /// <value>
        ///     The container id.
        /// </value>
        [DataMember]
        public Guid ContainerId { get; set; }

		/// <summary>
		/// Gets or sets the quote no.
		/// </summary>
		/// <value>The quote no.</value>
		[DataMember]
		public string QuoteNo { get; set; }

		/// <summary>
		/// Gets or sets the total order price.
		/// </summary>
		/// <value>The total order price.</value>
		[DataMember]
		public decimal? TotalOrderPrice { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currenct.
        /// </value>
        [DataMember]
        public string Currency { get; set; }
	}
}