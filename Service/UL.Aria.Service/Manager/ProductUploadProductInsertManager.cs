using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UL.Aria.Service.Logging;
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
    ///     Implements operations for adding <see cref="Product" /> objects while importing.
    /// </summary>
    public class ProductUploadProductInsertManager : IProductUploadProductInsertManager
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IProductUploadDocumentCharacteristicProvider _documentCharacteristicProvider;
        private readonly ILogManager _logManager;
        private readonly IProductCharacteristicChildManager _productCharacteristicChildManager;
        private readonly IScratchSpaceRepository _scratchSpaceRepository;
        private readonly IProfileManager _profileManager;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;
        private readonly IProductCharacteristicRepository _productCharacteristicRepository;
        private readonly IProductClaimAssignmentManager _productClaimAssignmentManager;
        private readonly IProductProvider _productProvider;
        private readonly IProductUploadDocumentImportManager _productUploadDocumentImportManager;
        private readonly IProductUploadFamilyCharacteristicProvider _productUploadFamilyCharacteristicProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductUploadProductInsertManager" /> class.
        /// </summary>
        /// <param name="productProvider">The product provider.</param>
        /// <param name="logManager">The log manager.</param>
        /// <param name="productUploadFamilyCharacteristicProvider">The product upload family characteristic provider.</param>
        /// <param name="productCharacteristicRepository">The product characteristic repository.</param>
        /// <param name="productUploadDocumentImportManager">The product upload document import manager.</param>
        /// <param name="assetProvider">The asset provider.</param>
        /// <param name="productClaimAssignmentManager">The product claim assignment manager.</param>
        /// <param name="documentCharacteristicProvider">The document characteristic provider.</param>
        /// <param name="productCharacteristicChildManager">The product characteristic child manager.</param>
        /// <param name="scratchSpaceRepository">The scratch space repository.</param>
        /// <param name="profileManager">The profile manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public ProductUploadProductInsertManager(IProductProvider productProvider, ILogManager logManager,
                                                 IProductUploadFamilyCharacteristicProvider
                                                     productUploadFamilyCharacteristicProvider,
                                                 IProductCharacteristicRepository productCharacteristicRepository,
                                                 IProductUploadDocumentImportManager productUploadDocumentImportManager,
                                                 IAssetProvider assetProvider,
                                                 IProductClaimAssignmentManager productClaimAssignmentManager,
                                                 IProductUploadDocumentCharacteristicProvider
                                                     documentCharacteristicProvider,
                                                 IProductCharacteristicChildManager productCharacteristicChildManager,
                                                 IScratchSpaceRepository scratchSpaceRepository,
                                                 IProfileManager profileManager,
                                                 IMapperRegistry mapperRegistry,
                                                 ITransactionFactory transactionFactory)
        {
            _productCharacteristicRepository = productCharacteristicRepository;
            _productUploadDocumentImportManager = productUploadDocumentImportManager;
            _assetProvider = assetProvider;
            _productClaimAssignmentManager = productClaimAssignmentManager;
            _documentCharacteristicProvider = documentCharacteristicProvider;
            _productCharacteristicChildManager = productCharacteristicChildManager;
            _scratchSpaceRepository = scratchSpaceRepository;
            _profileManager = profileManager;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
            _productClaimAssignmentManager = productClaimAssignmentManager;
            _productUploadFamilyCharacteristicProvider = productUploadFamilyCharacteristicProvider;
            _productProvider = productProvider;
            _logManager = logManager;
        }

        /// <summary>
        /// Persists the specified product upload result.
        /// </summary>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="uploadId">The upload id.</param>
        public void Persist(ProductUploadResult productUploadResult, Guid uploadId)
        {
            try
            {
                var scratchFiles = FetchScratchSpaceDocuments(_scratchSpaceRepository, _logManager, productUploadResult.CreatedById);
                var productUpload = new ProductUpload
                    {
                        Id = uploadId,
                        CreatedById = productUploadResult.CreatedById,
                        CreatedDateTime = productUploadResult.CreatedDateTime,
                        UpdatedById = productUploadResult.UpdatedById,
                        UpdatedDateTime = productUploadResult.UpdatedDateTime,
                        CompanyId  = productUploadResult.Product.CompanyId.GetValueOrDefault(),
                        CreatedByUserLoginId = productUploadResult.CreatedByUserLoginId
                    };
                var cachedCharacteristics = new Dictionary<Guid, ProductFamilyCharacteristicDomainEntity>();
                var profileBo = _profileManager.FetchById(productUploadResult.CreatedById);
                var profileDto = _mapperRegistry.Map<ProfileDto>(profileBo);
                Persist(productUpload, productUploadResult, cachedCharacteristics, scratchFiles, profileDto);
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
                            "Technical Error Occurred on insert. The correlation id was {0}",
                            Trace.CorrelationManager.ActivityId),
                    CreatedDateTime = DateTime.UtcNow,
                    UpdatedDateTime = DateTime.UtcNow,
                });
                _logManager.Log(ex.ToLogMessage(MessageIds.ProductUploadImportSaveSubmittedProductException, LogCategory.ProductUploadImportManager, LogPriority.Medium,
                                                TraceEventType.Error));
            }

            PersistProductUploadResult(_transactionFactory, _productProvider, _logManager, productUploadResult);
        }

        internal static void PersistProductUploadResult(ITransactionFactory transactionFactory, IProductProvider productProvider, ILogManager logManager, ProductUploadResult productUploadResult)
        {
            try
            {
                //Only do transactions to 1 tx per product result. No overall tx.
                using (var scope = transactionFactory.Create())
                {
                    if (!productUploadResult.Id.HasValue)
                        productUploadResult.Id = Guid.NewGuid();
                    productProvider.Create(productUploadResult);
                    if (null != productUploadResult.Messages)
                    {
                        foreach (var productUploadMessage in productUploadResult.Messages)
                        {
                            if (!productUploadMessage.Id.HasValue)
                                productUploadMessage.Id = Guid.NewGuid();
                            productUploadMessage.ProductUploadResultId = productUploadResult.Id.Value;
                            productProvider.Create(productUploadMessage);
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                productUploadResult.IsValid = false;
                logManager.Log(ex.ToLogMessage(MessageIds.ProductUploadImportPersistUploadResultException, LogCategory.ProductUploadImportManager, LogPriority.Medium,
                                                TraceEventType.Error));
            }
        }

        internal static IEnumerable<ScratchFileUpload> FetchScratchSpaceDocuments(IScratchSpaceRepository scratchSpaceRepository, ILogManager logManager, Guid userId)
        {
            try
            {
                return scratchSpaceRepository.FetchAll(userId).Select(scratchFileInfo => new ScratchFileUpload
                    {
                        ScratchFileInfo = scratchFileInfo,
                        Id = Guid.NewGuid()
                    }).ToList();
            }
            catch(DatabaseItemNotFoundException dbEx)
            {
                logManager.Log(dbEx.ToLogMessage(MessageIds.ProductUploadScratchSpaceDocumentNotFoundException, LogCategory.ProductUploadImportManager, LogPriority.Low,
                                                TraceEventType.Information));
                return new List<ScratchFileUpload>();
            }
            catch (Exception ex)
            {
                logManager.Log(ex.ToLogMessage(MessageIds.ProductUploadScratchSpaceException, LogCategory.ProductUploadImportManager, LogPriority.Medium,
                                                TraceEventType.Error));
                throw;
            }
        }

        internal void Persist(ProductUpload productUpload, ProductUploadResult productUploadResult,
                            IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> cachedCharacteristics,
                            IEnumerable<ScratchFileUpload> scratchFiles, ProfileDto profile)
        {
            var containerId = Guid.NewGuid();
            var product = productUploadResult.Product;
            product.Status = ProductStatus.Draft;
            product.UpdatedDateTime = DateTime.UtcNow;
            product.CreatedDateTime = product.UpdatedDateTime;
            product.CompanyId = productUpload.CompanyId;

            if (product.Id == null) //Added so testharness can call with already saved products
            {
                product.Id = Guid.NewGuid();
                _productProvider.Create(product);
            }
            cachedCharacteristics =
                _productUploadFamilyCharacteristicProvider.GetProductFamilyCharacteristics(product.ProductFamilyId);
            var containerDocuments = PersistCharacteristics(containerId, productUploadResult, cachedCharacteristics,
                                                            scratchFiles);

            containerId = _assetProvider.Create(containerId, product.Id.Value, product);
            _productClaimAssignmentManager.AssignClaim(productUpload, containerId, profile);

            _productUploadDocumentImportManager.StoreDocumentsForProduct(productUpload, productUploadResult, containerId,
                                                                         cachedCharacteristics, scratchFiles,
                                                                         DocumentPermissionEnumDto.Modify,
                                                                         containerDocuments);
        }

        /// <summary>
        ///     Persists the characteristics for new product.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="cachedCharacteristics">The cached characteristics.</param>
        /// <param name="scratchFiles">The scratch files.</param>
        public IList<ContainerImportFile> PersistCharacteristics(Guid containerId,
                                                                 ProductUploadResult productUploadResult,
                                                                 IDictionary
                                                                     <Guid, ProductFamilyCharacteristicDomainEntity>
                                                                     cachedCharacteristics,
                                                                 IEnumerable<ScratchFileUpload> scratchFiles)
        {
            IList<ContainerImportFile> containerImportFiles = new List<ContainerImportFile>();
            foreach (var productCharacteristic in productUploadResult.Product.Characteristics)
            {
                if (productCharacteristic.Id.HasValue)
                {
                    productUploadResult.Messages.Add(new ProductUploadMessage
                        {
                            MessageType = ProductUploadMessageTypeEnumDto.Error,
                            Title = "Attempt to add existing value to new product.",
                            Detail = string.Format("Characteristic exists. ID{0}", productCharacteristic.Id),
                            CreatedDateTime = DateTime.UtcNow,
                            UpdatedDateTime = DateTime.UtcNow,
                        });
                    productUploadResult.IsValid = false;
                    continue;
                }
                TryExecute(_logManager, productUploadResult, productCharacteristic, "Unable to save document.",
                           "There was an error saving a document for this product. {0}", x =>
                               {
                                   _productUploadFamilyCharacteristicProvider.FillCharacteristic(
                                       productUploadResult.Product.ProductFamilyId, productCharacteristic, cachedCharacteristics,
                                       scratchFiles);
                               });
               TryExecute(_logManager, productUploadResult, productCharacteristic, "Unable to save document.",
                           "There was an error saving a document for this product. {0}", x =>
                               {
                                   _documentCharacteristicProvider.FillCharacteristic(containerId, productUploadResult,
                                                                                      cachedCharacteristics, scratchFiles,
                                                                                      containerImportFiles, productCharacteristic,
                                                                                      new List<SearchResult>());
                               }
            );
                productCharacteristic.UpdatedDateTime = DateTime.UtcNow;
                productUploadResult.CreatedDateTime = productCharacteristic.UpdatedDateTime;
                productCharacteristic.Id = Guid.NewGuid();
                productCharacteristic.ProductId = productUploadResult.Product.Id.Value;
                if (!productCharacteristic.IsMultivalueAllowed && !productCharacteristic.IsRangeAllowed &&
                    !ProductCharacteristicChildManager.ValidateDataType(productUploadResult, productCharacteristic,
                                                                        productCharacteristic.Value)) 
                    continue;
                TryExecute(_logManager, productUploadResult, productCharacteristic, "Unable to save value.",
                           "There was an error saving a value for this product. {0}",
                           x => _productCharacteristicRepository.Add(x));

                _productCharacteristicChildManager.SaveMultiRangeValues(productCharacteristic, productUploadResult, cachedCharacteristics);
            }

            return containerImportFiles;
        }

        /// <summary>
        ///     Tries the execute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logManager">The log manager.</param>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="titleFormat">The title format.</param>
        /// <param name="detailFormat">The detail format.</param>
        /// <param name="action">The action.</param>
        public static void TryExecute<T>(ILogManager logManager, ProductUploadResult productUploadResult, T entity,
                                         string titleFormat, string detailFormat, Action<T> action)
            where T : TrackedDomainEntity
        {
            try
            {
                action(entity);
            }
            catch (DatabaseItemNotFoundException ex)
            {
                productUploadResult.Messages.Add(new ProductUploadMessage
                    {
                        MessageType = ProductUploadMessageTypeEnumDto.Error,
                        Title = titleFormat,
                        Detail =
                            string.Format("{0}\n{1}", string.Format(detailFormat, productUploadResult.Id),
                                          "The item was not found."),
                        CreatedDateTime = DateTime.UtcNow,
                        UpdatedDateTime = DateTime.UtcNow,
                    });
                productUploadResult.IsValid = false;
                LogMessage logMessage = ex.ToLogMessage(MessageIds.ProductUploadInsertManagerNotFoundException, LogCategory.ProductUploadImportManager, LogPriority.Medium,
                                                        TraceEventType.Error);
                logMessage.Data.Add("ProductUploadResult",
                                    string.Format("ProductUploadResult:{0}", productUploadResult.Id));
                logManager.Log(logMessage);
            }
            catch (Exception ex)
            {
                productUploadResult.Messages.Add(new ProductUploadMessage
                    {
                        MessageType = ProductUploadMessageTypeEnumDto.Error,
                        Title = titleFormat,
                        Detail = string.Format(detailFormat, productUploadResult.Id),
                        CreatedDateTime = DateTime.UtcNow,
                        UpdatedDateTime = DateTime.UtcNow,
                    });
                productUploadResult.IsValid = false;
                LogMessage logMessage = ex.ToLogMessage(MessageIds.ProductUploadInsertManagerException, LogCategory.ProductUploadImportManager, LogPriority.Medium,
                                                        TraceEventType.Error);
                logMessage.Data.Add("ProductUploadResult",
                                    string.Format("ProductUploadResult:{0}", productUploadResult.Id));
                logManager.Log(logMessage);
            }
        }
    }
}