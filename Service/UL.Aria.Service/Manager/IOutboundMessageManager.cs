using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Service.Host;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines operations for sending outbound messages.
    /// </summary>
    public interface IOutboundMessageManager:IProcessingManager
    {
        /// <summary>
        /// Publishes the specified project status message.
        /// </summary>
        /// <param name="projectStatusMessage">The project status message.</param>
        void Publish(ProjectStatusMessage projectStatusMessage);
    }
}
