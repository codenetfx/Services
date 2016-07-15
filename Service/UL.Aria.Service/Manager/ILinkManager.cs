using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILinkManager
    {
        /// <summary>
        /// Fetches the link matching the specified id.
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
        /// Searches the specified search criteria dto.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        ISearchResultSet<Link> Search(SearchCriteria searchCriteria);

        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <returns></returns>     
        IEnumerable<Lookup> GetLookups();
    }
}
