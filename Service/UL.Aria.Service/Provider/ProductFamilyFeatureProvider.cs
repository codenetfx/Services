using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Class ProductFamilyFeatureProvider
    /// </summary>
    public class ProductFamilyFeatureProvider : IProductFamilyFeatureProvider
    {
        private readonly IProductFamilyFeatureRepository _productFamilyFeatureRepository;
        private readonly IProductFeatureOptionRepository _optionRepository;
        private readonly IProductFamilyFeatureValueRepository _valueRepository;
        private readonly IProductFamilyFeatureAllowedValueRepository _allowedValueRepository;
        private readonly IProductFamilyFeatureAllowedValueDependencyRepository _dependencyRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyFeatureProvider" /> class.
        /// </summary>
        /// <param name="productFamilyFeatureRepository">The product family feature repository.</param>
        /// <param name="optionRepository">The option repository.</param>
        /// <param name="valueRepository">The value repository.</param>
        /// <param name="allowedValueRepository">The value repository.</param>
        /// <param name="dependencyRepository"></param>
        public ProductFamilyFeatureProvider(
            IProductFamilyFeatureRepository productFamilyFeatureRepository, 
            IProductFeatureOptionRepository optionRepository, 
            IProductFamilyFeatureValueRepository valueRepository,
            IProductFamilyFeatureAllowedValueRepository allowedValueRepository,
            IProductFamilyFeatureAllowedValueDependencyRepository dependencyRepository)
        {
            _productFamilyFeatureRepository = productFamilyFeatureRepository;
            _optionRepository = optionRepository;
            _valueRepository = valueRepository;
            _allowedValueRepository = allowedValueRepository;
            _dependencyRepository = dependencyRepository;
        }

        /// <summary>
        /// Creates the specified product family feature.
        /// </summary>
        /// <param name="productFamilyFeature">The product family feature.</param>
        /// <returns></returns>
        public Guid Create(ProductFamilyFeature productFamilyFeature)
        {
            return  _productFamilyFeatureRepository.Create(productFamilyFeature);
        }

        /// <summary>
        /// Gets the by product family id.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        public IList<ProductFamilyFeature> GetByProductFamilyId(Guid productFamilyId)
        {
            return _productFamilyFeatureRepository.FindByProductFamilyId(productFamilyId);
        }

        /// <summary>
        /// Gets the specified product family features ids.
        /// </summary>
        /// <param name="productFamilyIds">The product family attribute ids.</param>
        /// <returns></returns>
        public IList<ProductFamilyFeature> Get(IList<Guid> productFamilyIds)
        {
            return null;
        }

        /// <summary>
        /// Updates the specified product family feature.
        /// </summary>
        /// <param name="productFamilyFeature">The product family feature.</param>
        public void Update(ProductFamilyFeature productFamilyFeature)
        {
            _productFamilyFeatureRepository.Update(productFamilyFeature);
        }

        /// <summary>
        /// Adds the option.
        /// </summary>
        /// <param name="characteristicId">The characteristic id.</param>
        /// <param name="option">The option.</param>
        public void AddOption(Guid characteristicId, ProductFamilyCharacteristicOption option)
        {
            option.ProductFamilyCharacteristicId = characteristicId;
            _optionRepository.Add(option);
        }

        /// <summary>
        /// Adds the option.
        /// </summary>
        /// <param name="optionId">The option id.</param>
        public void RemoveOption(Guid optionId)
        {
            
            _optionRepository.Remove(optionId);
        }

        /// <summary>
        /// Gets the specified product family feature by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ProductFamilyFeature Get(Guid id)
        {
            return _productFamilyFeatureRepository.FindById(id);
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <param name="familyId"></param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureAllowedValue> FindAllowedValues(Guid featureId, Guid familyId)
        {
            return _allowedValueRepository.FindByFeatureId(featureId, familyId);
        }

        /// <summary>
        /// Finds the allowed values by family.
        /// </summary>
        /// <param name="familyId">The family id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureAllowedValue> FindAllowedValuesByFamily(Guid familyId)
        {
            return _allowedValueRepository.FindByFamilyId(familyId);
        }

        /// <summary>
        /// Finds the by scope id.
        /// </summary>
        /// <param name="scopeId">The scope id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeature> FindByScopeId(Guid scopeId)
        {
            return _productFamilyFeatureRepository.FindByScopeId(scopeId);
        }

        /// <summary>
        /// Finds the dependencies by family allowed feature id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureAllowedValueDependency> GetValueDependenciesByFamilyAllowedFeatureId(Guid id)
        {
            return _dependencyRepository.FindByFamilyAllowedFeatureId(id);
        }

        /// <summary>
        /// Gets the value dependencies by parent family allowed feature value id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureAllowedValueDependency> GetValueDependenciesByParentFamilyAllowedFeatureValueId(Guid id)
        {
            return _dependencyRepository.FindByParentFamilyAllowedFeatureValueId(id);
        }

        /// <summary>
        /// Removes the dependency.
        /// </summary>
        /// <param name="id">The id.</param>
        public void RemoveValueDependency(Guid id)
        {
            _dependencyRepository.Remove(id);
        }

        /// <summary>
        /// Gets the value dependency.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ProductFamilyFeatureAllowedValueDependency GetValueDependency(Guid id)
        {
            return _dependencyRepository.FindById(id);
        }

        /// <summary>
        /// Adds the dependency.
        /// </summary>
        /// <param name="dependency">The dependency.</param>
        public void AddValueDependency(ProductFamilyFeatureAllowedValueDependency dependency)
        {
            _dependencyRepository.Add(dependency);
        }

        /// <summary>
        /// Finds the values.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureValue> FindValues(Guid featureId)
        {
            return _valueRepository.FindByFeatureId(featureId);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IList<ProductFamilyFeature> GetAll()
        {
            return _productFamilyFeatureRepository.FindAll();
        }

        /// <summary>
        /// Deletes the specified feature.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        public void Delete(Guid id)
        {
            _productFamilyFeatureRepository.Remove(id);
        }
    }
}