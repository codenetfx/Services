using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Defines properites for domain entities with tracking.
    /// </summary>
    public interface ITrackedDomainEntity
    {
        /// <summary>
        ///     Gets or sets the user it was created by.
        /// </summary>
        /// <value>
        ///     The created by.
        /// </value>
        Guid CreatedById { get; set; }

        /// <summary>
        ///     Gets or sets the created on.
        /// </summary>
        /// <value>
        ///     The created on.
        /// </value>
        DateTime CreatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the updated on.
        /// </summary>
        /// <value>
        ///     The updated on.
        /// </value>
        DateTime UpdatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets who it was updated by.
        /// </summary>
        /// <value>
        ///     The updated by person.
        /// </value>
        Guid UpdatedById { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid? Id { get; set; }
    }
}