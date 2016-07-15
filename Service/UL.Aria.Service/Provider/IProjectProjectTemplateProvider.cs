using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provides an interface for a User Team Provider.
    /// </summary>
    public interface IProjectProjectTemplateProvider : ISearchProviderBase<ProjectProjectTemplate>
    {

       
        /// <summary>
        /// Updates the bulk.
        /// </summary>
        /// <param name="projectProjectTemplates">The business units.</param>
        /// <param name="parentId">The parent identifier.</param>
        void UpdateBulk(IEnumerable<ProjectProjectTemplate> projectProjectTemplates, Guid parentId);

        /// <summary>
        /// Fetches the group.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>IEnumerable&lt;BusinessUnit&gt;.</returns>
        IEnumerable<ProjectProjectTemplate> FetchGroup(Guid parentId);


    }
}
