using System;
using System.Collections.Generic;
using System.Linq;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    ///     Validates <see cref="Product" /> objects.
    /// </summary>
    public class ProductValidationManager : IProductValidationManager
    {
        private readonly IEnumerable<ICharacteristicValidator> _attributeValidators;
        private readonly IEnumerable<ICharacteristicValidator> _featureValidators;
        private readonly IProductFamilyManager _productFamilyManager;
        private readonly IEnumerable<IProductValidator> _productValidators;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductValidationManager" /> class.
        /// </summary>
        /// <param name="productFamilyManager">The product family manager.</param>
        /// <param name="productValidators">The product validators.</param>
        /// <param name="attributeValidators">The attribute validators.</param>
        /// <param name="featureValidators">The feature validators.</param>
        public ProductValidationManager(IProductFamilyManager productFamilyManager, IEnumerable<IProductValidator> productValidators, IEnumerable<ICharacteristicValidator> attributeValidators, IEnumerable<ICharacteristicValidator> featureValidators)
        {
            _productFamilyManager = productFamilyManager;
            _productValidators = productValidators;
            _attributeValidators = attributeValidators;
            _featureValidators = featureValidators;
        }

        /// <summary>
        ///     Validates the specified entity to validate.
        /// </summary>
        /// <param name="entityToValidate">The entity to validate.</param>
        public IList<string> Validate(Product entityToValidate)
        {
            Dictionary<Guid, ProductFamilyCharacteristicDomainEntity> cachedCharacteristics = _productFamilyManager.GetProductFamilyCharacteristics(entityToValidate.ProductFamilyId).ToDictionary(y => y.Id.Value);
            
            var errors = new List<string>();
            foreach (var validator in _productValidators)
            {
                validator.Validate(entityToValidate, cachedCharacteristics, errors);
            }
                foreach (var validator in _attributeValidators)
                {
                    validator.Validate(entityToValidate, entityToValidate.Characteristics.Where(x => x.ProductFamilyCharacteristicType == ProductFamilyCharacteristicType.Attribute), cachedCharacteristics, errors);
                }
            
                foreach (var validator in _featureValidators)
                {
                    validator.Validate(entityToValidate, entityToValidate.Characteristics.Where(x => x.ProductFamilyCharacteristicType == ProductFamilyCharacteristicType.Feature), cachedCharacteristics, errors);
                }
            
            return errors;
        }
    }
}