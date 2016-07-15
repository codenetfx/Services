using System;
using System.Collections.Generic;
using System.IO;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Defines operations for building documents based on a product.
    /// </summary>
    public interface  IProductDocumentBuilder
    {
        /// <summary>
        /// Builds the specified product family.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="productFamily">The product family.</param>
        /// <param name="creatingUser">The creating user.</param>
        /// <param name="baseCharacteristics">The base characteristics.</param>
        /// <param name="variableCharacteristics">The variable characteristics.</param>
        /// <param name="dependencyMapping"></param>
        /// <returns></returns>
        Stream Build(IEnumerable<Product> products, ProductFamily productFamily, ProfileBo creatingUser, IEnumerable<ProductFamilyCharacteristicDomainEntity> baseCharacteristics, IEnumerable<ProductFamilyCharacteristicDomainEntity> variableCharacteristics, IEnumerable<ProductFamilyFeatureAllowedValueDependencyMapping> dependencyMapping);
    }
}