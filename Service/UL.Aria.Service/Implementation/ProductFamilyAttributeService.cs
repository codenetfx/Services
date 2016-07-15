using System;
using System.Collections.Generic;
using System.Linq;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    /// Class ProductFamilyAttributeService
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    public class ProductFamilyAttributeService : IProductFamilyAttributeService
    {
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IProductFamilyAttributeManager _productFamilyAttributeManager;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyAttributeService" /> class.
        /// </summary>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="productFamilyAttributeManager">The product family attribute provider.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public ProductFamilyAttributeService(IMapperRegistry mapperRegistry, IProductFamilyAttributeManager productFamilyAttributeManager, ITransactionFactory transactionFactory)
        {
            _mapperRegistry = mapperRegistry;
            _productFamilyAttributeManager = productFamilyAttributeManager;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        /// Creates the product family attribute.
        /// </summary>
        /// <param name="productFamilyAttributeDto">The product family attribute.</param>
        /// <returns>
        /// Product family attribute id.
        /// </returns>
        public string Create(ProductFamilyAttributeDto productFamilyAttributeDto)
        {
            Guard.IsNotNull(productFamilyAttributeDto, "productFamilyAttribute");

            var productFamilyAttribute = _mapperRegistry.Map<ProductFamilyAttribute>(productFamilyAttributeDto);

            using ( var scope =_transactionFactory.Create())
            {
                var result = _productFamilyAttributeManager.Create(productFamilyAttribute).ToString();
                scope.Complete();
                return result;
            }
        }

        /// <summary>
        /// Updates the product family attribute.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="productFamilyAttributeDto">The product family attribute.</param>
        public void Update(string id, ProductFamilyAttributeDto productFamilyAttributeDto)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");
            Guard.IsNotNull(productFamilyAttributeDto, "productFamilyAttribute");
            Guard.IsNotNull(productFamilyAttributeDto.Id, "productFamilyAttributeDto");
            Guard.AreEqual(convertedId, productFamilyAttributeDto.Id.Value, "id");

            var productFamilyAttribute = _mapperRegistry.Map<ProductFamilyAttribute>(productFamilyAttributeDto);
            using (var scope = _transactionFactory.Create())
            {
                _productFamilyAttributeManager.Update(productFamilyAttribute);
                scope.Complete();
            }
        }

        /// <summary>
        /// Gets the product family attribute by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// Product family attribute data transfer object.
        /// </returns>
        public ProductFamilyAttributeDto GetById(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");

            ProductFamilyAttribute productFamilyAttribute;
            using (var scope = _transactionFactory.Create())
            {
                productFamilyAttribute = _productFamilyAttributeManager.FetchById(convertedId);
                scope.Complete();
            }

            var productFamilyAttributeDto = _mapperRegistry.Map<ProductFamilyAttributeDto>(productFamilyAttribute);
            return productFamilyAttributeDto;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IList<ProductFamilyAttributeDto> GetAll()
        {
            IList<ProductFamilyAttribute> productFamilyAttributes;
            using (var scope = _transactionFactory.Create())
            {
                productFamilyAttributes = _productFamilyAttributeManager.FetchAll();
                scope.Complete();
            }
            return productFamilyAttributes.Select(x=> _mapperRegistry.Map<ProductFamilyAttributeDto>(x)).ToList();
        }

        /// <summary>
        /// Gets attributes by scope.
        /// </summary>
        /// <param name="scopeids">The scopeids.</param>
        /// <returns></returns>
        public IList<ProductFamilyAttributeDto> GetByScope(string scopeids)
        {
            Guard.IsNotNullOrEmptyTrimmed(scopeids, "scopeids");
            var splitScopeIds = scopeids.Split(',');
            var convertedIds = new List<Guid>(splitScopeIds.Length);
            foreach (var scopeid in splitScopeIds)
            {
                var convertedId = scopeid.ParseOrDefault(Guid.Empty);
                Guard.IsNotEmptyGuid(convertedId, "scopeids");
                convertedIds.Add(convertedId);
            }

            IList<ProductFamilyAttribute> result;
            using (var scope = _transactionFactory.Create())
            {
                result = _productFamilyAttributeManager.FetchAll(convertedIds);
                scope.Complete();
            }
            return result.Select(x => _mapperRegistry.Map<ProductFamilyAttributeDto>(x)).ToList();

        }

        /// <summary>
        /// Removes the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        public void Remove(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");

            using (var scope = _transactionFactory.Create())
            {
                _productFamilyAttributeManager.Remove(convertedId);
                scope.Complete();
            }
        }
    }
}