using System;
using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     A BO for describing requests to create projects.
    /// </summary>
    public class ProjectCreationRequest
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectCreationRequest" /> class.
        /// </summary>
        public ProjectCreationRequest()
        {
            ServiceLineItems = new List<IncomingOrderServiceLine>();
        }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>
        ///     The id.
        /// </value>
        public Guid? Id { get; set; }

        /// <summary>
        ///     Gets or sets the order id.
        /// </summary>
        /// <value>
        ///     The order id.
        /// </value>
        public Guid? IncomingOrderId { get; set; }

        /// <summary>
        ///     Gets or sets the order number
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        ///     Gets or sets the service line items.
        /// </summary>
        /// <value>
        ///     The service line items.
        /// </value>
        public IList<IncomingOrderServiceLine> ServiceLineItems { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the start date.
        /// </summary>
        /// <value>
        ///     The start date.
        /// </value>
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///     Gets or sets the end date.
        /// </summary>
        /// <value>
        ///     The end date.
        /// </value>
        public DateTime? EndDate { get; set; }

        /// <summary>
        ///     Gets or sets the project handler.
        /// </summary>
        /// <value>
        ///     The project handler.
        /// </value>
        public string ProjectHandler { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is ready to create.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is ready to create; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadyToCreate { get; set; }

        /// <summary>
        ///     Gets or sets the project template id.
        /// </summary>
        /// <value>The project template id.</value>
        public Guid ProjectTemplateId { get; set; }

        /// <summary>
        ///     Gets or sets the company id.
        /// </summary>
        /// <value>The company id.</value>
        public Guid? CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the number of samples.
        /// </summary>
        /// <value>The number of samples.</value>
        public int? NumberOfSamples { get; set; }

        /// <summary>
        /// Gets or sets the sample reference numbers.
        /// </summary>
        /// <value>The sample reference numbers.</value>
        public string SampleReferenceNumbers { get; set; }

        /// <summary>
        /// Gets or sets the CCN.
        /// </summary>
        /// <value>The CCN.</value>
        public string CCN { get; set; }

        /// <summary>
        /// Gets or sets the file no.
        /// </summary>
        /// <value>The file no.</value>
        public string FileNo { get; set; }

        /// <summary>
        /// Gets or sets the status notes.
        /// </summary>
        /// <value>The status notes.</value>
        public string StatusNotes { get; set; }

        /// <summary>
        /// Gets or sets whether the user requested to send email to Project Handler
        /// </summary>
        public bool IsEmailRequested { get; set; }

        /// <summary>
        /// Gets or sets the additional criteria.
        /// </summary>
        /// <value>
        /// The additional criteria.
        /// </value>
        public string AdditionalCriteria { get; set; }

        /// <summary>
        /// Gets or sets the industry code.
        /// </summary>
        /// <value>
        /// The industry code.
        /// </value>
        public string IndustryCode { get; set; }

        /// <summary>
        /// Gets or sets the service code.
        /// </summary>
        /// <value>
        /// The service code.
        /// </value>
        public string ServiceCode { get; set; }

		/// <summary>
		/// Gets or sets the quote no.
		/// </summary>
		/// <value>
		/// The quote no.
		/// </value>
		public string QuoteNo { get; set; }

		/// <summary>
		/// Gets or sets the service request number.
		/// </summary>
		/// <value>
		/// The service request number.
		/// </value>
		public string ServiceRequestNumber { get; set; }

        /// <summary>
        ///     Gets or sets the order owner.
        /// </summary>
        /// <value>
        ///     The order owner.
        /// </value>
        public string OrderOwner { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is order owner email requested.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is order owner email requested; otherwise, <c>false</c>.
		/// </value>
		public bool IsOrderOwnerEmailRequested { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [override automatic complete].
		/// </summary>
		/// <value>
		/// <c>true</c> if [override automatic complete]; otherwise, <c>false</c>.
		/// </value>
		public bool OverrideAutoComplete { get; set; }
    }
}