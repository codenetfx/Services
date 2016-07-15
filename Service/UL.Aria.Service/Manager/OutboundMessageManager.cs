using System;
using System.Diagnostics;
using System.Threading;
using UL.Aria.Service.Logging;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;
using Task = System.Threading.Tasks.Task;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements operations for sending outbound messages.
    /// </summary>
    public class OutboundMessageManager : IOutboundMessageManager
    {
        private readonly IProjectStatusMessageProvider _projectStatusMessageProvider;
        private readonly IMessageService _messageService;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;
        private readonly ILogManager _logManager;

        internal int MilliSecondsTimeout { get; set; }

        /// <summary>
        /// Gets or sets the error count.
        /// </summary>
        /// <value>The error count.</value>
        public int ErrorCount { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutboundMessageManager" /> class.
        /// </summary>
        /// <param name="projectStatusMessageProvider">The project status message provider.</param>
        /// <param name="messageService">The message service.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="logManager">The log manager.</param>
        public OutboundMessageManager(IProjectStatusMessageProvider projectStatusMessageProvider, IMessageService messageService, IMapperRegistry mapperRegistry, ITransactionFactory transactionFactory, ILogManager logManager)
        {
            _projectStatusMessageProvider = projectStatusMessageProvider;
            _messageService = messageService;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
            _logManager = logManager;
            MilliSecondsTimeout = 10 * 1000;
            ErrorCount = 0;
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="isContinuous">if set to <c>true</c> [is continuous].</param>
        /// <returns></returns>
        public Task Process(CancellationToken token, bool isContinuous = true)
        {
            return new Task(() =>
            {
                do
                {

                    if (ErrorCount > 2)
                    {
                        Thread.Sleep(29 * 1000 + new Random().Next(1, 1000));
                        ErrorCount = 0;
                    }

                    ProjectStatusMessage projectStatusMessage = null;
                    try
                    {
                        Trace.CorrelationManager.ActivityId = Guid.NewGuid();
                        using (var scope = _transactionFactory.Create())
                        {
                            projectStatusMessage = _projectStatusMessageProvider.GetNext();

                            if (projectStatusMessage == null)
                            {
                                new ManualResetEvent(false).WaitOne(MilliSecondsTimeout);
                                continue;
                            }

                            Publish(projectStatusMessage);
                            scope.Complete();
                        }                       
                        ErrorCount = 0;
                    }
                    catch (Exception ex)
                    {
                        ErrorCount++;

                        // don't fail, just log.
                        var logMessage = ex.ToLogMessage(MessageIds.OutboundMessageException, LogCategory.OutboundMessage,
                                                         LogPriority.Critical, TraceEventType.Critical);
                        projectStatusMessage.DecorateLogMessage(logMessage);
                        _logManager.Log(logMessage);
                    }

                    if (token.IsCancellationRequested)
                        throw new OperationCanceledException(token);
                } while (isContinuous);
            }, token);
        }

        /// <summary>
        /// Publishes the specified project status message.
        /// </summary>
        /// <param name="projectStatusMessage">The project status message.</param>
        public void Publish(ProjectStatusMessage projectStatusMessage)
        {
            var existingCorrelationId = Trace.CorrelationManager.ActivityId;
            if (projectStatusMessage.CorrelationId != default(Guid))
            {
                Trace.CorrelationManager.ActivityId = projectStatusMessage.CorrelationId;
            }
            var startLogMessage = new LogMessage(LogEventIds.OutboundMessage.ProjectStatusMessageStartPublished, LogPriority.Low, TraceEventType.Verbose, "An outbound Project Status Message is being published.", LogCategory.OutboundMessage);
            projectStatusMessage.DecorateLogMessage(startLogMessage);
            _logManager.Log(startLogMessage);

            var projectStatusMessageDto = _mapperRegistry.Map<ProjectStatusMessageDto>(projectStatusMessage);

            _messageService.PublishProjectStatusMessage(projectStatusMessageDto);

            var endLogMessage = new LogMessage(LogEventIds.OutboundMessage.ProjectStatusMessageEndPublished, LogPriority.Low, TraceEventType.Verbose, "An outbound Project Status Message has been published.", LogCategory.OutboundMessage);
            projectStatusMessage.DecorateLogMessage(endLogMessage);
            _logManager.Log(endLogMessage);
            
            Trace.CorrelationManager.ActivityId = existingCorrelationId;
        }
    }
}