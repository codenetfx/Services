using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using UL.Aria.Service.Logging;
using UL.Enterprise.Foundation.Logging;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Implements operations for storing documents associated with an upload.
    /// </summary>
    public class ProductUploadDocumentImportManager : IProductUploadDocumentImportManager
    {
        private readonly IAssetProvider _assetProvider;
        private readonly ILogManager _logManager;
	    private readonly IDocumentContentProvider _documentContentProvider;
	    private readonly IScratchSpaceService _scratchSpaceService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProductUploadDocumentImportManager" /> class.
		/// </summary>
		/// <param name="assetProvider">The content provider.</param>
		/// <param name="scratchSpaceRepository">The scratch space repository.</param>
		/// <param name="logManager">The log manager.</param>
		/// <param name="documentContentProvider">The document content provider.</param>
        public ProductUploadDocumentImportManager(IAssetProvider assetProvider,
                                                  IScratchSpaceService scratchSpaceRepository, ILogManager logManager, IDocumentContentProvider documentContentProvider)
        {
            _assetProvider = assetProvider;
            _scratchSpaceService = scratchSpaceRepository;
            _logManager = logManager;
			_documentContentProvider = documentContentProvider;
        }

        /// <summary>
        ///     Stores the documents for product.
        /// </summary>
        /// <param name="productUpload">The product upload.</param>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="productContainerId"></param>
        /// <param name="cachedCharacteristics">The cached characteristics.</param>
        /// <param name="scratchFiles">The scratch files.</param>
        /// <param name="defaultPermission"></param>
        /// <param name="filesToImport"></param>
        public void StoreDocumentsForProduct(ProductUpload productUpload, ProductUploadResult productUploadResult, Guid productContainerId, IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> cachedCharacteristics, IEnumerable<ScratchFileUpload> scratchFiles, DocumentPermissionEnumDto defaultPermission, IList<ContainerImportFile> filesToImport)
        {
            var documentAttributeCharacteristics = cachedCharacteristics.Values.Where(x =>
                {
                    var attribute = x as ProductFamilyAttribute;
                    return (null != attribute && ProductFamilyCharacteristicDataType.DocumentReference == attribute.DataTypeId);
                });
            var product = productUploadResult.Product;

            var productDocumentCharacteristics =
                product.Characteristics.Where(
                    productCharacteristic =>
                    documentAttributeCharacteristics.Any(
                        familyCharacteristic =>
                        familyCharacteristic.Id == productCharacteristic.ProductFamilyCharacteristicId));

            var persistedFiles = new List<Guid>();
            foreach (var productDocumentCharacteristic in productDocumentCharacteristics)
            {
                Guid documentId;
                if (!Validate(productUploadResult, productDocumentCharacteristic, out documentId)) continue;
                if (persistedFiles.Any(x => x == documentId))
                {
                    continue;
                }
                persistedFiles.Add(documentId);
                try
                {
                    var scratchFile = scratchFiles.FirstOrDefault(x => x.Id == documentId);
                    if (null != scratchFile)
                    {
                        PersistScratchDocument(productUpload, productContainerId, documentId, scratchFile.Id.Value, scratchFile.ScratchFileInfo, product, defaultPermission);
                        continue;
                    }
                    var externalFile  =filesToImport.FirstOrDefault(x=> x.NewId == documentId);
                    if (null != externalFile)
                    {
                        PersistImportedDocument(productUpload, productContainerId, externalFile.OriginalId, externalFile.NewId, defaultPermission);
                        continue;
                    }
                    var documentMetadata = _assetProvider.Fetch(documentId);
                    if (null == documentMetadata || documentMetadata.Count == 0)
                    {
                        HandleExistingDocument(productUploadResult, productDocumentCharacteristic, documentId);
                    }

                    var message = String.Format("Could not find document {0}",
                        productDocumentCharacteristic.Value);
                    AddMessage(productUploadResult, "Unable to import document",
                               message);
                    productUploadResult.IsValid = false;
                    var logMessage = new LogMessage(MessageIds.ProductUploadDocumentImportMissingDocument, LogPriority.Low, TraceEventType.Information, message, LogCategory.ProductUploadImportManager);
                    logMessage.Data.Add("ProductUploadResult",
                                        string.Format("ProductUploadResult:{0}", productUploadResult.Id));
                    logMessage.Data.Add("Document Id", documentId.ToString());
                    _logManager.Log(logMessage);
                }
                catch (Exception ex)
                {
                    AddMessage(productUploadResult, "Unable to import document",
                               String.Format("An error was encountered importing the document {0}",
                                             productDocumentCharacteristic.Value));
                    var logMessage = ex.ToLogMessage(MessageIds.ProductUploadDocumentImportError,LogCategory.ProductUploadImportManager, LogPriority.Medium,
                                                     TraceEventType.Error);
                    logMessage.LogCategories.Add(LogCategory.ProductUploadImportManager);
                    logMessage.Data.Add("ProductUploadResult",
                                        string.Format("ProductUploadResult:{0}", productUploadResult.Id));
                    logMessage.Data.Add("Document Id", documentId.ToString());
                    _logManager.Log(logMessage);
                    productUploadResult.IsValid = false;
                }
            }
        }

        private bool Validate(ProductUploadResult productUploadResult,
                              ProductCharacteristic productDocumentCharacteristic, out Guid documentId)
        {
            if (string.IsNullOrEmpty(productDocumentCharacteristic.Value))
            {
                documentId = Guid.Empty; 
                return false;
            }

            if (!Guid.TryParse(productDocumentCharacteristic.Value, out documentId))
            {
                AddMessage(productUploadResult, "Document Identifier was not correct format.",
                           string.Format(
                               "{0}\n{1} was not a valid Unique Identifier value. Value must be 32 alphanumeric characters or 32 alphanumeric characters in the format xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.",
                               productDocumentCharacteristic.Id, productDocumentCharacteristic.Value));
                var logMessage = new LogMessage(MessageIds.ProductUploadDocumentIdentifierInvalidFormat) {LogPriority = LogPriority.Medium, Severity = TraceEventType.Error};
                logMessage.LogCategories.Add(LogCategory.ProductUploadImportManager);
                logMessage.Data.Add("ProductUploadResult",
                                    string.Format("ProductUploadResult:{0}", productUploadResult.Id));
                logMessage.Data.Add("Document Id", documentId.ToString());
                _logManager.Log(logMessage);
                return false;
            }
            return true;
        }

        private void HandleExistingDocument(ProductUploadResult productUploadResult,
                                            ProductCharacteristic productDocumentCharacteristic, Guid documentId)
        {
            AddMessage(productUploadResult, "The document was not found",
                       string.Format("{0}\nDocument {1} was not found", productDocumentCharacteristic.Id,
                                     productDocumentCharacteristic.Value));
            productUploadResult.Messages.Add(new ProductUploadMessage
                {
                    MessageType = ProductUploadMessageTypeEnumDto.Error,
                    Title = "The document was not found",
                    Detail = string.Format("{0}\n{1}", productUploadResult.Id, "The document was not found."),
                    CreatedDateTime = DateTime.UtcNow,
                    UpdatedDateTime = DateTime.UtcNow,
                });
            productUploadResult.IsValid = false;
            var logMessage = new LogMessage(MessageIds.ProductUploadDocumentExistingDocumentNotFonund) { LogPriority = LogPriority.Medium, Severity = TraceEventType.Error };
            logMessage.LogCategories.Add(LogCategory.ProductUploadImportManager);
            logMessage.Data.Add("ProductUploadResult", string.Format("ProductUploadResult:{0}", productUploadResult.Id));
            logMessage.Data.Add("Document Id", documentId.ToString());
            _logManager.Log(logMessage);
            productUploadResult.IsValid = false;
        }

        internal void PersistScratchDocument(ProductUpload productUpload, Guid productContainerId, Guid documentId, Guid newDocumentId, ScratchFileInfo scratchFile, Product product, DocumentPermissionEnumDto defaultPermission)
        {
            var content = _scratchSpaceService.FetchContent(productUpload.CreatedById.ToString(), scratchFile.Id.ToString());
            var dict = new Dictionary<string, string>
                {
                    {AssetFieldNames.AriaAssetType, AssetTypeEnumDto.Document.ToString()},
                    {AssetFieldNames.AriaContentType, scratchFile.Extension},
                    {AssetFieldNames.AriaName, scratchFile.Name},
                    {AssetFieldNames.AriaTitle, scratchFile.Name},
                    {AssetFieldNames.AriaProductDescription, scratchFile.Name},
                    {AssetFieldNames.AriaPermission, defaultPermission.ToString()},
                    {AssetFieldNames.AriaDocumentTypeId, "c3da7c2b-2350-4306-964b-3ea5b8846abc"},// this equals "Unspecified"
                    {AssetFieldNames.AriaSize,  scratchFile.Size.ToString()},
                    {AssetFieldNames.AriaLastModifiedBy, productUpload.CreatedByUserLoginId},
                    {AssetFieldNames.AriaLastModifiedOn, scratchFile.LastWriteTimeUtc.ToString()}
                };
           
            _assetProvider.Create(productContainerId, dict, newDocumentId);

	        _documentContentProvider.Create(newDocumentId, scratchFile.Extension, content, false);
        }

        internal void PersistImportedDocument(ProductUpload productUpload, Guid productContainerId, Guid documentId, Guid newDocumentId, DocumentPermissionEnumDto defaultPermission)
        {
            var metadata = _assetProvider.Fetch(documentId);
            metadata[AssetFieldNames.AriaPermission] = defaultPermission.ToString();
            _assetProvider.Create(productContainerId, metadata, newDocumentId);

			_documentContentProvider.Create(newDocumentId, "", _documentContentProvider.FetchById(documentId), false);
        }

        private void AddMessage(ProductUploadResult productUploadResult, string title, string detail)
        {
            productUploadResult.Messages.Add(new ProductUploadMessage
                {
                    MessageType = ProductUploadMessageTypeEnumDto.Error,
                    Title = title,
                    Detail = detail,
                    CreatedDateTime = DateTime.UtcNow,
                    UpdatedDateTime = DateTime.UtcNow,
                });
        }
    }
}