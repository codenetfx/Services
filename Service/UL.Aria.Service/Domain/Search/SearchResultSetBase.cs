using System.Collections.Generic;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Search
{
    /// <summary>
    /// Base class for search result sets.
    /// </summary>
    public class SearchResultSetBase<T>:ISearchResultSet<T>        
        where T:class, ISearchResult, new()
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="SearchResultSetBase{T}"/> class.
		/// </summary>
	    public SearchResultSetBase()
	    {
			Summary = new SearchSummary();
			Results = new List<T>();
			SearchCriteria = new SearchCriteria();
			RefinerResults = new Dictionary<string, List<IRefinementItem>>();
	    }

	    /// <summary>
        /// Gets or sets the summary for this result set
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        public ISearchSummary Summary { get; set; }

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public IList<T> Results { get; set; }

        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        /// <value>
        /// The search criteria.
        /// </value>
        public ISearchCriteria SearchCriteria { get; set; }

        /// <summary>
        ///     Gets or sets the refiner results.
        /// </summary>
        /// <value>The refiner results.</value>
        public Dictionary<string, List<IRefinementItem>> RefinerResults { get; set; }

      

    }
}