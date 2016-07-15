using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// <see cref="SearchResultDto"/> implementation that is customised to support <see cref="IncomingOrderDto"/> objects.
    /// </summary>
    [DataContract]
    public class IncomingOrderSearchResultSetDto : IResourceResultSet<IncomingOrderSearchResultDto>
    {
        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        [DataMember]
        public SearchSummaryDto Summary { get; set; }
        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        [DataMember]
		public IList<IncomingOrderSearchResultDto> Results { get; private set; }

		/// <summary>
		/// Gets or sets the refiner results.
		/// </summary>
		/// <value>
		/// The refiner results.
		/// </value>
		[DataMember]
		public Dictionary<string, List<RefinementItemDto>> RefinerResults { get; set; }

        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        /// <value>
        /// The search criteria.
        /// </value>
        [DataMember]
        public SearchCriteriaDto SearchCriteria { get; set; }
    }
}