using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class NewProjectCompany
    /// </summary>
    [Serializable]
    public class IncomingOrderCustomer : TrackedDomainEntity
    {
        /// <summary>
        ///     Gets or sets the incoming order id.
        /// </summary>
        /// <value>The incoming order id.</value>
        public Guid IncomingOrderId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        public string ProjectName { get; set; }

        /// <summary>
        ///     Gets or sets the DUNS.
        /// </summary>
        /// <value>The DUNS.</value>
        public string DUNS { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the external id.
        /// </summary>
        /// <value>The external id.</value>
        public string ExternalId { get; set; }

        /// <summary>
        ///     Gets or sets the agent details.
        /// </summary>
        /// <value>
        ///     The agent details.
        /// </value>
        public string AgentDetails { get; set; }

        /// <summary>
        ///     Gets or sets the subscriber number.
        /// </summary>
        /// <value>
        ///     The subscriber number.
        /// </value>
        public string SubscriberNumber { get; set; }

        /// <summary>
        ///     Maps the incoming order customer.
        /// </summary>
        /// <param name="incomingOrderCustomer">The incoming order customer.</param>
        /// <returns>IncomingOrderCustomer.</returns>
        public static IncomingOrderCustomer MapIncomingOrderCustomer(IncomingOrderCustomer incomingOrderCustomer)
        {
            return new IncomingOrderCustomer
            {
                AgentDetails = incomingOrderCustomer.AgentDetails,
                DUNS = incomingOrderCustomer.DUNS,
                ExternalId = incomingOrderCustomer.ExternalId,
                IncomingOrderId = incomingOrderCustomer.IncomingOrderId,
                Name = incomingOrderCustomer.Name,
                ProjectName = incomingOrderCustomer.ProjectName,
                SubscriberNumber = incomingOrderCustomer.SubscriberNumber
            };
        }
    }
}