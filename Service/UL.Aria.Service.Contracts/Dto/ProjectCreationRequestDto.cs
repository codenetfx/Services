using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     A DTO for describing requests to create projects.
    /// </summary>
    [DataContract]
    public class ProjectCreationRequestDto
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectCreationRequestDto" /> class.
        /// </summary>
        public ProjectCreationRequestDto()
        {
            ServiceLineItems = new List<IncomingOrderServiceLineDto>();
        }


        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>
        ///     The id.
        /// </value>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        ///     Gets or sets the order id.
        /// </summary>
        /// <value>
        ///     The order id.
        /// </value>
        [DataMember]
        public Guid? IncomingOrderId { get; set; }

        /// <summary>
        ///     Gets or sets the order number
        /// </summary>
        [DataMember]
        public string OrderNumber { get; set; }


        /// <summary>
        ///     Gets or sets the order owner
        /// </summary>
        [DataMember]
        public string OrderOwner { get; set; }

        /// <summary>
        ///     Gets or sets the service line items.
        /// </summary>
        /// <value>
        ///     The service line items.
        /// </value>
        [DataMember]
        public IList<IncomingOrderServiceLineDto> ServiceLineItems { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the start date.
        /// </summary>
        /// <value>
        ///     The start date.
        /// </value>
        [DataMember]
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///     Gets or sets the end date.
        /// </summary>
        /// <value>
        ///     The end date.
        /// </value>
        [DataMember]
        public DateTime? EndDate { get; set; }

        /// <summary>
        ///     Gets or sets the project handler.
        /// </summary>
        /// <value>
        ///     The project handler.
        /// </value>
        [DataMember]
        public string ProjectHandler { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is ready to create.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is ready to create; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsReadyToCreate { get; set; }

        /// <summary>
        ///     Gets or sets the project template id.
        /// </summary>
        /// <value>The project template id.</value>
        [DataMember]
        public Guid ProjectTemplateId { get; set; }

        /// <summary>
        ///     Gets or sets the company id.
        /// </summary>
        /// <value>The company id.</value>
        [DataMember]
        public Guid? CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the number of samples.
        /// </summary>
        /// <value>The number of samples.</value>
        [DataMember]
		public int? NumberOfSamples { get; set; }

        /// <summary>
        /// Gets or sets the sample reference numbers.
        /// </summary>
        /// <value>The sample reference numbers.</value>
        [DataMember]
        public string SampleReferenceNumbers { get; set; }

        /// <summary>
        /// Gets or sets the CCN.
        /// </summary>
        /// <value>The CCN.</value>
        [DataMember]
        public string CCN { get; set; }

        /// <summary>
        /// Gets or sets the file no.
        /// </summary>
        /// <value>The file no.</value>
        [DataMember]
        public string FileNo { get; set; }

        /// <summary>
        /// Gets or sets the status notes.
        /// </summary>
        /// <value>The status notes.</value>
        [DataMember]
        public string StatusNotes { get; set; }

        /// <summary>
        /// Gets or sets the additional criteria.
        /// </summary>
        /// <value>
        /// The additional criteria.
        /// </value>
        [DataMember]
        public string AdditionalCriteria { get; set; }

        /// <summary>
        /// Gets or sets whether the user requested to send email to Project Handler
        /// </summary>
        [DataMember]
        public bool IsEmailRequested { get; set; }


        /// <summary>
        /// Gets or sets the industry code.
        /// </summary>
        /// <value>
        /// The industry code.
        /// </value>
        [DataMember]
        public string IndustryCode { get; set; }

        /// <summary>
        /// Gets or sets the service code.
        /// </summary>
        /// <value>
        /// The service code.
        /// </value>
        [DataMember]
        public string ServiceCode { get; set; }

		/// <summary>
		/// Gets or sets the quote no.
		/// </summary>
		/// <value>
		/// The quote no.
		/// </value>
		 [DataMember]
		public string QuoteNo { get; set; }

		 /// <summary>
		 /// Gets or sets the service request number.
		 /// </summary>
		 /// <value>
		 /// The service request number.
		 /// </value>
		[DataMember]
		 public string ServiceRequestNumber { get; set; }


		/// <summary>
		/// Gets or sets a value indicating whether this instance is order owner email requested.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is order owner email requested; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool IsOrderOwnerEmailRequested { get; set; }


		/// <summary>
		/// Gets or sets a value indicating whether [override automatic complete].
		/// </summary>
		/// <value>
		/// <c>true</c> if [override automatic complete]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool OverrideAutoComplete { get; set; }




    }
}