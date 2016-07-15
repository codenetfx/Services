using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// ProfileSearchSpecification DTO
    /// </summary>
    [DataContract]
    public class ProfileSearchSpecificationDto
	{
        /// <summary>
        /// Gets or sets the start index.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        [DataMember]
        public long StartIndex { get; set; }

        /// <summary>
        /// Gets or sets the end index.
        /// </summary>
        /// <value>
        /// The end index.
        /// </value>
        [DataMember]        
        public long EndIndex { get; set; }

        /// <summary>
        /// Gets or sets the keyword.
        /// </summary>
        /// <value>
        /// The keyword.
        /// </value>
        [DataMember]
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets the sort by.
        /// </summary>
        /// <value>
        /// The sort by.
        /// </value>
        [DataMember]
        public string SortBy { get; set; }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        /// <value>
        /// The sort direction.
        /// </value>
        [DataMember]
        public SortDirectionDto SortDirection { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        [DataMember]
        public Guid? ContainerId { get; set; }

        /// <summary>
        /// Gets or sets the company id.
        /// </summary>
        /// <value>
        /// The company id.
        /// </value>
        [DataMember]
        public Guid? CompanyId { get; set; }
    }
}