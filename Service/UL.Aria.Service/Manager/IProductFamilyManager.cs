using System;
using System.Collections.Generic;
using System.IO;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Product Family Manager Interface definition
    /// </summary>
    public interface IProductFamilyManager
    {
        /// <summary>
        ///     Uploads the specified file to upload.
        /// </summary>
        /// <param name="fileToUpload">The file to upload.</param>
        /// <returns></returns>
        ProductFamilyUploadResult Upload(Stream fileToUpload);

        /// <summary>
        ///     Creates the specified product family.
        /// </summary>
        /// <param name="productFamily">The product family.</param>
        /// <returns></returns>
        Guid Create(ProductFamily productFamily);

        /// <summary>
        ///     Updates the specified family.
        /// </summary>
        /// <param name="productFamily">The family.</param>
        void Update(ProductFamily productFamily);

        /// <summary>
        ///     Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        ProductFamily Get(Guid id);

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProductFamily> GetAll();

        /// <summary>
        ///     Gets the product families by business unit.
        /// </summary>
        /// <param name="businessUnitId">The expected GUID.</param>
        /// <returns>
        ///     Readonly dictionary of <see cref="ProductFamily" /> keyed by id.
        /// </returns>
        IReadOnlyDictionary<Guid, ProductFamily> GetProductFamiliesByBusinessUnit(Guid businessUnitId);

        /// <summary>
        ///     Gets the product family characteristics.
        /// </summary>
        /// <param name="productFamilyId">The expected id.</param>
        /// <returns></returns>
        IReadOnlyCollection<ProductFamilyCharacteristicDomainEntity> GetProductFamilyCharacteristics(
            Guid productFamilyId);

        /// <summary>
        ///     Saves the product family attribute associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <param name="productFamilyAttributeAssociationIds">The product family attribute association ids.</param>
        void SaveProductFamilyAttributeAssociations(Guid productFamilyId,
            IList<Guid> productFamilyAttributeAssociationIds);

        /// <summary>
        ///     Saves the product family feature associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <param name="productFamilyFeatureAssociationIds">The product family feature association ids.</param>
        void SaveProductFamilyFeatureAssociations(Guid productFamilyId, IList<Guid> productFamilyFeatureAssociationIds);

        /// <summary>
        ///     Gets the product family attribute associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyAttributeAssociation> GetProductFamilyAttributeAssociations(Guid productFamilyId);

        /// <summary>
        ///     Gets the product family Feature associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeatureAssociation> GetProductFamilyFeatureAssociations(Guid productFamilyId);

        /// <summary>
        ///     Gets the value dependencies by family allowed feature id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeatureAllowedValueDependency> GetValueDependenciesByFamilyAllowedFeatureId(Guid id);

        /// <summary>
        ///     Gets the values.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <param name="familyId"></param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeatureAllowedValue> GetAllowedValues(Guid featureId, Guid familyId);

        /// <summary>
        ///     Gets the allowed values by family.
        /// </summary>
        /// <param name="familyId">The family id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeatureAllowedValue> GetAllowedValuesByFamily(Guid familyId);

        /// <summary>
        ///     Gets the dependencies.
        /// </summary>
        /// <param name="familiyId">The familiy id.</param>
        /// <param name="featureAssociations">The feature associations.</param>
        /// <returns>List{ProductFamilyFeatureAllowedValueDependencyMapping}.</returns>
        List<ProductFamilyFeatureAllowedValueDependencyMapping> GetDependencies(Guid familiyId,
            IEnumerable<ProductFamilyFeatureAssociation> featureAssociations);

        /// <summary>
        ///     Get the detail.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ProductFamilyDetail.</returns>
        ProductFamilyDetail GetDetail(Guid id);
    }
}