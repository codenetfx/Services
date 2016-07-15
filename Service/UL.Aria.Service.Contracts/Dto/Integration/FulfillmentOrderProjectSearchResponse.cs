using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto.Integration
{

    /// <summary>
    /// Data Contract for holding a list of <see cref="FulfillmentProject"/> objects.
    /// </summary>
    [CollectionDataContract(ItemName = "FulfillmentProject")]
    public class FulfillmentProjects : List<FulfillmentProject>
    {
        
    }

    /// <summary>
    /// Data Contract for holding a list of <see cref="FulfillmentProjectRefinerResultSet"/> objects.
    /// </summary>
    [CollectionDataContract(ItemName = "FulfillmentRefiner")]
    public class FulfillmentRefiners : List<FulfillmentProjectRefinerResultSet>
    {
        
    }

    /// <summary>
    /// Contract for supplying search results for fulfillment project information to external systems.
    /// </summary>
    [DataContract(Namespace = "http://portal.ul.com")]
    public class FulfillmentOrderProjectSearchResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FulfillmentOrderProjectSearchResponse"/> class.
        /// </summary>
        public FulfillmentOrderProjectSearchResponse()
        {
            FulfillmentProjects = new FulfillmentProjects();   
            Refiners = new FulfillmentRefiners();
        }
        /// <summary>
        /// Gets or sets the fulfillment projects.
        /// </summary>
        /// <value>
        /// The fulfillment projects.
        /// </value>
        [DataMember]
        public FulfillmentProjects FulfillmentProjects { get; set; }

        /// <summary>
        /// Gets or sets the refiners.
        /// </summary>
        /// <value>
        /// The refiners.
        /// </value>
        [DataMember]
        public FulfillmentRefiners Refiners { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has error; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool HasError { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        [DataMember]
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}