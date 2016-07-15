using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Interface IProjectTemplateManager
    /// </summary>
    public interface IProjectTemplateManager
    {
        /// <summary>
        ///     Deletes the specified project template id.
        /// </summary>
        /// <param name="projectTemplateId">The project template id.</param>
        void Delete(Guid projectTemplateId);

        /// <summary>
        ///     Fetches the project templates.
        /// </summary>
        /// <returns></returns>
        IList<ProjectTemplate> FindAll();

        /// <summary>
        ///     Finds the by id.
        /// </summary>
        /// <param name="projectTemplateId">The project template id.</param>
        /// <returns>ProjectTemplate.</returns>
        ProjectTemplate FindById(Guid projectTemplateId);

        /// <summary>
        ///     Finds editable version by id.
        /// </summary>
        /// <param name="projectTemplateId">The project template id.</param>
        /// <returns>ProjectTemplate.</returns>
        ProjectTemplate FindEditableById(Guid projectTemplateId);

        /// <summary>
        ///     Updates the specified project template.
        /// </summary>
        /// <param name="projectTemplate">The project template.</param>
        void Update(ProjectTemplate projectTemplate);

        /// <summary>
        ///     Creates the specified project template.
        /// </summary>
        /// <param name="projectTemplate">The project template.</param>
        /// <returns>Guid.</returns>
        Guid Create(ProjectTemplate projectTemplate);

		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns>ProjectTemplateSearchResultSet.</returns>
        SearchResultSetBase<ProjectTemplate> Search(SearchCriteria searchCriteria);
    }
}