using System;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     A domain entity object with audit tracking
    /// </summary>
    [Serializable]
    public abstract class TrackedDomainEntity : DomainEntity, ITrackedDomainEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TrackedDomainEntity" /> class.
        /// </summary>
        protected TrackedDomainEntity()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TrackedDomainEntity" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        protected TrackedDomainEntity(Guid? id) : base(id)
        {
        }

        /// <summary>
        ///     Gets or sets the user it was created by.
        /// </summary>
        /// <value>
        ///     The created by.
        /// </value>
        public Guid CreatedById { get; set; }

        /// <summary>
        ///     Gets or sets the created on.
        /// </summary>
        /// <value>
        ///     The created on.
        /// </value>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the updated on.
        /// </summary>
        /// <value>
        ///     The updated on.
        /// </value>
        public DateTime UpdatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets who it was updated by.
        /// </summary>
        /// <value>
        ///     The updated by person.
        /// </value>
        public Guid UpdatedById { get; set; }
    }
}