using System;
using System.Collections.Generic;
using System.Diagnostics;
using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Contracts;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.InboundOrderProcessing.Domain;
using UL.Aria.Service.InboundOrderProcessing.Logging;
using UL.Aria.Service.InboundOrderProcessing.Resolver;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.InboundOrderProcessing.Manager
{
	/// <summary>
	///     class to manage the whole process of retrieving a message from the Provider, Parsing and Publishing it as a project
	/// </summary>
	public sealed class InboundMessageWebJobManager : IInboundMessageWebJobManager
	{
		private readonly IBusinessMessageProvider _businessMessageProvider;
		private readonly ILogManager _logManager;
		private readonly IMessageProcessorResolver _messageProcessorResolver;

		/// <summary>
		/// Initializes a new instance of the <see cref="InboundMessageManager" /> class.
		/// </summary>
		/// <param name="logManager">The log manager.</param>
		/// <param name="messageProcessorResolver">The message processor resolve.</param>
		/// <param name="businessMessageProvider">The business message provider.</param>
		public InboundMessageWebJobManager(ILogManager logManager,
			IMessageProcessorResolver messageProcessorResolver, IBusinessMessageProvider businessMessageProvider)
		{
			_logManager = logManager;
			_businessMessageProvider = businessMessageProvider;
			_messageProcessorResolver = messageProcessorResolver;
		}

		/// <summary>
		/// Processes this instance.
		/// </summary>
		/// <param name="inboundMessage">The inbound message.</param>
		/// <param name="message">The message.</param>
		public void Process(InboundMessageDto inboundMessage, string message)
		{
			try
			{
				var logMessage = new LogMessage(MessageIds.InboundOrderProcessingMessageReceived, LogPriority.Medium,
					TraceEventType.Verbose,
					string.Format("MessageId:{0}, ExternalMessageId:{1}, Originator:{2}, Receiver:{3}",
						inboundMessage.MessageId, inboundMessage.ExternalMessageId, inboundMessage.Originator, inboundMessage.Receiver),
					LogCategory.InboundProcessor);
				logMessage.LogCategories.Add(LogCategory.InboundProcessor);
				_logManager.Log(logMessage);

				var receiver = inboundMessage.Receiver.ToLower();

				var messageProcessor = _messageProcessorResolver.Resolve(receiver);
				var orderMessage = new OrderMessage
				{
					Body = message,
					MessageId = inboundMessage.MessageId,
					ExternalMessageId = inboundMessage.ExternalMessageId,
					Receiver = inboundMessage.Receiver,
					Originator = inboundMessage.Originator
				};

				messageProcessor.ProcessMessage(orderMessage);

				logMessage = new LogMessage((int) AuditMessageIdEnumDto.BuyMessageProcessed, LogPriority.Low,
					TraceEventType.Information, "Buy Message Processed", LogCategory.System,
					LogCategory.InboundOrderMessageService);
				logMessage.Data.Add("MessageId", inboundMessage.MessageId);
				logMessage.Data.Add("ExternalMessageId", inboundMessage.ExternalMessageId);
				logMessage.Data.Add("Originator", inboundMessage.Originator);
				logMessage.Data.Add("Receiver", inboundMessage.Receiver);
				_logManager.Log(logMessage);
			}
			catch (Exception ex)
			{
				if (inboundMessage != null)
				{
					var errors = new Dictionary<string, string> {{"Exception", ex.Message}};
					LogMessage(inboundMessage, errors);
				}
				var logMessage = ex.ToLogMessage(MessageIds.InboundOrderProcessingProcessException, LogCategory.InboundProcessor,
					LogPriority.Critical,
					TraceEventType.Critical);
				if (inboundMessage != null)
				{
					logMessage.Data.Add("MessageId", inboundMessage.MessageId);
					logMessage.Data.Add("ExternalMessageId", inboundMessage.ExternalMessageId);
					logMessage.Data.Add("Originator", inboundMessage.Originator);
					logMessage.Data.Add("Receiver", inboundMessage.Receiver);
				}
				_logManager.Log(logMessage);
				throw;
			}
		}

		private void LogMessage(InboundMessageDto message, IEnumerable<KeyValuePair<string, string>> errors)
		{
			var logMessageExtendedProperties = new Dictionary<string, string>
			{
				{"MessageId", string.Format("{0}", message.MessageId)},
				{"ExternalMessageId", string.Format("{0}", message.ExternalMessageId)},
				{"Receiver", string.Format("{0}", message.Receiver)},
				{"Originator", string.Format("{0}", message.Originator)}
			};

			foreach (var error in errors)
			{
				logMessageExtendedProperties.Add(error.Key, error.Value);
			}

			_businessMessageProvider.Publish(AuditMessageIdEnumDto.IncomingRequestException,
				"Unable to Import an Order due to an exception.",
				logMessageExtendedProperties);
		}
	}
}