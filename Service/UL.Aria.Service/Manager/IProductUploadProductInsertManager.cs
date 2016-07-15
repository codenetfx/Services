using System;
using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Defines operations for adding <see cref="Product" /> objects while importing.
    /// </summary>
    public interface IProductUploadProductInsertManager
    {
        /// <summary>
        ///     Persists the specified product upload result.
        /// </summary>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="uploadId">The upload id.</param>
        void Persist(ProductUploadResult productUploadResult, Guid uploadId);

        /// <summary>
        ///     Persists the characteristics for new product.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="cachedCharacteristics">The cached characteristics.</param>
        /// <param name="scratchFiles">The scratch files.</param>
        /// <returns></returns>
        IList<ContainerImportFile> PersistCharacteristics(Guid containerId, ProductUploadResult productUploadResult,
                                                          IDictionary<Guid, ProductFamilyCharacteristicDomainEntity>
                                                              cachedCharacteristics,
                                                          IEnumerable<ScratchFileUpload> scratchFiles);
    }
}