using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Domain.Search
{
	/// <summary>
	/// 
	/// </summary>
	public class TaskCategorySearchResultSet 
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
		public IEnumerable<TaskCategory> Results { get; set; }

	}
}
