using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provides an interface for a Task Template Provider.
    /// </summary>
    public interface ITaskTemplateProvider
    {

        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        TaskTemplate FetchById(Guid id);

        /// <summary>
        /// Fetches the task template by project template.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="flatten">if set to <c>true</c> [flatten].</param>
        /// <returns></returns>
        IEnumerable<TaskTemplate> FetchTaskTemplateByProjectTemplate(Guid projectId, bool flatten = false);
        
        /// <summary>
        /// Deletes the specified task template identifier.
        /// </summary>
        /// <param name="taskTemplateId">The task template identifier.</param>
        void Delete(Guid taskTemplateId);
        
        /// <summary>
        /// Creates the specified task template.
        /// </summary>
        /// <param name="taskTemplate">The task template.</param>
        /// <returns></returns>
        TaskTemplate Create(TaskTemplate taskTemplate);
        
        /// <summary>
        /// Updates the specified task template.
        /// </summary>
        /// <param name="taskTemplate">The task template.</param>
        void Update(TaskTemplate taskTemplate);

        /// <summary>
        /// Upldates the bulk.
        /// </summary>
        /// <param name="projectTemplateId"></param>
        /// <param name="taskTemplates">The task templates.</param>
        void UpdateBulk(Guid projectTemplateId, IEnumerable<TaskTemplate> taskTemplates);
    }
}
