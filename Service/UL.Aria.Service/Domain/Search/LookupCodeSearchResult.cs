using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Domain.Search
{
    /// <summary>
    /// Search result for <see cref="LookupCodeBase"/> entities. 
    /// </summary>
    /// <typeparam name="TLookupCode">The type of the lookup code.</typeparam>
    public class LookupCodeSearchResult<TLookupCode> : SearchResult
        where TLookupCode : LookupCodeBase
    {

        /// <summary>
        /// Gets or sets the lookup code.
        /// </summary>
        /// <value>
        /// The industry code.
        /// </value>
        public TLookupCode LookupCode { get; set; }
    }
}