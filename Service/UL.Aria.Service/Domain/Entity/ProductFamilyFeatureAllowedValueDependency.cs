using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Describes dependencies between <see cref="ProductFamilyFeatureAllowedValue" /> objects within a family.
    /// </summary>
    public class ProductFamilyFeatureAllowedValueDependency :TrackedDomainEntity
    {
        /// <summary>
        /// Gets or sets the parent product family feature allowed value.
        /// </summary>
        /// <value>
        /// The parent product family feature allowed value.
        /// </value>
        public Guid ParentProductFamilyFeatureAllowedValueId { get; set; }

        /// <summary>
        /// Gets or sets the child product family feature allowed value.
        /// </summary>
        /// <value>
        /// The child product family feature allowed value.
        /// </value>
        public Guid ChildProductFamilyFeatureAllowedValueId { get; set; }
    }
}