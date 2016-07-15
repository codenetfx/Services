using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Aspose.Cells;

using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Implements Operations for Importing famlilys.
    /// </summary>
    public class ProductFamilyImportManager : IProductFamilyImportManager
    {
        private readonly IProductFamilyAttributeProvider _attributeProvider;
        private readonly IProductFamilyFeatureProvider _featureProvider;
        private readonly IProductMetaDataProvider _metaDataProvider;

        /// <summary>
        ///     The _product family provider
        /// </summary>
        private readonly IProductFamilyProvider _productFamilyProvider;

        private readonly ITransactionFactory _transactionFactory;
        private readonly IProductFamilyValidationManager _validationManager;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductFamilyImportManager" /> class.
        /// </summary>
        /// <param name="productFamilyProvider">The product family provider.</param>
        /// <param name="attributeProvider">The attribute provider.</param>
        /// <param name="featureProvider">The feature provider.</param>
        /// <param name="validationManager"></param>
        /// <param name="metaDataProvider">The meta data provider.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public ProductFamilyImportManager(IProductFamilyProvider productFamilyProvider,
            IProductFamilyAttributeProvider attributeProvider, IProductFamilyFeatureProvider featureProvider,
            IProductFamilyValidationManager validationManager, IProductMetaDataProvider metaDataProvider,
            ITransactionFactory transactionFactory)
        {
            _productFamilyProvider = productFamilyProvider;
            _attributeProvider = attributeProvider;
            _featureProvider = featureProvider;
            _validationManager = validationManager;
            _metaDataProvider = metaDataProvider;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Imports the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public ProductFamilyUploadResult Import(Stream stream)
        {
            return Import(new Workbook(stream));
        }

        /// <summary>
        ///     Creates the specified family.
        /// </summary>
        /// <param name="family">The family.</param>
        /// <param name="characteristicUploads">The characteristic uploads.</param>
        /// <param name="dependencies">The dependencies.</param>
        public void Create(ProductFamily family, IEnumerable<ProductFamilyCharacteristicAssociationModel> characteristicUploads, IList<ProductFamilyFeatureDependency> dependencies)
        {
            using (var scope = _transactionFactory.Create())
            {
                IEnumerable<ProductFamilyAttributeAssociation> allowedAttributes;
                IEnumerable<ProductFamilyFeatureAssociation> allowedFeatures;

                var removedAllowedAttributes = new List<ProductFamilyAttributeAssociation>();
                var removedAllowedFeatures = new List<ProductFamilyFeatureAssociation>();

                if (!family.Id.HasValue)
                {
                    family.Id = Guid.NewGuid();
                }
                _productFamilyProvider.Create(family);
                allowedAttributes = new List<ProductFamilyAttributeAssociation>();
                allowedFeatures = new List<ProductFamilyFeatureAssociation>();

                ProcessAttributesAndFeatures(family,
                    characteristicUploads,
                    dependencies,
                    allowedAttributes,
                    allowedFeatures,
                    removedAllowedAttributes,
                    removedAllowedFeatures);

                scope.Complete();
            }
        }

        /// <summary>
        ///     Updates the specified family.
        /// </summary>
        /// <param name="family">The family.</param>
        /// <param name="characteristicUploads">The characteristic uploads.</param>
        /// <param name="dependencies">The dependencies.</param>
        public void Update(ProductFamily family, IEnumerable<ProductFamilyCharacteristicAssociationModel> characteristicUploads, IList<ProductFamilyFeatureDependency> dependencies)
        {
            using (var scope = _transactionFactory.Create())
            {
                IEnumerable<ProductFamilyAttributeAssociation> allowedAttributes;
                IEnumerable<ProductFamilyFeatureAssociation> allowedFeatures;

                var removedAllowedAttributes = new List<ProductFamilyAttributeAssociation>();
                var removedAllowedFeatures = new List<ProductFamilyFeatureAssociation>();

                _productFamilyProvider.Update(family);
                allowedAttributes = _productFamilyProvider.GetProductFamilyAttributeAssociations(family.Id.Value);
                allowedFeatures = _productFamilyProvider.GetProductFamilyFeatureAssociations(family.Id.Value);
                removedAllowedAttributes = allowedAttributes.Select(y => y).ToList();
                removedAllowedFeatures = allowedFeatures.Select(y => y).ToList();

                ProcessAttributesAndFeatures(family,
                    characteristicUploads,
                    dependencies,
                    allowedAttributes,
                    allowedFeatures,
                    removedAllowedAttributes,
                    removedAllowedFeatures);

                scope.Complete();
            }
        }

        internal ProductFamilyUploadResult Import(Workbook workbook)
        {
            var familySheet = workbook.Worksheets[ExcelTemplateKeys.FamilyBasics];
            var family = ReadFamilyBasics(familySheet);
            var families = _productFamilyProvider.GetAll();
            if (families.Any(x => x.Name == family.Name && family.Id != x.Id))
                throw new DatabaseItemExistsException(string.Format("A family with the name {0} already exists",
                    family.Name));

            var attributesSheet = workbook.Worksheets[ExcelTemplateKeys.AttributesAndFeatures];
            var attributes = ReadCharacteristics(attributesSheet, family);
            var dependencies = ReadDependencyMappings(workbook);

            var result = _validationManager.Validate(family, attributes, dependencies);
            if (!result.IsValid && family.Id.HasValue && family.Status != ProductFamilyStatus.Draft)
            {
                // keep draft status and save if it was draft.
                var oldFamily = _productFamilyProvider.Get(family.Id.Value);
                if (oldFamily.Status == ProductFamilyStatus.Draft)
                {
                    family.AllowChanges = true;
                    family.IsDisabled = false;
                }
            }

            if (result.IsValid || family.Status == ProductFamilyStatus.Draft)
                Persist(family, attributes, dependencies);
            return result;
        }

        private void Persist(ProductFamily family, IEnumerable<ProductFamilyCharacteristicUpload> characteristicUploads,
            IList<ProductFamilyFeatureAllowedValueDependencyUpload> dependencies)
        {
            using (var scope = _transactionFactory.Create())
            {
                IEnumerable<ProductFamilyAttributeAssociation> allowedAttributes;
                IEnumerable<ProductFamilyFeatureAssociation> allowedFeatures;

                var removedAllowedAttributes = new List<ProductFamilyAttributeAssociation>();
                var removedAllowedFeatures = new List<ProductFamilyFeatureAssociation>();

                if (family.Id.HasValue)
                {
                    _productFamilyProvider.Update(family);
                    allowedAttributes = _productFamilyProvider.GetProductFamilyAttributeAssociations(family.Id.Value);
                    allowedFeatures = _productFamilyProvider.GetProductFamilyFeatureAssociations(family.Id.Value);
                    removedAllowedAttributes = allowedAttributes.Select(y => y).ToList();
                    removedAllowedFeatures = allowedFeatures.Select(y => y).ToList();
                }
                else
                {
                    family.Id = Guid.NewGuid();
                    _productFamilyProvider.Create(family);
                    allowedAttributes = new List<ProductFamilyAttributeAssociation>();
                    allowedFeatures = new List<ProductFamilyFeatureAssociation>();
                }

                ProcessAttributesAndFeatures(family,
                    characteristicUploads,
                    dependencies,
                    allowedAttributes,
                    allowedFeatures,
                    removedAllowedAttributes,
                    removedAllowedFeatures,
                    true);

                scope.Complete();
            }
        }

        internal void ProcessAttributesAndFeatures(ProductFamily family,
            IEnumerable<ProductFamilyCharacteristicAssociationModel> characteristicUploads,
           IList<ProductFamilyFeatureDependency> dependencies,
           IEnumerable<ProductFamilyAttributeAssociation> allowedAttributes,
           IEnumerable<ProductFamilyFeatureAssociation> allowedFeatures,
           List<ProductFamilyAttributeAssociation> removedAllowedAttributes,
           List<ProductFamilyFeatureAssociation> removedAllowedFeatures
           )
        {
            var newAllowedAttributes = new List<ProductFamilyAttributeAssociation>();
            var newAllowedFeatures = new List<ProductFamilyFeatureAssociation>();
            var newAllowedFeatureValues = new List<Guid>();
            foreach (var upload in characteristicUploads)
            {
                if (upload.CharacteristicType == ProductFamilyCharacteristicType.Attribute)
                {
                    var foundAttribute = allowedAttributes.FirstOrDefault(i => i.CharacteristicId == upload.CharacteristicId);
                        if (null == foundAttribute)
                        {
                            newAllowedAttributes.Add(new ProductFamilyAttributeAssociation
                            {
                                Id = Guid.NewGuid(),
                                CharacteristicId = upload.CharacteristicId,
                                ProductFamilyId = family.Id.Value,
                                OptionIds=upload.OptionIds
                            });
                        }
                        else if (foundAttribute.OptionIds.Count != upload.OptionIds.Count  ||!foundAttribute.OptionIds.All(f => upload.OptionIds.Any(u => f == u)))
                        {
                            newAllowedAttributes.Add(new ProductFamilyAttributeAssociation
                            {
                                Id = Guid.NewGuid(),
                                CharacteristicId = upload.CharacteristicId,
                                ProductFamilyId = family.Id.Value,
                                OptionIds = upload.OptionIds
                            });
                        }
                        else
                        {
                            var productFamilyAttributeAssociation = removedAllowedAttributes.FirstOrDefault(x => x.CharacteristicId == upload.CharacteristicId);
                            if (null != productFamilyAttributeAssociation)
                                removedAllowedAttributes.Remove(productFamilyAttributeAssociation);
                        }
                }
                else
                {
                    newAllowedFeatureValues.AddRange(upload.AllowedFeatureValueIds);
                     var existing = _featureProvider.FindAllowedValues(upload.CharacteristicId, family.Id.Value);
                    if (null != existing)
                    {
                        foreach (var productFamilyFeatureAllowedValue in existing)
                        {
                            Guid featureValueId = upload.AllowedFeatureValueIds !=null ? upload.AllowedFeatureValueIds.FirstOrDefault(x => x == productFamilyFeatureAllowedValue.FeatureValue.Id.Value): default(Guid);
                            if (default(Guid) == featureValueId)
                            {
                                _productFamilyProvider.RemoveProductFamilyAllowedFeatureValue(
                                    productFamilyFeatureAllowedValue.Id.Value);
                            }
                            else
                            {
                                newAllowedFeatureValues.Remove(featureValueId);
                            }
                        }
                    }

                    var foundFeature = allowedFeatures.FirstOrDefault(i => i.CharacteristicId == upload.CharacteristicId);
                        if (null == foundFeature)
                        {
                            newAllowedFeatures.Add(new ProductFamilyFeatureAssociation
                            {
                                Id = Guid.NewGuid(),
                                CharacteristicId = upload.CharacteristicId,
                                ProductFamilyId = family.Id.Value,
                                OptionIds = upload.OptionIds,
                                Dependent = "N"
                            });
                        }
                        else if (foundFeature.OptionIds.Count != upload.OptionIds.Count || !foundFeature.OptionIds.All(f => upload.OptionIds.Any(u => f == u)))
                        {
                            newAllowedFeatures.Add(new ProductFamilyFeatureAssociation
                            {
                                Id = Guid.NewGuid(),
                                CharacteristicId = upload.CharacteristicId,
                                ProductFamilyId = family.Id.Value,
                                OptionIds=upload.OptionIds,
                                Dependent = foundFeature.Dependent
                            });
                        }
                        else
                        {
                            removedAllowedFeatures.Remove(
                                removedAllowedFeatures.First(
                                    x => x.CharacteristicId == upload.CharacteristicId));
                        }
                }
            }
            _productFamilyProvider.RemoveProductFamilyAttributeAssociations(
                removedAllowedAttributes.Select(x => x.Id.Value));
            _productFamilyProvider.RemoveProductFamilyFeatureAssociations(
                removedAllowedFeatures.Select(x => x.Id.Value));

            _productFamilyProvider.SaveProductFamilyAttributeAssociations(family.Id.Value, newAllowedAttributes);
            _productFamilyProvider.SaveProductFamilyFeatureAssociations(family.Id.Value, newAllowedFeatures);
            foreach (var newAllowedFeatureValue in newAllowedFeatureValues)
            {
                _productFamilyProvider.CreateProductFamilyAllowedFeatureValue(family.Id.Value, new ProductFamilyFeatureValue {Id = newAllowedFeatureValue});
            }
            var productFamilyFeatureAssociations = _productFamilyProvider.GetProductFamilyFeatureAssociations(family.Id.Value);
            var allowedValues = _featureProvider.FindAllowedValuesByFamily(family.Id.Value);
            PersistDependencies(dependencies, productFamilyFeatureAssociations, characteristicUploads.Where(x=> x.CharacteristicType == ProductFamilyCharacteristicType.Feature), allowedValues);
        }

        internal void PersistDependencies(IList<ProductFamilyFeatureDependency> dependencyUploads, IEnumerable<ProductFamilyFeatureAssociation> existingAssociations, IEnumerable<ProductFamilyCharacteristicAssociationModel> allowedFeatures, IEnumerable<ProductFamilyFeatureAllowedValue> allowedValues)
        {
            
            if (null != existingAssociations)
            {
                foreach (var productFamilyFeatureAssociation in existingAssociations)
                {

                    var existingDependencies = _featureProvider.GetValueDependenciesByFamilyAllowedFeatureId(productFamilyFeatureAssociation.Id.Value);
                    foreach (var dependency in existingDependencies)
                    {
                        _featureProvider.RemoveValueDependency(dependency.Id.Value);
                    }
                }
            }
            
            foreach (var dependencyUpload in dependencyUploads)
            {
                foreach (var parentId in dependencyUpload.ParentValueIds)
                {
                    var parentValue =
                            allowedValues.First(x => x.FeatureValue.Id == parentId);
                    foreach (var childId in dependencyUpload.ChildValueIds)
                    {
                        var childvalue =
                            allowedValues.First(x => x.FeatureValue.Id == childId);
                        _featureProvider.AddValueDependency(new ProductFamilyFeatureAllowedValueDependency
                        {
                            Id = Guid.NewGuid(),
                            ChildProductFamilyFeatureAllowedValueId = childvalue.Id.Value,
                            ParentProductFamilyFeatureAllowedValueId = parentValue.Id.Value
                        }
                            );
                    }
                }
            }
        }

        private void ProcessAttributesAndFeatures(ProductFamily family,
            IEnumerable<ProductFamilyCharacteristicUpload> characteristicUploads,
            IList<ProductFamilyFeatureAllowedValueDependencyUpload> dependencies,
            IEnumerable<ProductFamilyAttributeAssociation> allowedAttributes,
            IEnumerable<ProductFamilyFeatureAssociation> allowedFeatures,
            List<ProductFamilyAttributeAssociation> removedAllowedAttributes,
            List<ProductFamilyFeatureAssociation> removedAllowedFeatures,
            bool persistAttributesAndFeatures)
        {
             var newAllowedAttributes = new List<ProductFamilyAttributeAssociation>();
            var newAllowedFeatures = new List<ProductFamilyFeatureAssociation>();
            var allowedValues = new Dictionary<Guid, IList<ProductFamilyFeatureAllowedValue>>();
            foreach (var upload in characteristicUploads)
            {
                upload.Entity.ScopeId = upload.Entity.ScopeId == default(Guid)
                    ? family.Id.Value
                    : upload.Entity.ScopeId;

                var attribute = upload.Entity as ProductFamilyAttribute;
                if (attribute != null)
                {
                    if (upload.UploadAction != UploadAction.Remove)
                    {
                        if (persistAttributesAndFeatures)
                        {
                            PersistAttribute(upload, attribute);
                        }
                        if (!allowedAttributes.Any(i => i.CharacteristicId == attribute.Id.Value))
                        {
                            newAllowedAttributes.Add(new ProductFamilyAttributeAssociation
                            {
                                Id = Guid.NewGuid(),
                                CharacteristicId = upload.Entity.Id.Value,
                                ProductFamilyId = family.Id.Value,
                                OptionIds = (upload.Entity.Options ?? new List<ProductFamilyCharacteristicOption>()).Select(x => x.Id.Value).ToList()
                            });
                        }
                        else
                        {
                            removedAllowedAttributes.Remove(
                                removedAllowedAttributes.First(x => x.CharacteristicId == attribute.Id.Value));
                        }
                    }
                }
                else
                {
                    if (upload.UploadAction != UploadAction.Remove)
                    {
                        var productFamilyFeature = (ProductFamilyFeature)upload.Entity;
                        IList<ProductFamilyFeatureAllowedValue> values;
                        if (persistAttributesAndFeatures)
                        {
                            values = PersistFeature(upload, productFamilyFeature, family.Id.Value);
                        }
                        else
                        {
                            values =
                                _featureProvider.FindAllowedValues(productFamilyFeature.Id.Value, family.Id.Value)
                                    .ToList();
                        }
                        if (!allowedFeatures.Any(i => i.CharacteristicId == productFamilyFeature.Id.Value))
                        {
                            newAllowedFeatures.Add(new ProductFamilyFeatureAssociation()
                            {
                                Id = Guid.NewGuid(),
                                CharacteristicId = upload.Entity.Id.Value,
                                ProductFamilyId = family.Id.Value,
                                OptionIds = (upload.Entity.Options?? new List< ProductFamilyCharacteristicOption>()).Select(x => x.Id.Value).ToList()
                            });
                        }
                        else
                        {
                            removedAllowedFeatures.Remove(
                                removedAllowedFeatures.First(
                                    x => x.CharacteristicId == productFamilyFeature.Id.Value));
                        }
                        allowedValues.Add(productFamilyFeature.Id.Value, values);
                    }
                }
            }
            _productFamilyProvider.RemoveProductFamilyAttributeAssociations(
                removedAllowedAttributes.Select(x => x.Id.Value));
            _productFamilyProvider.RemoveProductFamilyFeatureAssociations(
                removedAllowedFeatures.Select(x => x.Id.Value));

            _productFamilyProvider.SaveProductFamilyAttributeAssociations(family.Id.Value, newAllowedAttributes);
            _productFamilyProvider.SaveProductFamilyFeatureAssociations(family.Id.Value, newAllowedFeatures);

            PersistDependencies(dependencies, allowedValues, allowedFeatures);
        }

        private IList<ProductFamilyFeatureAllowedValue> PersistFeature(ProductFamilyCharacteristicUpload upload,
            ProductFamilyFeature productFamilyFeature, Guid familyId)
        {
            switch (upload.UploadAction)
            {
                case UploadAction.Add:
                    upload.Entity.Id = Guid.NewGuid();
                    _featureProvider.Create(productFamilyFeature);
                    foreach (var option in productFamilyFeature.Options)
                    {
                        _featureProvider.AddOption(productFamilyFeature.Id.Value, option);
                    }
                    break;
                case UploadAction.Update:
                    var old = _featureProvider.Get(productFamilyFeature.Id.Value);
                    foreach (var option in productFamilyFeature.Options)
                    {
                        if (!old.Options.Any(o => option.Id.Value == o.Id.Value))
                            _featureProvider.AddOption(productFamilyFeature.Id.Value, option);
                    }
                    foreach (var option in old.Options)
                    {
                        if (!productFamilyFeature.Options.Any(o => old.Id.Value == o.Id.Value))
                            _featureProvider.RemoveOption(option.Id.Value);
                    }
                    _featureProvider.Update(productFamilyFeature);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return ProcessFeatureAllowedValues(productFamilyFeature, familyId);
        }

        internal IList<ProductFamilyFeatureAllowedValue> ProcessFeatureAllowedValues(
            ProductFamilyFeature productFamilyFeature, Guid familyId)
        {
            var populatedValues = new List<ProductFamilyFeatureAllowedValue>();
            var values = productFamilyFeature.AllowedValues.SplitAndTrim(ExcelTemplateKeys.StandardDelimiter);
            var existing = _featureProvider.FindAllowedValues(productFamilyFeature.Id.Value, familyId);
            foreach (var productFamilyFeatureAllowedValue in existing)
            {
                if (!values.Any(x => x == productFamilyFeatureAllowedValue.FeatureValue.Value))
                    _productFamilyProvider.RemoveProductFamilyAllowedFeatureValue(
                        productFamilyFeatureAllowedValue.Id.Value);
            }
            foreach (var value in values)
            {
                var existingValue = existing.FirstOrDefault(x => x.FeatureValue.Value == value);
                if (null != existingValue)
                {
                    populatedValues.Add(existingValue);
                    continue;
                }
                var productFamilyFeatureValue =
                    _productFamilyProvider.GetProductFamilyFeatureValueByFeatureIdAndValue(
                        productFamilyFeature.Id.Value, value) ??
                    _productFamilyProvider.CreateProductFamilyFeatureValue(productFamilyFeature, value,
                        productFamilyFeature.UnitOfMeasureId);

                populatedValues.Add(_productFamilyProvider.CreateProductFamilyAllowedFeatureValue(familyId,
                    productFamilyFeatureValue));
            }
            return populatedValues;
        }

        internal void PersistDependencies(IList<ProductFamilyFeatureAllowedValueDependencyUpload> dependencyUploads,
            Dictionary<Guid, IList<ProductFamilyFeatureAllowedValue>> allowedValues,
            IEnumerable<ProductFamilyFeatureAssociation> allowedFeatures)
        {
            foreach (var dependencyUpload in dependencyUploads)
            {
                var parentId = dependencyUpload.Parent.ParseOrDefault(default(Guid));
                var productFamilyFeatureAssociation = allowedFeatures.FirstOrDefault(x => x.CharacteristicId == parentId);
                if (null == productFamilyFeatureAssociation)
                    continue;
                var parentFeatureId = productFamilyFeatureAssociation.Id.Value;

                var childId = dependencyUpload.Child.ParseOrDefault(default(Guid));

                var parentValues = allowedValues[parentId];
                var existing = _featureProvider.GetValueDependenciesByFamilyAllowedFeatureId(parentFeatureId);
                var childValues = allowedValues[childId];
                var found = new List<ProductFamilyFeatureAllowedValueDependency>();
                var productFamilyFeatureAllowedValues =
                    parentValues.Where(v => dependencyUpload.ParentValues.Any(p => p == v.FeatureValue.Value));
                foreach (var parentAllowedValue in productFamilyFeatureAllowedValues)
                {
                    var familyFeatureAllowedValues =
                        childValues.Where(v => dependencyUpload.ChildValues.Any(p => p == v.FeatureValue.Value));
                    foreach (var childAllowedValue in familyFeatureAllowedValues)
                    {
                        var ext = existing.FirstOrDefault(x =>
                            x.ChildProductFamilyFeatureAllowedValueId == childAllowedValue.Id.Value
                            && x.ParentProductFamilyFeatureAllowedValueId == parentAllowedValue.Id.Value
                            );
                        if (ext == null)
                        {
                            var productFamilyFeatureAllowedValueDependency = new ProductFamilyFeatureAllowedValueDependency
                            {
                                Id = Guid.NewGuid(),
                                ChildProductFamilyFeatureAllowedValueId = childAllowedValue.Id.Value,
                                ParentProductFamilyFeatureAllowedValueId = parentAllowedValue.Id.Value
                            };
                            found.Add(productFamilyFeatureAllowedValueDependency);
                            if (dependencyUpload.UploadAction != UploadAction.Remove)
                            {
                                _featureProvider.AddValueDependency(productFamilyFeatureAllowedValueDependency);
                            }
                        }
                        else
                        {
                            if (dependencyUpload.UploadAction == UploadAction.Remove)
                            {
                                _featureProvider.RemoveValueDependency(ext.Id.Value);
                            }
                            found.Add(ext);
                        }
                    }
                }

                foreach (
                    var ext in
                        existing.Where(
                            i =>
                                found.Any(
                                    j =>
                                        j.ParentProductFamilyFeatureAllowedValueId ==
                                        i.ParentProductFamilyFeatureAllowedValueId))
                            .Except(
                                found.Where(
                                    f =>
                                        existing.Any(
                                            e =>
                                                f.ChildProductFamilyFeatureAllowedValueId ==
                                                e.ChildProductFamilyFeatureAllowedValueId &&
                                                f.ParentProductFamilyFeatureAllowedValueId ==
                                                e.ParentProductFamilyFeatureAllowedValueId))))
                {
                    _featureProvider.RemoveValueDependency(ext.Id.Value);
                }
            }
        }

        private void PersistAttribute(ProductFamilyCharacteristicUpload upload, ProductFamilyAttribute attribute)
        {
            switch (upload.UploadAction)
            {
                case UploadAction.Add:
                    attribute.Id = Guid.NewGuid();
                    _attributeProvider.Create(attribute);
                    foreach (var option in attribute.Options)
                    {
                        _attributeProvider.AddOption(attribute.Id.Value, option);
                    }

                    break;
                case UploadAction.Update:
                    var old = _attributeProvider.Get(attribute.Id.Value);
                    foreach (var option in attribute.Options)
                    {
                        if (old.Options.All(o => option.Id.Value != o.Id.Value))
                            _attributeProvider.AddOption(attribute.Id.Value, option);
                    }
                    foreach (var option in old.Options)
                    {
                        if (!attribute.Options.Any(o => old.Id.Value == o.Id.Value))
                            _attributeProvider.RemoveOption(option.Id.Value);
                    }
                    _attributeProvider.Update(attribute);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal ProductFamily ReadFamilyBasics(Worksheet worksheet)
        {
            var family = new ProductFamily();
            Guid id;
            family.Name = worksheet.Cells[1, 1].StringValue;
            var stringValue = worksheet.Cells[0, 1].StringValue;
            if (Guid.TryParse(stringValue, out id))
                family.Id = id;
            stringValue = worksheet.Cells[2, 1].StringValue;
            if (Guid.TryParse(stringValue, out id))
                family.CategoryId = id;

            if (!family.Id.HasValue)
            {
                family.CreatedDateTime = DateTime.UtcNow;
            }
            family.UpdatedDateTime = DateTime.UtcNow;
            family.BusinessUnitId = worksheet.Cells[4, 1].StringValue.ParseOrDefault(default(Guid));
            SetProductFamilyStatus(family, worksheet.Cells[6, 1].StringValue);
            return family;
        }

        internal static void SetProductFamilyStatus(ProductFamily family, string stringValue)
        {
            var status = stringValue.ParseOrDefault(ProductFamilyStatus.Draft);
            switch (status)
            {
                case ProductFamilyStatus.Draft:
                    family.AllowChanges = true;
                    family.IsDisabled = false;
                    break;
                case ProductFamilyStatus.Active:
                    family.AllowChanges = false;
                    family.IsDisabled = false;
                    break;
                case ProductFamilyStatus.Inactive:
                    family.AllowChanges = false;
                    family.IsDisabled = true;
                    break;
            }
        }

        internal IEnumerable<ProductFamilyCharacteristicUpload> ReadCharacteristics(Worksheet worksheet,
            ProductFamily family)
        {
            var list = new List<ProductFamilyCharacteristicUpload>();

            var currentRow = 1;
            var lastRow = worksheet.FindLastFilledRow();
            for (int i = 0; i < lastRow; i++)
            {
                var characteristicUpload = new ProductFamilyCharacteristicUpload();


                var action = ReadValue(worksheet, ExcelTemplateKeys.ActionLabel, currentRow);
                characteristicUpload.UploadAction = (UploadAction) Enum.Parse(typeof (UploadAction), action);
                var datatype = ReadValue(worksheet, ExcelTemplateKeys.DataTypeLabel, currentRow);
                if ("list" == datatype)
                {
                    characteristicUpload.Entity = ReadFeature(worksheet, currentRow, family);
                }
                else
                {
                    characteristicUpload.Entity = ReadAttribute(worksheet, currentRow, datatype, family);
                }

                SetId(worksheet, currentRow, characteristicUpload);
                if (!characteristicUpload.Entity.Id.HasValue)
                    characteristicUpload.UploadAction = UploadAction.Add;
                list.Add(characteristicUpload);
                ++currentRow;
            }
            return list;
        }

        private static void SetId(Worksheet worksheet, int currentRow,
            ProductFamilyCharacteristicUpload characteristicUpload)
        {
            string value = worksheet.Cells[currentRow, 0].StringValue;
            Guid parseid;
            if (Guid.TryParse(value, out parseid))
            {
                Guid? id = parseid;
                characteristicUpload.Entity.Id = id;
            }
        }

        private string ReadValue(Worksheet worksheet, string headerlabel, int row)
        {
            var header = worksheet.Cells.Find(headerlabel, worksheet.Cells[0, 0], new FindOptions());
            return worksheet.Cells[row, header.Column].StringValue;
        }

        internal ProductFamilyAttribute ReadAttribute(Worksheet worksheet, int row, string dataType,
            ProductFamily family)
        {
            var entity = new ProductFamilyAttribute
            {
                DataTypeId =
                    (ProductFamilyCharacteristicDataType)
                        Enum.Parse(typeof (ProductFamilyCharacteristicDataType), dataType),
                Name = ReadValue(worksheet, ExcelTemplateKeys.NameLabel, row),
                Description = ReadValue(worksheet, ExcelTemplateKeys.DescriptionLabel, row),
                CharacteristicTypeId = _metaDataProvider.GetCharacteristicTypes()
                    .First(x => x.Value == ReadValue(worksheet, ExcelTemplateKeys.TypeLabel, row))
                    .Key,
                ScopeId = GetScopeId(ReadValue(worksheet, ExcelTemplateKeys.ScopeLabel, row), family),
                IsRequired = ReadValue(worksheet, ExcelTemplateKeys.RequiredLabel, row).FromYesNo(),
                IsValueRequired = ReadValue(worksheet, ExcelTemplateKeys.ValueRequiredLabel, row).FromYesNo(),
                UnitOfMeasureId = ReadValue(worksheet, ExcelTemplateKeys.UoMLabel, row).ParseOrDefault(null as Guid?)
            };
            var option = ReadValue(worksheet, ExcelTemplateKeys.AllowedValueTypeLabel, row);
            var options = option.Split(',').Select(s => s.Trim());
            foreach (var option1 in options)
            {
                switch (option1.ToLowerInvariant())
                {
                    case "range":
                        entity.Options.Add(new ProductFamilyCharacteristicOption
                        {
                            Id = new Guid("CF5A3589-FBEB-E211-BF96-54DA2537410C"),
                            Name = "AllowValueTypes",
                            Description = "Allowed value types for field.",
                            Value = "Range"
                        });
                        break;
                    case "multiple":
                        entity.Options.Add(new ProductFamilyCharacteristicOption
                        {
                            Id = new Guid("D05A3589-FBEB-E211-BF96-54DA2537410C"),
                            Name = "AllowValueTypes",
                            Description = "Allowed value types for field.",
                            Value = "Multiple"
                        });
                        break;
                    case "single":
                        entity.Options.Add(new ProductFamilyCharacteristicOption
                        {
                            Id = new Guid("D15A3589-FBEB-E211-BF96-54DA2537410C"),
                            Name = "AllowValueTypes",
                            Description = "Allowed value types for field.",
                            Value = "Single"
                        });
                        break;
                }
            }
            return entity;
        }


        internal ProductFamilyFeature ReadFeature(Worksheet worksheet, int row, ProductFamily family)
        {
            var entity = new ProductFamilyFeature
            {
                Name = ReadValue(worksheet, ExcelTemplateKeys.NameLabel, row),
                Description = ReadValue(worksheet, ExcelTemplateKeys.DescriptionLabel, row),
                CharacteristicTypeId = _metaDataProvider.GetCharacteristicTypes()
                    .First(x => x.Value == ReadValue(worksheet, ExcelTemplateKeys.TypeLabel, row))
                    .Key,
                ScopeId = GetScopeId(ReadValue(worksheet, ExcelTemplateKeys.ScopeLabel, row), family),
                IsRequired = ReadValue(worksheet, ExcelTemplateKeys.RequiredLabel, row).FromYesNo(),
                IsValueRequired = ReadValue(worksheet, ExcelTemplateKeys.ValueRequiredLabel, row).FromYesNo(),
                AllowChanges = ReadValue(worksheet, ExcelTemplateKeys.ListOpenLabel, row).FromYesNo(),
                AllowedValues = ReadValue(worksheet, ExcelTemplateKeys.AllowedValueLabel, row),
                UnitOfMeasureId = ReadValue(worksheet, ExcelTemplateKeys.UoMLabel, row).ParseOrDefault(null as Guid?)
            };

            var option = ReadValue(worksheet, ExcelTemplateKeys.AllowedValueTypeLabel, row);
            var options = option.Split(',').Select(s => s.Trim());
            foreach (var option1 in options)
            {
                switch (option1.ToLowerInvariant())
                {
                    case "range":
                        entity.Options.Add(new ProductFamilyCharacteristicOption
                        {
                            Id = new Guid("B9DAAD50-FBEB-E211-BF96-54DA2537410C"),
                            Name = "AllowValueTypes",
                            Description = "Allowed value types for field.",
                            Value = "Range"
                        });
                        break;
                    case "multiple":
                        entity.Options.Add(new ProductFamilyCharacteristicOption
                        {
                            Id = new Guid("0CB2391A-FBEB-E211-BF96-54DA2537410C"),
                            Name = "AllowValueTypes",
                            Description = "Allowed value types for field.",
                            Value = "Multiple"
                        });
                        break;
                    case "single":
                        entity.Options.Add(new ProductFamilyCharacteristicOption
                        {
                            Id = new Guid("CB2CA338-FBEB-E211-BF96-54DA2537410C"),
                            Name = "AllowValueTypes",
                            Description = "Allowed value types for field.",
                            Value = "Single"
                        });
                        break;
                }
            }
            return entity;
        }

        internal IList<ProductFamilyFeatureAllowedValueDependencyUpload> ReadDependencyMappings(Workbook workbook)
        {
            var worksheet = workbook.Worksheets[ExcelTemplateKeys.Dependencies];
            var lastRow = worksheet.FindLastFilledRow();
            var dependencyMappings = new List<ProductFamilyFeatureAllowedValueDependencyUpload>();

            for (int i = 1; i <= lastRow; ++i)
            {
                string uploadActionString = worksheet.Cells[i, 4].StringValue;
                var uploadAction = uploadActionString.ParseOrDefault(UploadAction.Unknown);
                dependencyMappings.Add(
                    new ProductFamilyFeatureAllowedValueDependencyUpload
                    {
                        Parent = worksheet.Cells[i, 0].StringValue,
                        ParentValues =
                            worksheet.Cells[i, 1].StringValue.SplitAndTrim(ExcelTemplateKeys.StandardDelimiter),
                        Child = worksheet.Cells[i, 2].StringValue,
                        ChildValues =
                            worksheet.Cells[i, 3].StringValue.SplitAndTrim(ExcelTemplateKeys.StandardDelimiter),
                        UploadAction = uploadAction
                    });
            }

            return dependencyMappings;
        }

        private Guid GetScopeId(string value, ProductFamily family)
        {
            switch (value.ToLowerInvariant())
            {
                case "global":
                    return new Guid(ExcelTemplateKeys.GlobalScopeId);
                case "family":
                    return default(Guid);
                default: //business unit 
                    return family.BusinessUnitId.GetValueOrDefault();
            }
        }
    }
}