using System.Collections.Generic;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class SharePointQueryResult.
	/// </summary>
	public class SharePointQueryResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SharePointQueryResult"/> class.
		/// </summary>
		public SharePointQueryResult()
		{
			SearchResults = new List<IDictionary<string, string>>();
			RefinerResults = new Dictionary<string, List<IRefinementItem>>();
			TotalRows = 0;
		}

		/// <summary>
		///     Gets or sets the get URI.
		/// </summary>
		/// <value>The get URI.</value>
		public string GetUri { get; set; }

		/// <summary>
		///     Gets or sets the json request.
		/// </summary>
		/// <value>The json request.</value>
		public string JsonRequest { get; set; }

		/// <summary>
		///     Gets or sets the json result.
		/// </summary>
		/// <value>The json result.</value>
		public string JsonResult { get; set; }

		/// <summary>
		///     Total rows in query but not total rows returned which is less based on the rowlimit sent in but this is needed to
		///     insure paging knows how many possuble rows are in the query.
		/// </summary>
		public long TotalRows { get; set; }

		/// <summary>
		///     Gets or sets the search results.
		/// </summary>
		/// <value>The search results.</value>
		public IList<IDictionary<string, string>> SearchResults { get; set; }

		/// <summary>
		///     Gets or sets the refiner results.
		/// </summary>
		/// <value>The refiner results.</value>
		public Dictionary<string, List<IRefinementItem>> RefinerResults { get; set; }
	}
}