using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Class SearchBaseDto.
    /// </summary>
    [DataContract]
    public class SearchBaseDto<T> where T : TrackedEntityDto
    {
        /// <summary>
        /// Gets or sets the search results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        [DataMember]
        public IEnumerable<T> Results { get; set; }

        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        /// <value>
        /// The search criteria.
        /// </value>
        [DataMember]
        public SearchCriteriaDto SearchCriteria { get; set; }

        /// <summary>
        /// Gets or sets the total results.
        /// </summary>
        /// <value>s
        /// The total results.
        /// </value>
        [DataMember]
        public SearchSummaryDto Summary { get; set; }
    }
}
