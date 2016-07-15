using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.InboundOrderProcessing.Logging;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Service.Host;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.InboundOrderProcessing.Domain;
using UL.Aria.Service.InboundOrderProcessing.Provider;
using UL.Aria.Service.InboundOrderProcessing.Resolver;

namespace UL.Aria.Service.InboundOrderProcessing.Manager
{
    /// <summary>
    ///     class to manage the whole process of retrieving a message from the Provider, Parsing and Publishing it as a project
    /// </summary>
    public sealed class InboundMessageManager : IProcessingManager
    {
        private readonly IInboundOrderProvider _inboundOrderProvider;
        private readonly ILogManager _logManager;
	    private readonly IBusinessMessageProvider _businessMessageProvider;
	    private readonly IMessageProcessorResolver _messageProcessorResolver;
        private readonly ITransactionFactory _transactionFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="InboundMessageManager" /> class.
		/// </summary>
		/// <param name="transactionFactory">The transaction factory.</param>
		/// <param name="inboundOrderProvider">The inbound order provider.</param>
		/// <param name="logManager">The log manager.</param>
		/// <param name="messageProcessorResolver">The message processor resolve.</param>
		/// <param name="businessMessageProvider">The business message provider.</param>
        public InboundMessageManager(ITransactionFactory transactionFactory, IInboundOrderProvider inboundOrderProvider,
            ILogManager logManager,
            IMessageProcessorResolver messageProcessorResolver, IBusinessMessageProvider businessMessageProvider)
        {
            _transactionFactory = transactionFactory;
            _inboundOrderProvider = inboundOrderProvider;
            _logManager = logManager;
			_businessMessageProvider = businessMessageProvider;
			_messageProcessorResolver = messageProcessorResolver;
            MilliSecondsTimeout = 10*1000;
            GetErrorWaitFunction = GetErrorWait;
            ErrorCount = 0;
        }

        /// <summary>
        ///     Gets or sets the milli seconds timeout.
        /// </summary>
        /// <value>
        ///     The milli seconds timeout.
        /// </value>
        public int MilliSecondsTimeout { get; set; }

        /// <summary>
        ///     Gets or sets the error count.
        /// </summary>
        /// <value>The error count.</value>
        public int ErrorCount { get; set; }

        internal Func<int> GetErrorWaitFunction
        {
            //be sure to update InboundMessageManager_Sets_Default_ErrorTimeout if changing this
            get; set;
        }

        /// <summary>
        ///     Processes this instance.
        /// </summary>
        public Task Process(CancellationToken token, bool isContinuous = true)
        {
            return new Task(() =>
            {
	            var processDelay = ProcessDelay;
                do
                {
                    if (ErrorCount > 2)
                    {
                        Thread.Sleep(GetErrorWaitFunction());
                        ErrorCount = 0;
                    }

                    OrderMessage orderMessage = null;

                    try
                    {
                        Trace.CorrelationManager.ActivityId = Guid.NewGuid();
                        using (var transaction = _transactionFactory.Create())
                        {
                            orderMessage = _inboundOrderProvider.Dequeue();
                            if (orderMessage != null)
                            {
                                var logMessage1 = new LogMessage((int)AuditMessageIdEnumDto.BuyMessageDequeued, LogPriority.Low, TraceEventType.Information, "Buy Message Dequeued" , LogCategory.System,
                                    LogCategory.InboundOrderMessageService);
                                logMessage1.Data.Add("ExternalMessageId", orderMessage.ExternalMessageId);
                                logMessage1.Data.Add("Originator", orderMessage.Originator);
                                logMessage1.Data.Add("Receiver", orderMessage.Receiver);
                                _logManager.Log(logMessage1);
                            }
                            transaction.Complete();
                        }

                        if (orderMessage == null)
                        {
                            Thread.Sleep(MilliSecondsTimeout);
                            continue;
                        }

                        var logMessage = new LogMessage(MessageIds.InboundOrderProcessingMessageReceived, LogPriority.Medium, TraceEventType.Verbose,
                            string.Format("ExternalMessageId:{0}, Originator:{1}, Receiver:{2}",
                                orderMessage.ExternalMessageId, orderMessage.Originator, orderMessage.Receiver),
                            LogCategory.InboundProcessor);
                        logMessage.LogCategories.Add(LogCategory.InboundProcessor);
                        _logManager.Log(logMessage);

                        var receiver = orderMessage.Receiver.ToLower();

                        var messageProcessor = _messageProcessorResolver.Resolve(receiver);
                        messageProcessor.ProcessMessage(orderMessage);

                        logMessage = new LogMessage((int)AuditMessageIdEnumDto.BuyMessageProcessed, LogPriority.Low, TraceEventType.Information, "Buy Message Processed", LogCategory.System,
                                LogCategory.InboundOrderMessageService);
						logMessage.Data.Add("ExternalMessageId", orderMessage.ExternalMessageId);
						logMessage.Data.Add("Originator", orderMessage.Originator);
						logMessage.Data.Add("Receiver", orderMessage.Receiver);
						_logManager.Log(logMessage);

						ErrorCount = 0;
						if (processDelay > 0)
						{
							Thread.Sleep(processDelay);
						}
                    }
                    catch (Exception ex)
                    {
                        ErrorCount++;
	                    if (orderMessage != null)
	                    {
		                    var errors = new Dictionary<string, string> {{"Exception", ex.Message}};
		                    LogMessage(orderMessage, errors);
	                    }
	                    var logMessage = ex.ToLogMessage(MessageIds.InboundOrderProcessingProcessException, LogCategory.InboundProcessor, LogPriority.Critical,
                            TraceEventType.Critical);
	                    if (orderMessage != null)
	                    {
							logMessage.Data.Add("ExternalMessageId", orderMessage.ExternalMessageId);
							logMessage.Data.Add("Originator", orderMessage.Originator);
		                    logMessage.Data.Add("Receiver", orderMessage.Receiver);
	                    }
	                    _logManager.Log(logMessage);
                    }

                    if (token.IsCancellationRequested)
                        throw new OperationCanceledException(token);
                } while (isContinuous);
            }, token);
        }

		private void LogMessage(OrderMessage message, IEnumerable<KeyValuePair<string, string>> errors)
		{
			var logMessageExtendedProperties = new Dictionary<string, string>
			{
				{"MessageId", string.Format("{0}", message.Id)},
				{"MessageExternalId", string.Format("{0}", message.ExternalMessageId)},
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

        private static int GetErrorWait()
        {
            //be sure to update InboundMessageManager_Sets_Default_ErrorTimeout if changing this
            return 29*1000 + new Random().Next(1, 1000);
        }

		/// <summary>
		/// Gets the process delay.
		/// </summary>
		/// <value>The process delay.</value>
		internal static int ProcessDelay
		{
			get
			{
				string appSetting = ConfigurationManager.AppSettings["UL.Aria.ProcessDelay"];
				return !string.IsNullOrWhiteSpace(appSetting) ? Convert.ToInt32(appSetting) : 0;
			}
		}
	}
}