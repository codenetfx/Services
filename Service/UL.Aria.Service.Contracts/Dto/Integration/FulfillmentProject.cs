using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto.Integration
{

    /// <summary>
    /// Data Contract for holding a list of <see cref="OrderServiceLine"/> objects.
    /// </summary>
   [CollectionDataContract(ItemName ="OrderLine")]
    public class FulfillmentProjectOrderServiceLines : List<OrderServiceLine>
    {
        
    }
    /// <summary>
    /// Contract for supplying fulfillment project information to external systems.
    /// </summary>
    [DataContract(Namespace = "http://portal.ul.com")]
    public class FulfillmentProject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FulfillmentProject"/> class.
        /// </summary>
        public FulfillmentProject()
        {
            OrderLineNumbers = new List<string>();
            OrderLines = new FulfillmentProjectOrderServiceLines();
        }
        /// <summary>
        /// Gets or sets the fulfillment identifier.
        /// </summary>
        /// <value>
        /// The fulfillment identifier.
        /// </value>
        [DataMember]
        public string FulfillmentId { get; set; }

        /// <summary>
        /// Gets or sets the system identifier.
        /// This identifies the system providing this data. It is not an identifier for the data.
        /// </summary>
        /// <value>
        /// The system identifier.
        /// </value>
        [DataMember]
        public string SystemId { get; set; }

        /// <summary>
        /// Gets or sets the name of the customer company.
        /// </summary>
        /// <value>
        /// The name of the customer company.
        /// </value>
        [DataMember]
        public string CustomerCompanyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the fulfillment project.
        /// </summary>
        /// <value>
        /// The name of the fulfillment project.
        /// </value>
        [DataMember]
        public string FulfillmentProjectName { get; set; }

        /// <summary>
        /// Gets or sets the party site number.
        /// </summary>
        /// <value>
        /// The party site number.
        /// </value>
        [DataMember]
        public int? PartySiteNumber { get; set; }

        /// <summary>
        /// Gets or sets the subscriber number.
        /// </summary>
        /// <value>
        /// The subscriber number.
        /// </value>
        [DataMember]
        public int? SubscriberNumber { get; set; }

        /// <summary>
        /// Gets or sets the quote number.
        /// </summary>
        /// <value>
        /// The quote number.
        /// </value>
        [DataMember]
        public string QuoteNumber { get; set; }

        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        /// <value>
        /// The order number.
        /// </value>
        [DataMember]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the project number.
        /// </summary>
        /// <value>
        /// The project number.
        /// </value>
        [DataMember]
        public string ProjectNumber { get; set; }

        /// <summary>
        /// Gets or sets the product file number.
        /// </summary>
        /// <value>
        /// The product file number.
        /// </value>
        [DataMember(Name = "FileNumber")]
        public string ProductFileNumber { get; set; }

        /// <summary>
        /// Gets or sets the product CCN.
        /// Nobody knows what CCN stands for anymore.
        /// </summary>
        /// <value>
        /// The product CCN.
        /// </value>
        [DataMember (Name="CCN")]
        public string ProductCcn { get; set; }

        /// <summary>
        /// Gets or sets the project status in the source fulfillment system.
        /// </summary>
        /// <value>
        /// The project status.
        /// </value>
        [DataMember]
        public string FulfillmentStatus { get; set; }

        /// <summary>
        /// Gets or sets the fulfillment status notes.
        /// </summary>
        /// <value>
        /// The fulfillment status notes.
        /// </value>
        [DataMember]
        public string FulfillmentStatusNotes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this project is read only in its source system.
        /// For example, a project In Progress will be not read only. A completed or cancelled project is read only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the project is read only; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets the estimated completion date.
        /// </summary>
        /// <value>
        /// The estimated completion date.
        /// </value>
        [DataMember]
        public DateTime? EstimatedCompletionDate { get; set; }

        /// <summary>
        /// Gets or sets the fulfillment project creation date.
        /// </summary>
        /// <value>
        /// The fulfillment project creation date.
        /// </value>
        [DataMember]
        public DateTime FulfillmentProjectCreationDate { get; set; }

        /// <summary>
        /// Gets or sets the project handler identifier.
        /// </summary>
        /// <value>
        /// The project handler identifier.
        /// </value>
        [DataMember]
        public int? ProjectHandlerId { get; set; }

        /// <summary>
        /// Gets or sets the project handler login.
        /// </summary>
        /// <value>
        /// The project handler identifier.
        /// </value>
        [DataMember]
        public string ProjectHandler { get; set; }


        /// <summary>
        /// Gets or sets the project handler name.
        /// </summary>
        /// <value>
        /// The project handler identifier.
        /// </value>
        [DataMember]
        public string ProjectHandlerName { get; set; }

        /// <summary>
        /// Gets or sets the order line numbers.
        /// </summary>
        /// <value>
        /// The order line numbers.
        /// </value>
        [DataMember]
        public List<string> OrderLineNumbers { get; set; }

        /// <summary>
        /// Gets or sets the order lines.
        /// </summary>
        /// <value>
        /// The order lines.
        /// </value>
       [DataMember]
        public FulfillmentProjectOrderServiceLines OrderLines { get; set; }

        /// <summary>
        /// Gets or sets the task count.
        /// </summary>
        /// <value>
        /// The task count.
        /// </value>
        [DataMember]
        public long TaskCount { get; set; }

        /// <summary>
        /// Gets or sets the completed task count.
        /// </summary>
        /// <value>
        /// The completed task count.
        /// </value>
        [DataMember]
        public long CompletedTaskCount { get; set; }

        /// <summary>
        /// Gets or sets the container identifier.
        /// </summary>
        /// <value>
        /// The container identifier.
        /// </value>
        [IgnoreDataMember]
        public Guid ContainerId { get; set; }
    }
}