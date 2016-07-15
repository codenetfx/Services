using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Domain.Search
{
    /// <summary>
    /// <see cref="SearchResult"/> implementation specifically for <see cref="IncomingOrder"/> objects.
    /// </summary>
    public class IncomingOrderSearchResult : SearchResult
    {
        /// <summary>
        /// Gets or sets the <see cref="IncomingOrder"/>.
        /// </summary>
        /// <value>
        /// The incoming order.
        /// </value>
        public Entity.IncomingOrder IncomingOrder { get; set; }
    }
}