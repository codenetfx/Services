using System;
using System.Collections.Generic;
using System.Linq;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Implements operations for locating <see cref="ProductFamilyCharacteristicDomainEntity"/> data for a <see cref="ProductCharacteristic"/>
    /// </summary>
    public class ProductUploadFamilyCharacteristicProvider : IProductUploadFamilyCharacteristicProvider
    {
        private readonly IProductFamilyAttributeProvider _productFamilyAttributeProvider;
        private readonly IProductFamilyFeatureProvider _productFamilyFeatureProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductUploadFamilyCharacteristicProvider"/> class.
        /// </summary>
        /// <param name="productFamilyAttributeProvider">The product family attribute provider.</param>
        /// <param name="productFamilyFeatureProvider">The product family feature provider.</param>
        public ProductUploadFamilyCharacteristicProvider(IProductFamilyAttributeProvider productFamilyAttributeProvider, IProductFamilyFeatureProvider productFamilyFeatureProvider)
        {
            _productFamilyAttributeProvider = productFamilyAttributeProvider;
            _productFamilyFeatureProvider = productFamilyFeatureProvider;
        }

        /// <summary>
        /// Fills the characteristic.
        /// </summary>
        /// <param name="familyId"></param>
        /// <param name="characteristic">The characteristic.</param>
        /// <param name="cachedCharacteristics">The cached characteristics.</param>
        /// <param name="scratchFiles"></param>
        public void FillCharacteristic(Guid familyId, ProductCharacteristic characteristic, IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> cachedCharacteristics, IEnumerable<ScratchFileUpload> scratchFiles)
        {
            ProductFamilyCharacteristicDomainEntity characteristicDefinition;

            if (cachedCharacteristics.TryGetValue(characteristic.ProductFamilyCharacteristicId, out characteristicDefinition))
            {
            }
            else if (characteristic.ProductFamilyCharacteristicType == ProductFamilyCharacteristicType.Attribute)
            {
                characteristicDefinition = _productFamilyAttributeProvider.Get(characteristic.ProductFamilyCharacteristicId);
                cachedCharacteristics.Add(characteristicDefinition.Id.Value, characteristicDefinition);
            }   
            else
            {
                characteristicDefinition = _productFamilyFeatureProvider.Get(characteristic.ProductFamilyCharacteristicId);
                cachedCharacteristics.Add(characteristicDefinition.Id.Value, characteristicDefinition);
            }
            if (characteristic.ProductFamilyCharacteristicType == ProductFamilyCharacteristicType.Attribute)
            {
                characteristic.DataType = ((ProductFamilyAttribute) characteristicDefinition).DataTypeId;
            }
            characteristic.Name = characteristicDefinition.Name;
            characteristic.Description = characteristicDefinition.Description;
            characteristic.IsMultivalueAllowed = characteristicDefinition.IsMultivalueAllowed;
            characteristic.IsRangeAllowed = characteristicDefinition.IsRangeAllowed;
        }

        /// <summary>
        /// Gets the product family characteristics.
        /// </summary>
        /// <param name="familyId">The family id.</param>
        /// <returns></returns>
        public IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> GetProductFamilyCharacteristics(Guid familyId)
        {
            IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> cachedCharacteristics;
            cachedCharacteristics = _productFamilyAttributeProvider.GetByProductFamilyId(familyId)
                                                                   .Union<ProductFamilyCharacteristicDomainEntity>(_productFamilyFeatureProvider.GetByProductFamilyId(familyId))
                                                                   .ToDictionary(x => x.Id.Value, x => x);
            return cachedCharacteristics;
        }
    }
}