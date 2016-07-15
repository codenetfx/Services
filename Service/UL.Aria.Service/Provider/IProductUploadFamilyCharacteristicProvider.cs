using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Defines operations for locating <see cref="ProductFamilyCharacteristicDomainEntity"/> data for a <see cref="ProductCharacteristic"/>
    /// </summary>
    public interface IProductUploadFamilyCharacteristicProvider
    {
        /// <summary>
        /// Fills the characteristic.
        /// </summary>
        /// <param name="familyId"></param>
        /// <param name="characteristic">The characteristic.</param>
        /// <param name="cachedCharacteristics">The cached characteristics.</param>
        /// <param name="scratchFiles"></param>
        void FillCharacteristic(Guid familyId, ProductCharacteristic characteristic, IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> cachedCharacteristics, IEnumerable<ScratchFileUpload> scratchFiles);

        /// <summary>
        /// Gets the product family characteristics.
        /// </summary>
        /// <param name="familyId">The family id.</param>
        /// <returns></returns>
        IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> GetProductFamilyCharacteristics(Guid familyId);
    }
}