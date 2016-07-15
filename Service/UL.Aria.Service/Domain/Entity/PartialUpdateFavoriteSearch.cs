using System;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Partial update favorite search class.
    /// </summary>
    /// <remarks>This class should only be consumed for partial updates.</remarks>
    public class PartialUpdateFavoriteSearch
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        /// <value>
        /// The search criteria.
        /// </value>
        public SearchCriteriaDto SearchCriteria { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [active default].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [active default]; otherwise, <c>false</c>.
        /// </value>
        public bool ActiveDefault { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [available default]. 
        /// </summary>
        /// <value>
        ///   <c>true</c> if [available default]; otherwise, <c>false</c>
        /// </value>
        public bool AvailableDefault { get; set; }
    }
}