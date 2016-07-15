using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class Category
    /// </summary>
    public class Category : TrackedDomainEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TrackedDomainEntity" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public Category(Guid? id) : base(id)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Category" /> class.
        /// </summary>
        public Category()
            : this(null)
        {
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
    }
}