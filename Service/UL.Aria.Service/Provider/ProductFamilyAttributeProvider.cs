using System;
using System.Collections.Generic;
using System.Linq;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Class ProductFamilyAttributeProvider
    /// </summary>
    public class ProductFamilyAttributeProvider : IProductFamilyAttributeProvider
    {
        private readonly IProductFamilyAttributeRepository _productFamilyAttributeRepository;
        private readonly IProductAttributeOptionRepository _optionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyAttributeProvider" /> class.
        /// </summary>
        /// <param name="productFamilyAttributeRepository">The product family attribute repository.</param>
        /// <param name="optionRepository"></param>
        public ProductFamilyAttributeProvider(IProductFamilyAttributeRepository productFamilyAttributeRepository, IProductAttributeOptionRepository optionRepository)
        {
            _productFamilyAttributeRepository = productFamilyAttributeRepository;
            _optionRepository = optionRepository;
        }

        /// <summary>
        /// Creates the specified product family attribute.
        /// </summary>
        /// <param name="productFamilyAttribute">The product family attribute.</param>
        /// <returns>
        /// Guid.
        /// </returns>
        public Guid Create(ProductFamilyAttribute productFamilyAttribute)
        {
            return _productFamilyAttributeRepository.Create(productFamilyAttribute);
        }

        /// <summary>
        /// Updates the specified product family attribute.
        /// </summary>
        /// <param name="productFamilyAttribute">The product family attribute.</param>
        public void Update(ProductFamilyAttribute productFamilyAttribute)
        {
            _productFamilyAttributeRepository.Update(productFamilyAttribute);
        }

        /// <summary>
        /// Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// Characteristic.
        /// </returns>
        public ProductFamilyAttribute Get(Guid id)
        {
            return _productFamilyAttributeRepository.FindById(id);
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
        /// Finds the by scope id.
        /// </summary>
        /// <param name="scopeId">The scope id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyAttribute> FindByScopeId(Guid scopeId)
        {
            return _productFamilyAttributeRepository.FindByScopeId(scopeId);
        }

        /// <summary>
        /// Deletes the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        public void Delete(Guid id)
        {
            _productFamilyAttributeRepository.Remove(id);
        }

        /// <summary>
        /// Gets the by product family id.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns>Product family attribute list.</returns>
        public IList<ProductFamilyAttribute> GetByProductFamilyId(Guid productFamilyId)
        {
            return _productFamilyAttributeRepository.FindByProductFamilyId(productFamilyId);
        }

        /// <summary>
        /// Gets the specified product family attribute ids.
        /// </summary>
        /// <param name="productFamilyIds">The product family attribute ids.</param>
        /// <returns></returns>
        public IList<ProductFamilyAttribute> Get(IList<Guid> productFamilyIds)
        {
            return _productFamilyAttributeRepository.FindByIds(productFamilyIds);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IList<ProductFamilyAttribute> GetAll()
        {
            return _productFamilyAttributeRepository.FindAll();
        }
    }
}