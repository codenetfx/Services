using System;
using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Company Business Object
    /// </summary>
    [Serializable]
    public sealed class Company : TrackedDomainEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Company" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public Company(Guid? id)
            : base(id)
        {
            ExternalIds = new List<string>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Company" /> class.
        /// </summary>
        public Company()
            : this(null)
        {
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the external ids.
        /// </summary>
        /// <value>The external ids.</value>
        public IList<string> ExternalIds { get; set; }
    }
}