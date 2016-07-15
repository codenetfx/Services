using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    ///     Defines operations to Validate <see cref="ProductCharacteristic" /> objects.
    /// </summary>
    public interface ICharacteristicValidator
    {
        /// <summary>
        ///     Validates the instance.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="characteristics"></param>
        /// <param name="characteristicDefinitions">The characteristic definitions.</param>
        /// <param name="errors">The errors.</param>
        void Validate(Product product, IEnumerable<ProductCharacteristic> characteristics, IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> characteristicDefinitions, List<string> errors);
    }
}