using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto.Integration
{
    /// <summary>
    /// Payload object for a list of  <see cref="OrderFulfillmentSummary"/>
    /// </summary>
    [CollectionDataContract(ItemName = "OrderFulfillmentSummary")]
    public class OrderFulfillmentSummaries : List<OrderFulfillmentSummary>
    {
        
    }

    /// <summary>
    /// Contains a summary of fulfillment activities for a given order.
    /// </summary>
    [DataContract]
    public class OrderFulfillmentSummary
    {
        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        /// <value>
        /// The order number.
        /// </value>
        [DataMember]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the projects on hold.
        /// </summary>
        /// <value>
        /// The projects on hold.
        /// </value>
        [DataMember]
        public long ProjectsOnHold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the order is on hold
        /// </summary>
        /// <value><c>true</c> if [on hold]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool OnHold { get; set; }
    }

    /// <summary>
    /// Payload object for order numbers
    /// </summary>
    [CollectionDataContract(ItemName = "OrderNumber")]
    public class OrderNumbers : List<string> { }

    /// <summary>
    /// Payload object for PartySiteNumbers
    /// </summary>
    [CollectionDataContract(ItemName = "PartySiteNumber")]
    public class PartySiteNumbers : List<string> { }

    /// <summary>
    /// Class OrderFulfillmentSummarySearchRequest.
    /// </summary>
    [DataContract]
    public class OrderFulfillmentSummarySearchRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderFulfillmentSummarySearchRequest"/> class.
        /// </summary>
        public OrderFulfillmentSummarySearchRequest()
        {
            Filters = new FulfillmentOrderProjectSearchRequestFilters();
        }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        [DataMember]
        public FulfillmentOrderProjectSearchRequestFilters Filters { get; set; }
    }
    
    /// <summary>
    /// REsponse object for a search for <see cref="OrderFulfillmentSummary"/> objects.
    /// </summary>
    [DataContract]
    public class OrderFulfillmentSummarySearchResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderFulfillmentSummarySearchResponse"/> class.
        /// </summary>
        public OrderFulfillmentSummarySearchResponse()
        {
            OrderFulfillmentSummaries = new OrderFulfillmentSummaries();
        }

        /// <summary>
        /// Gets or sets the order numbers.
        /// </summary>
        /// <value>
        /// The order numbers.
        /// </value>
        [DataMember]
        public OrderFulfillmentSummaries OrderFulfillmentSummaries { get; set; }
    }
}
