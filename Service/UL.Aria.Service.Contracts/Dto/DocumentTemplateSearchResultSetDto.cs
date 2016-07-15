using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Class DocumentTemplateSearchResultSetDto.
	/// </summary>
	[DataContract]
	public class DocumentTemplateSearchResultSetDto
	{
		/// <summary>
		/// Gets or sets the summary.
		/// </summary>
		/// <value>The summary.</value>
		[DataMember]
		public SearchSummaryDto Summary { get; set; }

		/// <summary>
		/// Gets or sets the results.
		/// </summary>
		/// <value>The results.</value>
		[DataMember]
		public IList<DocumentTemplateSearchResultDto> Results { get; set; }

		/// <summary>
		/// Gets or sets the refiner results.
		/// </summary>
		/// <value>The refiner results.</value>
		[DataMember]
		public Dictionary<string, List<RefinementItemDto>> RefinerResults { get; set; }

		/// <summary>
		/// Gets or sets the search criteria.
		/// </summary>
		/// <value>The search criteria.</value>
		[DataMember]
		public SearchCriteriaDto SearchCriteria { get; set; }
	}
}