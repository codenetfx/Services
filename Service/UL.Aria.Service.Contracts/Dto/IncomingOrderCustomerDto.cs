using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class NewProjectCompanyDto
    /// </summary>
    [DataContract]
    public class IncomingOrderCustomerDto
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
        ///     Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        [DataMember]
        public string ProjectName { get; set; }

        /// <summary>
        ///     Gets or sets the DUNS.
        /// </summary>
        /// <value>The DUNS.</value>
        [DataMember]
        public string DUNS { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        public string Name { get; set; }

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
        ///     Gets or sets the external id.
        /// </summary>
        /// <value>The external id.</value>
        [DataMember]
        public string ExternalId { get; set; }

        /// <summary>
        ///     Gets or sets the agent details.
        /// </summary>
        /// <value>
        ///     The agent details.
        /// </value>
        [DataMember]
        public string AgentDetails { get; set; }

        /// <summary>
        ///     Gets or sets the subscriber number.
        /// </summary>
        /// <value>
        ///     The subscriber number.
        /// </value>
        [DataMember]
        public string SubscriberNumber { get; set; }
    }
}