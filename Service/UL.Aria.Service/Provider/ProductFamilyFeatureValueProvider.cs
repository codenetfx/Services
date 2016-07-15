using System;
using System.Collections.Generic;
using System.Linq;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Class ProductFamilyFeatureValueProvider
    /// </summary>
    public class ProductFamilyFeatureValueProvider : IProductFamilyFeatureValueProvider
    {
        private readonly IProductFamilyFeatureValueRepository _productFamilyFeatureValueRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyFeatureValueProvider"/> class.
        /// </summary>
        /// <param name="productFamilyFeatureValueRepository">The product family feature value repository.</param>
        public ProductFamilyFeatureValueProvider(IProductFamilyFeatureValueRepository productFamilyFeatureValueRepository)
        {
            _productFamilyFeatureValueRepository = productFamilyFeatureValueRepository;
        }

        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{ProductFamilyFeatureValue}.</returns>
        public IList<ProductFamilyFeatureValue> FetchAll()
        {
            return _productFamilyFeatureValueRepository.FindAll();
        }

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>IList{ProductFamilyFeatureValue}.</returns>
        public ProductFamilyFeatureValue Fetch(Guid id)
        {
            return _productFamilyFeatureValueRepository.FindById(id);
        }

        /// <summary>
        ///     Fetches the by product family id.
        /// </summary>
        /// <param name="productFeatureId">The product feature id.</param>
        /// <returns>IList{ProductFamilyFeatureValue}.</returns>
        public IList<ProductFamilyFeatureValue> FetchByProductFeatureId(Guid productFeatureId)
        {
            return _productFamilyFeatureValueRepository.FindByFeatureId(productFeatureId).ToList();
        }

        /// <summary>
        ///     Creates the specified product family feature value.
        /// </summary>
        /// <param name="productFamilyFeatureValue">The product family feature value.</param>
        /// <returns>Guid.</returns>
        public Guid Create(ProductFamilyFeatureValue productFamilyFeatureValue)
        {
            _productFamilyFeatureValueRepository.Add(productFamilyFeatureValue);
            return productFamilyFeatureValue.Id.Value;
        }

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="productFamilyFeatureValue">The product family feature value.</param>
        public void Update(Guid id, ProductFamilyFeatureValue productFamilyFeatureValue)
        {
            _productFamilyFeatureValueRepository.Update(productFamilyFeatureValue);
        }

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(Guid id)
        {
            _productFamilyFeatureValueRepository.Remove(id);
        }
    }
}