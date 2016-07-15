using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Implements operations for Validating a <see cref="ProductFamily" />
    /// </summary>
    public class ProductFamilyValidationManager : IProductFamilyValidationManager
    {
        private readonly IProductFamilyAttributeProvider _attributeProvider;
        private readonly IProductFamilyFeatureProvider _featureProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductFamilyValidationManager" /> class.
        /// </summary>
        /// <param name="attributeProvider">The attribute provider.</param>
        /// <param name="featureProvider">The feature provider.</param>
        public ProductFamilyValidationManager(IProductFamilyAttributeProvider attributeProvider, IProductFamilyFeatureProvider featureProvider)
        {
            _attributeProvider = attributeProvider;
            _featureProvider = featureProvider;
        }

        /// <summary>
        ///     Validates the specified family.
        /// </summary>
        /// <param name="family">The family.</param>
        /// <param name="characteristicUploads">The characteristic uploads.</param>
        /// <param name="dependencies"></param>
        /// <returns></returns>
        public ProductFamilyUploadResult Validate(ProductFamily family, IEnumerable<ProductFamilyCharacteristicUpload> characteristicUploads, IList<ProductFamilyFeatureAllowedValueDependencyUpload> dependencies)
        {
            var result = new ProductFamilyUploadResult {ProductFamily = family, IsValid = true, CharacteristicUploads = characteristicUploads, DependencyUploads = dependencies};
            ValidateCharacteristicIdUniqueness(result);
            ValidateCharacteristicNameUniqueness(result);
            ValidateScopeRequiredCharacteristics(result, result.ProductFamily.BusinessUnitId.GetValueOrDefault());
            ValidateScopeRequiredCharacteristics(result, new Guid(ExcelTemplateKeys.GlobalScopeId));
            ValidateDependency(result);
            result.IsValid = !result.Messages.Any();
            return result;
        }

        internal void ValidateDependency(ProductFamilyUploadResult result)
        {
            foreach (ProductFamilyFeatureAllowedValueDependencyUpload dependencyUpload in result.DependencyUploads)
            {
                Guid parentId = dependencyUpload.Parent.ParseOrDefault(default(Guid));
                Guid childId = dependencyUpload.Child.ParseOrDefault(default(Guid));
                bool shouldValidateValues = true;
                if (parentId == default(Guid))
                {
                    result.Messages.Add(new ProductUploadMessage
                    {
                        Title = string.Format("Parent ID must be a valid identifier. Id found was {0}. ", dependencyUpload.Parent),
                        Detail = string.Format("Parent ID must be a valid identifier. Id found was {0}. ", dependencyUpload.Parent),
                        MessageType = ProductUploadMessageTypeEnumDto.Error,
                        Id = Guid.NewGuid(),
                    }
                        );
                    shouldValidateValues = false;
                }
                if (childId == default(Guid))
                {
                    result.Messages.Add(new ProductUploadMessage
                    {
                        Title = string.Format("Child ID must be a valid identifier. Id found was {0}. ", dependencyUpload.Child),
                        Detail = string.Format("Child ID must be a valid identifier. Id found was {0}. ", dependencyUpload.Child),
                        MessageType = ProductUploadMessageTypeEnumDto.Error,
                        Id = Guid.NewGuid(),
                    }
                        );
                    shouldValidateValues = false;
                }
                if (!shouldValidateValues || dependencyUpload.UploadAction == UploadAction.Remove)
                    continue;


                ProductFamilyFeature parentFeature = FindFeature(parentId, result);
                ProductFamilyFeature childFeature = FindFeature(parentId, result);
                if (null == parentFeature || null == childFeature)
                    continue;

                ValidateDependencyValues(result, parentFeature, dependencyUpload.ParentValues);
                ValidateDependencyValues(result, childFeature, dependencyUpload.ChildValues);
            }
        }

        internal ProductFamilyFeature FindFeature(Guid id, ProductFamilyUploadResult result)
        {
            ProductFamilyCharacteristicUpload upload = result.CharacteristicUploads.FirstOrDefault(x => x.Entity.Id == id);
            if (upload == null)
            {
                result.Messages.Add(new ProductUploadMessage
                {
                    Title = String.Format("A Feature Id used in a dependency was not found. The Id was {0}.", id),
                    Detail = String.Format("A Feature Id used in a dependency was not found. The Id was {0}.", id),
                    MessageType = ProductUploadMessageTypeEnumDto.Error,
                    Id = Guid.NewGuid(),
                });
                return null;
            }
            var feature = upload.Entity as ProductFamilyFeature;
            if (null == feature)
            {
                result.Messages.Add(new ProductUploadMessage
                {
                    Title = String.Format("An Attribute(non-List of Values) was specified for a dependency. Only features are allowed. The Id was {0}.", id),
                    Detail = String.Format("An Attribute(non-List of Values) was specified for a dependency. Only features are allowed. The Id was {0}.", id),
                    MessageType = ProductUploadMessageTypeEnumDto.Error,
                    Id = Guid.NewGuid(),
                });
            }
            return feature;
        }

        internal void ValidateDependencyValues(ProductFamilyUploadResult result, ProductFamilyFeature feature, IEnumerable<string> dependencyValues)
        {
            string[] allowedValues = feature.AllowedValues.SplitAndTrim(ExcelTemplateKeys.StandardDelimiter);
            foreach (string dependencyValue in dependencyValues.Except(allowedValues))
            {
                result.Messages.Add(new ProductUploadMessage
                {
                    Title = string.Format("Feature '{0}' does not contain value '{1}'.", feature.Name, dependencyValue),
                    Detail = string.Format("Feature '{0}' does not contain value '{1}'.", feature.Name, dependencyValue),
                    MessageType = ProductUploadMessageTypeEnumDto.Error,
                    Id = Guid.NewGuid(),
                }
                    );
            }
        }

        internal void ValidateCharacteristicIdUniqueness(ProductFamilyUploadResult result)
        {
            IEnumerable<Guid?> duplicates = result.CharacteristicUploads.Select(x => x.Entity.Id).GroupBy(x => x).Where(y => y.Count() > 1).Select(z => z.Key);
            foreach (var duplicate in duplicates)
            {
                result.Messages.Add(new ProductUploadMessage
                {
                    Title = string.Format("Attributes and Features {0} was duplicated", duplicate),
                    Detail = string.Format("Attributes and Features {0} was duplicated", duplicate),
                    MessageType = ProductUploadMessageTypeEnumDto.Error,
                    Id = Guid.NewGuid(),
                }
                    );
            }
        }

        internal void ValidateCharacteristicNameUniqueness(ProductFamilyUploadResult result)
        {
            IEnumerable<string> duplicates = result.CharacteristicUploads.Select(x=> x.Entity.Name + ":" + x.Entity.ScopeId.ToString()).GroupBy(x => x).Where(y => y.Count() > 1).Select(z => z.Key);
            foreach (string duplicate in duplicates)
            {
                result.Messages.Add(new ProductUploadMessage
                {
                    Title = string.Format("Attributes and Features {0} was duplicated", duplicate),
                    Detail = string.Format("Attributes and Features {0} was duplicated", duplicate),
                    MessageType = ProductUploadMessageTypeEnumDto.Error,
                    Id = Guid.NewGuid(),
                }
                    );
            }
        }

        internal void ValidateScopeRequiredCharacteristics(ProductFamilyUploadResult result, Guid scopeId)
        {
            if (scopeId == default (Guid))
                return;
            IEnumerable<ProductFamilyCharacteristicDomainEntity> requiredAttributes = _attributeProvider.FindByScopeId(scopeId).Where(x => x.IsRequired);
            IEnumerable<ProductFamilyCharacteristicDomainEntity> requiredFeatures = _featureProvider.FindByScopeId(scopeId).Where(x => x.IsRequired);
            IEnumerable<ProductFamilyCharacteristicDomainEntity> union = requiredAttributes.Union(requiredFeatures);

            IEnumerable<ProductFamilyCharacteristicDomainEntity> missingItems = union.Where(x => !result.CharacteristicUploads.Any(y => y.Entity.Id == x.Id));
            foreach (ProductFamilyCharacteristicDomainEntity missing in missingItems)
            {
                string message = string.Format("Attributes and Features {0}:{1} was not provided but is required.", missing.Name, missing.Id);
                result.Messages.Add(new ProductUploadMessage
                {
                    Title = message, Detail = message, MessageType = ProductUploadMessageTypeEnumDto.Error
                }
                    );
            }
        }
    }
}