using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Search results data transfer object class.
	/// </summary>
	[DataContract]
	public class SearchResultSetDto : IResourceResultSet<SearchResultDto>
	{
		/// <summary>
		/// Gets or sets the total results.
		/// </summary>
		/// <value>
		/// The total results.
		/// </value>
		[DataMember]
		public SearchSummaryDto Summary { get; set; }

		/// <summary>
		/// Gets or sets the results.
		/// </summary>
		/// <value>
		/// The results.
		/// </value>
		[DataMember]
		public virtual IList<SearchResultDto> Results { get; set; }

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