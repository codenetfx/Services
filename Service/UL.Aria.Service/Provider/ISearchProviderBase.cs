using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Defines a generic search Provider
    /// </summary>
    /// <typeparam name="TSearchResult">The type of the search result.</typeparam>
    public interface ISearchProviderBase<TSearchResult> : IProviderBase<TSearchResult>
        where TSearchResult : AuditableEntity, IAuditableEntity, ISearchResult, new()
    {
        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        ISearchResultSet<TSearchResult> Search(ISearchCriteria searchCriteria);
    }
}