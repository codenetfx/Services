using System;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Azure.WebJobs;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;

using UL.Aria.Service.ContactProcessor.WebJob.Logging;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.InboundOrderProcessing.MessageProcessor;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.ContactProcessor.WebJob
{
	/// <summary>
	/// Class Functions.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class Functions
	{
		/// <summary>
		/// The _max dequeue count
		/// </summary>
		// ReSharper disable once InconsistentNaming
		private static readonly int _maxDequeueCount;

		/// <summary>
		/// Initializes static members of the <see cref="Functions"/> class.
		/// </summary>
		static Functions()
		{
			_maxDequeueCount = ConfigurationManager.AppSettings.GetValue("ContactOrder.DequeueCount", 3);
		}

		/// <summary>
		/// Processes the contact message.
		/// </summary>
		/// <param name="brokeredMessage">The brokered message.</param>
		public static void ProcessContactMessage(
			[ServiceBusTrigger("%ContactOrder.QueueName%")] BrokeredMessage brokeredMessage)
		{
			using (new NativeMethods.Impersonation(WorkerRole._domain, WorkerRole._user, WorkerRole._pwd))
			{
				var logManager =
					(ILogManager) ContainerLocator.Container.Resolve(typeof (ILogManager));
				LogMessage logMessage;
				var contactOrder = brokeredMessage.GetBody<ContactOrderDto>();

				try
				{
					Trace.CorrelationManager.ActivityId = Guid.NewGuid();

					logMessage = new LogMessage((int) AuditMessageIdEnumDto.ContactOrderMessageDequeued, LogPriority.Low,
						TraceEventType.Information, "Contact Order Message Dequeued", LogCategory.System,
						LogCategory.InboundOrderMessageService);
					logMessage.Data.Add("MessageId", contactOrder.MessageId);
					logMessage.Data.Add("OrderNumber", contactOrder.OrderNumber);
					logMessage.Data.Add("Receiver", contactOrder.Receiver);
					logManager.Log(logMessage);

					if (brokeredMessage.DeliveryCount > _maxDequeueCount)
					{
						logMessage = new LogMessage(MessageIds.ContactProcessingDequeueCountExceeded)
						{
							LogPriority = LogPriority.Critical,
							Severity = TraceEventType.Critical,
							Message = "Dequeue count exceeded"
						};
						logMessage.LogCategories.Add(LogCategory.InboundProcessor);
						logMessage.Data.Add("MessageId", contactOrder.MessageId);
						logMessage.Data.Add("OrderNumber", contactOrder.OrderNumber);
						logMessage.Data.Add("Receiver", contactOrder.Receiver);
						logManager.Log(logMessage);

						return;
					}

					var contactProcessor =
						(IContactProcessor) ContainerLocator.Container.Resolve(typeof (IContactProcessor));

					contactProcessor.Process(contactOrder);
				}
				catch (Exception ex)
				{
					logMessage = ex.ToLogMessage(MessageIds.ContactProcessingProcessContactMessageException,
						LogCategory.InboundProcessor,
						LogPriority.Critical,
						TraceEventType.Critical);
					logMessage.Data.Add("MessageId", contactOrder.MessageId);
					logMessage.Data.Add("OrderNumber", contactOrder.OrderNumber);
					logMessage.Data.Add("Receiver", contactOrder.Receiver);
					logManager.Log(logMessage);
					throw;
				}
			}
		}
	}
}