using System;
using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class IncomingOrder
    /// </summary>
    [Serializable]
    public class IncomingOrder : PrimarySearchEntityBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IncomingOrder" /> class.
        /// </summary>
        public IncomingOrder()
        {
            IncomingOrderContact = new IncomingOrderContact {ContactRoleId = ContactRoleEnum.Customer};
            BillToContact = new IncomingOrderContact {ContactRoleId = ContactRoleEnum.BillTo};
            ShipToContact = new IncomingOrderContact {ContactRoleId = ContactRoleEnum.ShipTo};
            IncomingOrderCustomer = new IncomingOrderCustomer();
            ServiceLines = new List<IncomingOrderServiceLine>();
        }

        /// <summary>
        ///     +
        ///     Gets or sets the original XML parsed.
        /// </summary>
        /// <value>The original XML parsed.</value>
        public string OriginalXmlParsed { get; set; }

        /// <summary>
        ///     Gets or sets the task organization.
        /// </summary>
        /// <value>The task organization.</value>
        public string BusinessUnit { get; set; }

        /// <summary>
        ///     Gets or sets the project header status.
        /// </summary>
        /// <value>The project header status.</value>
        public string ProjectHeaderStatus { get; set; }

        /// <summary>
        ///     Gets or sets the creation date.
        /// </summary>
        /// <value>The creation date.</value>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        ///     Gets or sets the customer requested date.
        /// </summary>
        /// <value>The customer requested date.</value>
        public DateTime? CustomerRequestedDate { get; set; }

        /// <summary>
        ///     Gets or sets the date booked.
        /// </summary>
        /// <value>The date booked.</value>
        public DateTime? DateBooked { get; set; }

        /// <summary>
        ///     Gets or sets the date ordered.
        /// </summary>
        /// <value>The date ordered.</value>
        public DateTime? DateOrdered { get; set; }

        /// <summary>
        ///     Gets or sets the last update date.
        /// </summary>
        /// <value>The last update date.</value>
        public DateTime? LastUpdateDate { get; set; }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public string ExternalProjectId { get; set; }

        /// <summary>
        ///     Gets or sets the incoming order customer.
        /// </summary>
        /// <value>The incoming order customer.</value>
        public IncomingOrderCustomer IncomingOrderCustomer { get; set; }

        /// <summary>
        ///     Gets or sets the incoming order contact.
        /// </summary>
        /// <value>The incoming order contact.</value>
        public IncomingOrderContact IncomingOrderContact { get; set; }

        /// <summary>
        ///     Gets or sets the bill to contact.
        /// </summary>
        /// <value>The bill to contact.</value>
        public IncomingOrderContact BillToContact { get; set; }

        /// <summary>
        ///     Gets or sets the ship to contact.
        /// </summary>
        /// <value>The ship to contact.</value>
        public IncomingOrderContact ShipToContact { get; set; }

        /// <summary>
        ///     Gets the Oracle project name.  This field cannot be edited in Aria and only comes from Oracle.
        /// </summary>
        /// <value>The name of the project.</value>
        public string ProjectName { get; set; }

        /// <summary>
        ///     Gets or sets the project number.
        /// </summary>
        /// <value>The project number.</value>
        public string ProjectNumber { get; set; }

        /// <summary>
        ///     Gets or sets the order number.
        /// </summary>
        /// <value>The order number.</value>
        public string OrderNumber { get; set; }

        /// <summary>
        ///     Gets or sets the type of the order.
        /// </summary>
        /// <value>The type of the order.</value>
        public string OrderType { get; set; }

        /// <summary>
        ///     Gets or sets the service lines.
        /// </summary>
        /// <value>The service lines.</value>
        public IList<IncomingOrderServiceLine> ServiceLines { get; set; }

        /// <summary>
        ///     Gets or sets the customer PO.
        /// </summary>
        /// <value>The customer PO.</value>
        public string CustomerPo { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }

        /// <summary>
        ///     Gets or sets the work order business component id.
        /// </summary>
        /// <value>The work order business component id.</value>
        public string WorkOrderBusinessComponentId { get; set; }

        /// <summary>
        ///     Gets or sets the work order id.
        /// </summary>
        /// <value>The work order id.</value>
        public string WorkOrderId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the company.
        /// </summary>
        /// <value>
        ///     The name of the company.
        /// </value>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets the item count.
        /// </summary>
        /// <value>
        /// The item count.
        /// </value>
        public int ServiceLineItemCount { get { return ServiceLines == null ? 0 : ServiceLines.Count; } }

        /// <summary>
        /// Gets or sets a value indicating whether [hide from customer].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [hide from customer]; otherwise, <c>false</c>.
        /// </value>
        public bool HideFromCustomer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has order number.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has order number; otherwise, <c>false</c>.
        /// </value>
        public bool HasOrderNumber { get { return !string.IsNullOrEmpty(OrderNumber); }  }

		/// <summary>
		/// Gets or sets the message identifier.
		/// </summary>
		/// <value>The message identifier.</value>
	    public string MessageId { get; set; }

		/// <summary>
		/// Gets or sets the quote no.
		/// </summary>
		/// <value>The quote no.</value>
		public string QuoteNo { get; set; }

		/// <summary>
		/// Gets or sets the total order price.
		/// </summary>
		/// <value>The total order price.</value>
		public decimal? TotalOrderPrice { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public string Currency { get; set; }
    }
}