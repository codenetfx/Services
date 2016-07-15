using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// DTO for ContainerSearchSpecification
    /// </summary>
    [DataContract]
    public class SearchCriteriaDto
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
        public Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the company id.
        /// </summary>
        /// <value>
        /// The company id.
        /// </value>
        [DataMember]
        public Guid? CompanyId { get; set; }

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		[DataMember]
		public EntityTypeEnumDto? EntityType { get; set; }

        /// <summary>
        /// Gets or sets the refiners.
        /// </summary>
        /// <value>The refiners.</value>
        [DataMember]
        public IList<string> Refiners { get; set; }

        /// <summary>
        ///     Gets or sets the filters.
        /// </summary>
        /// <value>
        ///     The filters.
        /// </value>
        [DataMember]
        public Dictionary<string, List<string>> Filters { get; set; }

        /// <summary>
        ///     Gets or sets the sorts.
        /// </summary>
        /// <value>The sorts.</value>
        [DataMember]
        public List<SortDto> Sorts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [include deleted records].
        /// </summary>
        /// <value>
        /// <c>true</c> if [include deleted records]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeDeletedRecords { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [filter containers].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [filter containers]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool FilterContainers { get; set; }
    }
}
