using System;
using System.Diagnostics;
using System.ServiceModel.Channels;
using UL.Aria.Common.Authorization;
using UL.Aria.Service.Logging;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Handles publish of <see cref="ProjectStatusMessage"/> objects
    /// </summary>
    public class ProjectStatusMessageProvider : IProjectStatusMessageProvider
    {
        private readonly IProjectStatusMessageRepository _projectStatusMessageRepository;
        private readonly ITransactionFactory _transactionFactory;
        private readonly IPrincipalResolver _principalResolver;
        private readonly ILogManager _logManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectStatusMessageProvider" /> class.
        /// </summary>
        /// <param name="projectStatusMessageRepository">The project status message repository.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="logManager">The log manager.</param>
        public ProjectStatusMessageProvider(IProjectStatusMessageRepository projectStatusMessageRepository, ITransactionFactory transactionFactory, IPrincipalResolver principalResolver, ILogManager logManager)
        {
            _projectStatusMessageRepository = projectStatusMessageRepository;
            _transactionFactory = transactionFactory;
            _principalResolver = principalResolver;
            _logManager = logManager;
        }

        /// <summary>
        /// Publishes the specified project status message.
        /// </summary>
        /// <param name="projectStatusMessage">The project status message.</param>
        public void Publish(ProjectStatusMessage projectStatusMessage)
        {   
            using (var scope = _transactionFactory.Create())
            {
                if (null == projectStatusMessage.Id)
                    projectStatusMessage.Id = Guid.NewGuid();
                projectStatusMessage.CreatedDateTime = DateTime.UtcNow;
                projectStatusMessage.UpdatedDateTime = DateTime.UtcNow;
                projectStatusMessage.CreatedById = _principalResolver.UserId;
                projectStatusMessage.UpdatedById = _principalResolver.UserId;
                try
                {
                    _projectStatusMessageRepository.Add(projectStatusMessage);
                    
                    scope.Complete();
                    var logMessage = new LogMessage(MessageIds.ProjectStatusMessagePublished, LogPriority.Low, TraceEventType.Information, "Project Status Message Published.", LogCategory.Project);   
                    projectStatusMessage.DecorateLogMessage(logMessage);
                    logMessage.LogCategories.Add(LogCategory.Project);
                    _logManager.Log(logMessage);
                }
                catch (Exception exception)
                {
                    var logMessage = exception.ToLogMessage(MessageIds.ProjectStatusMessagePublishError, LogCategory.Project, LogPriority.High, TraceEventType.Error);
                    projectStatusMessage.DecorateLogMessage(logMessage);
                    logMessage.LogCategories.Add(LogCategory.Project);
                    _logManager.Log(logMessage);
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the next.
        /// </summary>
        /// <returns></returns>
        public ProjectStatusMessage GetNext()
        {
            try
            {
                var nextProjectStatusMessage = _projectStatusMessageRepository.GetNext();

                var logMessage = new LogMessage(MessageIds.ProjectStatusMessageDequeued, LogPriority.Low, TraceEventType.Verbose, "Project Status Message Dequeued.", LogCategory.Project);
                nextProjectStatusMessage.DecorateLogMessage(logMessage);
                logMessage.LogCategories.Add(LogCategory.Project);
                _logManager.Log(logMessage);

                return nextProjectStatusMessage;
            }
            catch (Exception exception)
            {
                _logManager.Log(exception.ToLogMessage(MessageIds.ProjectStatusMessageDequeueError, LogCategory.Project, LogPriority.High, TraceEventType.Error));
                throw;
            }
        }
    }
}