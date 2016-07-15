using System;
using System.Collections.Generic;

using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class Container
    /// </summary>
    public class Container : DomainEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Container" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public Container(Guid? id) : base(id)
        {
            AvailableClaims = new List<ContainerAvailableClaim>();
            ContainerLists = new List<ContainerList>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Container" /> class.
        /// </summary>
        public Container() : this(null)
        {
        }

        /// <summary>
        ///     Gets or sets the company id.
        /// </summary>
        /// <value>The company id.</value>
        public Guid? CompanyId { get; set; }

        /// <summary>
        ///     Gets or sets the primary search entity id.
        /// </summary>
        /// <value>The primary search entity id.</value>
        public Guid PrimarySearchEntityId { get; set; }

        /// <summary>
        ///     Gets or sets the type of the primary search entity.
        /// </summary>
        /// <value>The type of the primary search entity.</value>
        public string PrimarySearchEntityType { get; set; }

        /// <summary>
        ///     Gets or sets the available claims.
        /// </summary>
        /// <value>The available claims.</value>
        public IList<ContainerAvailableClaim> AvailableClaims { get; set; }

        /// <summary>
        ///     Gets or sets the permission groupings.
        /// </summary>
        /// <value>The permission groupings.</value>
        public IList<ContainerList> ContainerLists { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; set; }
    }
}