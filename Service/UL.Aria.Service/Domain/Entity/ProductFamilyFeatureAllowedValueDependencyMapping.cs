using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Describes a mapping of dependencies between <see cref="ProductFamilyFeatureAllowedValue" /> objects within a family.
    /// </summary>
    public class ProductFamilyFeatureAllowedValueDependencyMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyFeatureAllowedValueDependencyMapping"/> class.
        /// </summary>
        public ProductFamilyFeatureAllowedValueDependencyMapping()
        {
            ParentValues = new List<ProductFamilyFeatureAllowedValue>();
            ChildValues = new List<ProductFamilyFeatureAllowedValue>();
        }

        /// <summary>
        /// Gets or sets the parent assocation.
        /// </summary>
        /// <value>
        /// The parent assocation.
        /// </value>
        public ProductFamilyFeatureAssociation ParentAssocation { get; set; }

        /// <summary>
        /// Gets or sets the parent values.
        /// </summary>
        /// <value>
        /// The parent values.
        /// </value>
        public IList<ProductFamilyFeatureAllowedValue> ParentValues { get; set; }

        /// <summary>
        /// Gets or sets the child assocation.
        /// </summary>
        /// <value>
        /// The parent assocation.
        /// </value>
        public ProductFamilyFeatureAssociation ChildAssocation { get; set; }

        /// <summary>
        /// Gets or sets the child values.
        /// </summary>
        /// <value>
        /// The parent values.
        /// </value>
        public IList<ProductFamilyFeatureAllowedValue> ChildValues { get; set; }
    }
}