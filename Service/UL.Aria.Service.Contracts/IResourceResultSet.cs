using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts
{
	/// <summary>
	/// Limited metadata to support resource result paging
	/// </summary>
	public interface IResourceResultSet<T> where T:SearchResultDto
	{
		/// <summary>
		/// Gets or sets the total results.
		/// </summary>
		/// <value>
		/// The total results.
		/// </value>
		[DataMember]
		SearchSummaryDto Summary { get; set; }

		/// <summary>
		/// Gets or sets the results.
		/// </summary>
		/// <value>
		/// The results.
		/// </value>
		[DataMember]
		IList<T> Results { get; }

		/// <summary>
		/// Gets or sets the refiner results.
		/// </summary>
		/// <value>
		/// The refiner results.
		/// </value>
		[DataMember]
		Dictionary<string, List<RefinementItemDto>> RefinerResults { get; set; }

        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        /// <value>
        /// The search criteria.
        /// </value>
        [DataMember]
        SearchCriteriaDto SearchCriteria { get; set; }
	}
}
