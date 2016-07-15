using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Defines operations for mananging document characteristics.
    /// </summary>
    public interface IProductUploadDocumentCharacteristicProvider
    {
        /// <summary>
        /// Fills the characteristic.
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="cachedCharacteristics">The cached characteristics.</param>
        /// <param name="scratchFiles">The scratch files.</param>
        /// <param name="externalDocuments">The external documents.</param>
        void FillCharacteristics(Guid containerId, ProductUploadResult productUploadResult, IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> cachedCharacteristics, IEnumerable<ScratchFileUpload> scratchFiles, IList<ContainerImportFile> externalDocuments);

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
        void FillCharacteristic(Guid containerId, ProductUploadResult productUploadResult, IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> cachedCharacteristics, IEnumerable<ScratchFileUpload> scratchFiles, IList<ContainerImportFile> externalDocuments, ProductCharacteristic productCharacteristic, IList<SearchResult> results);
    }
}