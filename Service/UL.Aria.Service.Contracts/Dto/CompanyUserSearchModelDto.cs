using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Model for a list of company users.
    /// </summary>
    [DataContract]
    public class CompanyUserSearchModelDto
    {
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        [DataMember]
        public IEnumerable<ProfileDto> Users { get; set; }

        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        /// <value>
        /// The search criteria.
        /// </value>
        [DataMember]
        public SearchCriteriaDto SearchCriteria { get; set; }

        /// <summary>
        /// Gets or sets the total results.
        /// </summary>
        /// <value>s
        /// The total results.
        /// </value>
        [DataMember]
        public SearchSummaryDto Summary { get; set; }

        /// <summary>
        /// Gets or sets the company id.
        /// </summary>
        /// <value>
        /// The company id.
        /// </value>
        [DataMember]
        public Guid CompanyId { get; set; }
    }
}
