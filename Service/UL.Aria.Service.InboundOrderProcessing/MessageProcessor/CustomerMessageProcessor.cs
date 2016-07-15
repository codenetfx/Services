using System.Collections.Generic;
using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.InboundOrderProcessing.Domain;
using UL.Aria.Service.InboundOrderProcessing.Logging;
using UL.Aria.Service.InboundOrderProcessing.Resolver;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.InboundOrderProcessing.MessageProcessor
{
    /// <summary>
    /// Processor for customer messages.
    /// </summary>
    public sealed class CustomerMessageProcessor : MessageProcessorBase, IMessageProcessor
    {
        private readonly IIncomingOrderProvider _incomingOrderProvider;
        private readonly IProjectProvider _projectProvider;
	    private readonly IInboundMessageProvider _inboundMessageProvider;

	    /// <summary>
		/// Initializes a new instance of the <see cref="CustomerMessageProcessor" /> class.
		/// </summary>
		/// <param name="businessMessageProvider">The business message provider.</param>
		/// <param name="validatorResolver">The validator resolver.</param>
		/// <param name="xmlParserResolver">The XML parser resolver.</param>
		/// <param name="incomingOrderProvider">The incoming order provider.</param>
		/// <param name="projectProvider">The project provider.</param>
		/// <param name="logManager">The log manager.</param>
		/// <param name="inboundMessageProvider">The inbound message provider.</param>
        public CustomerMessageProcessor(
            IBusinessMessageProvider businessMessageProvider, 
            IValidatorResolver validatorResolver, 
            IXmlParserResolver xmlParserResolver,
            IIncomingOrderProvider incomingOrderProvider,
			IProjectProvider projectProvider, ILogManager logManager, IInboundMessageProvider inboundMessageProvider)
			: base(businessMessageProvider, validatorResolver, xmlParserResolver, logManager)
        {
            _incomingOrderProvider = incomingOrderProvider;
            _projectProvider = projectProvider;
		    _inboundMessageProvider = inboundMessageProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override string Name
        {
            get { return  EntityTypeEnumDto.CustomerOrganization.ToString(); }
        }

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="orderMessage">The order message.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ProcessMessage(OrderMessage orderMessage)
        {
            var customerOrganization = (CustomerOrganization)Parse(orderMessage.Body);
			LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageCustomerParsed, "Buy Message Customer Parsed", orderMessage,
				null);
			foreach (var location in customerOrganization.Locations)
            {
                if (string.IsNullOrWhiteSpace(location.ExternalId))
                    continue;
                _incomingOrderProvider.UpdateAllContactsForExternalId(location.ExternalId, location);
                _projectProvider.UpdateAllContactsForExternalId(location.ExternalId, location);
            }

            foreach (var contact in customerOrganization.Contacts)
            {
                if (string.IsNullOrWhiteSpace(contact.ExternalId))
                    continue;
                _incomingOrderProvider.UpdateAllContactsForExternalId(contact.ExternalId, contact);
                _projectProvider.UpdateAllContactsForExternalId(contact.ExternalId, contact);
            }

	        var blobMetadata = new Dictionary<string, string> { { "MessageId", orderMessage.MessageId }, { "ExternalMessageId", orderMessage.ExternalMessageId }, { "Receiver", orderMessage.Receiver }, { "Originator", orderMessage.Originator } };

	        if (
		        string.IsNullOrWhiteSpace(customerOrganization.Customer.ExternalId)
		        ||
		        string.IsNullOrWhiteSpace(customerOrganization.Customer.Name)
		        ||
		        string.IsNullOrWhiteSpace(customerOrganization.Customer.DUNS))
	        {
				_inboundMessageProvider.SaveFailedMessage(orderMessage.MessageId, orderMessage.Body, blobMetadata);
				_inboundMessageProvider.DeleteNewMessage(orderMessage.MessageId);
				return;
	        }

	        _incomingOrderProvider.UpdateAllCustomersForExternalId(customerOrganization.Customer.ExternalId, customerOrganization.Customer);
            _projectProvider.UpdateAllCustomersForExternalId(customerOrganization.Customer.ExternalId, customerOrganization.Customer);
			_inboundMessageProvider.SaveSuccessfulMessage(orderMessage.MessageId, orderMessage.Body, blobMetadata);
		}
    }
}