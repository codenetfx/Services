using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Logging
{
    /// <summary>
    /// Extension and static helpers for Logging
    /// </summary>
    public static class LoggingUtility
    {
        /// <summary>
        /// The project identifier label
        /// </summary>
        public const string ProjectIdLabel = "ProjectId";

        /// <summary>
        /// The project new status label
        /// </summary>
        public const string ProjectNewStatusLabel = "Project NewStatus";

        /// <summary>
        /// The project old status
        /// </summary>
        public const string ProjectOldStatusLabel = "Project OldStatus";

        /// <summary>
        /// Decorates the supplied <see cref="LogMessage"/> with data specific to the supplied <see cref="ProjectStatusMessage"/>.
        /// </summary>
        /// <param name="logMessage">The log message.</param>
        /// <param name="projectStatusMessage">The project status message.</param>
        public static void DecorateLogMessage(this ProjectStatusMessage projectStatusMessage, LogMessage logMessage)
        {
            if (projectStatusMessage != null)
            {
                logMessage.Data.Add(ProjectIdLabel, projectStatusMessage.ProjectId.ToString());
                logMessage.Data.Add(ProjectNewStatusLabel, projectStatusMessage.NewStatus.ToString());
                logMessage.Data.Add(ProjectOldStatusLabel, projectStatusMessage.OldStatus.ToString());
            }
            else
            {
                logMessage.Data.Add(ProjectIdLabel, "Not Found");
            }
        }

        /// <summary>
        /// Decorates the supplied <see cref="LogMessage"/> with data specific to the supplied <see cref="ProjectStatusMessageDto"/>.
        /// </summary>
        /// <param name="logMessage">The log message.</param>
        /// <param name="projectStatusMessage">The project status message.</param>
        public static void DecorateLogMessage(this ProjectStatusMessageDto projectStatusMessage, LogMessage logMessage)
        {
            if (projectStatusMessage != null)
            {
                logMessage.Data.Add(ProjectIdLabel, projectStatusMessage.ProjectId.ToString());
                logMessage.Data.Add(ProjectNewStatusLabel, projectStatusMessage.NewStatus.ToString());
                logMessage.Data.Add(ProjectOldStatusLabel, projectStatusMessage.OldStatus.ToString());
            }
            else
            {
                logMessage.Data.Add(ProjectIdLabel, "Not Found");
            }
        }
    }
}
