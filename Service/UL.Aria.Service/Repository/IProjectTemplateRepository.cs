using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Interface IProjectTemplateRepository
    /// </summary>
    public interface IProjectTemplateRepository : ISearchRepositoryBase<ProjectTemplate>
    {
        /// <summary>
        ///     Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Guid.</returns>
        Guid Create(ProjectTemplate entity);

        /// <summary>
        ///     Gets the by id.
        /// </summary>
        /// <param name="projectTemplateId">The project task template id.</param>
        /// <returns>ProjectTemplate.</returns>
        ProjectTemplate GetById(Guid projectTemplateId);

        /// <summary>
        ///     Finds the project templates with the same correlationId.
        /// </summary>
        /// <returns>IList{ProjectTemplate}.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        IList<ProjectTemplate> GetAllByCorrelationId(Guid correlationId);        
    }
}