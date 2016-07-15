using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// 
	/// </summary>
	[DataContract]
	public class TaskCategorySearchModelDto
	{

		/// <summary>
		/// Gets or sets the search results.
		/// </summary>
		/// <value>
		/// The results.
		/// </value>
		[DataMember]
		public IEnumerable<TaskCategoryDto> Results { get; set; }

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

	}
}
