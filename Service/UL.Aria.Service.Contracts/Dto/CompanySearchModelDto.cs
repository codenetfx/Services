using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class CompanySearchModelDto
    {
        /// <summary>
        /// Gets or sets the search results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public IEnumerable<CompanyDto> Results { get; set; }

        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        /// <value>
        /// The search criteria.
        /// </value>
        public SearchCriteriaDto SearchCriteria { get; set; }

        /// <summary>
        /// Gets or sets the total results.
        /// </summary>
        /// <value>s
        /// The total results.
        /// </value>
        public SearchSummaryDto Summary { get; set; }
    }
}
