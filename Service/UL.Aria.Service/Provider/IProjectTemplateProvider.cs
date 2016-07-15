using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Interface IProjectTemplateProvider
    /// </summary>
    public interface IProjectTemplateProvider
    {
        /// <summary>
        ///     Creates the specified project template.
        /// </summary>
        /// <param name="projectTemplate">The project template.</param>
        /// <returns>Guid.</returns>
        Guid Create(ProjectTemplate projectTemplate);

        /// <summary>
        ///     Updates the specified project template.
        /// </summary>
        /// <param name="projectTemplate">The project template.</param>
        void Update(ProjectTemplate projectTemplate);

        /// <summary>
        /// Fetches the project templates.
        /// </summary>
        /// <returns></returns>
        IList<ProjectTemplate> GetAll();

        /// <summary>
        ///     Gets the by id.
        /// </summary>
        /// <param name="projectTemplateId">The project template id.</param>
        /// <returns>ProjectTemplate.</returns>
        ProjectTemplate GetById(Guid projectTemplateId);

        /// <summary>
        ///     Fetches the project templates with the same correlationId.
        /// </summary>
        /// <param name="correlationId">The project template correlation id.</param>
        /// <returns>List{ProjectTemplate}.</returns>
        IList<ProjectTemplate> GetAllByCorrelationId(Guid correlationId);

        /// <summary>
        ///     Deletes the specified project template id.
        /// </summary>
        /// <param name="projectTemplateId">The project template id.</param>
        void Delete(Guid projectTemplateId);

		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns>ProjectTemplateSearchResultSet.</returns>
        SearchResultSetBase<ProjectTemplate> Search(SearchCriteria searchCriteria);
    }
}