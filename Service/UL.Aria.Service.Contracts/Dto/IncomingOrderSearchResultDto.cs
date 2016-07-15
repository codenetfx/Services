using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// <see cref="SearchResultDto"/> implementation that is customized to support <see cref="IncomingOrderSearchResultDto"/> objects.
    /// </summary>
    [DataContract]
    public class IncomingOrderSearchResultDto:SearchResultDto
    {
        /// <summary>
        /// Gets or sets the <see cref="IncomingOrderDto"/>.
        /// </summary>
        /// <value>
        /// The incoming order.
        /// </value>
        [DataMember]
        public IncomingOrderDto IncomingOrder { get; set; }
    }
}