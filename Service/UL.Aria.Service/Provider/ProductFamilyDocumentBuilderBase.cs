using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Cells;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Base implementation for classes which build templates for <see cref="ProductFamily"/>
    /// </summary>
    public abstract class ProductFamilyDocumentBuilderBase : DocumentBuilderBase, IProductFamilyDocumentBuilder
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
        public Stream Build(ProductFamily productFamily, ProfileBo creatingUser, IEnumerable<ProductFamilyCharacteristicDomainEntity> baseCharacteristics, IEnumerable<ProductFamilyCharacteristicDomainEntity> variableCharacteristics, IEnumerable<ProductFamilyFeatureAllowedValueDependencyMapping> sourceAndDependentCharacteristics)
        {
            var workbook = InitializeDocument();
            return BuildImpl(workbook, productFamily, creatingUser, baseCharacteristics, variableCharacteristics, sourceAndDependentCharacteristics);
        }

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
        public Stream Build(Stream stream, ProductFamily productFamily, ProfileBo creatingUser, IEnumerable<ProductFamilyCharacteristicDomainEntity> baseCharacteristics, IEnumerable<ProductFamilyCharacteristicDomainEntity> variableCharacteristics, IEnumerable<ProductFamilyFeatureAllowedValueDependencyMapping> sourceAndDependentCharacteristics)
        {
            var workbook = InitializeDocument(stream);
            return BuildImpl(workbook, productFamily, creatingUser, baseCharacteristics, variableCharacteristics, sourceAndDependentCharacteristics);
        }

        private Stream BuildImpl(Workbook workbook, ProductFamily productFamily, ProfileBo creatingUser, IEnumerable<ProductFamilyCharacteristicDomainEntity> baseCharacteristics, IEnumerable<ProductFamilyCharacteristicDomainEntity> variableCharacteristics, IEnumerable<ProductFamilyFeatureAllowedValueDependencyMapping> sourceAndDependentCharacteristics)
        {
            AddProductFamily(workbook, productFamily, creatingUser);
            AddBaseCharacteristics(workbook, baseCharacteristics, productFamily);
            AddOtherCharacteristics(workbook, variableCharacteristics, productFamily);
            AddDependencies(workbook, sourceAndDependentCharacteristics, productFamily);

            return FinalizeDocument(workbook);
        }
        
        /// <summary>
        ///     Adds the product family.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="productFamily">The product family.</param>
        /// <param name="creatingUser"></param>
        public abstract void AddProductFamily(Workbook workbook, ProductFamily productFamily, ProfileBo creatingUser);

        /// <summary>
        ///     Adds the base characteristics.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="characteristics"></param>
        /// <param name="familyToAddTo"></param>
        public abstract void AddBaseCharacteristics(Workbook workbook, IEnumerable<ProductFamilyCharacteristicDomainEntity> characteristics, ProductFamily familyToAddTo);

        /// <summary>
        ///     Adds the other character istics.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="characteristics"></param>
        /// <param name="familyToAddTo"></param>
        public abstract void AddOtherCharacteristics(Workbook workbook, IEnumerable<ProductFamilyCharacteristicDomainEntity> characteristics, ProductFamily familyToAddTo);

        /// <summary>
        ///     Adds the dependencies.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="dependencies"></param>
        /// <param name="familyToAddto"></param>
        public abstract void AddDependencies(Workbook workbook, IEnumerable<ProductFamilyFeatureAllowedValueDependencyMapping> dependencies, ProductFamily familyToAddto);
    }
}