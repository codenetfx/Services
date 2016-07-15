using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    ///     Defines operations to Validate <see cref="Product" /> objects for a specific rule.
    /// </summary>
    public interface IProductValidator
    {
        /// <summary>
        ///     Validates the instance.
        /// </summary>
        /// <param name="entityToValidate">The entity to validate.</param>
        /// <param name="characteristicDefinitions">The characteristic definitions.</param>
        /// <param name="errors">The errors.</param>
        void Validate(Product entityToValidate, IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> characteristicDefinitions, List<string> errors);
    }
}