using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    /// Validates dependencties for product features.
    /// </summary>
    public class ProductFeatureDependencyValidator : ProductFeatureValueValidator
    {
        private IProductFamilyFeatureProvider _productFamilyFeatureProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFeatureDependencyValidator"/> class.
        /// </summary>
        /// <param name="productFamilyFeatureProvider">The product family feature provider.</param>
        public ProductFeatureDependencyValidator(IProductFamilyFeatureProvider productFamilyFeatureProvider) : base(productFamilyFeatureProvider)
        {
            _productFamilyFeatureProvider = productFamilyFeatureProvider;
            
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
        internal override void ValidateValues(IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> characteristicDefinitions, List<string> errors, ProductCharacteristic productCharacteristic, IEnumerable<ProductCharacteristic> characteristics, Dictionary<Guid, IEnumerable<ProductFamilyFeatureAllowedValue>> foundValues, string[] values)
        {
            IEnumerable<ProductFamilyFeatureAllowedValue> featureFoundValues;
            
            if (!foundValues.TryGetValue(productCharacteristic.ProductFamilyCharacteristicId, out featureFoundValues))
            {
                return;
            }
            foreach (var value in featureFoundValues.Where(f => values.Any(v => v == f.FeatureValue.Value)))
            {
                var dependencies = _productFamilyFeatureProvider.GetValueDependenciesByParentFamilyAllowedFeatureValueId(value.Id.Value);
                if (!dependencies.Any())
                    continue;
                var grouped =  foundValues.Where(x => x.Value.Any(v => dependencies.Any(d => d.ChildProductFamilyFeatureAllowedValueId == v.Id))).ToDictionary(x=> x.Key);
                bool wasFound = false;
                foreach (var key in grouped.Keys)
                {
                    wasFound = true;
                    var dependentCharacteristic = characteristics.FirstOrDefault(x => x.ProductFamilyCharacteristicId == key);
                    var expected = grouped[key].Value.Where(x => dependencies.Any(y => y.ChildProductFamilyFeatureAllowedValueId == x.Id));
                    var name = "There was not a valid value found for {0}";
                    if (characteristicDefinitions.ContainsKey(key))
                    {
                        name = string.Format(name, characteristicDefinitions[key].Name);
                    }
                    else
                    {
                        try
                        {
                            name = string.Format(name, _productFamilyFeatureProvider.Get(key).Name);
                        }
                        catch (Exception)
                        {
                            name = "A dependency was missing";
                        }
                    }
                    if (null == dependentCharacteristic)
                    {

                        errors.Add(string.Format("Feature {0} requires one or more dependent values.{1}.", characteristicDefinitions[productCharacteristic.ProductFamilyCharacteristicId].Name, name));
                        continue;
                    }
                    
                    
                    if (!expected.Any(c =>   dependentCharacteristic.ParseMultiValue(false).Any(v => v == c.FeatureValue.Value)))
                    {
                        errors.Add(string.Format("Feature {0} requires one or more dependent values. {1}. Valid values are {2}", characteristicDefinitions[productCharacteristic.ProductFamilyCharacteristicId].Name, name, string.Join(",", expected.Select(x=> x.FeatureValue.Value))));
                    }
                    //if (!dependencies.Any(x => foundValues.Any(y => y.Value.Any(z => z.Id == x.ChildProductFamilyFeatureAllowedValueId))))
                    //{
                        
                    //}
                }
                if (!wasFound)
                {
                    errors.Add(string.Format("Feature {0} requires one or more dependent values.", characteristicDefinitions[productCharacteristic.ProductFamilyCharacteristicId].Name));
                }
            }
        }
    }
}