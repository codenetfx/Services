using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Relay.Domain
{
    /// <summary>
    /// Internal Entity for returning <see cref="SearchResultSetDto"/> objects.
    /// </summary>
    public class RelaySearchResultSet<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelaySearchResultSet{T}"/> class.
        /// </summary>
        public RelaySearchResultSet()
        {
            Results = new List<T>();
            RefinerResults = new Dictionary<string, List<RefinementItemDto>>();
        }
        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public List<T> Results { get; set; }

        /// <summary>
        /// Gets or sets the refiner results.
        /// </summary>
        /// <value>
        /// The refiner results.
        /// </value>
        public Dictionary<string, List<RefinementItemDto>> RefinerResults { get; set; }

        		/// <summary>
		/// Gets or sets the total results.
		/// </summary>
		/// <value>
		/// The total results.
		/// </value>
		public SearchSummaryDto Summary { get; set; }
    }
}

