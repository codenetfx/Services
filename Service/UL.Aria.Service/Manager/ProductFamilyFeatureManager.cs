using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements operations for working with <see cref="ProductFamilyFeature"/> objects.
    /// </summary>
    public class ProductFamilyFeatureManager : IProductFamilyFeatureManager
    {
        private readonly IProductFamilyFeatureProvider _productFamilyFeatureProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyFeatureManager" /> class.
        /// </summary>
        /// <param name="productFamilyFeatureProvider">The product family Feature provider.</param>
        public ProductFamilyFeatureManager(IProductFamilyFeatureProvider productFamilyFeatureProvider)
        {
            _productFamilyFeatureProvider = productFamilyFeatureProvider;
        }

        /// <summary>
        /// Creates the product family Feature.
        /// </summary>
        /// <param name="productFamilyFeature">The product family Feature.</param>
        /// <returns>Product family Feature id.</returns>
        public Guid Create(ProductFamilyFeature productFamilyFeature)
        {
            if (!productFamilyFeature.Id.HasValue)
            {
                productFamilyFeature.Id = Guid.NewGuid();
            }
            FillOptions(productFamilyFeature);
            _productFamilyFeatureProvider.Create(productFamilyFeature);

            return productFamilyFeature.Id.Value;
        }

        /// <summary>
        /// Updates the product family Feature.
        /// </summary>
        /// <param name="productFamilyFeature">The product family Feature.</param>
        public void Update(ProductFamilyFeature productFamilyFeature)
        {
            FillOptions(productFamilyFeature);
            _productFamilyFeatureProvider.Update(productFamilyFeature);
        }

        private void FillOptions(ProductFamilyFeature entity)
        {
            foreach (var option1 in entity.Options)
            {
                if (option1.Id.HasValue)
                    continue;
                switch (option1.Value.ToLowerInvariant())
                {
                    case "range":
                        option1.Id = new Guid("B9DAAD50-FBEB-E211-BF96-54DA2537410C");
                        break;
                    case "multiple":
                        option1.Id = new Guid("0CB2391A-FBEB-E211-BF96-54DA2537410C");
                        break;
                    case "single":
                        option1.Id = new Guid("CB2CA338-FBEB-E211-BF96-54DA2537410C");
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the product family Feature by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Product family Feature data transfer object.</returns>
        public ProductFamilyFeature FetchById(Guid id)
        {
            return _productFamilyFeatureProvider.Get(id);
        }

        /// <summary>
        /// Fetches the by unique identifier.
        /// </summary>
        /// <returns></returns>
        public IList<ProductFamilyFeature> FetchAll()
        {
            return _productFamilyFeatureProvider.GetAll();
        }

        /// <summary>
        /// Fetches the by their scope unique identifier.
        /// </summary>
        /// <returns></returns>
        public IList<ProductFamilyFeature> FetchAll(IEnumerable<Guid> scopeIds)
        {
            var results = new List<ProductFamilyFeature>();
            foreach (var scopeId in scopeIds)
            {
                results.AddRange(_productFamilyFeatureProvider.FindByScopeId(scopeId));
            }

            return results;
        }

        /// <summary>
        /// Removes the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        public void Remove(Guid id)
        {
            _productFamilyFeatureProvider.Delete(id);
        }
    }
}