using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.Linq;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    /// Validates required <see cref="ProductCharacteristic"/> objects on <see cref="Product"/> objetcs
    /// </summary>
    public class ProductRequiredCharacteristicValidator : IProductValidator
    {
        /// <summary>
        ///     Validates the instance.
        /// </summary>
        /// <param name="entityToValidate">The entity to validate.</param>
        /// <param name="characteristicDefinitions">The characteristic definitions.</param>
        /// <param name="errors">The errors.</param>
        public void Validate(Product entityToValidate, IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> characteristicDefinitions, List<string> errors)
        {
            var requiredCharacteristics = characteristicDefinitions.Values.Where(x => x.IsValueRequired);


            foreach (var requiredCharacteristic in requiredCharacteristics)
            {
                if (!requiredCharacteristic.IsValueRequired)
                    continue;
                if(!entityToValidate.Characteristics.Any(x => x.ProductFamilyCharacteristicId == requiredCharacteristic.Id.Value && !String.IsNullOrEmpty(x.Value)))
                    errors.Add(string.Format("A value is required for attribute {0}.", requiredCharacteristic.Name));
            }
        }
    }
}