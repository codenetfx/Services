using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Handles publish of <see cref="ProjectStatusMessage"/> objects
    /// </summary>
    public interface IProjectStatusMessageProvider
    {
        /// <summary>
        /// Publishes the specified project status message.
        /// </summary>
        /// <param name="projectStatusMessage">The project status message.</param>
        void Publish(ProjectStatusMessage projectStatusMessage);

        /// <summary>
        /// Gets the next.
        /// </summary>
        /// <returns></returns>
        ProjectStatusMessage GetNext();
    }
}
