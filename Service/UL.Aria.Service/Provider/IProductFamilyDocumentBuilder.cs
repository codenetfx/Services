using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Defines Operations for building documents based on a <see cref="ProductFamily"/>.
    /// </summary>
    public interface IProductFamilyDocumentBuilder
    {
        /// <summary>
        /// Builds the specified product family.
        /// </summary>
        /// <param name="productFamily">The product family.</param>
        /// <param name="creatingUser">The creating user.</param>
        /// <param name="baseCharacteristics">The base characteristics.</param>
        /// <param name="variableCharacteristics">The variable characteristics.</param>
        /// <param name="sourceAndDependentCharacteristics">The source and dependent characteristics.</param>
        /// <returns></returns>
        Stream Build(ProductFamily productFamily, ProfileBo creatingUser, IEnumerable<ProductFamilyCharacteristicDomainEntity> baseCharacteristics, IEnumerable<ProductFamilyCharacteristicDomainEntity> variableCharacteristics, IEnumerable<ProductFamilyFeatureAllowedValueDependencyMapping> sourceAndDependentCharacteristics);

        /// <summary>
        /// Builds the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="productFamily">The product family.</param>
        /// <param name="creatingUser">The creating user.</param>
        /// <param name="baseCharacteristics">The base characteristics.</param>
        /// <param name="variableCharacteristics">The variable characteristics.</param>
        /// <param name="sourceAndDependentCharacteristics">The source and dependent characteristics.</param>
        /// <returns></returns>
        Stream Build(Stream stream, ProductFamily productFamily, ProfileBo creatingUser, IEnumerable<ProductFamilyCharacteristicDomainEntity> baseCharacteristics, IEnumerable<ProductFamilyCharacteristicDomainEntity> variableCharacteristics, IEnumerable<ProductFamilyFeatureAllowedValueDependencyMapping> sourceAndDependentCharacteristics);
    }
}
