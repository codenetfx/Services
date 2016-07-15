using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Domain.Search
{
    /// <summary>
    /// A search result for a <see cref="Company"/>
    /// </summary>
    public class CompanySearchResult : SearchResult
    {
        /// <summary>
        /// Gets or sets the <see cref="Company"/>.
        /// </summary>
        /// <value>
        /// The company.
        /// </value>
        public Company Company { get; set; }
    }
}