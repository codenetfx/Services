using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class OrderServiceLineDetailSearchResultDto.
    /// </summary>
    [DataContract]
    public class OrderServiceLineDetailSearchResultDto : SearchResultDto
    {
        /// <summary>
        ///     Gets or sets the order service line detail.
        /// </summary>
        /// <value>The order service line detail.</value>
        [DataMember]
        public OrderServiceLineDetailDto OrderServiceLineDetail { get; set; }
    }
}