using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Domain.Search
{
    /// <summary>
    /// Class ProductUploadResultSearchResult
    /// </summary>
    public class ProductUploadResultSearchResult : SearchResult
    {
        /// <summary>
        /// Gets or sets the product upload result.
        /// </summary>
        /// <value>The product upload result.</value>
        public ProductUploadResult ProductUploadResult { get; set; }
    }
}