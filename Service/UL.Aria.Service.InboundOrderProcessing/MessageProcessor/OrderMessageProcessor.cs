using System.Collections.Generic;
using System.Linq;
using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.InboundOrderProcessing.Domain;
using UL.Aria.Service.InboundOrderProcessing.Logging;
using UL.Aria.Service.InboundOrderProcessing.Resolver;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.InboundOrderProcessing.MessageProcessor
{
	/// <summary>
	///     Class OrderMessageProcessor.
	/// </summary>
	public sealed class OrderMessageProcessor : MessageProcessorBase, IMessageProcessor
	{
		private readonly ICompanyProvider _companyProvider;
		private readonly IOrderProvider _orderProvider;
		private readonly IProjectManager _projectManager;
		private readonly IMapperRegistry _mapperRegistry;
		private readonly IInboundMessageProvider _inboundMessageProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderMessageProcessor" /> class.
		/// </summary>
		/// <param name="xmlParserResolver">The XML parser resolver.</param>
		/// <param name="companyProvider">The company provider.</param>
		/// <param name="orderProvider">The order provider.</param>
		/// <param name="businessMessageProvider">The business message provider.</param>
		/// <param name="validatorResolver">The validator resolver.</param>
		/// <param name="logManager">The log manager.</param>
		/// <param name="projectManager">The project manager.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		/// <param name="inboundMessageProvider">The inbound message provider.</param>
		public OrderMessageProcessor(IXmlParserResolver xmlParserResolver, ICompanyProvider companyProvider,
			IOrderProvider orderProvider,
			IBusinessMessageProvider businessMessageProvider, IValidatorResolver validatorResolver, ILogManager logManager, IProjectManager projectManager, IMapperRegistry mapperRegistry, IInboundMessageProvider inboundMessageProvider)
			: base(businessMessageProvider, validatorResolver, xmlParserResolver, logManager)
		{
			_companyProvider = companyProvider;
			_orderProvider = orderProvider;
			_projectManager = projectManager;
			_mapperRegistry = mapperRegistry;
			_inboundMessageProvider = inboundMessageProvider;
		}

		/// <summary>
		///     Gets the name of the validator.
		/// </summary>
		/// <value>The name of the validator.</value>
		protected override string Name
		{
			get { return "Order"; }
		}

		internal string InternalName
		{
			get { return Name; }
		}

		/// <summary>
		///     Processes the message.
		/// </summary>
		/// <param name="orderMessage">The order message.</param>
		public void ProcessMessage(OrderMessage orderMessage)
		{
			var incomingOrderDto = Parse(orderMessage.Body) as IncomingOrderDto;
			LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageOrderParsed, "Buy Message Order Parsed", orderMessage,
				incomingOrderDto);

			var company =
				// ReSharper disable once PossibleNullReferenceException
				_companyProvider.FetchByExternalId(incomingOrderDto.IncomingOrderCustomer.ExternalId);

			if (company != null)
			{
				LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageOrderCompanyFound, "Buy Message Order Company Found",
					orderMessage, incomingOrderDto);
				incomingOrderDto.CompanyId = company.Id;
			}
			else
			{
				LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageOrderCompanyNotFound, "Buy Message Order Company Not Found",
					orderMessage, incomingOrderDto);
			}

			var orderErrors = Validate(incomingOrderDto);
			var isValid = !orderErrors.Any();
			var blobMetadata = new Dictionary<string, string> { { "MessageId", orderMessage.MessageId }, { "ExternalMessageId", orderMessage.ExternalMessageId }, { "Receiver", orderMessage.Receiver }, { "Originator", orderMessage.Originator } };

			if (isValid)
			{
				LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageOrderValidationPassed, "Buy Message Order Validation Passed",
					orderMessage, incomingOrderDto);
				CreateOrUpdateOrder(orderMessage, incomingOrderDto);
				// this will be re-enabled once we have b/p closed status.
				// UpdateProjectStatuses(incomingOrderDto);
				UpdateProjectStatus(incomingOrderDto);
				_inboundMessageProvider.SaveSuccessfulMessage(orderMessage.MessageId, orderMessage.Body, blobMetadata);
			}
			else
			{
				if (orderErrors.Count == 1 && orderErrors.Keys.ToList()[0].StartsWith("hasCompany(Customer External Id:"))
				{
					_inboundMessageProvider.SaveOrderMessage(incomingOrderDto.OrderNumber, orderMessage.Body, blobMetadata);
					LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageOrderMissingCompany, "Buy Message Order Company Missing, stored for future processing",
						orderMessage, incomingOrderDto);
					LogMessageExtendedProperties.Add("OrderNumber", incomingOrderDto.OrderNumber);
					LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageOrderMissingCompany, "Buy Message Order Company Missing, stored for future processing", orderMessage, incomingOrderDto);
				}
				else
				{
					LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageOrderValidationFailed, "Buy Message Order Validation Failed",
						orderMessage, incomingOrderDto);
					LogMessageExtendedProperties.Add("OrderNumber", incomingOrderDto.OrderNumber);
					LogMessage(orderMessage, incomingOrderDto, orderErrors);
					_inboundMessageProvider.SaveFailedMessage(orderMessage.MessageId, orderMessage.Body, blobMetadata);
					_inboundMessageProvider.DeleteNewMessage(orderMessage.MessageId);
				}
			}
		}


		private void UpdateProjectStatus(IncomingOrderDto incomingOrderDto)
		{
			var incomingOrder = _mapperRegistry.Map<IncomingOrder>(incomingOrderDto);
			_projectManager.UpdateStatusFromOrder(incomingOrder);
		}

		// this will be re-enabled once we have b/p closed status.
		//private void UpdateProjectStatuses(IncomingOrderDto incomingOrderDto)
		//{
		//    const string closed = "closed";
		//    var updateDateTime = DateTime.UtcNow;
		//    var updatedBy = _principalResolver.UserId;
		//    if (incomingOrderDto.Status.ToLowerInvariant() != closed && incomingOrderDto.ServiceLines.All(x => x.Status.ToLowerInvariant() != closed))
		//    { // This is somewhat of an optimization.
		//        return;
		//    }

		//    var projects = _projectProvider.FetchByOrderNumber(incomingOrderDto.OrderNumber);
		//    foreach (var project in projects)
		//    {
		//        foreach (var serviceLine in project.ServiceLines)
		//        {
		//            var incomingServiceLine =
		//                incomingOrderDto.ServiceLines.FirstOrDefault(x => x.LineNumber == serviceLine.LineNumber);
		//            if (null != incomingServiceLine && closed == incomingServiceLine.Status.ToLowerInvariant())
		//            {
		//                serviceLine.Status = incomingServiceLine.Status;
		//                serviceLine.UpdatedDateTime = updateDateTime;
		//                serviceLine.UpdatedById = updatedBy;
		//            }
		//        }
		//        if (incomingOrderDto.Status.ToLowerInvariant() == closed)
		//        { // Need to keep the project status as-is if the top order status is not closed.
		//            project.Status = incomingOrderDto.Status;
		//            project.UpdatedDateTime = updateDateTime;
		//            project.UpdatedById = updatedBy;
		//        } 
		//        _projectProvider.UpdateStatusFromOrder(project);
		//    }
		//}

		private void CreateOrUpdateOrder(OrderMessage orderMessage, IncomingOrderDto incomingOrderDto)
		{
			Order order = null;
			try
			{
				order = _orderProvider.FindByOrderNumber(incomingOrderDto.OrderNumber);
				LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageOrderFound, "Buy Message Order Found",
					orderMessage, incomingOrderDto);
			}
			catch (DatabaseItemNotFoundException)
			{
				LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageOrderNotFound, "Buy Message Order NotFound",
					orderMessage, incomingOrderDto);
			}
			if (order == null)
			{
				_orderProvider.Create(orderMessage.MessageId, incomingOrderDto.OriginalXmlParsed);
				LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageOrderCreated, "Buy Message Order Created",
					orderMessage, incomingOrderDto);
			}
			else
			{
				_orderProvider.Update(orderMessage.MessageId, incomingOrderDto.OriginalXmlParsed);
				_inboundMessageProvider.DeleteSuccessfulMessage(order.MessageId);
				LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageOrderUpdated, "Buy Message Order Updated",
					orderMessage, incomingOrderDto);
			}
		}
	}
}