using System.ServiceModel;
using System.Linq;

using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using System.Collections.Generic;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     Class ProjectTemplateService
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class ProjectTemplateService : IProjectTemplateService
    {
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IProjectTemplateManager _projectTemplateManager;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectTemplateService" /> class.
        /// </summary>
        /// <param name="projectTemplateManager">The project task template manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public ProjectTemplateService(IProjectTemplateManager projectTemplateManager,
            IMapperRegistry mapperRegistry)
        {
            _projectTemplateManager = projectTemplateManager;
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        ///     Fetches the specified <see cref="ProjectTemplateDto" />.
        /// </summary>
        /// <returns>List{ProjectTemplateDto}</returns>
        public IList<ProjectTemplateDto> FetchAll()
        {
            return
                _projectTemplateManager.FindAll().Select(_mapperRegistry.Map<ProjectTemplateDto>).ToList();
        }

        /// <summary>
        ///     Fetches the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ProjectTemplateDto.</returns>
        public ProjectTemplateDto FetchById(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var idGuid = id.ToGuid();
            Guard.IsNotEmptyGuid(idGuid, "id");
            var projectTemplate = _projectTemplateManager.FindById(idGuid);
            return _mapperRegistry.Map<ProjectTemplateDto>(projectTemplate);
        }

        /// <summary>
        ///     Fetches editable version by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ProjectTemplateDto.</returns>
        public ProjectTemplateDto FetchEditableById(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var idGuid = id.ToGuid();
            Guard.IsNotEmptyGuid(idGuid, "id");
            var projectTemplate = _projectTemplateManager.FindEditableById(idGuid);

            var projectTemplateDto = _mapperRegistry.Map<ProjectTemplateDto>(projectTemplate);
            //if (projectTemplateDto != null)
            //{
            //    projectTemplateDto.TaskTemplates =
            //        projectTemplate.TaskTemplates.Select(x => _mapperRegistry.Map<TaskTemplateDto>(x)).ToList();
            //}

            return projectTemplateDto;
        }

        /// <summary>
        ///     Creates the specified project task template.
        /// </summary>
        /// <param name="projectTemplate">The project task template.</param>
        /// <returns>System.String.</returns>
        public string Create(ProjectTemplateDto projectTemplate)
        {
            Guard.IsNotNull(projectTemplate, "ProjectTemplate");
            var projectTemplateBo = _mapperRegistry.Map<ProjectTemplate>(projectTemplate);
            return _projectTemplateManager.Create(projectTemplateBo).ToString();
        }

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var idGuid = id.ToGuid();
            Guard.IsNotEmptyGuid(idGuid, "id");
            _projectTemplateManager.Delete(idGuid);
        }

        /// <summary>
        ///     Updates the specified project task template.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="projectTemplate">The project task template.</param>
        public void Update(string id, ProjectTemplateDto projectTemplate)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var idGuid = id.ToGuid();
            Guard.IsNotEmptyGuid(idGuid, "id");
            Guard.IsNotNull(projectTemplate, "ProjectTemplate");
            var projectTemplateBo = _mapperRegistry.Map<ProjectTemplate>(projectTemplate);
            _projectTemplateManager.Update(projectTemplateBo);
        }

		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns>ProjectTemplateSearchResultSetDto.</returns>
		public ProjectTemplateSearchResultSetDto Search(SearchCriteriaDto searchCriteria)
		{
			Guard.IsNotNull(searchCriteria, "searchCriteria");

			var criteria = _mapperRegistry.Map<SearchCriteria>(searchCriteria);
			return _mapperRegistry.Map<ProjectTemplateSearchResultSetDto>(_projectTemplateManager.Search(criteria));
		}
	}
}