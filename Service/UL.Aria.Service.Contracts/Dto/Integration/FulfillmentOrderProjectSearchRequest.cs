using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto.Integration
{
    /// <summary>
    /// Data Contract for holding a list of <see cref="FulfillmentOrderProjectSearchFilter"/> objects.
    /// </summary>
    [CollectionDataContract(ItemName="Filter")]
    public class FulfillmentOrderProjectSearchRequestFilters : List<FulfillmentOrderProjectSearchFilter>
    {
        
    }

    /// <summary>
    /// Data Contract for holding a list of <see cref="FulfillmentProjectRefinerField"/> objects.
    /// </summary>
    [CollectionDataContract(ItemName="Refiner")]
    public class FulfillmentOrderProjectSearchRequestRefiners : List<FulfillmentProjectRefinerField>
    {
        
    }
    /// <summary>
    /// Search criteria for fulfillment projects by Orders
    /// </summary>
    [DataContract]
    public class FulfillmentOrderProjectSearchRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FulfillmentOrderProjectSearchRequest"/> class.
        /// </summary>
        public FulfillmentOrderProjectSearchRequest()
        {
            Filters = new FulfillmentOrderProjectSearchRequestFilters();
            Refiners = new FulfillmentOrderProjectSearchRequestRefiners();
        }
        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        [DataMember]
        public FulfillmentOrderProjectSearchRequestFilters Filters { get; set; }

        /// <summary>
        /// Gets or sets the refiners.
        /// </summary>
        /// <value>
        /// The refiners.
        /// </value>
        [DataMember]
        public FulfillmentOrderProjectSearchRequestRefiners Refiners { get; set; }

        /// <summary>
        /// Gets or sets the end index. Maximum  total results is 500.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        [DataMember]
        public int? StartIndex { get; set; }

        /// <summary>
        /// Gets or sets the end index. Maximum  total results is 500.
        /// </summary>
        /// <value>
        /// The end index.
        /// </value>
        [DataMember]
        public int? EndIndex { get; set; }

    }
}