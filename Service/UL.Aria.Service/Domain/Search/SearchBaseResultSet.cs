using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Domain.Search
{
	/// <summary>
	/// Class SearchBaseResultSet.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SearchBaseResultSet<T> where T : TrackedDomainEntity
	{
		/// <summary>
		/// Gets or sets the search results.
		/// </summary>
		/// <value>
		/// The results.
		/// </value>
		public IEnumerable<T> Results { get; set; }

		/// <summary>
		/// Gets or sets the search criteria.
		/// </summary>
		/// <value>
		/// The search criteria.
		/// </value>
		public SearchCriteria SearchCriteria { get; set; }

		/// <summary>
		/// Gets or sets the total results.
		/// </summary>
		/// <value>s
		/// The total results.
		/// </value>
		public SearchSummary Summary { get; set; }
	}
}
