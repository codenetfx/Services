using UL.Enterprise.Foundation.Data;
namespace UL.Aria.Service.Domain.Search
{
    /// <summary>
    ///     Provides summary counts and timings for a search.
    /// </summary>
    public class SearchSummary:ISearchSummary
    {
        /// <summary>
        ///     Gets or sets the index of the first result
        /// </summary>
        /// <value>
        ///     The start index.
        /// </value>
        public long StartIndex { get; set; }

        /// <summary>
        ///     Gets or sets the index of the last result returned
        /// </summary>
        /// <value>
        ///     The end index.
        /// </value>
        public long EndIndex { get; set; }

        /// <summary>
        ///     Gets or sets the total number of results
        /// </summary>
        /// <value>
        ///     The total results.
        /// </value>
        public long TotalResults { get; set; }

        /// <summary>
        ///     Gets or sets the last command.
        /// </summary>
        /// <value>The last command.</value>
        public string LastCommand { get; set; }
    }
}