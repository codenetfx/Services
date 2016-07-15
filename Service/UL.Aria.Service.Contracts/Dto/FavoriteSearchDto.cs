using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Search query data transfer object from persisting search criteria.
    /// </summary>
    [DataContract]
    public class FavoriteSearchDto
    {
        /// <summary>
        /// Gets or sets the saved search name.
        /// </summary>
        /// <value>
        /// The saved search name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [DataMember]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the search criteria dto.
        /// </summary>
        /// <value>
        /// The search criteria dto.
        /// </value>
        [DataMember]
        public SearchCriteriaDto SearchCriteria { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        [DataMember]
        public Guid UserId { get; set; }

        /// <summary>
        ///     Gets or sets the user it was created by.
        /// </summary>
        /// <value>
        ///     The created by.
        /// </value>
        [DataMember]
        public Guid CreatedById { get; set; }

        /// <summary>
        ///     Gets or sets the created on.
        /// </summary>
        /// <value>
        ///     The created on.
        /// </value>
        [DataMember]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the updated on.
        /// </summary>
        /// <value>
        ///     The updated on.
        /// </value>
        [DataMember]
        public DateTime UpdatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets who it was updated by.
        /// </summary>
        /// <value>
        ///     The updated by person.
        /// </value>
        [DataMember]
        public Guid UpdatedById { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [active default].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [active default]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool ActiveDefault { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [available default].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [available default]; otherwise, <c>false</c>.
        /// </value>       
        [DataMember]
        public bool AvailableDefault { get; set; }
    }
}