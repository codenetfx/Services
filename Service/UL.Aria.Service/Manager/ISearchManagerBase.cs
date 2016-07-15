using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines a generic search manager
    /// </summary>
    /// <typeparam name="TSearchResult">The type of the search result.</typeparam>
    public interface ISearchManagerBase<TSearchResult> where TSearchResult : class, ISearchResult, new()
    {
        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        ISearchResultSet<TSearchResult> Search(ISearchCriteria searchCriteria);
    }
}