using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto.Integration
{
    /// <summary>
    ///     Class OrderServiceLine, used for integration contracts.
    /// </summary>
    [DataContract]
    public class OrderServiceLine
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
        ///     Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        [DataMember]
        public int Quantity { get; set; }

        /// <summary>
        ///     Gets or sets the allow charges from other operating units.
        /// </summary>
        /// <value>The allow charges from other operating units.</value>
        [DataMember]
        public string AllowChargesFromOtherOperatingUnits { get; set; }

        /// <summary>
        ///     Gets or sets the billable expenses.
        /// </summary>
        /// <value>The billable expenses.</value>
        [DataMember]
        public string BillableExpenses { get; set; }

        /// <summary>
        ///     Gets or sets the customer model number.
        /// </summary>
        /// <value>The customer model number.</value>
        [DataMember]
        public string CustomerModelNumber { get; set; }

        /// <summary>
        ///     Gets or sets the external id.
        /// </summary>
        /// <value>The external id.</value>
        [DataMember]
        public string ExternalId { get; set; }

        /// <summary>
        ///     Gets or sets the line number.
        /// </summary>
        /// <value>The line number.</value>
        [DataMember]
        public string LineNumber { get; set; }

        /// <summary>
        ///     Gets or sets the parent external id.
        /// </summary>
        /// <value>The parent external id.</value>
        [DataMember]
        public string ParentExternalId { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the type code.
        /// </summary>
        /// <value>The type code.</value>
        [DataMember]
        public string TypeCode { get; set; }

        /// <summary>
        ///     Gets or sets the item categories.
        /// </summary>
        /// <value>The item categories.</value>
        [DataMember]
        public string ItemCategories { get; set; }

        /// <summary>
        ///     Gets or sets the fulfillment method code.
        /// </summary>
        /// <value>The fulfillment method code.</value>
        [DataMember]
        public string FulfillmentMethodCode { get; set; }

        /// <summary>
        ///     Gets or sets the fulfillment set.
        /// </summary>
        /// <value>The fulfillment set.</value>
        [DataMember]
        public string FulfillmentSet { get; set; }

        /// <summary>
        ///     Gets or sets the configuration id.
        /// </summary>
        /// <value>The configuration id.</value>
        [DataMember]
        public string ConfigurationId { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        ///     Gets or sets the promise date.
        /// </summary>
        /// <value>The promise date.</value>
        [DataMember]
        public DateTime PromiseDate { get; set; }

        /// <summary>
        ///     Gets or sets the request date.
        /// </summary>
        /// <value>The request date.</value>
        [DataMember]
        public DateTime RequestDate { get; set; }

        /// <summary>
        ///     Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        [DataMember]
        public DateTime StartDate { get; set; }

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
        ///     Gets or sets the billable.
        /// </summary>
        /// <value>
        ///     The billable.
        /// </value>
        [DataMember]
        public string Billable { get; set; }

        /// <summary>
        ///     Gets or sets the program.
        /// </summary>
        /// <value>
        ///     The program.
        /// </value>
        [DataMember]
        public string Program { get; set; }

        /// <summary>
        ///     Gets or sets the category.
        /// </summary>
        /// <value>
        ///     The category.
        /// </value>
        [DataMember]
        public string Category { get; set; }

        /// <summary>
        ///     Gets or sets the sub category.
        /// </summary>
        /// <value>
        ///     The sub category.
        /// </value>
        [DataMember]
        public string SubCategory { get; set; }

        /// <summary>
        ///     Gets or sets the segment.
        /// </summary>
        /// <value>
        ///     The segment.
        /// </value>
        [DataMember]
        public string Segment { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the client detail service.
        /// </summary>
        /// <value>
        ///     The client detail service.
        /// </value>
        [DataMember]
        public string ClientDetailService { get; set; }

        /// <summary>
        ///     Gets or sets the work order line business component id.
        /// </summary>
        /// <value>The work order line business component id.</value>
        [DataMember]
        public string WorkOrderLineBusinessComponentId { get; set; }

        /// <summary>
        ///     Gets or sets the work order line id.
        /// </summary>
        /// <value>The work order line id.</value>
        [DataMember]
        public string WorkOrderLineId { get; set; }

        /// <summary>
        ///     Gets or sets the application object key id.
        /// </summary>
        /// <value>The application object key id.</value>
        [DataMember]
        public string ApplicationObjectKeyId { get; set; }

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
        /// Gets or sets the preferred fulfillment location.
        /// </summary>
        /// <value>
        /// The preferred fulfillment location.
        /// </value>
        [DataMember]
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or sets the preferred fulfillment location code.
        /// </summary>
        /// <value>
        /// The preferred fulfillment location code.
        /// </value>
        [DataMember]
        public string LocationCode { get; set; }

        /// <summary>
        /// Gets or sets the location code label.
        /// </summary>
        /// <value>The location code label.</value>
        [DataMember]
        public string LocationCodeLabel { get; set; }

        /// <summary>
        /// Gets or sets the service code label.
        /// </summary>
        /// <value>The service code label.</value>
        [DataMember]
        public string ServiceCodeLabel { get; set; }

        /// <summary>
        /// Gets or sets the industry code label.
        /// </summary>
        /// <value>The industry code label.</value>
        [DataMember]
        public string IndustryCodeLabel { get; set; }

        /// <summary>
        /// Gets or sets the hold.
        /// </summary>
        /// <value>
        /// The hold.
        /// </value>
        [DataMember]
        public string Hold { get; set; }

        /// <summary>
        /// Gets or sets the service segment.
        /// </summary>
        /// <value>The service segment.</value>
        [DataMember]
        public string ServiceSegment { get; set; }

        /// <summary>
        /// Gets or sets the service segment description.
        /// </summary>
        /// <value>The service segment description.</value>
        [DataMember]
        public string ServiceSegmentDescription { get; set; }

        /// <summary>
        /// Gets or sets the service category.
        /// </summary>
        /// <value>The service category.</value>
        [DataMember]
        public string ServiceCategory { get; set; }

        /// <summary>
        /// Gets or sets the service category description.
        /// </summary>
        /// <value>The service category description.</value>
        [DataMember]
        public string ServiceCategoryDescription { get; set; }

        /// <summary>
        /// Gets or sets the service sub category.
        /// </summary>
        /// <value>The service sub category.</value>
        [DataMember]
        public string ServiceSubCategory { get; set; }

        /// <summary>
        /// Gets or sets the service sub category description.
        /// </summary>
        /// <value>The service sub category description.</value>
        [DataMember]
        public string ServiceSubCategoryDescription { get; set; }

        /// <summary>
        /// Gets or sets the service program.
        /// </summary>
        /// <value>The service program.</value>
        [DataMember]
        public string ServiceProgram { get; set; }

        /// <summary>
        /// Gets or sets the service program description.
        /// </summary>
        /// <value>The service program description.</value>
        [DataMember]
        public string ServiceProgramDescription { get; set; }

        /// <summary>
        /// Gets or sets the detailed service.
        /// </summary>
        /// <value>The detailed service.</value>
        [DataMember]
        public string DetailedService { get; set; }

        /// <summary>
        /// Gets or sets the detailed service description.
        /// </summary>
        /// <value>The detailed service description.</value>
        [DataMember]
        public string DetailedServiceDescription { get; set; }

        /// <summary>
        /// Gets or sets the industry.
        /// </summary>
        /// <value>The industry.</value>
        [DataMember]
        public string Industry { get; set; }

        /// <summary>
        /// Gets or sets the industry description.
        /// </summary>
        /// <value>The industry description.</value>
        [DataMember]
        public string IndustryDescription { get; set; }

        /// <summary>
        /// Gets or sets the industry category.
        /// </summary>
        /// <value>The industry category.</value>
        [DataMember]
        public string IndustryCategory { get; set; }

        /// <summary>
        /// Gets or sets the industry category description.
        /// </summary>
        /// <value>The industry category description.</value>
        [DataMember]
        public string IndustryCategoryDescription { get; set; }

        /// <summary>
        /// Gets or sets the industry sub category.
        /// </summary>
        /// <value>The industry sub category.</value>
        [DataMember]
        public string IndustrySubCategory { get; set; }

        /// <summary>
        /// Gets or sets the industry sub category description.
        /// </summary>
        /// <value>The industry sub category description.</value>
        [DataMember]
        public string IndustrySubCategoryDescription { get; set; }

        /// <summary>
        /// Gets or sets the product group.
        /// </summary>
        /// <value>The product group.</value>
        [DataMember]
        public string ProductGroup { get; set; }

        /// <summary>
        /// Gets or sets the product group description.
        /// </summary>
        /// <value>The product group description.</value>
        [DataMember]
        public string ProductGroupDescription { get; set; }

        /// <summary>
        /// Gets or sets the type of the product.
        /// </summary>
        /// <value>The type of the product.</value>
        [DataMember]
        public string ProductType { get; set; }

        /// <summary>
        /// Gets or sets the product type description.
        /// </summary>
        /// <value>The product type description.</value>
        [DataMember]
        public string ProductTypeDescription { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        [DataMember]
        public decimal? Price { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        [DataMember]
        public string Currency { get; set; }
    }
}