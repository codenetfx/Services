using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements operations for working with <see cref="ProductFamilyAttribute"/> objects.
    /// </summary>
    public class ProductFamilyAttributeManager : IProductFamilyAttributeManager
    {
        private readonly IProductFamilyAttributeProvider _productFamilyAttributeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyAttributeManager" /> class.
        /// </summary>
        /// <param name="productFamilyAttributeProvider">The product family attribute provider.</param>
        public ProductFamilyAttributeManager(IProductFamilyAttributeProvider productFamilyAttributeProvider)
        {
            _productFamilyAttributeProvider = productFamilyAttributeProvider;
        }

        /// <summary>
        /// Creates the product family attribute.
        /// </summary>
        /// <param name="productFamilyAttribute">The product family attribute.</param>
        /// <returns>Product family attribute id.</returns>
        public Guid Create(ProductFamilyAttribute productFamilyAttribute)
        {
            if (!productFamilyAttribute.Id.HasValue)
            {
                productFamilyAttribute.Id = Guid.NewGuid();
            }
            FillOptions(productFamilyAttribute);
            _productFamilyAttributeProvider.Create(productFamilyAttribute);

            return productFamilyAttribute.Id.Value;
        }

        /// <summary>
        /// Updates the product family attribute.
        /// </summary>
        /// <param name="productFamilyAttribute">The product family attribute.</param>
        public void Update(ProductFamilyAttribute productFamilyAttribute)
        {
            FillOptions(productFamilyAttribute);
            _productFamilyAttributeProvider.Update(productFamilyAttribute);
        }

        private void FillOptions(ProductFamilyAttribute entity)
        {
            foreach (var option1 in entity.Options)
            {
                if (option1.Id.HasValue)
                    continue;
                switch (option1.Value.ToLowerInvariant())
                {
                    case "range":
                        option1.Id = new Guid("CF5A3589-FBEB-E211-BF96-54DA2537410C");
                        break;
                    case "multiple":
                        option1.Id = new Guid("D05A3589-FBEB-E211-BF96-54DA2537410C");
                        break;
                    case "single":
                        option1.Id = new Guid("D15A3589-FBEB-E211-BF96-54DA2537410C");
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the product family attribute by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Product family attribute data transfer object.</returns>
        public ProductFamilyAttribute FetchById(Guid id)
        {
            return _productFamilyAttributeProvider.Get(id);
        }

        /// <summary>
        /// Fetches the by unique identifier.
        /// </summary>
        /// <returns></returns>
        public IList<ProductFamilyAttribute> FetchAll()
        {
            return _productFamilyAttributeProvider.GetAll();
        }

        /// <summary>
        /// Fetches the by their scope unique identifier.
        /// </summary>
        /// <returns></returns>
        public IList<ProductFamilyAttribute> FetchAll(IEnumerable<Guid> scopeIds)
        {
            var results = new List<ProductFamilyAttribute>();
            foreach (var scopeId in scopeIds)
            {
               results.AddRange(_productFamilyAttributeProvider.FindByScopeId(scopeId));
            }

            return results;
        }

        /// <summary>
        /// Removes the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        public void Remove(Guid id)
        {
            _productFamilyAttributeProvider.Delete(id);
        }
    }
}