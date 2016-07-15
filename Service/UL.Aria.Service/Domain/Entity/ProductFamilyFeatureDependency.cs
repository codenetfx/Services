using System;
using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Dependency for product family features.
    /// </summary>
    public class ProductFamilyFeatureDependency
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyFeatureDependency"/> class.
        /// </summary>
        public ProductFamilyFeatureDependency()
        {
            ChildValueIds = new List<Guid>();
            ParentValueIds = new List<Guid>();
        }

        /// <summary>
        ///     Gets or sets the parent.
        /// </summary>
        /// <value>
        ///     The parent.
        /// </value>
        public Guid ParentId { get; set; }

        /// <summary>
        ///     Gets or sets the parent value ids.
        /// </summary>
        /// <value>The parent value ids.</value>
        public IList<Guid> ParentValueIds { get; set; }

        /// <summary>
        ///     Gets or sets the child.
        /// </summary>
        /// <value>
        ///     The child.
        /// </value>
        public Guid ChildId { get; set; }

        /// <summary>
        ///     Gets or sets the child value ids.
        /// </summary>
        /// <value>The child value ids.</value>
        public IList<Guid> ChildValueIds { get; set; }
    }
}