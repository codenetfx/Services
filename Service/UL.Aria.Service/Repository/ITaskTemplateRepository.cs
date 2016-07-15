using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Provides a Repository interface for Task Templates.
    /// </summary>
    public interface ITaskTemplateRepository:IRepositoryBase<TaskTemplate>
    {
        /// <summary>
        /// Fetches the task template by project template.
        /// </summary>
        /// <param name="projectTemplateId">The project identifier.</param>
        /// <returns></returns>
        IEnumerable<TaskTemplate> FetchTaskTemplateByProjectTemplate(Guid projectTemplateId);

        /// <summary>
        /// Updates the a list of TaskTemplates in bulk.
        /// </summary>
        /// <param name="ProjectTemplateId"></param>
        /// <param name="taskTemplates">The task templates.</param>
        void UpdateBulk(Guid ProjectTemplateId, IEnumerable<TaskTemplate> taskTemplates);
    }
}
