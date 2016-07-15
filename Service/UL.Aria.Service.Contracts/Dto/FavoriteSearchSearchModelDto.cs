using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Favorite search model used for searching favorite searches
    /// </summary>
    [DataContract]
    public class FavoriteSearchSearchModelDto
    {
        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        /// <value>
        /// The search criteria.
        /// </value>
        [DataMember]
        public SearchCriteriaDto Criteria { get; set; }

        /// <summary>
        /// Gets or sets the total results.
        /// </summary>
        /// <value>s
        /// The total results.
        /// </value>
        [DataMember]
        public SearchSummaryDto Summary { get; set; }

        /// <summary>
        /// Gets or sets the favorite searches.
        /// </summary>
        /// <value>
        /// The favorite searches.
        /// </value>
        [DataMember]
        public IEnumerable<FavoriteSearchDto> FavoriteSearches { get; set; }
    }
}