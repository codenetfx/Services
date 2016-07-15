using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provides an interface for a Link Provider.
    /// </summary>
    public interface ILinkProvider
    {
        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Link FetchById(Guid id);

        /// <summary>
        /// Fetches all active links associated with the specified entityId.
        /// </summary>
        /// <param name="entityId">The owner entity identifier.</param>
        /// <returns></returns>
        IEnumerable<Link> FetchLinksByEntity(Guid entityId);

        /// <summary>
        /// Deletes a link with the specified linkId.
        /// </summary>
        /// <param name="linkId">The link identifier.</param>      
        void Delete(Guid linkId);

        /// <summary>
        /// Creates the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns></returns>
        Link Create(Link link);

        /// <summary>
        /// Updates the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        void Update(Link link);

        /// <summary>
        /// Updates the link associations.
        /// </summary>
        /// <param name="links">The links.</param>
        /// <param name="parentId">The parent identifier.</param>
	    void UpdateLinkAssociations(IEnumerable<Link> links, Guid parentId);

        /// <summary>
        /// Searches the specified search criteria dto.
        /// </summary>
        /// <param name="searchCriteriaDto">The search criteria dto.</param>
        /// <returns>Link search result set.</returns>
        ISearchResultSet<Link> Search(SearchCriteria searchCriteriaDto);

        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <returns></returns>     
        IEnumerable<Lookup> GetLookups();
    }
}
