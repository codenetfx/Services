using System.Collections.Generic;
using System.Linq;

using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.InboundOrderProcessing.Logging;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.InboundOrderProcessing.Domain;
using UL.Aria.Service.InboundOrderProcessing.Resolver;
using UL.Aria.Service.InboundOrderProcessing.Service;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.InboundOrderProcessing.MessageProcessor
{
    /// <summary>
    ///     Class OrderServiceLineDetailMessageProcessor. This class cannot be inherited.
    /// </summary>
    public sealed class OrderServiceLineDetailMessageProcessor : MessageProcessorBase, IMessageProcessor
    {
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IProxyConfigurationSource _proxyConfigurationSource;
        private readonly ISenderProvider _senderProvider;
	    private readonly IInboundMessageProvider _inboundMessageProvider;
	    private readonly IOrderProvider _orderProvider;
        private readonly IOrderServiceLineDetailProvider _orderServiceLineDetailProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderServiceLineDetailMessageProcessor" /> class.
		/// </summary>
		/// <param name="xmlParserResolver">The XML parser resolver.</param>
		/// <param name="orderProvider">The order provider.</param>
		/// <param name="orderServiceLineDetailProvider">The order service line detail provider.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		/// <param name="businessMessageProvider">The business message provider.</param>
		/// <param name="validatorResolver">The validator resolver.</param>
		/// <param name="proxyConfigurationSource">The proxy configuration source.</param>
		/// <param name="senderProvider">The sender provider.</param>
		/// <param name="logManager">The log manager.</param>
		/// <param name="inboundMessageProvider">The inbound message provider.</param>
        public OrderServiceLineDetailMessageProcessor(IXmlParserResolver xmlParserResolver, IOrderProvider orderProvider,
            IOrderServiceLineDetailProvider orderServiceLineDetailProvider,
            IMapperRegistry mapperRegistry, IBusinessMessageProvider businessMessageProvider,
			IValidatorResolver validatorResolver, IProxyConfigurationSource proxyConfigurationSource, ISenderProvider senderProvider, ILogManager logManager, IInboundMessageProvider inboundMessageProvider)
            : base(businessMessageProvider, validatorResolver, xmlParserResolver, logManager)
        {
            _orderProvider = orderProvider;
            _orderServiceLineDetailProvider = orderServiceLineDetailProvider;
            _mapperRegistry = mapperRegistry;
            _proxyConfigurationSource = proxyConfigurationSource;
            _senderProvider = senderProvider;
			_inboundMessageProvider = inboundMessageProvider;
        }

        /// <summary>
        ///     Gets the name of the validator.
        /// </summary>
        /// <value>The name of the validator.</value>
        protected override string Name
        {
            get { return "OrderServiceLineDetail"; }
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
            var orderServiceLineDetailDtos = Parse(orderMessage.Body) as IList<OrderServiceLineDetailDto>;
			LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageOrderServiceLineDetailParsed, "Buy Message Order Service Line Detail Parsed", orderMessage,
				null);

// ReSharper disable once PossibleNullReferenceException
            foreach (var orderServiceLineDetailDto in orderServiceLineDetailDtos)
            {
                Order order = null;
                Sender sender = null;

                try
                {
                    order =
                        _orderProvider.FindByOrderNumber(orderServiceLineDetailDtos[0].OrderNumber);
                }
                catch (DatabaseItemNotFoundException)
                {
                }

                if (order != null)
                    orderServiceLineDetailDto.OrderId = order.Id;

                try
                {
                    sender =
                        _senderProvider.FindByName(orderServiceLineDetailDtos[0].SenderName);
                }
                catch (DatabaseItemNotFoundException)
                {
                }

                if (sender != null)
                    orderServiceLineDetailDto.SenderId = sender.Id;

                var orderErrors = Validate(orderServiceLineDetailDto);
                var isValid = !orderErrors.Any();
	            var blobMetadata = new Dictionary<string, string> { { "MessageId", orderMessage.MessageId }, { "ExternalMessageId", orderMessage.ExternalMessageId }, { "Receiver", orderMessage.Receiver }, { "Originator", orderMessage.Originator } };

	            if (isValid)
                {
                    OrderServiceLineDetail existingOrderServiceLineDetail = null;
                    try
                    {
                        existingOrderServiceLineDetail =
// ReSharper disable once PossibleInvalidOperationException
                            _orderServiceLineDetailProvider.FindByIds(orderServiceLineDetailDto.OrderId.Value,
                                orderServiceLineDetailDto.LineId);
                    }
                    catch (DatabaseItemNotFoundException)
                    {
                    }

                    var orderServiceLineDetail = _mapperRegistry.Map<OrderServiceLineDetail>(orderServiceLineDetailDto);
                    if (!_proxyConfigurationSource.StoreOriginalXml)
                        orderServiceLineDetail.OriginalXml = "";

                    if (existingOrderServiceLineDetail == null)
                        _orderServiceLineDetailProvider.Create(orderServiceLineDetail);
                    else
                        _orderServiceLineDetailProvider.Update(orderServiceLineDetail);
					_inboundMessageProvider.SaveSuccessfulMessage(orderMessage.MessageId, orderMessage.Body, blobMetadata);
				}
                else
                {
					LogMessageExtendedProperties.Add("OrderNumber", orderServiceLineDetailDto.OrderNumber);
					LogMessageExtendedProperties.Add("LineId", orderServiceLineDetailDto.LineId);
					LogMessage(orderMessage, orderServiceLineDetailDtos, orderErrors);
					_inboundMessageProvider.SaveFailedMessage(orderMessage.MessageId, orderMessage.Body, blobMetadata);
					_inboundMessageProvider.DeleteNewMessage(orderMessage.MessageId);
				}
            }
        }
    }
}