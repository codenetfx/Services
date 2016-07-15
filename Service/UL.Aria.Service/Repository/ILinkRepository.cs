using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Provides a Repository interface for Links.
    /// </summary>
    public interface ILinkRepository : IRepositoryBase<Link>
    {
      
        /// <summary>
        /// Fetches the links by entity.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns></returns>
        IEnumerable<Link> FetchLinksByEntity(Guid entityId);
         
        /// <summary>
        /// Searches the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        ISearchResultSet<Link> Search(SearchCriteria criteria);

        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Lookup> GetLookups();

    }
}
