using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// User Team Provider implementation
    /// </summary>
    public class ProjectProjectTemplateProvider : SearchProviderBase<ProjectProjectTemplate>, IProjectProjectTemplateProvider
    {
     
        private readonly IProjectProjectTemplateRepository _projectProjectTemplateRepository;

		/// <summary>
        /// Initializes a new instance of the <see cref="ProjectProjectTemplateProvider" /> class.
		/// </summary>
        /// <param name="projectProjectTemplateRepository">The ProjectProjectTemplate repository.</param>
		/// <param name="principalResolver">The principal resolver.</param>
        
        public ProjectProjectTemplateProvider(IProjectProjectTemplateRepository projectProjectTemplateRepository, IPrincipalResolver principalResolver)
            : base(projectProjectTemplateRepository, principalResolver)
		{
            _projectProjectTemplateRepository = projectProjectTemplateRepository;
            
		}

	

		/// <summary>
		/// Updates the bulk.
		/// </summary>
        /// <param name="projectProjectTemplates">The project Project Template.</param>
		/// <param name="parentId">The parent identifier.</param>
        public void UpdateBulk(IEnumerable<ProjectProjectTemplate>projectProjectTemplates, Guid parentId)
		{            
		    //ParentId = parentId;
            _projectProjectTemplateRepository.Save(projectProjectTemplates, parentId);
		}

		/// <summary>
		/// Fetches the group.
		/// </summary>
		/// <param name="parentId">The parent identifier.</param>
        /// <returns>IEnumerable&lt;ProjectProjectTemplate&gt;.</returns>
        public IEnumerable<ProjectProjectTemplate> FetchGroup(Guid parentId)
		{
            return _projectProjectTemplateRepository.FetchGroup(parentId);
		}

       
    }
}
