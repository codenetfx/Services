using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    /// Validates <see cref="ProductCharacteristic"/> objects of type date time.
    /// </summary>
    public class ProductCharacteristicTypeValidator<T> : ICharacteristicValidator
    {
        private readonly ProductFamilyCharacteristicDataType _expectedType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCharacteristicTypeValidator{T}"/> class.
        /// </summary>
        /// <param name="expectedType">The expected type.</param>
        public ProductCharacteristicTypeValidator(ProductFamilyCharacteristicDataType expectedType)
        {
            _expectedType = expectedType;
        }

        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="characteristics"></param>
        /// <param name="characteristicDefinitions">The characteristic definitions.</param>
        /// <param name="errors">The errors.</param>
        public void Validate(Product product, IEnumerable<ProductCharacteristic> characteristics, IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> characteristicDefinitions, List<string> errors)
        {
            var failures =
                characteristics.Where(c => c.DataType == _expectedType
                                           &&
                                           !String.IsNullOrEmpty(c.Value) // handle this with is required.
                                           && !TypeDescriptor.GetConverter(typeof(T)).IsValid(c.Value));
            errors.AddRange(
                    failures.Where(fd => characteristicDefinitions.ContainsKey(fd.ProductFamilyCharacteristicId)).Select(f =>
                        string.Format("There was not a valid {0} value for attribute {1}.", _expectedType.ToString().SpaceIt().ToLower(),characteristicDefinitions[f.ProductFamilyCharacteristicId].Name)
                    )

                );

        }
    }
}