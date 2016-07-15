using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Domain.Search
{
    /// <summary>
    ///     <see cref="SearchResult" /> implementation specifically for <see cref="ProductUpload" /> objects.
    /// </summary>
    public class ProductUploadSearchResult : SearchResult
    {
        /// <summary>
        ///     Gets or sets the <see cref="ProductUpload" />.
        /// </summary>
        /// <value>
        ///     The product upload.
        /// </value>
        public ProductUpload ProductUpload { get; set; }
    }
}