using System;
using System.Collections.Generic;
using System.Threading;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;
using System.Linq;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Domain.Lookup;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Class projectTemplateManager
    /// </summary>
    public sealed class ProjectTemplateManager : IProjectTemplateManager
    {
        private readonly IProjectTemplateProvider _projectTemplateProvider;
        private readonly IPrincipalResolver _principalResolver;
        private readonly ITaskTemplateProvider _taskTemplateProvider;
        private readonly ILookupProvider _businessUnitProvider;
        private readonly ITransactionFactory _transactionFactory;
        private readonly IMapperRegistry _mapperRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTemplateManager" /> class.
        /// </summary>
        /// <param name="projectTemplateProvider">The project template provider.</param>
        /// <param name="taskTemplateProvider">The task template provider.</param>
        /// <param name="businessUnitProvider">The business unit provider.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public ProjectTemplateManager(IProjectTemplateProvider projectTemplateProvider, ITaskTemplateProvider taskTemplateProvider,
            ILookupProvider businessUnitProvider, IPrincipalResolver principalResolver, ITransactionFactory transactionFactory, IMapperRegistry mapperRegistry)
        {
            _projectTemplateProvider = projectTemplateProvider;
            _businessUnitProvider = businessUnitProvider;
            _principalResolver = principalResolver;
            _taskTemplateProvider = taskTemplateProvider;
            _transactionFactory = transactionFactory;
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        ///     Deletes the specified project template id.
        /// </summary>
        /// <param name="projectTemplateId">The project template id.</param>
        public void Delete(Guid projectTemplateId)
        {
            _projectTemplateProvider.Delete(projectTemplateId);
        }

        /// <summary>
        ///     Creates the specified project template.
        /// </summary>
        /// <param name="projectTemplate">The project template.</param>
        /// <returns>Guid.</returns>
        public Guid Create(ProjectTemplate projectTemplate)
        {
            Guid? projectTemplateId = null;
            using (var transaction = _transactionFactory.Create())
            {
                projectTemplateId = CreateNew(projectTemplate);
                transaction.Complete();
            }
            return projectTemplateId.GetValueOrDefault();
        }


        private Guid CreateNew(ProjectTemplate projectTemplate)
        {
            Guid? projectTemplateId = null;
            if (projectTemplate.CorrelationId == Guid.Empty)
                projectTemplate.CorrelationId = Guid.NewGuid();

            projectTemplateId = _projectTemplateProvider.Create(projectTemplate);
            if (Guid.Empty != projectTemplateId.GetValueOrDefault())
            {
                _taskTemplateProvider.UpdateBulk(projectTemplateId.GetValueOrDefault(), projectTemplate.TaskTemplates);
                _businessUnitProvider.UpdateBulk(projectTemplate.BusinessUnits, projectTemplateId.GetValueOrDefault());
            }

            return projectTemplateId.GetValueOrDefault();
        }


        /// <summary>
        /// Fetches the project templates.
        /// </summary>
        /// <returns>List{ProjectTemplate}</returns>
        public IList<ProjectTemplate> FindAll()
        {
            return _projectTemplateProvider.GetAll();
        }

        /// <summary>
        ///     Finds the by id.
        /// </summary>
        /// <param name="projectTemplateId">The project template id.</param>
        /// <returns>ProjectTemplate.</returns>
        public ProjectTemplate FindById(Guid projectTemplateId)
        {
            var projectTemplate = _projectTemplateProvider.GetById(projectTemplateId);
            projectTemplate.BusinessUnits = _businessUnitProvider.FetchBusinessUnitByEntity(projectTemplate.Id.GetValueOrDefault()).ToList();
            if (!projectTemplate.TaskTemplates.Any())
                projectTemplate.TaskTemplates = _taskTemplateProvider.FetchTaskTemplateByProjectTemplate(projectTemplate.Id.GetValueOrDefault()).ToList();
            return projectTemplate;
        }

        /// <summary>
        ///     Finds editable version by id.
        /// </summary>
        /// <param name="projectTemplateId">The project template id.</param>
        /// <returns>ProjectTemplate.</returns>
        public ProjectTemplate FindEditableById(Guid projectTemplateId)
        {
            var projectTemplate = _projectTemplateProvider.GetById(projectTemplateId);
            // If in Publish state, then copy published template to new one in draft status, keeping correlationId same
            if (projectTemplate != null && !projectTemplate.IsDraft)
            {
                var correlatedProjectTemplates = _projectTemplateProvider.GetAllByCorrelationId(projectTemplate.CorrelationId);
                var draftTemplate = correlatedProjectTemplates.FirstOrDefault(x => x.IsDraft);
                if (draftTemplate != null)
                    projectTemplate = draftTemplate;
            }

            projectTemplate.BusinessUnits = _businessUnitProvider.FetchBusinessUnitByEntity(projectTemplate.Id.GetValueOrDefault()).ToList();
            if (!projectTemplate.TaskTemplates.Any())
                projectTemplate.TaskTemplates = _taskTemplateProvider.FetchTaskTemplateByProjectTemplate(projectTemplate.Id.GetValueOrDefault()).ToList();

            return projectTemplate;
        }

        /// <summary>
        ///     Updates the specified project template.
        /// </summary>
        /// <param name="projectTemplate">The project template.</param>
        public void Update(ProjectTemplate projectTemplate)
        {
            using (var transaction = _transactionFactory.Create())
            {
                var correlatedProjectTemplates = _projectTemplateProvider.GetAllByCorrelationId(projectTemplate.CorrelationId).ToList();
                
                // If Save as Publish state, then delete existing published template
                if (projectTemplate != null && !projectTemplate.IsDraft)
                {
                   
                    if (correlatedProjectTemplates != null)
                    {
                        var existingPublishedTemplate = correlatedProjectTemplates.FirstOrDefault(x => !x.IsDraft);
                        if (existingPublishedTemplate != null && existingPublishedTemplate.Id != null)
                        {
                            Delete((Guid)existingPublishedTemplate.Id);
                        }
                    }
                    projectTemplate.Version = projectTemplate.Version + 1;
                    
                  
                }
				projectTemplate = GetProjectTemplateToUpdate(projectTemplate, correlatedProjectTemplates);

                if(projectTemplate.Id.GetValueOrDefault() != Guid.Empty 
                    && correlatedProjectTemplates.Exists(x=> x.Id == projectTemplate.Id))
                {
                    _projectTemplateProvider.Update(projectTemplate);
                }
                else
                {
                        _projectTemplateProvider.Create(projectTemplate);
                }             
             
            _taskTemplateProvider.UpdateBulk(projectTemplate.Id.GetValueOrDefault(), projectTemplate.TaskTemplates);
            _businessUnitProvider.UpdateBulk(projectTemplate.BusinessUnits, projectTemplate.Id.GetValueOrDefault());
            transaction.Complete();

            }
        }


        private ProjectTemplate GetProjectTemplateToUpdate(ProjectTemplate projectTemplate, List<ProjectTemplate> correlatedProjectTemplates)
        {
            if (projectTemplate == null)
                return null;       
       
			if (correlatedProjectTemplates != null && correlatedProjectTemplates.Any())
            {
				
				var firstDraft = correlatedProjectTemplates.FirstOrDefault(x => x.IsDraft);

				if (firstDraft !=null)
				{
					_mapperRegistry.Map<ProjectTemplate, ProjectTemplate>(projectTemplate, firstDraft);
					return firstDraft;
                }
                else
                {
                    var temp = new ProjectTemplate(Guid.NewGuid());                    
                    _mapperRegistry.Map<ProjectTemplate, ProjectTemplate>(projectTemplate, temp);
                    temp.CorrelationId = projectTemplate.CorrelationId;
					return temp;
                }   
           
            }         

            return projectTemplate;           
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>ProjectTemplateSearchResultSet.</returns>
        public SearchResultSetBase<ProjectTemplate> Search(SearchCriteria searchCriteria)
        {
            return _projectTemplateProvider.Search(searchCriteria);
        }
    }
}