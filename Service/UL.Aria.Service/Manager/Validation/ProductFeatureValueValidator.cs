using System;
using System.Collections.Generic;
using System.Linq;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    /// Validates values of a feature for a product.
    /// </summary>
    public class ProductFeatureValueValidator : ICharacteristicValidator
    {
        private readonly IProductFamilyFeatureProvider _productFamilyFeatureProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFeatureValueValidator"/> class.
        /// </summary>
        /// <param name="productFamilyFeatureProvider">The product family feature provider.</param>
        public ProductFeatureValueValidator(IProductFamilyFeatureProvider productFamilyFeatureProvider)
        {
            _productFamilyFeatureProvider = productFamilyFeatureProvider;
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
            var features = characteristicDefinitions.Where(x => x.Value is ProductFamilyFeature).Select(x => x.Value).Cast<ProductFamilyFeature>();


            var foundValues = new Dictionary<Guid, IEnumerable<ProductFamilyFeatureAllowedValue>>();
            foreach (ProductFamilyFeature feature in features)
            {
                foundValues.Add(feature.Id.Value, _productFamilyFeatureProvider.FindAllowedValues(feature.Id.Value, product.ProductFamilyId));
            }
            foreach (var productCharacteristic in characteristics.Where(c => c.ProductFamilyCharacteristicType == ProductFamilyCharacteristicType.Feature))
            {
                if (string.IsNullOrEmpty(productCharacteristic.Value))
                    continue;

                ValidateValues(characteristicDefinitions, errors, productCharacteristic, characteristics,foundValues, productCharacteristic.ParseMultiValue(false));
            }
        }

        /// <summary>
        /// Validates the values.
        /// </summary>
        /// <param name="characteristicDefinitions">The characteristic definitions.</param>
        /// <param name="errors">The errors.</param>
        /// <param name="productCharacteristic">The product characteristic.</param>
        /// <param name="characteristics">The characteristics.</param>
        /// <param name="foundValues">The found values.</param>
        /// <param name="values">The values.</param>
        internal virtual void ValidateValues(IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> characteristicDefinitions, List<string> errors, ProductCharacteristic productCharacteristic, IEnumerable<ProductCharacteristic> characteristics, Dictionary<Guid, IEnumerable<ProductFamilyFeatureAllowedValue>> foundValues, string[] values)
        {
            if (!((ProductFamilyFeature) characteristicDefinitions[productCharacteristic.ProductFamilyCharacteristicId]).AllowChanges)
            {
                if (!foundValues[productCharacteristic.ProductFamilyCharacteristicId].Any(c => values.Any(v => v == c.FeatureValue.Value)))
                {
                    errors.Add(string.Format("There was not a valid value for feature {0}.", characteristicDefinitions[productCharacteristic.ProductFamilyCharacteristicId].Name));
                }
            }
        }
    }
}