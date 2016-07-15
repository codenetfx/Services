using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.InboundOrderProcessing.Domain;
using UL.Aria.Service.InboundOrderProcessing.Resolver;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.InboundOrderProcessing.MessageProcessor
{
	/// <summary>
	///     Class MessageProcessorBase.
	/// </summary>
	public abstract class MessageProcessorBase
	{
		private readonly IBusinessMessageProvider _businessMessageProvider;
		private readonly IValidatorResolver _validatorResolver;
		private readonly IXmlParserResolver _xmlParserResolver;
		private readonly ILogManager _logManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="MessageProcessorBase" /> class.
		/// </summary>
		/// <param name="businessMessageProvider">The business message provider.</param>
		/// <param name="validatorResolver">The validator resolver.</param>
		/// <param name="xmlParserResolver">The XML parser resolver.</param>
		/// <param name="logManager">The log manager.</param>
		protected MessageProcessorBase(IBusinessMessageProvider businessMessageProvider,
			IValidatorResolver validatorResolver, IXmlParserResolver xmlParserResolver, ILogManager logManager)
		{
			_businessMessageProvider = businessMessageProvider;
			_validatorResolver = validatorResolver;
			_xmlParserResolver = xmlParserResolver;
			_logManager = logManager;
			LogMessageExtendedProperties = new Dictionary<string, string>();
		}

		/// <summary>
		///     Gets the name.
		/// </summary>
		/// <value>The name.</value>
		protected abstract string Name { get; }

		/// <summary>
		///     Gets or sets the log message extended properties.
		/// </summary>
		/// <value>The log message extended properties.</value>
		protected IDictionary<string, string> LogMessageExtendedProperties { get; private set; }

		/// <summary>
		///     Validates the specified dto.
		/// </summary>
		/// <param name="dto">The dto.</param>
		/// <returns>IDictionary{System.StringSystem.String}.</returns>
		protected IDictionary<string, string> Validate(object dto)
		{
			var validator = _validatorResolver.Resolve(Name);
			return validator.Validate(dto);
		}

		/// <summary>
		///     Parses the specified XML.
		/// </summary>
		/// <param name="xml">The XML.</param>
		/// <returns>System.Object.</returns>
		protected object Parse(string xml)
		{
			var xmlParser = _xmlParserResolver.Resolve(Name);
			return xmlParser.Parse(xml);
		}

		/// <summary>
		///     Logs the message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="dto">The dto.</param>
		/// <param name="errors">The errors.</param>
		protected void LogMessage(OrderMessage message, object dto, IEnumerable<KeyValuePair<string, string>> errors)
		{
			LogMessageExtendedProperties.Add("MessageId", string.Format("{0}", message.Id));
			LogMessageExtendedProperties.Add("MessageExternalId", string.Format("{0}", message.ExternalMessageId));
			LogMessageExtendedProperties.Add("Receiver", string.Format("{0}", message.Receiver));
			LogMessageExtendedProperties.Add("Originator", string.Format("{0}", message.Originator));

			foreach (var error in errors)
			{
				LogMessageExtendedProperties.Add(error.Key, error.Value);
			}

			using (var stream = new MemoryStream())
			{
				var type = dto.GetType();
				var serializer = new DataContractSerializer(type);
				serializer.WriteObject(stream, dto);
				stream.Seek(0, SeekOrigin.Begin);
				var data = Encoding.UTF8.GetString(stream.ToArray());
				LogMessageExtendedProperties.Add(type.Name, data);
			}

			_businessMessageProvider.Publish(AuditMessageIdEnumDto.IncomingRequestValidationError,
				"Unable to Import an Order due to one or more validation errors.",
				LogMessageExtendedProperties);

			// Reset for multiple possible log calls
			LogMessageExtendedProperties = new Dictionary<string, string>();
		}

		/// <summary>
		/// Logs the information message.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		/// <param name="message">The message.</param>
		/// <param name="orderMessage">The order message.</param>
		/// <param name="incomingOrderDto">The incoming order dto.</param>
		/// <param name="serviceLine">The service line.</param>
		protected void LogInfoMessage(int messageId, string message, OrderMessage orderMessage,
			IncomingOrderDto incomingOrderDto, IncomingOrderServiceLineDto serviceLine = null)
		{
			LogMessage(messageId, message, orderMessage, incomingOrderDto, TraceEventType.Information, serviceLine);
		}

		/// <summary>
		/// Logs the verbose message.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		/// <param name="message">The message.</param>
		/// <param name="orderMessage">The order message.</param>
		/// <param name="incomingOrderDto">The incoming order dto.</param>
		/// <param name="serviceLine">The service line.</param>
		protected void LogVerboseMessage(int messageId, string message, OrderMessage orderMessage,
			IncomingOrderDto incomingOrderDto, IncomingOrderServiceLineDto serviceLine = null)
		{
			LogMessage(messageId, message, orderMessage, incomingOrderDto, TraceEventType.Verbose, serviceLine);
		}

		/// <summary>
		/// Logs the message.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		/// <param name="message">The message.</param>
		/// <param name="orderMessage">The order message.</param>
		/// <param name="incomingOrderDto">The incoming order dto.</param>
		/// <param name="traceEventType">Type of the trace event.</param>
		/// <param name="serviceLine">The service line.</param>
		protected void LogMessage(int messageId, string message, OrderMessage orderMessage,
			IncomingOrderDto incomingOrderDto, TraceEventType traceEventType, IncomingOrderServiceLineDto serviceLine = null)
		{
			var logMessage = new LogMessage(messageId, LogPriority.Low, TraceEventType.Information, message,
				LogCategory.System,
				LogCategory.InboundOrderMessageService);
			logMessage.Data.Add("OrderNumber",
				incomingOrderDto == null || string.IsNullOrWhiteSpace(incomingOrderDto.OrderNumber)
					? "N/A"
					: incomingOrderDto.OrderNumber);
			logMessage.Data.Add("Customer ExternalId",
				incomingOrderDto == null || incomingOrderDto.IncomingOrderCustomer == null ||
				string.IsNullOrWhiteSpace(incomingOrderDto.IncomingOrderCustomer.ExternalId)
					? "N/A"
					: incomingOrderDto.IncomingOrderCustomer.ExternalId);
			logMessage.Data.Add("Customer Name",
				incomingOrderDto == null || incomingOrderDto.IncomingOrderCustomer == null ||
				string.IsNullOrWhiteSpace(incomingOrderDto.IncomingOrderCustomer.Name)
					? "N/A"
					: incomingOrderDto.IncomingOrderCustomer.Name);
			logMessage.Data.Add("MessageId",
				orderMessage == null || string.IsNullOrWhiteSpace(orderMessage.MessageId)
					? "N/A"
					: orderMessage.MessageId);
			logMessage.Data.Add("ExternalMessageId",
				orderMessage == null || string.IsNullOrWhiteSpace(orderMessage.ExternalMessageId)
					? "N/A"
					: orderMessage.ExternalMessageId);
			logMessage.Data.Add("Originator",
				orderMessage == null || string.IsNullOrWhiteSpace(orderMessage.Originator) ? "N/A" : orderMessage.Originator);
			logMessage.Data.Add("Receiver",
				orderMessage == null || string.IsNullOrWhiteSpace(orderMessage.Receiver) ? "N/A" : orderMessage.Receiver);
			if (serviceLine != null)
			{
				logMessage.Data.Add("IndustryCode",
					string.IsNullOrWhiteSpace(serviceLine.IndustryCode) ? "N/A" : serviceLine.IndustryCode);
				logMessage.Data.Add("IndustryCodeLabel",
					string.IsNullOrWhiteSpace(serviceLine.IndustryCodeLabel) ? "N/A" : serviceLine.IndustryCodeLabel);
				logMessage.Data.Add("ServiceCode",
					string.IsNullOrWhiteSpace(serviceLine.ServiceCode) ? "N/A" : serviceLine.ServiceCode);
				logMessage.Data.Add("ServiceCodeLabel",
					string.IsNullOrWhiteSpace(serviceLine.ServiceCodeLabel) ? "N/A" : serviceLine.ServiceCodeLabel);
				logMessage.Data.Add("LocationCode",
					string.IsNullOrWhiteSpace(serviceLine.LocationCode) ? "N/A" : serviceLine.LocationCode);
				logMessage.Data.Add("LocationCodeLabel",
					string.IsNullOrWhiteSpace(serviceLine.LocationCodeLabel) ? "N/A" : serviceLine.LocationCodeLabel);
			}
			_logManager.Log(logMessage);
		}
	}
}