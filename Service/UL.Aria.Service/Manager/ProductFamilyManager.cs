using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Class ProductFamilyManager
    /// </summary>
    public class ProductFamilyManager : IProductFamilyManager
    {
        private readonly IProductFamilyImportManager _importManager;
        private readonly IProductFamilyAttributeProvider _productFamilyAttributeProvider;
        private readonly IProductFamilyFeatureProvider _productFamilyFeatureProvider;
        private readonly IProductFamilyProvider _productFamilyProvider;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductFamilyManager" /> class.
        /// </summary>
        /// <param name="productFamilyProvider">The product family provider.</param>
        /// <param name="productFamilyFeatureProvider">The product family feature provider.</param>
        /// <param name="productFamilyAttributeProvider">The product family attribute provider.</param>
        /// <param name="importManager">The import manager.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public ProductFamilyManager(IProductFamilyProvider productFamilyProvider,
            IProductFamilyFeatureProvider productFamilyFeatureProvider,
            IProductFamilyAttributeProvider productFamilyAttributeProvider, IProductFamilyImportManager importManager,
            ITransactionFactory transactionFactory)
        {
            _productFamilyProvider = productFamilyProvider;
            _productFamilyFeatureProvider = productFamilyFeatureProvider;
            _productFamilyAttributeProvider = productFamilyAttributeProvider;
            _importManager = importManager;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Uploads the specified file to upload.
        /// </summary>
        /// <param name="fileToUpload">The file to upload.</param>
        /// <returns></returns>
        public ProductFamilyUploadResult Upload(Stream fileToUpload)
        {
            return _importManager.Import(fileToUpload);
        }

        /// <summary>
        ///     Creates the specified product family.
        /// </summary>
        /// <param name="productFamily">The product family.</param>
        /// <returns>Guid.</returns>
        public Guid Create(ProductFamily productFamily)
        {
            return _productFamilyProvider.Create(productFamily);
        }

        /// <summary>
        ///     Updates the specified family.
        /// </summary>
        /// <param name="productFamily">The family.</param>
        public void Update(ProductFamily productFamily)
        {
            _productFamilyProvider.Update(productFamily);
        }

        /// <summary>
        ///     Get the details.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ProductFamilyDetail.</returns>
        public ProductFamilyDetail GetDetail(Guid id)
        {
            var productFamily = Get(id);
            IReadOnlyCollection<ProductFamilyCharacteristicDomainEntity> characteristics;
            using (var scope = _transactionFactory.Create())
            {
                characteristics = GetProductFamilyCharacteristics(productFamily.Id.Value);
                scope.Complete();
            }

            var featureAssociations = GetProductFamilyFeatureAssociations(id);
            var dependencyMappings = GetDependenciesForDto(id, featureAssociations);
            var allowedValues = _productFamilyFeatureProvider.FindAllowedValuesByFamily(id);
            return new ProductFamilyDetail
            {
                ProductFamily = productFamily,
                Characteristics = characteristics.Select(y=> 
                    new ProductFamilyCharacteristicAssociationModel
                    {
                        CharacteristicId = y.Id.Value, 
                        CharacteristicType = (y is ProductFamilyAttribute ? ProductFamilyCharacteristicType.Attribute : ProductFamilyCharacteristicType.Feature),
                        AllowedFeatureValueIds = allowedValues.Where(z => z.FeatureValue.FeatureId == y.Id.Value).Select(x=> x.FeatureValue.Id.Value).ToList(),
                        OptionIds = y.Options.Select(x=>x.Id.Value).ToList()
                        

                    }).ToList(),
                Dependencies = dependencyMappings
            };
        }

        /// <summary>
        ///     Gets the dependencies.
        /// </summary>
        /// <param name="familiyId">The familiy id.</param>
        /// <param name="featureAssociations">The feature associations.</param>
        /// <returns>List{ProductFamilyFeatureAllowedValueDependencyMapping}.</returns>
        public List<ProductFamilyFeatureDependency> GetDependenciesForDto(Guid familiyId,
            IEnumerable<ProductFamilyFeatureAssociation> featureAssociations)
        {
            var allowedValues = GetAllowedValuesByFamily(familiyId);
            var dependencyMappings = new List<ProductFamilyFeatureDependency>();
            foreach (var featureAssociation in featureAssociations)
            {
                var depends = GetValueDependenciesByFamilyAllowedFeatureId(featureAssociation.Id.Value);

                if (null != depends)
                {
                    foreach (var dependency in depends)
                    {
                        var childvalue =
                            allowedValues.First(x => x.Id == dependency.ChildProductFamilyFeatureAllowedValueId);
                        var childAssociation =
                            featureAssociations.First(x => childvalue.FeatureValue.FeatureId == x.CharacteristicId);
                        var parentValue = allowedValues.First(x => x.Id == dependency.ParentProductFamilyFeatureAllowedValueId);
                        var foundMapping =
                            dependencyMappings.FirstOrDefault(
                                x =>
                                    x.ChildId== childAssociation.Id &&
                                    x.ParentValueIds.Any(y => y == dependency.ParentProductFamilyFeatureAllowedValueId));
                        if (null == foundMapping)
                        {
                            var mapping = new ProductFamilyFeatureDependency
                            {
                                ParentId = parentValue.FeatureValue.FeatureId,
                                ChildId = childvalue.FeatureValue.FeatureId,
                            };
                            mapping.ChildValueIds.Add(childvalue.FeatureValue.Id.Value);
                            mapping.ParentValueIds.Add(parentValue.FeatureValue.Id.Value);
                            dependencyMappings.Add(mapping);
                        }
                        else
                        {
                            foundMapping.ChildValueIds.Add(dependency.ChildProductFamilyFeatureAllowedValueId);
                        }
                    }
                }
            }
            return dependencyMappings;
        }

        /// <summary>
        ///     Gets the dependencies.
        /// </summary>
        /// <param name="familiyId">The familiy id.</param>
        /// <param name="featureAssociations">The feature associations.</param>
        /// <returns>List{ProductFamilyFeatureAllowedValueDependencyMapping}.</returns>
        public List<ProductFamilyFeatureAllowedValueDependencyMapping> GetDependencies(Guid familiyId,
            IEnumerable<ProductFamilyFeatureAssociation> featureAssociations)
        {
            var allowedValues = GetAllowedValuesByFamily(familiyId);
            var dependencyMappings = new List<ProductFamilyFeatureAllowedValueDependencyMapping>();
            foreach (var featureAssociation in featureAssociations)
            {
                var depends = GetValueDependenciesByFamilyAllowedFeatureId(featureAssociation.Id.Value);

                if (null != depends)
                {
                    foreach (var dependency in depends)
                    {
                        var childvalue =
                            allowedValues.First(x => x.Id == dependency.ChildProductFamilyFeatureAllowedValueId);
                        var childAssociation =
                            featureAssociations.First(x => childvalue.FeatureValue.FeatureId == x.CharacteristicId);
                        var parentvalue =
                            allowedValues.First(x => x.Id == dependency.ParentProductFamilyFeatureAllowedValueId);

                        var foundMapping =
                            dependencyMappings.FirstOrDefault(
                                x =>
                                    x.ChildAssocation.Id == childAssociation.Id &&
                                    x.ParentValues.Any(y => y.Id == parentvalue.Id));
                        if (null == foundMapping)
                        {
                            var mapping = new ProductFamilyFeatureAllowedValueDependencyMapping
                            {
                                ParentAssocation = featureAssociation,
                                ChildAssocation = childAssociation,
                            };
                            mapping.ChildValues.Add(childvalue);
                            mapping.ParentValues.Add(parentvalue);
                            dependencyMappings.Add(mapping);
                        }
                        else
                        {
                            foundMapping.ChildValues.Add(childvalue);
                        }
                    }
                }
            }
            return dependencyMappings;
        }

        /// <summary>
        ///     Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ProductFamily.</returns>
        public ProductFamily Get(Guid id)
        {
            return _productFamilyProvider.Get(id);
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductFamily> GetAll()
        {
            return _productFamilyProvider.GetAll();
        }

        /// <summary>
        ///     Gets the product families by business unit.
        /// </summary>
        /// <param name="businessUnitId">The expected GUID.</param>
        /// <returns>
        ///     Readonly dictionary of <see cref="ProductFamily" /> keyed by id.
        /// </returns>
        public IReadOnlyDictionary<Guid, ProductFamily> GetProductFamiliesByBusinessUnit(Guid businessUnitId)
        {
            return _productFamilyProvider.GetProductFamiliesByBusinessUnit(businessUnitId);
        }

        /// <summary>
        ///     Gets the product family characteristics.
        /// </summary>
        /// <param name="productFamilyId">The expected id.</param>
        /// <returns></returns>
        public IReadOnlyCollection<ProductFamilyCharacteristicDomainEntity> GetProductFamilyCharacteristics(
            Guid productFamilyId)
        {
            return _productFamilyAttributeProvider.GetByProductFamilyId(productFamilyId)
                .Union<ProductFamilyCharacteristicDomainEntity>(
                    _productFamilyFeatureProvider.GetByProductFamilyId(productFamilyId))
                .OrderBy(c => c.SortOrder).ThenBy(c => c.Name)
                .ToList();
        }

        /// <summary>
        ///     Saves the product family attribute associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <param name="productFamilyAttributeAssociationIds">The product family attribute association ids.</param>
        public void SaveProductFamilyAttributeAssociations(Guid productFamilyId,
            IList<Guid> productFamilyAttributeAssociationIds)
        {
            //Migrate the code to a new association provider
            var existingAttributes = _productFamilyAttributeProvider.Get(productFamilyAttributeAssociationIds)
                .Select(x => x.Id.Value);
            var missingAttributeIds = productFamilyAttributeAssociationIds.Except(existingAttributes).ToArray();
            if (missingAttributeIds.Any())
            {
                throw new ValidationException(string.Format(CultureInfo.InvariantCulture,
                    "Attributes being saved do not exist in the database ({0})", string.Join(",", missingAttributeIds)));
            }

            _productFamilyProvider.SaveProductFamilyAttributeAssociations(productFamilyId,
                productFamilyAttributeAssociationIds.Select(upload => new ProductFamilyAttributeAssociation
                {
                    Id = Guid.NewGuid(),
                    CharacteristicId = upload,
                    ProductFamilyId = productFamilyId
                }).ToList());
        }

        /// <summary>
        ///     Saves the product family feature associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <param name="productFamilyFeatureAssociationIds">The product family feature association ids.</param>
        public void SaveProductFamilyFeatureAssociations(Guid productFamilyId,
            IList<Guid> productFamilyFeatureAssociationIds)
        {
            //Migrate the code to a new association provider
            var existingAttributes = _productFamilyFeatureProvider.Get(productFamilyFeatureAssociationIds)
                .Select(x => x.Id.Value);
            var missingAttributeIds = productFamilyFeatureAssociationIds.Except(existingAttributes).ToArray();
            if (missingAttributeIds.Any())
            {
                throw new ValidationException(string.Format(CultureInfo.InvariantCulture,
                    "Attributes being saved do not exist in the database ({0})", string.Join(",", missingAttributeIds)));
            }

            _productFamilyProvider.SaveProductFamilyFeatureAssociations(productFamilyId,
                productFamilyFeatureAssociationIds.Select(upload => new ProductFamilyFeatureAssociation()
                {
                    Id = Guid.NewGuid(),
                    CharacteristicId = upload,
                    ProductFamilyId = productFamilyId
                }).ToList());
        }

        /// <summary>
        ///     Gets the product family attribute associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyAttributeAssociation> GetProductFamilyAttributeAssociations(Guid productFamilyId)
        {
            return _productFamilyProvider.GetProductFamilyAttributeAssociations(productFamilyId);
        }

        /// <summary>
        ///     Gets the product family Feature associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureAssociation> GetProductFamilyFeatureAssociations(Guid productFamilyId)
        {
            return _productFamilyProvider.GetProductFamilyFeatureAssociations(productFamilyId);
        }

        /// <summary>
        ///     Gets the value dependencies by family allowed feature id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureAllowedValueDependency> GetValueDependenciesByFamilyAllowedFeatureId(
            Guid id)
        {
            return _productFamilyFeatureProvider.GetValueDependenciesByFamilyAllowedFeatureId(id);
        }

        /// <summary>
        ///     Gets the values.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <param name="familyId"></param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureAllowedValue> GetAllowedValues(Guid featureId, Guid familyId)
        {
            return _productFamilyFeatureProvider.FindAllowedValues(featureId, familyId);
        }

        /// <summary>
        ///     Gets the allowed values by family.
        /// </summary>
        /// <param name="familyId">The family id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureAllowedValue> GetAllowedValuesByFamily(Guid familyId)
        {
            return _productFamilyFeatureProvider.FindAllowedValuesByFamily(familyId);
        }
    }
}