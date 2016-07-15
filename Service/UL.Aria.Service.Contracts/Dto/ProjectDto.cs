using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     class that represents a dto on the wire
    /// </summary>
    [DataContract]
    public class ProjectDto : IncomingOrderDto
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectDto" /> class.
        /// </summary>
        public ProjectDto()
        {
            IncomingOrderContact = new IncomingOrderContactDto();
            BillToContact = new IncomingOrderContactDto();
            ShipToContact = new IncomingOrderContactDto();
            IncomingOrderCustomer = new IncomingOrderCustomerDto();
            ServiceLines = new List<IncomingOrderServiceLineDto>();
        }

        /// <summary>
        ///     Gets or sets the project handler.
        /// </summary>
        /// <value>
        ///     The project handler.
        /// </value>
        [DataMember]
        public string ProjectHandler { get; set; }

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
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
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
        ///     Gets or sets the status description.
        /// </summary>
        /// <value>The status description.</value>
        [DataMember]
        public string StatusDescription { get; set; }

        /// <summary>
        ///     Gets or sets the estimated TAT date.
        /// </summary>
        /// <value>
        ///     The estimated TAT date.
        /// </value>
        [DataMember]
        public DateTime? EstimatedTATDate { get; set; }

        /// <summary>
        ///     Gets or sets the estimated lab hours.
        /// </summary>
        /// <value>
        ///     The estimated lab hours.
        /// </value>
        [DataMember]
        public decimal? EstimatedLabEffort { get; set; }

        /// <summary>
        ///     Gets or sets the estimate engineering effort.
        /// </summary>
        /// <value>
        ///     The estimate engineering effort.
        /// </value>
        [DataMember]
        public decimal? EstimateEngineeringEffort { get; set; }

        /// <summary>
        ///     Gets or sets the scope.
        /// </summary>
        /// <value>
        ///     The scope.
        /// </value>
        [DataMember]
        public string Scope { get; set; }

        /// <summary>
        ///     Gets or sets the assumptions.
        /// </summary>
        /// <value>
        ///     The assumptions.
        /// </value>
        [DataMember]
        public string Assumptions { get; set; }

        /// <summary>
        ///     Gets or sets the engineering office limitations.
        /// </summary>
        /// <value>
        ///     The engineering office limitations.
        /// </value>
        [DataMember]
        public string EngineeringOfficeLimitations { get; set; }

        /// <summary>
        ///     Gets or sets the laboratry limitations.
        /// </summary>
        /// <value>
        ///     The laboratry limitations.
        /// </value>
        [DataMember]
        public string LaboratoryLimitations { get; set; }

        /// <summary>
        ///     Gets or sets the complexity.
        /// </summary>
        /// <value>
        ///     The complexity.
        /// </value>
        [DataMember]
        public string Complexity { get; set; }

        /// <summary>
        ///     Gets or sets the additional criteria.
        /// </summary>
        /// <value>
        ///     The additional criteria.
        /// </value>
        [DataMember]
        public string AdditionalCriteria { get; set; }

        /// <summary>
        ///     Gets or sets the industry for manual projects.
        /// </summary>
        /// <value>
        ///     The industry.
        /// </value>
        [DataMember]
        public string Industry { get; set; }

        /// <summary>
        /// Gets or sets the industry code for manually created projects.
        /// </summary>
        /// <value>
        /// The industry code.
        /// </value>
        [DataMember]
        public string IndustryCode { get; set; }

        /// <summary>
        /// Gets or sets the service code for manual projects.
        /// </summary>
        /// <value>
        /// The service code.
        /// </value>
        [DataMember]
        public string ServiceCode { get; set; }

        /// <summary>
        ///     Gets or sets the industry category.
        /// </summary>
        /// <value>
        ///     The industry category.
        /// </value>
        [DataMember]
        public string IndustryCategory { get; set; }

        /// <summary>
        ///     Gets or sets the industry subcategory.
        /// </summary>
        /// <value>
        ///     The industry subcategory.
        /// </value>
        [DataMember]
        public string IndustrySubcategory { get; set; }

        /// <summary>
        ///     Gets or sets the location.
        /// </summary>
        /// <value>
        ///     The location.
        /// </value>
        [DataMember]
        public string Location { get; set; }

        /// <summary>
        ///     Gets or sets the product group.
        /// </summary>
        /// <value>
        ///     The product group.
        /// </value>
        [DataMember]
        public string ProductGroup { get; set; }

        /// <summary>
        ///     Gets or sets the project status.
        /// </summary>
        /// <value>The project status.</value>
        [DataMember]
        public ProjectStatusEnumDto ProjectStatus { get; set; }

        /// <summary>
        /// Gets or sets the original project template identifier.
        /// </summary>
        /// <value>
        /// The original project template identifier.
        /// </value>
        [DataMember]
        public Guid OriginalProjectTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the additional project template identifier.
        /// </summary>
        /// <value>
        /// The additional project template identifier.
        /// </value>
        [DataMember]
        public Guid? AdditionalProjectTemplateId { get; set; }

        /// <summary>
        ///     Gets or sets the order id.
        /// </summary>
        /// <value>
        ///     The order id.
        /// </value>
        [DataMember]
        public Guid? IncomingOrderId { get; set; }

        /// <summary>
        ///     Gets or sets the completion date.
        /// </summary>
        /// <value>The completion date.</value>
        [DataMember]
        public DateTime? CompletionDate { get; set; }

        /// <summary>
        ///     Gets or sets the days in current phase.
        /// </summary>
        /// <value>The days in current phase.</value>
        [DataMember]
        public int DaysInCurrentPhase { get; set; }

        /// <summary>
        ///     Gets or sets the estimated reviewer effort.
        /// </summary>
        /// <value>The estimated reviewer effort.</value>
        [DataMember]
        public decimal? EstimatedReviewerEffort { get; set; }

        /// <summary>
        ///     Gets or sets the number of samples.
        /// </summary>
        /// <value>The number of samples.</value>
        [DataMember]
        public int? NumberOfSamples { get; set; }

        /// <summary>
        ///     Gets or sets the sample reference numbers.
        /// </summary>
        /// <value>The sample reference numbers.</value>
        [DataMember]
        public string SampleReferenceNumbers { get; set; }

        /// <summary>
        ///     Gets or sets the CCN.
        /// </summary>
        /// <value>The CCN.</value>
        [DataMember]
        public string CCN { get; set; }

        /// <summary>
        ///     Gets or sets the file number.
        /// </summary>
        /// <value>The file number.</value>
        [DataMember]
        public string FileNo { get; set; }

        /// <summary>
        ///     Gets or sets the status notes.
        /// </summary>
        /// <value>The status notes.</value>
        [DataMember]
        public string StatusNotes { get; set; }

        /// <summary>
        ///     Gets or sets the inventory item catalog numbers.
        /// </summary>
        /// <value>The inventory item catalog numbers.</value>
        [DataMember]
        public string InventoryItemCatalogNumbers { get; set; }

        /// <summary>
        ///     Gets or sets the inventory item numbers descriptions.
        /// </summary>
        /// <value>The inventory item numbers descriptions.</value>
        [DataMember]
        public string InventoryItemNumbersDescriptions { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="ProjectDto" /> is expedited.
        /// </summary>
        /// <value><c>true</c> if expedited; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool Expedited { get; set; }

        /// <summary>
        ///     Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        [DataMember]
        public string Price { get; set; }

        /// <summary>
        ///     Gets or sets the standards.
        /// </summary>
        /// <value>The standards.</value>
        [DataMember]
        public string Standards { get; set; }

        /// <summary>
        ///     Gets or sets the type of the project.
        /// </summary>
        /// <value>The type of the project.</value>
        [DataMember]
        public string ProjectType { get; set; }

        /// <summary>
        ///     Gets or sets the service description.
        /// </summary>
        /// <value>The service description.</value>
        [DataMember]
        public string ServiceDescription { get; set; }

        /// <summary>
        ///     Gets or sets the name of the project template.
        /// </summary>
        /// <value>The name of the project template.</value>
        [DataMember]
        public string ProjectTemplateName { get; set; }

        /// <summary>
        /// Gets or sets whether the user requested to send email to Project Handler
        /// </summary>
        [DataMember]
        public bool IsEmailRequested { get; set; }

        /// <summary>
        /// Gets or sets whether the user requested to send email to Order Owner
        /// </summary>
        [DataMember]
        public bool IsOrderOwnerEmailRequested { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is on track.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is on track; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsOnTrack { get; set; }

        /// <summary>
        /// Gets or sets the order status.
        /// </summary>
        /// <value>
        /// The order status.
        /// </value>
         [DataMember]
        public string OrderStatus { get; set; }

         /// <summary>
         /// Gets or sets the order owner.
         /// </summary>
         /// <value>
         /// The order owner.
         /// </value>
         [DataMember]
         public string OrderOwner { get; set; }

         /// <summary>
         /// Gets or sets a value indicating whether [hide from customer].
         /// </summary>
         /// <value>
         ///   <c>true</c> if [hide from customer]; otherwise, <c>false</c>.
         /// </value>
        [DataMember]
         public bool HideFromCustomer { get; set; }

		/// <summary>
		/// Gets or sets the service request number.
		/// </summary>
		/// <value>
		/// The service request number.
		/// </value>
		[DataMember]
		public string ServiceRequestNumber { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [override automatic complete].
		/// </summary>
		/// <value>
		/// <c>true</c> if [override automatic complete]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool OverrideAutoComplete { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance has automatic complete.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance has automatic complete; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool HasAutoComplete { get; set; }
    }
}