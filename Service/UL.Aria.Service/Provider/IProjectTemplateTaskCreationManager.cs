using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Defines operations for creating <see cref="Task"/> entities for <see cref="Domain.Entity.Project"/> entities based on
    /// <see cref="Domain.Entity.ProjectTemplate"/> entities.
    /// </summary>
    public interface IProjectTemplateTaskCreationManager
    {
        /// <summary>
        /// Creates the project template tasks.
        /// </summary>
        /// <param name="projectTemplateId">The project template identifier.</param>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="taskNumberSeed">The task number seed.</param>
        void CreateProjectTemplateTasks(Guid projectTemplateId, Guid containerId, int taskNumberSeed = 0);

        /// <summary>
        /// Creates the project template tasks for update.
        /// </summary>
        /// <param name="projectTemplateId">The project template identifier.</param>
        /// <param name="containerId">The container identifier.</param>
        void CreateProjectTemplateTasksForUpdate(Guid? projectTemplateId, Guid containerId);
    }
}
