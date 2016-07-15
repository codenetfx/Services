using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines operations that help manage grouping <see cref="ProductFamilyCharacteristicDomainEntity" /> objects. 
    /// </summary>
    public interface IProductFamilyCharacteristicGroupHelper
    {
        /// <summary>
        /// Groups the characteristics.
        /// </summary>
        /// <param name="characteristics">The characteristics.</param>
        /// <param name="baseCharacteristics">The base characteristics.</param>
        /// <param name="variableCharacteristics">The variable characteristics.</param>
        void GroupCharacteristics(
            IEnumerable<ProductFamilyCharacteristicDomainEntity> characteristics
            , out IEnumerable<ProductFamilyCharacteristicDomainEntity> baseCharacteristics
            , out IEnumerable<ProductFamilyCharacteristicDomainEntity> variableCharacteristics
            );
    }
}