using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Domain.Search
{
    /// <summary>
    ///     Class OrderServiceLineDetailSearchResult.
    /// </summary>
    public class OrderServiceLineDetailSearchResult : SearchResult
    {
        /// <summary>
        ///     Gets or sets the order service line detail.
        /// </summary>
        /// <value>The order service line detail.</value>
        public OrderServiceLineDetail OrderServiceLineDetail { get; set; }
    }
}