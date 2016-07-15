using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     Product Family Service implementation.
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall
        )]
    public class ProductFamilyService : IProductFamilyService
    {
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IProductFamilyImportManager _productFamilyImportManager;
        private readonly IProductFamilyManager _productFamilyManager;
        private readonly IProductFamilyTemplateManager _productFamilyTemplateManager;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductFamilyService" /> class.
        /// </summary>
        /// <param name="productFamilyManager">The product family manager.</param>
        /// <param name="productFamilyTemplateManager">The product family template manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="productFamilyImportManager">The product family import manager.</param>
        public ProductFamilyService(IProductFamilyManager productFamilyManager,
            IProductFamilyTemplateManager productFamilyTemplateManager, IMapperRegistry mapperRegistry,
            ITransactionFactory transactionFactory,
            IProductFamilyImportManager productFamilyImportManager)
        {
            _productFamilyManager = productFamilyManager;
            _productFamilyTemplateManager = productFamilyTemplateManager;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
            _productFamilyImportManager = productFamilyImportManager;
        }

        /// <summary>
        ///     Creates the specified product family from the create request.
        /// </summary>
        /// <param name="productFamilyCreateUpdateRequest">The product family create request.</param>
        public void Create(ProductFamilyDetailDto productFamilyCreateUpdateRequest)
        {
            Guard.IsNotNull(productFamilyCreateUpdateRequest, "productFamilyCreateUpdateRequest");

            using (var scope = _transactionFactory.Create())
            {
                var family = _mapperRegistry.Map<ProductFamily>(productFamilyCreateUpdateRequest.ProductFamily);
                var characteristicUploads =
                    _mapperRegistry.Map<List<ProductFamilyCharacteristicAssociationModel>>(
                        productFamilyCreateUpdateRequest.Characteristics);
                var dependencies =
                    _mapperRegistry.Map<List<ProductFamilyFeatureDependency>>(
                        productFamilyCreateUpdateRequest.Dependencies);
                _productFamilyImportManager.Create(family, characteristicUploads, dependencies);
                scope.Complete();
            }
        }

        /// <summary>
        ///     Updates the specified product family from the update request.
        /// </summary>
        /// <param name="productFamilyCreateUpdateRequest">The product family update request.</param>
        public void Update(ProductFamilyDetailDto productFamilyCreateUpdateRequest)
        {
            Guard.IsNotNull(productFamilyCreateUpdateRequest, "productFamilyCreateUpdateRequest");

            using (var scope = _transactionFactory.Create())
            {
                var family = _mapperRegistry.Map<ProductFamily>(productFamilyCreateUpdateRequest.ProductFamily);
                var characteristicUploads =
                    _mapperRegistry.Map<List<ProductFamilyCharacteristicAssociationModel>>(
                        productFamilyCreateUpdateRequest.Characteristics);
                var dependencies =
                    _mapperRegistry.Map<List<ProductFamilyFeatureDependency>>(
                        productFamilyCreateUpdateRequest.Dependencies);
                _productFamilyImportManager.Update(family, characteristicUploads, dependencies);
                scope.Complete();
            }
        }

        /// <summary>
        ///     Gets the product family by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ProductFamilyDto GetProductFamilyById(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");
            using (var scope = _transactionFactory.Create())
            {
                var family = _productFamilyManager.Get(convertedId);
                scope.Complete();
                return _mapperRegistry.Map<ProductFamilyDto>(family);
            }
        }

        /// <summary>
        /// Gets the product family detail by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ProductFamilyDetailDto.</returns>
        public ProductFamilyDetailDto GetProductFamilyDetailById(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");
            using (var scope = _transactionFactory.Create())
            {
                var familyDetail = _productFamilyManager.GetDetail(convertedId);
                scope.Complete();
                return _mapperRegistry.Map<ProductFamilyDetailDto>(familyDetail);
            }
        }

        /// <summary>
        ///     Gets the product family template.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="template"></param>
        /// <returns>
        ///     A <see cref="Stream" /> with the template contents.
        /// </returns>
        public Stream GetProductFamilyTemplate(string id, string template)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");

            ProductFamilyTemplate templateType;
            if (!Enum.TryParse(template, out templateType))
                throw new EndpointNotFoundException("Unable to locate requested template.");

            var productFamilyTemplate = _productFamilyTemplateManager.FetchProductFamilyTemplate(convertedId,
                templateType);
            var context = WebOperationContext.Current;
            if (null != context)
            {
                context.OutgoingResponse.Headers["Content-Disposition"] = "attachment; filename=" + id + ".xlsx";
                context.OutgoingResponse.ContentLength = productFamilyTemplate.Length;
                context.OutgoingResponse.ContentType =
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            return productFamilyTemplate;
        }

        /// <summary>
        ///     Gets the product families by business unit.
        /// </summary>
        /// <param name="businessUnitId">The business unit id.</param>
        /// <returns></returns>
        public ProductFamiliesDto GetProductFamiliesByBusinessUnit(string businessUnitId)
        {
            Guard.IsNotNullOrEmpty(businessUnitId, "businessUnitId");
            var convertedId = Guid.Parse(businessUnitId);
            using (var scope = _transactionFactory.Create())
            {
                var productFamilies = _productFamilyManager.GetProductFamiliesByBusinessUnit(convertedId);
                scope.Complete();
                var result = _mapperRegistry.Map<ProductFamiliesDto>(productFamilies);

                return result;
            }
        }

        /// <summary>
        ///     Gets the product families.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductFamilyDto> GetProductFamilies()
        {
            using (var scope = _transactionFactory.Create())
            {
                var productFamilyDtos =
                    _productFamilyManager.GetAll().Select(_mapperRegistry.Map<ProductFamilyDto>).ToList();
                scope.Complete();
                return productFamilyDtos;
            }
        }

        /// <summary>
        ///     Saves the product family attribute associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <param name="productFamilyAttributeAssociationIds">The product family attribute association ids.</param>
        public void SaveProductFamilyAttributeAssociations(string productFamilyId,
            IList<Guid> productFamilyAttributeAssociationIds)
        {
            Guard.IsNotNullOrEmptyTrimmed(productFamilyId, "productFamilyId");
            Guard.IsNotNull(productFamilyAttributeAssociationIds, "productFamilyAttributeAssociationIds");
            Guard.IsGreaterThan(0, productFamilyAttributeAssociationIds.Count, "productFamilyAttributeAssociationIds");
            Guard.AreEqual(productFamilyAttributeAssociationIds.Count,
                productFamilyAttributeAssociationIds.Count(x => x != Guid.Empty), "productFamilyAttributeAssociationIds");
            using (var scope = _transactionFactory.Create())
            {
                _productFamilyManager.SaveProductFamilyAttributeAssociations(productFamilyId.ToGuid(),
                    productFamilyAttributeAssociationIds);
                scope.Complete();
            }
        }

        /// <summary>
        ///     Uploads the specified file to upload.
        /// </summary>
        /// <param name="fileToUpload">The file to upload.</param>
        /// <returns></returns>
        public IEnumerable<string> Upload(Stream fileToUpload)
        {
            var ar = ProductService.ReadToEnd(fileToUpload);
            using (var stream = new MemoryStream(ar))
            {
                var result = _productFamilyManager.Upload(stream);
                return result.Messages.Select(x => x.Title).ToList();
            }
        }
    }
}