using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Defines the product family feature provider.
    /// </summary>
    public interface IProductFamilyFeatureProvider
    {
        /// <summary>
        /// Gets the specified product family feature by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        ProductFamilyFeature Get(Guid id);

        /// <summary>
        /// Updates the specified product family feature.
        /// </summary>
        /// <param name="productFamilyFeature">The product family feature.</param>
        void Update(ProductFamilyFeature productFamilyFeature);

        /// <summary>
        /// Creates the specified product family feature.
        /// </summary>
        /// <param name="productFamilyFeature">The product family feature.</param>
        /// <returns></returns>
        Guid Create(ProductFamilyFeature productFamilyFeature);

        /// <summary>
        /// Gets the by product family id.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        IList<ProductFamilyFeature> GetByProductFamilyId(Guid productFamilyId);

        /// <summary>
        /// Gets the specified product family features ids.
        /// </summary>
        /// <param name="productFamilyIds">The product family attribute ids.</param>
        /// <returns></returns>
        IList<ProductFamilyFeature> Get(IList<Guid> productFamilyIds);

        /// <summary>
        /// Adds the option.
        /// </summary>
        /// <param name="characteristicId">The characteristic id.</param>
        /// <param name="option">The option.</param>
        void AddOption(Guid characteristicId, ProductFamilyCharacteristicOption option);

        /// <summary>
        /// Adds the option.
        /// </summary>
        /// <param name="optionId">The option id.</param>
        void RemoveOption(Guid optionId);

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <param name="familyId"></param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeatureAllowedValue> FindAllowedValues(Guid featureId, Guid familyId);

        /// <summary>
        /// Finds the by scope id.
        /// </summary>
        /// <param name="scopeId">The scope id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeature> FindByScopeId(Guid scopeId);

        /// <summary>
        /// Finds the dependencies by family allowed feature id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeatureAllowedValueDependency> GetValueDependenciesByFamilyAllowedFeatureId(Guid id);

        /// <summary>
        /// Removes the dependency.
        /// </summary>
        /// <param name="id">The id.</param>
        void RemoveValueDependency(Guid id);

        /// <summary>
        /// Gets the value dependency.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        ProductFamilyFeatureAllowedValueDependency GetValueDependency(Guid id);

        /// <summary>
        /// Adds the dependency.
        /// </summary>
        /// <param name="dependency">The dependency.</param>
        void AddValueDependency(ProductFamilyFeatureAllowedValueDependency dependency);

        /// <summary>
        /// Finds the allowed values by family.
        /// </summary>
        /// <param name="familyId">The family id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeatureAllowedValue> FindAllowedValuesByFamily(Guid familyId);

        /// <summary>
        /// Gets the value dependencies by parent family allowed feature value id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeatureAllowedValueDependency> GetValueDependenciesByParentFamilyAllowedFeatureValueId(Guid id);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IList<ProductFamilyFeature> GetAll();

        /// <summary>
        /// Deletes the specified feature.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        void Delete(Guid id);
    }
}