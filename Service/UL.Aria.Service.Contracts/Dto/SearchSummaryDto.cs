using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Provides summary counts and timings for a search.
    /// </summary>
    [DataContract]
    public class SearchSummaryDto
    {
        /// <summary>
        ///     Gets or sets the index of the first result
        /// </summary>
        /// <value>
        ///     The start index.
        /// </value>
        [DataMember]
        public long StartIndex { get; set; }

        /// <summary>
        ///     Gets or sets the index of the last result returned
        /// </summary>
        /// <value>
        ///     The end index.
        /// </value>
        [DataMember]
        public long EndIndex { get; set; }

        /// <summary>
        ///     Gets or sets the total number of results
        /// </summary>
        /// <value>
        ///     The total results.
        /// </value>
        [DataMember]
        public long TotalResults { get; set; }

        /// <summary>
        ///     Gets or sets the last command.
        /// </summary>
        /// <value>The last command.</value>
        [DataMember]
        public string LastCommand { get; set; }
    }
}