using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Class ProductUploadResultSearchResultDto
    /// </summary>
    [DataContract]
    public class ProductUploadResultSearchResultDto : SearchResultDto
    {
        /// <summary>
        /// Gets or sets the product upload result.
        /// </summary>
        /// <value>The product upload result.</value>
        [DataMember]
        public ProductUploadResultDto ProductUploadResult { get; set; }
    }
}