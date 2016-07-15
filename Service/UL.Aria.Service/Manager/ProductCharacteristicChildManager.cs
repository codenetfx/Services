using System;
using System.Collections.Generic;
using System.Linq;
using UL.Enterprise.Foundation.Logging;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Class ProductCharacteristicChildManager
    /// </summary>
    public class ProductCharacteristicChildManager : IProductCharacteristicChildManager
    {
        private readonly ILogManager _logManager;
        private readonly IProductCharacteristicRepository _productCharacteristicRepository;
        private readonly IProductFamilyFeatureProvider _familyFeatureProvider;
        private readonly IProductFamilyProvider _productFamilyProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCharacteristicChildManager" /> class.
        /// </summary>
        /// <param name="logManager">The log manager.</param>
        /// <param name="productCharacteristicRepository">The product characteristic repository.</param>
        /// <param name="familyFeatureProvider">The family feature provider.</param>
        /// <param name="productFamilyProvider">The family provider.</param>
        public ProductCharacteristicChildManager(ILogManager logManager,
            IProductCharacteristicRepository productCharacteristicRepository,
            IProductFamilyFeatureProvider familyFeatureProvider,
            IProductFamilyProvider productFamilyProvider
            )
        {
            _productCharacteristicRepository = productCharacteristicRepository;
            _familyFeatureProvider = familyFeatureProvider;
            _productFamilyProvider = productFamilyProvider;
            _logManager = logManager;
        }

        /// <summary>
        ///     Saves the multi range values.
        /// </summary>
        /// <param name="productCharacteristic">The product characteristic.</param>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <param name="cachedCharacteristics"></param>
        public void SaveMultiRangeValues(ProductCharacteristic productCharacteristic, ProductUploadResult productUploadResult, IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> cachedCharacteristics)
        {
            List<string> valuesFound = new List<string>();
            SaveMultiRangeValuesInternal(productCharacteristic, productUploadResult, valuesFound);
            ProductUploadProductInsertManager.TryExecute(_logManager, productUploadResult, productCharacteristic,
                "Unable to save value.",
                "There was an error saving new feature values for this product. {0}", x => 
            SaveOpenValues(productUploadResult, x, valuesFound, cachedCharacteristics));
        }

        private void SaveMultiRangeValuesInternal(ProductCharacteristic productCharacteristic, ProductUploadResult productUploadResult, List<string> valuesFound)
        {
            if (productCharacteristic.IsMultivalueAllowed)
            {
                var values = productCharacteristic.ParseMultiValue();

                if (productCharacteristic.IsRangeAllowed)
                    foreach (var value in values)
                    {
                        bool singleValue;
                        var ranges = SplitRange(value, out singleValue);
                        if (!ValidateDataType(productUploadResult, productCharacteristic, ranges[0], false)) return;
                        if (!singleValue)
                            if (!ValidateDataType(productUploadResult, productCharacteristic, ranges[1], false)) return;
                        SaveValue(productCharacteristic, productUploadResult, ranges[0], valuesFound);
                        if (!singleValue)
                            SaveValue(productCharacteristic, productUploadResult, ranges[1], valuesFound);
                    }
                else
                    foreach (var value in values)
                    {
                        if (ValidateDataType(productUploadResult, productCharacteristic, value))
                            SaveValue(productCharacteristic, productUploadResult, value, valuesFound);
                    }
            }
            else if (productCharacteristic.IsRangeAllowed)
            {
                bool singleValue;
                var ranges = SplitRange(productCharacteristic.Value, out singleValue);
                if (!ValidateDataType(productUploadResult, productCharacteristic, ranges[0], false)) return;
                if (!ValidateDataType(productUploadResult, productCharacteristic, ranges[1], false)) return;
                SaveValue(productCharacteristic, productUploadResult, ranges[0], valuesFound);
                SaveValue(productCharacteristic, productUploadResult, ranges[1], valuesFound);
            }
        }

        internal static string[] SplitRange(string value, out bool singleValue)
        {
            var ranges = new[] { "", "" };
            singleValue = true;

            if (!string.IsNullOrEmpty(value))
            {
                var valueTrimmed = value.Trim();

                if (valueTrimmed != "-")
                {
                    var splitIndex = valueTrimmed[0] == '-' ? value.IndexOf('-', 1) : value.IndexOf('-');

                    if (splitIndex == -1)
                        ranges[0] = value;
                    else
                    {
                        singleValue = false;
                        ranges[0] = value.Substring(0, splitIndex);

                        if (value.Length > splitIndex)
                            ranges[1] = value.Substring(splitIndex + 1);
                    }
                }
            }

            return ranges;
        }

        private void SaveValue(ProductCharacteristic productCharacteristic, ProductUploadResult productUploadResult, string value, List<string> valuesFound)
        {
            ProductCharacteristic newProductCharacteristic;
            newProductCharacteristic = CloneProductCharacteristicAsChild(productCharacteristic,
                ProductCharacteristicChildType
                    .RangeValue, value);
            ProductUploadProductInsertManager.TryExecute(_logManager, productUploadResult, newProductCharacteristic,
                "Unable to save value.",
                "There was an error saving a value for this product. {0}",
                x => _productCharacteristicRepository.Add(x));
            valuesFound.Add(value);
        }

        internal void SaveOpenValues(ProductUploadResult result, ProductCharacteristic characteristic, List<string> foundValues,  IDictionary<Guid, ProductFamilyCharacteristicDomainEntity> cachedCharacteristics)
        {
            if (characteristic.ProductFamilyCharacteristicType != ProductFamilyCharacteristicType.Feature)
                return;
            var feature = (ProductFamilyFeature)cachedCharacteristics[characteristic.ProductFamilyCharacteristicId];
            
            if (!feature.AllowChanges)
                return;
            Guid productFamilyId = result.Product.ProductFamilyId;
            var values = _familyFeatureProvider.FindAllowedValues(feature.Id.Value, productFamilyId);

            if (foundValues.Any())
            {
                foreach (var foundValue in foundValues.Where(y => !values.Any(z=> z.FeatureValue.Value == y)))
                {
                   SaveNewFeatureValue(feature, foundValue, productFamilyId);
                }
                return;
            }
            // multi values not found, just add the extant value
            if (!values.Any(z => characteristic.Value == z.FeatureValue.Value))
            {
                SaveNewFeatureValue(feature, characteristic.Value, productFamilyId);
            }
        }

        private void SaveNewFeatureValue(ProductFamilyFeature feature, string foundValue, Guid productFamilyId)
        {
            var productFamilyFeatureValue = _productFamilyProvider.GetProductFamilyFeatureValueByFeatureIdAndValue(feature.Id.Value, foundValue);
            if (productFamilyFeatureValue == null)
                productFamilyFeatureValue = _productFamilyProvider.CreateProductFamilyFeatureValue(feature, foundValue, feature.UnitOfMeasureId);
            _productFamilyProvider.CreateProductFamilyAllowedFeatureValue(productFamilyId, productFamilyFeatureValue);
        }

        /// <summary>
        ///     Deletes the children.
        /// </summary>
        /// <param name="id">The id.</param>
        public void DeleteChildren(Guid id)
        {
            _productCharacteristicRepository.DeleteChildren(id);
        }

        internal static bool ValidateDataType(ProductUploadResult productUploadResult,
                                              ProductCharacteristic productCharacteristic, string value, bool isEmptyAllowed = true)
        {
            if (string.IsNullOrEmpty(value) && isEmptyAllowed)
                return productUploadResult.IsValid;
            if (productCharacteristic.DataType == ProductFamilyCharacteristicDataType.Date)
            {
                DateTime dateTimeValue;
                if (!DateTime.TryParse(value, out dateTimeValue))
                    MarkValueInvalidForDataType(productUploadResult, productCharacteristic, value);
                return productUploadResult.IsValid;
            }

            if (productCharacteristic.DataType == ProductFamilyCharacteristicDataType.DocumentReference)
            {
                Guid guidValue;
                if (!Guid.TryParse(value, out guidValue))
                    MarkValueInvalidForDataType(productUploadResult, productCharacteristic, value);
                if (productUploadResult.IsValid && guidValue == Guid.Empty)
                    MarkValueInvalidForDataType(productUploadResult, productCharacteristic, value);
                return productUploadResult.IsValid;
            }

            if (productCharacteristic.DataType == ProductFamilyCharacteristicDataType.Number)
            {
                decimal decimalValue;
                if (!decimal.TryParse(value, out decimalValue))
                    MarkValueInvalidForDataType(productUploadResult, productCharacteristic, value);
                return productUploadResult.IsValid;
            }

            return true;
        }

        internal static void MarkValueInvalidForDataType(ProductUploadResult productUploadResult,
                                                         ProductCharacteristic productCharacteristic, string value)
        {
            productUploadResult.Messages.Add(new ProductUploadMessage
                {
                    MessageType = ProductUploadMessageTypeEnumDto.Error,
                    Title = string.Format("Invalid value for characteristic '{0}'.", productCharacteristic.Name),
                    Detail =
                        string.Format("Full Value: {0}, Parsed Value: {1}, DataType: {2}", productCharacteristic.Value,
                                      value, productCharacteristic.DataType.ToString()),
                    CreatedDateTime = DateTime.UtcNow,
                    UpdatedDateTime = DateTime.UtcNow,
                });
            productUploadResult.IsValid = false;
        }

        internal static ProductCharacteristic CloneProductCharacteristicAsChild(
            ProductCharacteristic productCharacteristic,
            ProductCharacteristicChildType productCharacteristicChildType, string value)
        {
            var newProductCharacteristic = new ProductCharacteristic
                {
                    Id = Guid.NewGuid(),
                    CreatedById = productCharacteristic.CreatedById,
                    CreatedDateTime = productCharacteristic.CreatedDateTime,
                    DataType = productCharacteristic.DataType,
                    Description = productCharacteristic.Description,
                    Group = productCharacteristic.Group,
                    IsMultivalueAllowed = productCharacteristic.IsMultivalueAllowed,
                    IsRangeAllowed = productCharacteristic.IsRangeAllowed,
                    Name = productCharacteristic.Name,
                    ProductFamilyCharacteristicId = productCharacteristic.ProductFamilyCharacteristicId,
                    ProductFamilyCharacteristicType = productCharacteristic.ProductFamilyCharacteristicType,
                    ProductId = productCharacteristic.ProductId,
                    UpdatedById = productCharacteristic.UpdatedById,
                    UpdatedDateTime = productCharacteristic.UpdatedDateTime,
                    ParentId = productCharacteristic.Id,
                    ChildType = productCharacteristicChildType,
                    Value = value
                };

            return newProductCharacteristic;
        }
    }
}