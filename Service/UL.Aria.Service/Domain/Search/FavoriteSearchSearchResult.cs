using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Domain.Search
{
    /// <summary>
    /// Favorite search result class.
    /// </summary>
    public class FavoriteSearchSearchResult
    {
        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        /// <value>
        /// The search criteria.
        /// </value>
        public SearchCriteria Criteria { get; set; }

        /// <summary>
        /// Gets or sets the search summary.
        /// </summary>
        /// <value>
        /// The search summary.
        /// </value>
        public SearchSummary Summary { get; set; }

        /// <summary>
        /// Gets or sets the favorite searches.
        /// </summary>
        /// <value>
        /// The favorite searches.
        /// </value>
        public IEnumerable<FavoriteSearch> FavoriteSearches { get; set; }
    }
}