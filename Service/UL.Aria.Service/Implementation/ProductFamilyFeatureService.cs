using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
    ///     Class ProductFamilyFeatureService
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class ProductFamilyFeatureService : IProductFamilyFeatureService
    {
        private readonly IProductFamilyFeatureManager _productFamilyFeatureManager;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductFamilyFeatureService" /> class.
        /// </summary>
        /// <param name="productFamilyFeatureManager">The ProductFamilyFeature manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory"></param>
        public ProductFamilyFeatureService(IProductFamilyFeatureManager productFamilyFeatureManager,
                                           IMapperRegistry mapperRegistry, ITransactionFactory transactionFactory)
        {
            _productFamilyFeatureManager = productFamilyFeatureManager;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Creates the specified ProductFamilyFeature.
        /// </summary>
        /// <param name="productFamilyFeatureDto">The ProductFamilyFeature dto.</param>
        /// <returns>The created ProductFamilyFeature id.</returns>
        public string Create(ProductFamilyFeatureDto productFamilyFeatureDto)
        {
            Guard.IsNotNull(productFamilyFeatureDto, "productFamilyFeature");

            var charcteristic = _mapperRegistry.Map<ProductFamilyFeature>(productFamilyFeatureDto);
            using (var scope = _transactionFactory.Create())
            {
                var result = _productFamilyFeatureManager.Create(charcteristic).ToString();
                scope.Complete();
                return result;
            }
        }

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="productFamilyFeatureDto">The ProductFamilyFeature dto.</param>
        public void Update(string id, ProductFamilyFeatureDto productFamilyFeatureDto)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");
            Guard.IsNotNull(productFamilyFeatureDto, "productFamilyFeature");
            Guard.IsNotNull(productFamilyFeatureDto.Id, "productFamilyFeatureDto");
            Guard.AreEqual(convertedId, productFamilyFeatureDto.Id.Value, "id");

            var productFamilyFeature = _mapperRegistry.Map<ProductFamilyFeature>(productFamilyFeatureDto);
            using (var scope = _transactionFactory.Create())
            {
                _productFamilyFeatureManager.Update(productFamilyFeature);
                scope.Complete();
            }
        }

        /// <summary>
        ///     Gets the <see cref="ProductFamilyFeature" />specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ProductFamilyFeatureDto.</returns>
        public ProductFamilyFeatureDto GetById(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");

            ProductFamilyFeature productFamilyFeature;
            using (var scope = _transactionFactory.Create())
            {
                productFamilyFeature = _productFamilyFeatureManager.FetchById(convertedId);
                scope.Complete();
            }
            return _mapperRegistry.Map<ProductFamilyFeatureDto>(productFamilyFeature);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IList<ProductFamilyFeatureDto> GetAll()
        {
            IList<ProductFamilyFeature> productFamilyAttributes;
            using (var scope = _transactionFactory.Create())
            {
                productFamilyAttributes = _productFamilyFeatureManager.FetchAll();
                scope.Complete();
            }
            return productFamilyAttributes.Select(x => _mapperRegistry.Map<ProductFamilyFeatureDto>(x)).ToList();
        }

        /// <summary>
        /// Gets features by scope.
        /// </summary>
        /// <param name="scopeids">The scopeids.</param>
        /// <returns></returns>
        public IList<ProductFamilyFeatureDto> GetByScope(string scopeids)
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

            IList<ProductFamilyFeature> result = new List<ProductFamilyFeature>();
            using (var scope = _transactionFactory.Create())
            {
                result = _productFamilyFeatureManager.FetchAll(convertedIds);
                scope.Complete();
            }
            return result.Select(x => _mapperRegistry.Map<ProductFamilyFeatureDto>(x)).ToList();

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
                _productFamilyFeatureManager.Remove(convertedId);
                scope.Complete();
            }
        }
    }
}