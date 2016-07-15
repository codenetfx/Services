using System;
using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Defines operations for updating <see cref="Product" /> objects while importing.
    /// </summary>
    public interface IProductUploadProductUpdateManager
    {
        /// <summary>
        ///     Persists the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="uploadId">The upload id.</param>
        void Persist(Guid id, ProductUploadResult productUploadResult, Guid uploadId);

        /// <summary>
        ///     Persists the characteristics for existing product.
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="cachedCharacteristics">The cached characteristics.</param>
        /// <param name="scratchFiles"></param>
        IList<ContainerImportFile> PersistCharacteristics(Guid containerId, ProductUploadResult productUploadResult,
                                                          IDictionary<Guid, ProductFamilyCharacteristicDomainEntity>
                                                              cachedCharacteristics,
                                                          IEnumerable<ScratchFileUpload> scratchFiles);
    }
}