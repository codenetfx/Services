using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Interface IProductFamilyProvider
    /// </summary>
    public interface IProductFamilyProvider
    {
        /// <summary>
        ///     Creates the specified product family.
        /// </summary>
        /// <param name="productFamily">The product family.</param>
        /// <returns>Guid.</returns>
        Guid Create(ProductFamily productFamily);

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="productFamily">The product family.</param>
        void Update(ProductFamily productFamily);

        /// <summary>
        ///     Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ProductFamily.</returns>
        ProductFamily Get(Guid id);

        /// <summary>
        ///     Gets the product families by business unit.
        /// </summary>
        /// <param name="businessUnitId">The business unit id.</param>
        /// <returns>IReadOnlyDictionary{GuidProductFamily}.</returns>
        IReadOnlyDictionary<Guid, ProductFamily> GetProductFamiliesByBusinessUnit(Guid businessUnitId);

        /// <summary>
        /// Saves the product family attribute associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <param name="models">The product family attribute association ids.</param>
        void SaveProductFamilyAttributeAssociations(Guid productFamilyId, IList<ProductFamilyAttributeAssociation> models);

        /// <summary>
        /// Saves the product family feature associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <param name="models">The product family feature association ids.</param>
        void SaveProductFamilyFeatureAssociations(Guid productFamilyId, IList<ProductFamilyFeatureAssociation> models);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProductFamily> GetAll();

        /// <summary>
        /// Gets the product family attribute associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyAttributeAssociation> GetProductFamilyAttributeAssociations(Guid productFamilyId);

        /// <summary>
        /// Gets the product family Feature associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeatureAssociation> GetProductFamilyFeatureAssociations(Guid productFamilyId);

        /// <summary>
        /// Removes the product family attribute associations.
        /// </summary>
        /// <param name="productFamilyAttributeAssociationIds">The product family attribute association ids.</param>
        void RemoveProductFamilyAttributeAssociations(IEnumerable<Guid> productFamilyAttributeAssociationIds);

        /// <summary>
        /// Saves the product family feature associations.
        /// </summary>
        /// <param name="productFamilyFeatureAssociationIds">The product family feature association ids.</param>
        void RemoveProductFamilyFeatureAssociations(IEnumerable<Guid> productFamilyFeatureAssociationIds);

        /// <summary>
        /// Gets the product family feature value by id and value.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <param name="value">The value.</param>
        /// <returns>ProductFamilyFeatureValue.</returns>
        ProductFamilyFeatureValue GetProductFamilyFeatureValueByFeatureIdAndValue(Guid featureId, string value);

        /// <summary>
        /// Creates the product family feature value.
        /// </summary>
        /// <param name="productFamilyFeature">The product family feature.</param>
        /// <param name="value">The value.</param>
        /// <param name="unitOfMeasureId"></param>
        /// <returns>ProductFamilyFeatureValue.</returns>
        ProductFamilyFeatureValue CreateProductFamilyFeatureValue(ProductFamilyFeature productFamilyFeature, string value, Guid? unitOfMeasureId);

        /// <summary>
        /// Removes the product family allowed feature value.
        /// </summary>
        /// <param name="familyId">The family id.</param>
        /// <param name="featureValueId">The feature value id.</param>
        void RemoveProductFamilyAllowedFeatureValue(Guid familyId , Guid featureValueId);

        /// <summary>
        /// Creates the product family allowed feature value.
        /// </summary>
        /// <param name="familyId">The family id.</param>
        /// <param name="productFamilyFeatureValue">The product family feature value.</param>
        /// <returns>ProductFamilyFeatureAllowedValue.</returns>
        ProductFamilyFeatureAllowedValue CreateProductFamilyAllowedFeatureValue(Guid familyId, ProductFamilyFeatureValue productFamilyFeatureValue);

        /// <summary>
        /// Removes the product family allowed feature value.
        /// </summary>
        /// <param name="allowedFeatureValueId">The allowed feature value id.</param>
        void RemoveProductFamilyAllowedFeatureValue(Guid allowedFeatureValueId);
    }
}