using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class ProductFamilyDetail
    /// </summary>
    public class ProductFamilyDetail
    {
        /// <summary>
        ///     Gets or sets the product family.
        /// </summary>
        /// <value>The product family.</value>
        public ProductFamily ProductFamily { get; set; }

        /// <summary>
        ///     Gets or sets the characteristics.
        /// </summary>
        /// <value>The characteristics.</value>
        public IList<ProductFamilyCharacteristicAssociationModel> Characteristics { get; set; }

        /// <summary>
        ///     Gets or sets the dependency mappings.
        /// </summary>
        /// <value>The dependency mappings.</value>
        public IList<ProductFamilyFeatureDependency> Dependencies { get; set; }
    }
}