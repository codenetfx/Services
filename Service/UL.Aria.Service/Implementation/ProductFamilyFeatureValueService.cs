using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     Class ProductFamilyFeatureValueService
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class ProductFamilyFeatureValueService : IProductFamilyFeatureValueService
    {
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IProductFamilyFeatureValueProvider _productFamilyFeatureValueProvider;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductFamilyFeatureValueService" /> class.
        /// </summary>
        /// <param name="productFamilyFeatureValueProvider">The product family feature value provider.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public ProductFamilyFeatureValueService(IProductFamilyFeatureValueProvider productFamilyFeatureValueProvider,
            IMapperRegistry mapperRegistry,
            ITransactionFactory transactionFactory)
        {
            _productFamilyFeatureValueProvider = productFamilyFeatureValueProvider;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{ProductFamilyFeatureValueDto}.</returns>
        public IList<ProductFamilyFeatureValueDto> FetchAll()
        {
            return _productFamilyFeatureValueProvider.FetchAll().Select(_mapperRegistry.Map<ProductFamilyFeatureValueDto>).ToList();
        }

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>IList{ProductFamilyFeatureValueDto}.</returns>
        public ProductFamilyFeatureValueDto Fetch(string id)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "id");

            return _mapperRegistry.Map<ProductFamilyFeatureValueDto>(_productFamilyFeatureValueProvider.Fetch(convertedId));
        }

        /// <summary>
        /// Fetches the by product family id.
        /// </summary>
        /// <param name="productFeatureId">The product feature id.</param>
        /// <returns>IList{ProductFamilyFeatureValueDto}.</returns>
        public IList<ProductFamilyFeatureValueDto> FetchByProductFeatureId(string productFeatureId)
        {
            Guard.IsNotNullOrEmpty(productFeatureId, "productFeatureId");
            var convertedProductFeatureId = productFeatureId.ToGuid();
            Guard.IsNotEmptyGuid(convertedProductFeatureId, "productFeatureId");

            return _productFamilyFeatureValueProvider.FetchByProductFeatureId(convertedProductFeatureId).Select(_mapperRegistry.Map<ProductFamilyFeatureValueDto>).ToList();
        }

        /// <summary>
        ///     Creates the specified product family feature value.
        /// </summary>
        /// <param name="productFamilyFeatureValue">The product family feature value.</param>
        /// <returns>System.String.</returns>
        public string Create(ProductFamilyFeatureValueDto productFamilyFeatureValue)
        {
            Guard.IsNotNull(productFamilyFeatureValue, "Category");
            Guid id;

            var productFamilyFeatureValueBo = _mapperRegistry.Map<ProductFamilyFeatureValue>(productFamilyFeatureValue);

            using (var transactionScope = _transactionFactory.Create())
            {
                id = _productFamilyFeatureValueProvider.Create(productFamilyFeatureValueBo);
                transactionScope.Complete();
            }

            return id.ToString();
        }

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="productFamilyFeatureValue">The product family feature value.</param>
        public void Update(string id, ProductFamilyFeatureValueDto productFamilyFeatureValue)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");
            Guard.IsNotNull(productFamilyFeatureValue, "productFamilyFeatureValue");

            var productFamilyFeatureValueBo = _mapperRegistry.Map<ProductFamilyFeatureValue>(productFamilyFeatureValue);

            using (var transactionScope = _transactionFactory.Create())
            {
                _productFamilyFeatureValueProvider.Update(id.ToGuid(), productFamilyFeatureValueBo);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(string id)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");

            using (var transactionScope = _transactionFactory.Create())
            {
                _productFamilyFeatureValueProvider.Delete(convertedId);
                transactionScope.Complete();
            }
        }
    }
}