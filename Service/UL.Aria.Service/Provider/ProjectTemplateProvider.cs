using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Class ProjectTemplateProvider
    /// </summary>
    public sealed class ProjectTemplateProvider : IProjectTemplateProvider
    {
        private readonly IProjectTemplateRepository _projectTemplateRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectTemplateProvider" /> class.
        /// </summary>
        /// <param name="projectTemplateRepository">The project template repository.</param>
        public ProjectTemplateProvider(IProjectTemplateRepository projectTemplateRepository)
        {
            _projectTemplateRepository = projectTemplateRepository;
        }

        /// <summary>
        ///     Creates the specified project template.
        /// </summary>
        /// <param name="projectTemplate">The project template.</param>
        /// <returns>Guid.</returns>
        public Guid Create(ProjectTemplate projectTemplate)
        {
            return _projectTemplateRepository.Create(projectTemplate);
        }

        /// <summary>
        ///     Fetches the project templates.
        /// </summary>
        /// <returns></returns>
        public IList<ProjectTemplate> GetAll()
        {
            return _projectTemplateRepository.FindAll();
        }

        /// <summary>
        ///     Gets the by id.
        /// </summary>
        /// <param name="projectTemplateId">The project template id.</param>
        /// <returns>ProjectTemplate.</returns>
        public ProjectTemplate GetById(Guid projectTemplateId)
        {
            return _projectTemplateRepository.GetById(projectTemplateId);
        }

        /// <summary>
        ///     Fetches the project templates with the same correlationId.
        /// </summary>
        /// <param name="correlationId">The project template correlation id.</param>
        /// <returns>List{ProjectTemplate}.</returns>
        public IList<ProjectTemplate> GetAllByCorrelationId(Guid correlationId)
        {
            return _projectTemplateRepository.GetAllByCorrelationId(correlationId);
        }

        /// <summary>
        ///     Updates the specified project template.
        /// </summary>
        /// <param name="projectTemplate">The project template.</param>
        public void Update(ProjectTemplate projectTemplate)
        {
            _projectTemplateRepository.Update(projectTemplate);
        }

        /// <summary>
        ///     Deletes the specified project template id.
        /// </summary>
        /// <param name="projectTemplateId">The project template id.</param>
        public void Delete(Guid projectTemplateId)
        {
            _projectTemplateRepository.Remove(projectTemplateId);
        }

		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns>ProjectTemplateSearchResultSet.</returns>
		public SearchResultSetBase<ProjectTemplate> Search(SearchCriteria searchCriteria)
		{
			return _projectTemplateRepository.DefaultSearch<ProjectTemplate>(searchCriteria);
		}
	}
}