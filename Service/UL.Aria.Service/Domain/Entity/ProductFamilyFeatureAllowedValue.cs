using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Describes values allwoed for a feature.
    /// </summary>
    public class ProductFamilyFeatureAllowedValue: TrackedDomainEntity
    {
        /// <summary>
        /// Gets or sets the family.
        /// </summary>
        /// <value>
        /// The family id.
        /// </value>
        public Guid FamilyId { get; set; }
        /// <summary>
        /// Gets or sets the feature value.
        /// </summary>
        /// <value>
        /// The feature value id.
        /// </value>
        public ProductFamilyFeatureValue FeatureValue { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is disabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisabled { get; set; }
    }
}