using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Available values for a <see cref="ProductFamilyFeature"/>
    /// </summary>
    public class ProductFamilyFeatureValue:TrackedDomainEntity
    {
        /// <summary>
        /// Gets or sets the product family feature id.
        /// </summary>
        /// <value>
        /// The product family feature id.
        /// </value>
        public Guid FeatureId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the xtype.
        /// </summary>
        /// <value>
        /// The xtype.
        /// </value>
        public byte Xtype { get; set; }

        /// <summary>
        /// Gets or sets the max.
        /// </summary>
        /// <value>
        /// The max.
        /// </value>
        public string Maximum { get; set; }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public string Minimum { get; set; }

        /// <summary>
        /// Gets or sets the unit of measure.
        /// </summary>
        /// <value>
        /// The unit of measure.
        /// </value>
        public UnitOfMeasure UnitOfMeasure { get; set; }

    }
}