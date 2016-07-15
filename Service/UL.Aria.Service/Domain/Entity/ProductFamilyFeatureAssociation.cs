using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Product Family Feature Associatio domain entity
    /// </summary>
    public sealed class ProductFamilyFeatureAssociation :ProductFamilyCharacteristicAssociation
    {
        /// <summary>
        /// Gets or sets the dependent.
        /// </summary>
        /// <value>
        /// The dependent.
        /// </value>
        public string Dependent { get; set; }

        /// <summary>
        /// Gets or sets the parent family allowed feature id.
        /// </summary>
        /// <value>
        /// The parent family allowed feature id.
        /// </value>
        public Guid ParentFamilyAllowedFeatureId { get; set; }
    }
}