using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.InboundOrderProcessing.Domain;
using UL.Aria.Service.InboundOrderProcessing.Logging;
using UL.Aria.Service.InboundOrderProcessing.Resolver;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.InboundOrderProcessing.MessageProcessor
{
	/// <summary>
	///     Class RequestMessageProcessor.
	/// </summary>
	public sealed class RequestMessageProcessor : MessageProcessorBase, IMessageProcessor
	{
		private const string UnknownLabel = "Unknown";
		private readonly ICompanyProvider _companyProvider;
		private readonly IContactOrderProvider _contactOrderProvider;
		private readonly IInboundMessageProvider _inboundMessageProvider;
		private readonly IIncomingOrderProvider _incomingOrderProvider;
		private readonly IIndustryCodeProvider _industryCodeProvider;
		private readonly ILocationCodeProvider _locationCodeProvider;
		private readonly ILogManager _logManager;
		private readonly IMapperRegistry _mapperRegistry;
		private readonly IMessageProcessorResolver _messageProcessorResolver;
		private readonly IServiceCodeProvider _serviceCodeProvider;
		private readonly ITransactionFactory _transactionFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderMessageProcessor" /> class.
		/// </summary>
		/// <param name="xmlParserResolver">The XML parser resolver.</param>
		/// <param name="companyProvider">The company provider.</param>
		/// <param name="incomingOrderProvider">The incoming order provider.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		/// <param name="businessMessageProvider">The business message provider.</param>
		/// <param name="validatorResolver">The validator resolver.</param>
		/// <param name="industryCodeProvider">The industry code provider.</param>
		/// <param name="serviceCodeProvider">The service code provider.</param>
		/// <param name="locationCodeProvider">The location code provider.</param>
		/// <param name="logManager">The log manager.</param>
		/// <param name="inboundMessageProvider">The inbound message provider.</param>
		/// <param name="messageProcessorResolver">The message processor resolver.</param>
		/// <param name="contactOrderProvider">The contact order provider.</param>
		/// <param name="transactionFactory">The transaction factory.</param>
		public RequestMessageProcessor(IXmlParserResolver xmlParserResolver, ICompanyProvider companyProvider,
			IIncomingOrderProvider incomingOrderProvider,
			IMapperRegistry mapperRegistry, IBusinessMessageProvider businessMessageProvider,
			IValidatorResolver validatorResolver,
			IIndustryCodeProvider industryCodeProvider, IServiceCodeProvider serviceCodeProvider,
			ILocationCodeProvider locationCodeProvider, ILogManager logManager,
			IInboundMessageProvider inboundMessageProvider,
			IMessageProcessorResolver messageProcessorResolver,
			IContactOrderProvider contactOrderProvider,
			ITransactionFactory transactionFactory)
			: base(businessMessageProvider, validatorResolver, xmlParserResolver, logManager)
		{
			_companyProvider = companyProvider;
			_incomingOrderProvider = incomingOrderProvider;
			_mapperRegistry = mapperRegistry;
			_industryCodeProvider = industryCodeProvider;
			_serviceCodeProvider = serviceCodeProvider;
			_locationCodeProvider = locationCodeProvider;
			_logManager = logManager;
			_inboundMessageProvider = inboundMessageProvider;
			_messageProcessorResolver = messageProcessorResolver;
			_contactOrderProvider = contactOrderProvider;
			_transactionFactory = transactionFactory;
		}

		/// <summary>
		///     Gets the name of the validator.
		/// </summary>
		/// <value>The name of the validator.</value>
		protected override string Name
		{
			get { return "Request"; }
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
			ContactOrderDto contactOrder = null;

			using (var transactionScope = _transactionFactory.Create())
			{
				var incomingOrderDto = (IncomingOrderDto) Parse(orderMessage.Body);
				LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageRequestParsed, "Buy Message Request Parsed", orderMessage,
					incomingOrderDto);

				ProcessCompany(orderMessage, incomingOrderDto);
				ProcessServiceLinesIndustryCode(orderMessage, incomingOrderDto);
				ProcessServiceLinesServiceCode(orderMessage, incomingOrderDto);
				ProcessServiceLinesLocationCode(orderMessage, incomingOrderDto);

				var orderErrors = Validate(incomingOrderDto);
				var isValid = !orderErrors.Any();
				var blobMetadata = new Dictionary<string, string>
				{
					{"MessageId", orderMessage.MessageId},
					{"ExternalMessageId", orderMessage.ExternalMessageId},
					{"Receiver", orderMessage.Receiver},
					{"Originator", orderMessage.Originator}
				};

				if (isValid)
				{
					LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageRequestValidationPassed,
						"Buy Message Request Validation Passed",
						orderMessage, incomingOrderDto);
					IncomingOrder incomingOrderExisting = null;

					try
					{
						incomingOrderExisting = _incomingOrderProvider.FindByOrderNumber(incomingOrderDto.OrderNumber);
						LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageRequestFound, "Buy Message Request Found", orderMessage,
							incomingOrderDto);
					}
					catch (DatabaseItemNotFoundException)
					{
						LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageRequestNotFound, "Buy Message Request Not Found",
							orderMessage,
							incomingOrderDto);
					}

					incomingOrderDto.Id = incomingOrderExisting == null ? Guid.NewGuid() : incomingOrderExisting.Id;

					var incomingOrder = _mapperRegistry.Map<IncomingOrder>(incomingOrderDto);
					incomingOrder.MessageId = orderMessage.MessageId;
					_incomingOrderProvider.Update(default(Guid), incomingOrder);
					_inboundMessageProvider.SaveSuccessfulMessage(orderMessage.MessageId, orderMessage.Body, blobMetadata);

					contactOrder = new ContactOrderDto
					{
						MessageId = Guid.NewGuid().ToString(),
						OrderNumber = incomingOrder.OrderNumber,
						Receiver = "lhs"
					};
				}
				else
				{
					LogInfoMessage(MessageIds.InboundOrderProcessingBuyMessageRequestValidationFailed,
						"Buy Message Request Validation Failed",
						orderMessage, incomingOrderDto);
					LogMessageExtendedProperties.Add("OrderNumber", incomingOrderDto.OrderNumber);
					LogMessage(orderMessage, incomingOrderDto, orderErrors);
					_inboundMessageProvider.SaveFailedMessage(orderMessage.MessageId, orderMessage.Body, blobMetadata);
					_inboundMessageProvider.DeleteNewMessage(orderMessage.MessageId);
				}
				transactionScope.Complete();
			}

			if (contactOrder != null)
			{
				_contactOrderProvider.QueueContactOrder(contactOrder);
			}
		}

		private void ProcessCompany(OrderMessage orderMessage, IncomingOrderDto incomingOrderDto)
		{
			Company company;

			try
			{
				company = _companyProvider.FetchByExternalId(incomingOrderDto.IncomingOrderCustomer.ExternalId);
				LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestCompanyFound,
					"Buy Message Request Company Found",
					orderMessage, incomingOrderDto);
			}
			catch (DatabaseItemNotFoundException)
			{
				LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestCompanyNotFound,
					"Buy Message Request Company Not Found",
					orderMessage, incomingOrderDto);
				company = null;
			}

			if (company == null && incomingOrderDto.IncomingOrderCustomer != null)
			{
				company = new Company {Id = Guid.NewGuid(), Name = incomingOrderDto.IncomingOrderCustomer.Name};
				company.ExternalIds.Add(incomingOrderDto.IncomingOrderCustomer.ExternalId);
				_companyProvider.Create(company);
				LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestCompanyCreated,
					"Buy Message Request Company Created",
					orderMessage, incomingOrderDto);
				var inboundMessageBlob = _inboundMessageProvider.FetchOrderMessage(incomingOrderDto.OrderNumber);
				if (inboundMessageBlob != null)
				{
					var orderMessage2 = new OrderMessage
					{
						Body = inboundMessageBlob.Message,
						MessageId = inboundMessageBlob.Metadata["MessageId"],
						ExternalMessageId = inboundMessageBlob.Metadata["ExternalMessageId"],
						Receiver = inboundMessageBlob.Metadata["Receiver"],
						Originator = inboundMessageBlob.Metadata["Originator"]
					};
					var messageProcessor = _messageProcessorResolver.Resolve(orderMessage2.Receiver.ToLower());
					messageProcessor.ProcessMessage(orderMessage2);
					_inboundMessageProvider.DeleteOrderMessage(incomingOrderDto.OrderNumber);
				}
			}

			if (company != null && company.Id != null)
			{
				incomingOrderDto.CompanyId = company.Id;
			}
		}

		private void ProcessServiceLinesIndustryCode(OrderMessage orderMessage, IncomingOrderDto incomingOrderDto)
		{
			foreach (var serviceLine in incomingOrderDto.ServiceLines)
			{
				if (string.IsNullOrWhiteSpace(serviceLine.IndustryCode))
					continue;
				try
				{
					IndustryCode industryCode;
					try
					{
						industryCode = _industryCodeProvider.FetchByExternalId(serviceLine.IndustryCode);
						LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestIndustryCodeFound,
							"Buy Message Request Industry Code Found",
							orderMessage, incomingOrderDto, serviceLine);
					}
					catch (DatabaseItemNotFoundException)
					{
						LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestIndustryCodeNotFound,
							"Buy Message Request Industry Code Not Found",
							orderMessage, incomingOrderDto, serviceLine);
						industryCode = null;
					}

					if (null == industryCode && !string.IsNullOrWhiteSpace(serviceLine.IndustryCode))
					{
						_industryCodeProvider.Create(
							new IndustryCode
							{
								Id = Guid.NewGuid(),
								ExternalId = serviceLine.IndustryCode,
								Label = (!string.IsNullOrEmpty(serviceLine.IndustryCodeLabel))
									? serviceLine.IndustryCodeLabel
									: UnknownLabel
							});
						LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestIndustryCodeCreated,
							"Buy Message Request Industry Code Created",
							orderMessage, incomingOrderDto, serviceLine);
					}
					else
					{
						if (!string.IsNullOrWhiteSpace(serviceLine.IndustryCodeLabel)
							// ReSharper disable once PossibleNullReferenceException
						    && serviceLine.IndustryCodeLabel != industryCode.Label)
						{
							industryCode.Label = serviceLine.IndustryCodeLabel;
							_industryCodeProvider.Update(industryCode.Id.GetValueOrDefault(), industryCode);
							LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestIndustryCodeUpdated,
								"Buy Message Request Industry Code Updated",
								orderMessage, incomingOrderDto, serviceLine);
						}
					}
				}
				catch (Exception ex)
				{
					// don't throw, just log these errors

					var logMessage = ex.ToLogMessage(MessageIds.InboundOrderProcessingProcessException_Unable_To_Add_IndustryCode,
						LogCategory.InboundProcessor, LogPriority.High, TraceEventType.Error);
					logMessage.Data.Add("OrderNumber", incomingOrderDto.OrderNumber);
					_logManager.Log(logMessage);
				}
			}
		}

		private void ProcessServiceLinesServiceCode(OrderMessage orderMessage, IncomingOrderDto incomingOrderDto)
		{
			foreach (var serviceLine in incomingOrderDto.ServiceLines)
			{
				if (string.IsNullOrWhiteSpace(serviceLine.ServiceCode))
					continue;
				try
				{
					ServiceCode serviceCode;
					try
					{
						serviceCode = _serviceCodeProvider.FetchByExternalId(serviceLine.ServiceCode);
						LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestServiceCodeFound,
							"Buy Message Request Service Code Found",
							orderMessage, incomingOrderDto, serviceLine);
					}
					catch (DatabaseItemNotFoundException)
					{
						LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestServiceCodeNotFound,
							"Buy Message Request Service Code Not Found",
							orderMessage, incomingOrderDto, serviceLine);
						serviceCode = null;
					}

					if (null == serviceCode && !string.IsNullOrWhiteSpace(serviceLine.ServiceCode))
					{
						_serviceCodeProvider.Create(
							new ServiceCode
							{
								Id = Guid.NewGuid(),
								ExternalId = serviceLine.ServiceCode,
								Label = (!string.IsNullOrWhiteSpace(serviceLine.ServiceCodeLabel))
									? serviceLine.ServiceCodeLabel
									: UnknownLabel
							});
						LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestServiceCodeCreated,
							"Buy Message Request Service Code Created",
							orderMessage, incomingOrderDto, serviceLine);
					}
					else
					{
						if (!string.IsNullOrWhiteSpace(serviceLine.ServiceCodeLabel)
							// ReSharper disable once PossibleNullReferenceException
						    && serviceLine.ServiceCodeLabel != serviceCode.Label)
						{
							serviceCode.Label = serviceLine.ServiceCodeLabel;
							_serviceCodeProvider.Update(serviceCode.Id.GetValueOrDefault(), serviceCode);
							LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestServiceCodeUpdated,
								"Buy Message Request Service Code Updated",
								orderMessage, incomingOrderDto, serviceLine);
						}
					}
				}
				catch (Exception ex)
				{
// don't throw, just log these errors

					var logMessage = ex.ToLogMessage(MessageIds.InboundOrderProcessingProcessException_Unable_To_Add_ServiceLine,
						LogCategory.InboundProcessor, LogPriority.High, TraceEventType.Error);
					logMessage.Data.Add("OrderNumber", incomingOrderDto.OrderNumber);
					_logManager.Log(logMessage);
				}
			}
		}

		private void ProcessServiceLinesLocationCode(OrderMessage orderMessage, IncomingOrderDto incomingOrderDto)
		{
			foreach (var serviceLine in incomingOrderDto.ServiceLines)
			{
				if (string.IsNullOrWhiteSpace(serviceLine.LocationCode))
					continue;
				try
				{
					LocationCode locationCode;
					try
					{
						locationCode = _locationCodeProvider.FetchByExternalId(serviceLine.LocationCode);
						LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestLocationCodeFound,
							"Buy Message Request Location Code Found",
							orderMessage, incomingOrderDto, serviceLine);
					}
					catch (DatabaseItemNotFoundException)
					{
						LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestLocationCodeNotFound,
							"Buy Message Request Location Code Not Found",
							orderMessage, incomingOrderDto, serviceLine);
						locationCode = null;
					}

					if (null == locationCode)
					{
						_locationCodeProvider.Create(
							new LocationCode
							{
								Id = Guid.NewGuid(),
								ExternalId = serviceLine.LocationCode,
								Label = (!string.IsNullOrEmpty(serviceLine.LocationCodeLabel))
									? serviceLine.LocationCodeLabel
									: UnknownLabel
							});
						LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestLocationCodeCreated,
							"Buy Message Request Location Code Created",
							orderMessage, incomingOrderDto, serviceLine);
					}
					else
					{
						if (!string.IsNullOrWhiteSpace(serviceLine.LocationCodeLabel)
						    && serviceLine.LocationCodeLabel != locationCode.Label)
						{
							locationCode.Label = serviceLine.LocationCodeLabel;
							_locationCodeProvider.Update(locationCode.Id.GetValueOrDefault(), locationCode);
							LogVerboseMessage(MessageIds.InboundOrderProcessingBuyMessageRequestLocationCodeUpdated,
								"Buy Message Request Location Code Updated",
								orderMessage, incomingOrderDto, serviceLine);
						}
					}
				}
				catch (Exception ex)
				{
// don't throw, just log these errors

					var logMessage = ex.ToLogMessage(MessageIds.InboundOrderProcessingProcessException_Unable_To_Add_LocationCode,
						LogCategory.InboundProcessor, LogPriority.High, TraceEventType.Error);
					logMessage.Data.Add("OrderNumber", incomingOrderDto.OrderNumber);
					_logManager.Log(logMessage);
				}
			}
		}
	}
}