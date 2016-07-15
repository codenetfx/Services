using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     <see cref="SearchResultDto" /> implementation that is customised to support
    ///     <see
    ///         cref="ProductUploadSearchResultDto" />
    ///     objects.
    /// </summary>
    [DataContract]
    public class ProductUploadSearchResultDto : SearchResultDto
    {
        /// <summary>
        ///     Gets or sets the <see cref="ProductUploadDto" />.
        /// </summary>
        /// <value>
        ///     The incoming order.
        /// </value>
        [DataMember]
        public ProductUploadDto ProductUpload { get; set; }
    }
}