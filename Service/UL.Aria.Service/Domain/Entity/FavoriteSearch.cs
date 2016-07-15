using System;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Saved search domain entity.
    /// </summary>
    public class FavoriteSearch : TrackedDomainEntity
    {
        /// <summary>
        /// Gets or sets the saved search name.
        /// </summary>
        /// <value>
        /// The saved search name.
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
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        /// <value>
        /// The search criteria.
        /// </value>
        public SearchCriteria SearchCriteria { get; set; }

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
        ///   <c>true</c> if [available default]; otherwise, <c>false</c>.
        /// </value>
        public bool AvailableDefault { get; set; }
    }
}