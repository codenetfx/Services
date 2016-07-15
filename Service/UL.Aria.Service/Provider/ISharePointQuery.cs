using System.Collections.Generic;

using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     SharePoint Query class
    /// </summary>
    public interface ISharePointQuery
    {
		/// <summary>
		/// Runs the SharePoint web request.
		/// </summary>
		/// <param name="query">The SharePoint query parameter.</param>
		/// <param name="selectProperties">The SharePoint select properties parameter.</param>
		/// <param name="refiners">The refiners.</param>
		/// <param name="refinementFilters">The refinement filters.</param>
		/// <param name="startindex">The SharePoint start index parameter.</param>
		/// <param name="rowlimit">The SharePoint row limit parameter.</param>
		/// <param name="sortList">The sort list.</param>
		/// <param name="additionalFilter">The additional filter.</param>
		/// <returns>
		/// SharePointQueryResult.
		/// </returns>
		SharePointQueryResult SubmitQuery(string query, IEnumerable<string> selectProperties, IList<string> refiners, Dictionary<string, List<string>> refinementFilters, long startindex, long rowlimit, List<ISort> sortList, string additionalFilter="");
    }
}