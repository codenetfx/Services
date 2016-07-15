using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Interface IProductCharacteristicChildManager
    /// </summary>
    public interface IProductCharacteristicChildManager
    {
        /// <summary>
        /// Saves the multi range values.
        /// </summary>
        /// <param name="productCharacteristic">The product characteristic.</param>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="cachedCharacteristics">The cached characteristics.</param>
        void SaveMultiRangeValues(ProductCharacteristic productCharacteristic,
                                  ProductUploadResult productUploadResult
                                    , IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> cachedCharacteristics);

        /// <summary>
        ///     Deletes the children.
        /// </summary>
        /// <param name="id">The id.</param>
        void DeleteChildren(Guid id);
    }
}