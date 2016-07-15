using System;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// ProfileSearchSpecification DTO
    /// </summary>
    public class ProfileSearchSpecification
	{
		/// <summary>
		/// Gets or sets the start index.
		/// </summary>
		/// <value>
		/// The start index.
		/// </value>
		public long StartIndex { get; set; }

		/// <summary>
		/// Gets or sets the end index.
		/// </summary>
		/// <value>
		/// The end index.
		/// </value>
		public long EndIndex { get; set; }

        /// <summary>
        /// Gets or sets the keyword.
        /// </summary>
        /// <value>
        /// The keyword.
        /// </value>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets the sort by.
        /// </summary>
        /// <value>
        /// The sort by.
        /// </value>
        public string SortBy { get; set; }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        /// <value>
        /// The sort direction.
        /// </value>
        public SortDirectionDto SortDirection { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        public Guid? ContainerId { get; set; }

        /// <summary>
        /// Gets or sets the company id.
        /// </summary>
        /// <value>
        /// The company id.
        /// </value>
        public Guid? CompanyId { get; set; }
    }
}