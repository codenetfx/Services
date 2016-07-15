using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines operations for storing documents associated with an upload.
    /// </summary>
    public interface IProductUploadDocumentImportManager
    {
        /// <summary>
        /// Stores the documents for product.
        /// </summary>
        /// <param name="productUpload">The product upload.</param>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="productContainerId"></param>
        /// <param name="cachedCharacteristics">The cached characteristics.</param>
        /// <param name="scratchFiles">The scratch files.</param>
        /// <param name="defaultPermission"></param>
        /// <param name="filesToImport"></param>
        void StoreDocumentsForProduct(ProductUpload productUpload, ProductUploadResult productUploadResult, Guid productContainerId, IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> cachedCharacteristics, IEnumerable<ScratchFileUpload> scratchFiles, DocumentPermissionEnumDto defaultPermission, IList<ContainerImportFile> filesToImport);
    }
}