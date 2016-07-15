using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto.Integration
{
    /// <summary>
    ///     Contract for refiners
    /// </summary>
    [DataContract(Namespace = "http://portal.ul.com")]
    public class FulfillmentProjectRefinerResult
    {
        /// <summary>
        ///     Gets or sets the count.
        /// </summary>
        /// <value>
        ///     The count.
        /// </value>
        [DataMember]
        public long Count { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        [DataMember]
        public string Value { get; set; }
    }
}