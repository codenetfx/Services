using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

using UL.Aria.Common;
using UL.Aria.Common.Authorization;
using UL.Aria.Service.Logging;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Implements operations for updating <see cref="Product" /> objects while importing.
    /// </summary>
    public class ProductUploadProductUpdateManager : IProductUploadProductUpdateManager
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IProductUploadDocumentCharacteristicProvider _documentCharacteristicProvider;
        private readonly ILogManager _logManager;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IPrincipalResolver _principalResolver;
        private readonly IProductCharacteristicChildManager _productCharacteristicChildManager;
        private readonly IProductUploadFamilyCharacteristicProvider _productCharacteristicImportManager;
        private readonly IProductCharacteristicRepository _productCharacteristicRepository;
        private readonly IProductClaimAssignmentManager _productClaimAssignmentManager;
        private readonly IProductManager _productManager;
        private readonly IProductProvider _productProvider;
        private readonly IProductUploadDocumentImportManager _productUploadDocumentImportManager;
        private readonly IProfileManager _profileManager;
        private readonly IScratchSpaceRepository _scratchSpaceRepository;
        private readonly ISearchProvider _searchProvider;
        private readonly ITransactionFactory _transactionFactory;


        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductUploadProductUpdateManager" /> class.
        /// </summary>
        /// <param name="productProvider">The product provider.</param>
        /// <param name="assetProvider">The asset provider.</param>
        /// <param name="searchProvider">The search provider.</param>
        /// <param name="logManager">The log manager.</param>
        /// <param name="productCharacteristicImportManager">The product characteristic import manager.</param>
        /// <param name="productCharacteristicRepository">The product characteristic repository.</param>
        /// <param name="productUploadDocumentImportManager">The product upload document import manager.</param>
        /// <param name="productClaimAssignmentManager">The claim provider.</param>
        /// <param name="documentCharacteristicProvider">The document characteristic provider.</param>
        /// <param name="productCharacteristicChildManager">The product characteristic child manager.</param>
        /// <param name="scratchSpaceRepository">The scratch space repository.</param>
        /// <param name="profileManager">The profile manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="productManager">The product manager.</param>
        public ProductUploadProductUpdateManager(IProductProvider productProvider, IAssetProvider assetProvider,
            ISearchProvider searchProvider, ILogManager logManager,
            IProductUploadFamilyCharacteristicProvider
                productCharacteristicImportManager,
            IProductCharacteristicRepository productCharacteristicRepository,
            IProductUploadDocumentImportManager productUploadDocumentImportManager,
            IProductClaimAssignmentManager productClaimAssignmentManager,
            IProductUploadDocumentCharacteristicProvider
                documentCharacteristicProvider,
            IProductCharacteristicChildManager productCharacteristicChildManager,
            IScratchSpaceRepository scratchSpaceRepository,
            IProfileManager profileManager,
            IMapperRegistry mapperRegistry,
            ITransactionFactory transactionFactory,
            IPrincipalResolver principalResolver,
            IProductManager productManager)
        {
            _productProvider = productProvider;
            _assetProvider = assetProvider;
            _searchProvider = searchProvider;
            _logManager = logManager;
            _productCharacteristicImportManager = productCharacteristicImportManager;
            _productCharacteristicRepository = productCharacteristicRepository;
            _productUploadDocumentImportManager = productUploadDocumentImportManager;
            _productClaimAssignmentManager = productClaimAssignmentManager;
            _documentCharacteristicProvider = documentCharacteristicProvider;
            _productCharacteristicChildManager = productCharacteristicChildManager;
            _scratchSpaceRepository = scratchSpaceRepository;
            _profileManager = profileManager;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
            _principalResolver = principalResolver;
            _productManager = productManager;
        }

        /// <summary>
        ///     Persists the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="uploadId">The upload id.</param>
        public void Persist(Guid id, ProductUploadResult productUploadResult, Guid uploadId)
        {
            try
            {
                var scratchFiles = ProductUploadProductInsertManager.FetchScratchSpaceDocuments(
                    _scratchSpaceRepository, _logManager, productUploadResult.CreatedById);
                var productUpload = new ProductUpload
                {
                    Id = uploadId,
                    CreatedById = productUploadResult.CreatedById,
                    CreatedDateTime = productUploadResult.CreatedDateTime,
                    UpdatedById = productUploadResult.UpdatedById,
                    UpdatedDateTime = productUploadResult.UpdatedDateTime,
                    CreatedByUserLoginId = productUploadResult.CreatedByUserLoginId
                };
                var profileBo = _profileManager.FetchById(productUploadResult.CreatedById);
                var profileDto = _mapperRegistry.Map<ProfileDto>(profileBo);
                Persist(productUpload, productUploadResult, scratchFiles, profileDto);
            }
            catch (Exception ex)
            {
                productUploadResult.IsValid = false;
                productUploadResult.Product = null;
                productUploadResult.Messages.Add(new ProductUploadMessage
                {
                    MessageType = ProductUploadMessageTypeEnumDto.Error,
                    Title = "Unable to save submitted Product.",
                    Detail =
                        String.Format(
                            "Technical Error Occurred on update. The correlation id was {0}",
                            Trace.CorrelationManager.ActivityId),
                    CreatedDateTime = DateTime.UtcNow,
                    UpdatedDateTime = DateTime.UtcNow,
                });
                _logManager.Log(ex.ToLogMessage(MessageIds.ProductUploadImportPersistSubmittedProductException, LogCategory.ProductUploadImportManager, LogPriority.Medium,
                    TraceEventType.Error));
            }

            ProductUploadProductInsertManager.PersistProductUploadResult(_transactionFactory, _productProvider,
                _logManager, productUploadResult);
        }

        /// <summary>
        ///     Persists the characteristics for existing product.
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="cachedCharacteristics">The cached characteristics.</param>
        /// <param name="scratchFiles"></param>
        public IList<ContainerImportFile> PersistCharacteristics(Guid containerId,
            ProductUploadResult productUploadResult,
            IDictionary
                <Guid, ProductFamilyCharacteristicDomainEntity>
                cachedCharacteristics,
            IEnumerable<ScratchFileUpload> scratchFiles)
        {
            SearchResultSet searchResultSet = new SearchResultSet {Results = new List<SearchResult>()};
            try
            {
                searchResultSet = FindExistingDocuments(productUploadResult);
            }
            catch (Exception ex)
            {
                productUploadResult.Messages.Add(new ProductUploadMessage
                {
                    MessageType = ProductUploadMessageTypeEnumDto.Error,
                    Title = "Unable to locate existing documents for product.",
                    Detail =
                        string.Format("Unable to locate existing documents for product {0}.", productUploadResult.Id),
                    CreatedDateTime = DateTime.UtcNow,
                    UpdatedDateTime = DateTime.UtcNow,
                });
                productUploadResult.IsValid = false;
                LogMessage logMessage = ex.ToLogMessage(MessageIds.ProductUploadPersistCharacteristicsException, LogCategory.ProductUploadImportManager, LogPriority.Medium,
                    TraceEventType.Error);
                logMessage.Data.Add("ProductUploadResult",
                    string.Format("ProductUploadResult:{0}", productUploadResult.Id));
                _logManager.Log(logMessage);
            }
            IList<ContainerImportFile> containerImportFiles = new List<ContainerImportFile>();
            foreach (var productCharacteristic in productUploadResult.Product.Characteristics)
            {
                _productCharacteristicImportManager.FillCharacteristic(productUploadResult.Product.ProductFamilyId,
                    productCharacteristic, cachedCharacteristics,
                    scratchFiles);
                _documentCharacteristicProvider.FillCharacteristic(containerId, productUploadResult,
                    cachedCharacteristics, scratchFiles,
                    containerImportFiles, productCharacteristic,
                    searchResultSet.Results);
                productCharacteristic.ProductId = productUploadResult.Product.Id.Value;
                if (!productCharacteristic.Id.HasValue)
                {
                    productCharacteristic.Id = Guid.NewGuid();
                    productUploadResult.Product.UpdatedDateTime = DateTime.UtcNow;
                    if (!productCharacteristic.IsMultivalueAllowed && !productCharacteristic.IsRangeAllowed &&
                        !ProductCharacteristicChildManager.ValidateDataType(productUploadResult, productCharacteristic,
                            productCharacteristic.Value))
                        continue;
                    ProductUploadProductInsertManager.TryExecute(_logManager, productUploadResult, productCharacteristic,
                        "Unable to save value.",
                        "There was an error saving a value for this product. {0}",
                        x => _productCharacteristicRepository.Add(x));
                }
                else
                {
                    productUploadResult.Product.UpdatedDateTime = DateTime.UtcNow;
                    productUploadResult.Product.CreatedDateTime = productUploadResult.Product.UpdatedDateTime;
                    if (!productCharacteristic.IsMultivalueAllowed && !productCharacteristic.IsRangeAllowed &&
                        !ProductCharacteristicChildManager.ValidateDataType(productUploadResult, productCharacteristic,
                            productCharacteristic.Value))
                        continue;
                    ProductUploadProductInsertManager.TryExecute(_logManager, productUploadResult, productCharacteristic,
                        "Unable to save value.",
                        "There was an error saving a value for this product. {0}",
                        x => _productCharacteristicRepository.Update(x));
                }

                _productCharacteristicChildManager.DeleteChildren(productCharacteristic.Id.Value);
                _productCharacteristicChildManager.SaveMultiRangeValues(productCharacteristic, productUploadResult,
                    cachedCharacteristics);
            }

            return containerImportFiles;
        }

        internal void Persist(ProductUpload productUpload, ProductUploadResult productUploadResult,
            IEnumerable<ScratchFileUpload> scratchFiles, ProfileDto profile)
        {
            var product = productUploadResult.Product;

            var productStatus = _productProvider.GetStatus(product.Id.Value);

            if (ProductStatus.Submitted == productStatus)
            {
                var claimsPrincipal = _principalResolver.Current;

                if (!claimsPrincipal.HasClaim(c => ClaimTypes.Role == c.Type && c.Value == "UL-Employee"))
                {
                    productUploadResult.IsValid = false;
                    productUploadResult.Messages.Add(new ProductUploadMessage
                    {
                        MessageType = ProductUploadMessageTypeEnumDto.Error,
                        Title = "Unable to update submitted Product.",
                        Detail =
                            String.Format(
                                "The uploading user may not update a submitted product. The user was {0}",
                                profile.LoginId),
                        CreatedDateTime = DateTime.UtcNow,
                        UpdatedDateTime = DateTime.UtcNow,
                    });
                    return;
                }
            }

            var metadata = _assetProvider.Fetch(product.Id.Value);
            Guid containerId = default(Guid);

            if (metadata == null || metadata.Count == 0)
                HandleMissingContainer(productUploadResult);
            else
                containerId = metadata[AssetFieldNames.AriaContainerId].ParseOrDefault(default(Guid));

            product.Status = productStatus;

            product.UpdatedDateTime = DateTime.UtcNow;
            _productManager.Update(product);
            var cachedCharacteristics =
                _productCharacteristicImportManager.GetProductFamilyCharacteristics(product.ProductFamilyId);
            var containerDocuments = PersistCharacteristics(containerId, productUploadResult, cachedCharacteristics,
                scratchFiles);

            if (containerId != default(Guid))
            {
                _productClaimAssignmentManager.AssignClaim(productUpload, containerId, profile);
                _assetProvider.Update(product.Id.Value, product);
                var documentPermissionEnumDto = GetDocumentPermission(product);
                _productUploadDocumentImportManager.StoreDocumentsForProduct(productUpload, productUploadResult,
                    containerId, cachedCharacteristics,
                    scratchFiles, documentPermissionEnumDto,
                    containerDocuments);
            }
        }

        private SearchResultSet FindExistingDocuments(ProductUploadResult productUploadResult)
        {
            var searchCriteria = new SearchCriteria
            {
                EntityType = EntityTypeEnumDto.Document,
                Keyword = "ariaProductId:" + productUploadResult.Product.Id.Value,
                StartIndex = 0,
                EndIndex = 0
            };
            var searchResultSet = _searchProvider.Search(searchCriteria);
            return searchResultSet;
        }

        private void HandleMissingContainer(ProductUploadResult productUploadResult)
        {
            productUploadResult.Messages.Add(new ProductUploadMessage
            {
                MessageType = ProductUploadMessageTypeEnumDto.Error,
                Title = "Unable to locate storage for product documents.",
                Detail = String.Format("The supplied product does not have associated document storage."),
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
            });
            productUploadResult.IsValid = false;
        }

        private static DocumentPermissionEnumDto GetDocumentPermission(Product product)
        {
            if (ProductStatus.Submitted == product.Status)
                return DocumentPermissionEnumDto.ReadOnly;

            return DocumentPermissionEnumDto.Modify;
        }
    }
}