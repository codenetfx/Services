using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using UL.Aria.Common;
using UL.Aria.Service.Logging;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Logging;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Implements operations for mananging document characteristics.
    /// </summary>
    public class ProductUploadDocumentCharacteristicProvider : IProductUploadDocumentCharacteristicProvider
    {
        
        private readonly ILogManager _logManager;
        
        private readonly IAssetProvider _assetProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductUploadDocumentCharacteristicProvider" /> class.
        /// </summary>
        /// <param name="assetProvider">The asset provider.</param>
        /// <param name="logManager">The log manager.</param>
        public ProductUploadDocumentCharacteristicProvider(IAssetProvider assetProvider ,ILogManager logManager)
        {
            
            _assetProvider = assetProvider;
            _logManager = logManager;
        }

        /// <summary>
        /// Fills the characteristic.
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="cachedCharacteristics">The cached characteristics.</param>
        /// <param name="scratchFiles">The scratch files.</param>
        /// <param name="externalDocuments"></param>
        public void FillCharacteristics(Guid containerId, ProductUploadResult productUploadResult, IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> cachedCharacteristics, IEnumerable<ScratchFileUpload> scratchFiles, IList<ContainerImportFile> externalDocuments)
        {
            foreach (var productCharacteristic in productUploadResult.Product.Characteristics.Where(x=> x.ProductFamilyCharacteristicType == ProductFamilyCharacteristicType.Attribute))
            {
                FillCharacteristic(containerId, productUploadResult, cachedCharacteristics, scratchFiles, externalDocuments, productCharacteristic, new List<SearchResult>());
            }
        }

        /// <summary>
        /// Fills the characteristic.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="cachedCharacteristics">The cached characteristics.</param>
        /// <param name="scratchFiles">The scratch files.</param>
        /// <param name="externalDocuments">The external documents.</param>
        /// <param name="productCharacteristic">The product characteristic.</param>
        /// <param name="results"></param>
        public void FillCharacteristic(Guid containerId, ProductUploadResult productUploadResult, IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> cachedCharacteristics, IEnumerable<ScratchFileUpload> scratchFiles, IList<ContainerImportFile> externalDocuments, ProductCharacteristic productCharacteristic, IList<SearchResult> results)
        {
            var productFamilyAttribute = cachedCharacteristics[productCharacteristic.ProductFamilyCharacteristicId] as ProductFamilyAttribute;
            if (null == productFamilyAttribute)
                return;

            if (productFamilyAttribute.DataTypeId != ProductFamilyCharacteristicDataType.DocumentReference)
                return;
            Guid documentId;
            if (!Guid.TryParse(productCharacteristic.Value, out documentId))
                return;
            var document = results.FirstOrDefault(x => x.Id == documentId);
            if (null != document)
                return;

            var scratchFile = scratchFiles.FirstOrDefault(x => x.ScratchFileInfo.Id == documentId);
            if (null != scratchFile)
            {
                productCharacteristic.Value = scratchFile.Id.ToString();
                return;
            }
            var externalFile = externalDocuments.FirstOrDefault(x => x.OriginalId == documentId);
            if (null != externalFile)
            {
                productCharacteristic.Value = externalFile.NewId.ToString();
                return;
            }
           
            var documentF = _assetProvider.Fetch(documentId);
            if (null == documentF || documentF.Count == 0)
            {
                HandleMissingDocument(productUploadResult, documentId, "Unable to locate document for import.");
                return;
            }
            var foundContainerId = documentF.GetValue(AssetFieldNames.AriaContainerId, default(Guid));
            if (foundContainerId == default(Guid))
            {
                HandleMissingDocument(productUploadResult, documentId, "Entity specified for document import was not part of a container.");
                return;
            }
            if (foundContainerId == containerId)
                return;
            var containerImportFile = new ContainerImportFile { OriginalContainerId = foundContainerId, OriginalId = documentId, NewId = Guid.NewGuid() };
            externalDocuments.Add(containerImportFile);
            productCharacteristic.Value = containerImportFile.NewId.ToString();
            
        }

        private void HandleMissingDocument(ProductUploadResult productUploadResult, Guid documentId, string message)
        {
            var logMessage = new LogMessage(MessageIds.ProductUploadDocumentImportMissingDocument, LogPriority.Medium,
                                            TraceEventType.Error, message, LogCategory.ProductUploadImportManager);
            logMessage.LogCategories.Add(LogCategory.ProductUploadImportManager);
            logMessage.Data.Add("ProductUploadResult",
                                string.Format("ProductUploadResult:{0}", productUploadResult.Id));
            logMessage.Data.Add("Document Id", documentId.ToString());
            _logManager.Log(logMessage);
            productUploadResult.IsValid = false;
            productUploadResult.Messages.Add(new ProductUploadMessage
            {
                MessageType = ProductUploadMessageTypeEnumDto.Error,
                Title = message,
                Detail = string.Format("{0}. The specified document id was {1}", message, documentId),
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
            });
        }
    }
}