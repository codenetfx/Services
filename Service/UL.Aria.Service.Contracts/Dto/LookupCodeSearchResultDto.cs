using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Search result for <see cref="IndustryCodeDto"/> objects
    /// </summary>
    [DataContract]
    public class LookupCodeSearchResultDto : SearchResultDto
    {

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [DataMember]
        public string Label { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        [DataMember]
        public string ExternalId { get; set; }
    }
}