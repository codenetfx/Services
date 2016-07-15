using System;
using System.Linq;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Aria.Service.Contracts.Dto;
using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class Project
    /// </summary>
    [Serializable]
    public class Project : IncomingOrder
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Project" /> class.
        /// </summary>
        public Project()
        {
            Type = EntityTypeEnumDto.Project;
            Tasks = new List<Task>();
        }

        /// <summary>
        /// Gets or sets whether the user requested to send email to Project Handler
        /// </summary>        
        public bool IsEmailRequested { get; set; }

        /// <summary>
        /// Gets or sets whether the user requested to send email to Order Owner
        /// </summary>        
        public bool IsOrderOwnerEmailRequested { get; set; }  

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
        ///     Gets or sets the estimated TAT date.
        /// </summary>
        /// <value>
        ///     The estimated TAT date.
        /// </value>
        public DateTime? EstimatedTATDate { get; set; }

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
        ///     Gets or sets the estimated lab hours.
        /// </summary>
        /// <value>
        ///     The estimated lab hours.
        /// </value>
        public decimal? EstimatedLabEffort { get; set; }

        /// <summary>
        ///     Gets or sets the estimate engineering effort.
        /// </summary>
        /// <value>
        ///     The estimate engineering effort.
        /// </value>
        public decimal? EstimateEngineeringEffort { get; set; }

        /// <summary>
        ///     Gets or sets the scope.
        /// </summary>
        /// <value>
        ///     The scope.
        /// </value>
        public string Scope { get; set; }

        /// <summary>
        ///     Gets or sets the assumptions.
        /// </summary>
        /// <value>
        ///     The assumptions.
        /// </value>
        public string Assumptions { get; set; }

        /// <summary>
        ///     Gets or sets the engineering office limitations.
        /// </summary>
        /// <value>
        ///     The engineering office limitations.
        /// </value>
        public string EngineeringOfficeLimitations { get; set; }

        /// <summary>
        ///     Gets or sets the laboratry limitations.
        /// </summary>
        /// <value>
        ///     The laboratry limitations.
        /// </value>
        public string LaboratoryLimitations { get; set; }

        /// <summary>
        ///     Gets or sets the complexity.
        /// </summary>
        /// <value>
        ///     The complexity.
        /// </value>
        public string Complexity { get; set; }

        /// <summary>
        ///     Gets or sets the additional criteria.
        /// </summary>
        /// <value>
        ///     The additional criteria.
        /// </value>
        public string AdditionalCriteria { get; set; }

        /// <summary>
        ///     Gets or sets the industry.
        /// </summary>
        /// <value>
        ///     The industry.
        /// </value>
        public string Industry { get; set; }

        /// <summary>
        /// Gets or sets the industry code for manually created projects.
        /// </summary>
        /// <value>
        /// The industry code.
        /// </value>
        public string IndustryCode { get; set; }

        /// <summary>
        /// Gets or sets the service code for manual projects.
        /// </summary>
        /// <value>
        /// The service code.
        /// </value>
        public string ServiceCode { get; set; }

        /// <summary>
        ///     Gets or sets the industry category.
        /// </summary>
        /// <value>
        ///     The industry category.
        /// </value>
        public string IndustryCategory { get; set; }

        /// <summary>
        ///     Gets or sets the industry subcategory.
        /// </summary>
        /// <value>
        ///     The industry subcategory.
        /// </value>
        public string IndustrySubcategory { get; set; }

        /// <summary>
        ///     Gets or sets the location.
        /// </summary>
        /// <value>
        ///     The location.
        /// </value>
        public string Location { get; set; }

        /// <summary>
        ///     Gets or sets the product group.
        /// </summary>
        /// <value>
        ///     The product group.
        /// </value>
        public string ProductGroup { get; set; }

        /// <summary>
        ///     Gets or sets the status description.
        /// </summary>
        /// <value>The status description.</value>
        public string StatusDescription { get; set; }

        /// <summary>
        ///     Gets or sets the project status.
        /// </summary>
        /// <value>The project status.</value>
        public ProjectStatusEnumDto ProjectStatus { get; set; }

        /// <summary>
        ///     Gets or sets the project status.
        /// </summary>
        /// <value>The project status.</value>
        public string ProjectStatusLabel { get { return ProjectStatus.GetDisplayName(); }}

        /// <summary>
        /// Gets or sets a value indicating whether this instance is on track.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is on track; otherwise, <c>false</c>.
        /// </value>
        public bool IsOnTrack { get; set; }

        /// <summary>
        ///     Gets or sets the project template id used for tasks
        /// </summary>
        /// <value>
        ///     The project template id.
        /// </value>
        public Guid ProjectTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the additional project template identifier, used only when adding new templates to existing projects.
        /// Will not be 
        /// </summary>
        /// <value>
        /// The additional project template identifier.
        /// </value>
        public Guid? AdditionalProjectTemplateId { get; set; }

        /// <summary>
        ///     Gets or sets the order id.
        /// </summary>
        /// <value>
        ///     The order id.
        /// </value>
        public Guid? IncomingOrderId { get; set; }

        /// <summary>
        ///     Gets or sets the completion date.
        /// </summary>
        /// <value>The completion date.</value>
        public DateTime? CompletionDate { get; set; }

        /// <summary>
        ///     Gets or sets the days in current phase.
        /// </summary>
        /// <value>The days in current phase.</value>
        public int DaysInCurrentPhase { get; set; }

        /// <summary>
        ///     Gets or sets the estimated reviewer effort.
        /// </summary>
        /// <value>The estimated reviewer effort.</value>
        public decimal? EstimatedReviewerEffort { get; set; }

        /// <summary>
        ///     Gets or sets the number of samples.
        /// </summary>
        /// <value>The number of samples.</value>
        public int? NumberOfSamples { get; set; }

        /// <summary>
        ///     Gets or sets the sample reference numbers.
        /// </summary>
        /// <value>The sample reference numbers.</value>
        public string SampleReferenceNumbers { get; set; }

        /// <summary>
        ///     Gets or sets the CCN.
        /// </summary>
        /// <value>The CCN.</value>
        public string CCN { get; set; }

        /// <summary>
        ///     Gets or sets the file number.
        /// </summary>
        /// <value>The file number.</value>
        public string FileNo { get; set; }

        /// <summary>
        ///     Gets or sets the status notes.
        /// </summary>
        /// <value>The status notes.</value>
        public string StatusNotes { get; set; }

        /// <summary>
        ///     Gets or sets the inventory item catalog numbers.
        /// </summary>
        /// <value>The inventory item catalog numbers.</value>
        public string InventoryItemCatalogNumbers { get; set; }

        /// <summary>
        ///     Gets or sets the inventory item numbers descriptions.
        /// </summary>
        /// <value>The inventory item numbers descriptions.</value>
        public string InventoryItemNumbersDescriptions { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="Project" /> is expedited.
        /// </summary>
        /// <value><c>true</c> if expedited; otherwise, <c>false</c>.</value>
        public bool Expedited { get; set; }

        /// <summary>
        ///     Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public string Price { get; set; }

        /// <summary>
        ///     Gets or sets the standards.
        /// </summary>
        /// <value>The standards.</value>
        public string Standards { get; set; }

        /// <summary>
        ///     Gets or sets the type of the project.
        /// </summary>
        /// <value>The type of the project.</value>
        public string ProjectType { get; set; }

        /// <summary>
        ///     Gets or sets the service description.
        /// </summary>
        /// <value>The service description.</value>
        public string ServiceDescription { get; set; }

        /// <summary>
        ///     Gets or sets the name of the project template.
        /// </summary>
        /// <value>The name of the project template.</value>
        public string ProjectTemplateName { get; set; }

        /// <summary>
        /// Gets or sets the order status.
        /// </summary>
        /// <value>
        /// The order status.
        /// </value>
        public string OrderStatus { get; set; }

		/// <summary>
		/// Gets or sets the task minimum due date.
		/// </summary>
		/// <value>The task minimum due date.</value>
		public DateTime? TaskMinimumDueDate { get; set; }

		/// <summary>
		/// Gets or sets the minimum due date task identifier.
		/// </summary>
		/// <value>The minimum due date task identifier.</value>
		public Guid? MinimumDueDateTaskId { get; set; }


        /// <summary>
        /// Gets the incoming order service line industry code.
        /// </summary>
        /// <value>
        /// The incoming order service line industry code.
        /// </value>
        public string IncomingOrderServiceLineIndustryCode
        {
            get
            {
                AssureServicelinesMetaParsed();
                return this.serviceLineMetaItemsDictionary[AssetFieldNames.AriaProjectIndustryCode];
            }
        }

        /// <summary>
        /// Gets the incoming order service line service code.
        /// </summary>
        /// <value>
        /// The incoming order service line service code.
        /// </value>
        public string IncomingOrderServiceLineServiceCode
        {
            get
            {
                AssureServicelinesMetaParsed();
                return this.serviceLineMetaItemsDictionary[AssetFieldNames.AriaProjectServiceCode];
            }
        }

        /// <summary>
        /// Gets the name of the incoming order service line location.
        /// </summary>
        /// <value>
        /// The name of the incoming order service line location.
        /// </value>
        public string IncomingOrderServiceLineLocationName
        {
            get
            {
                AssureServicelinesMetaParsed();
                return this.serviceLineMetaItemsDictionary[AssetFieldNames.AriaProjectLocationName];
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is closed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is closed; otherwise, <c>false</c>.
        /// </value>
        public bool IsClosed
        {
            get
            {
                return this.ProjectStatus == ProjectStatusEnumDto.Completed ||
                       this.ProjectStatus == ProjectStatusEnumDto.Canceled;
            }
            
        }

        /// <summary>
        /// Gets the incoming order service line location code.
        /// </summary>
        /// <value>
        /// The incoming order service line location code.
        /// </value>
        public string IncomingOrderServiceLineLocationCode
        {
            get
            {
                AssureServicelinesMetaParsed();
                return this.serviceLineMetaItemsDictionary[AssetFieldNames.AriaProjectLocationCode];
            }
        }

        private Dictionary<string, string> serviceLineMetaItemsDictionary = null;
        private static object pad_lock = new object();

        private void AssureServicelinesMetaParsed()
        {
            string sharePointDelimiter = AssetFieldNames.SharePointMultivalueSeparator.ToString();
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            Dictionary<string, string> finalDict = new Dictionary<string, string>();

            if (serviceLineMetaItemsDictionary == null)
            {
                lock (pad_lock)
                {
                    if (serviceLineMetaItemsDictionary == null)
                    {
                        dict.Add(AssetFieldNames.AriaProjectIndustryCode, new List<string>());
                        dict.Add(AssetFieldNames.AriaProjectLocationCode, new List<string>());
                        dict.Add(AssetFieldNames.AriaProjectServiceCode, new List<string>());
                        dict.Add(AssetFieldNames.AriaProjectLocationName, new List<string>());

                        if (ServiceLines != null && ServiceLines.Any())
                        {
                            foreach (var line in ServiceLines)
                            {
								if (!string.IsNullOrWhiteSpace(line.IndustryCode) && !dict[AssetFieldNames.AriaProjectIndustryCode].Any(x=> string.Equals(x, line.IndustryCode, StringComparison.OrdinalIgnoreCase )))
                                    dict[AssetFieldNames.AriaProjectIndustryCode].Add(line.IndustryCode);

								if (!string.IsNullOrWhiteSpace(line.LocationCode) && !dict[AssetFieldNames.AriaProjectLocationCode].Any(x => string.Equals(x, line.LocationCode, StringComparison.OrdinalIgnoreCase)))
                                    dict[AssetFieldNames.AriaProjectLocationCode].Add(line.LocationCode);

								if (!string.IsNullOrWhiteSpace(line.ServiceCode) && !dict[AssetFieldNames.AriaProjectServiceCode].Any(x => string.Equals(x, line.ServiceCode, StringComparison.OrdinalIgnoreCase)))
                                    dict[AssetFieldNames.AriaProjectServiceCode].Add(line.ServiceCode);

								if (!string.IsNullOrWhiteSpace(line.LocationName) && !dict[AssetFieldNames.AriaProjectLocationName].Any(x => string.Equals(x, line.LocationName, StringComparison.OrdinalIgnoreCase)))
                                    dict[AssetFieldNames.AriaProjectLocationName].Add(line.LocationName);
                            }
                        }

                        finalDict.Add(AssetFieldNames.AriaProjectIndustryCode, string.Join(sharePointDelimiter, dict[AssetFieldNames.AriaProjectIndustryCode]));
                        finalDict.Add(AssetFieldNames.AriaProjectLocationCode, string.Join(sharePointDelimiter, dict[AssetFieldNames.AriaProjectLocationCode]));
                        finalDict.Add(AssetFieldNames.AriaProjectServiceCode, string.Join(sharePointDelimiter, dict[AssetFieldNames.AriaProjectServiceCode]));
                        finalDict.Add(AssetFieldNames.AriaProjectLocationName, string.Join(sharePointDelimiter, dict[AssetFieldNames.AriaProjectLocationName]));
                        this.serviceLineMetaItemsDictionary = finalDict;

                    }
                }
            }
        }


		/// <summary>
		/// Gets or sets the service request number.
		/// </summary>
		/// <value>
		/// The service request number.
		/// </value>
		public string ServiceRequestNumber { get; set; }


        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        /// <value>
        /// The tasks.
        /// </value>
        public List<Task> Tasks { get; set; }

        /// <summary>
        ///     Gets or sets the order owner.
        /// </summary>
        /// <value>
        ///     The order owner.
        /// </value>
        public string OrderOwner { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [override automatic complete].
		/// </summary>
		/// <value>
		/// <c>true</c> if [override automatic complete]; otherwise, <c>false</c>.
		/// </value>
	    public bool OverrideAutoComplete { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance has automatic complete.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance has automatic complete; otherwise, <c>false</c>.
		/// </value>
	    public bool HasAutoComplete { get; set; }


        /// <summary>
        /// Gets the party site number.
        /// </summary>
        /// <value>
        /// The party site number.
        /// </value>
        public string PartySiteNumber
        {
            get
            {
                string sharePointDelimiter = AssetFieldNames.SharePointMultivalueSeparator.ToString();
                var list = new List<string> {};
                if (null != ShipToContact && ! string.IsNullOrWhiteSpace(ShipToContact.PartySiteNumber))
                    list.Add(ShipToContact.PartySiteNumber);
                if (null != BillToContact && !string.IsNullOrWhiteSpace(BillToContact.PartySiteNumber))
                    list.Add(BillToContact.PartySiteNumber);
                if (null != IncomingOrderContact && !string.IsNullOrWhiteSpace(IncomingOrderContact.PartySiteNumber))
                    list.Add(IncomingOrderContact.PartySiteNumber);

                return string.Join(sharePointDelimiter,list);
            }
        }
        /// <summary>
        /// Gets the ship to party site number.
        /// </summary>
        /// <value>
        /// The ship to party site number.
        /// </value>
        public string ShipToPartySiteNumber
        {
            get
            {
                return GetPartySiteNumber(ShipToContact);
            }
        }

        /// <summary>
        /// Gets the sold to party site number.
        /// </summary>
        /// <value>
        /// The sold to party site number.
        /// </value>
        public string BillToPartySiteNumber
        {
            get
            {
                return GetPartySiteNumber(this.BillToContact);
            }
        }
        /// <summary>
        /// Gets the incoming order contact party site number.
        /// </summary>
        /// <value>
        /// The incoming order contact party site number.
        /// </value>
        public string IncomingOrderContactPartySiteNumber
        {
            get
            {
                return GetPartySiteNumber(this.IncomingOrderContact);
            }
        }

        private static string GetPartySiteNumber(IncomingOrderContact incomingOrderContact)
        {
            if (null == incomingOrderContact)
                return null;
            return incomingOrderContact.PartySiteNumber;
        }
    }
}