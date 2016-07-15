using System;
using System.Collections.Generic;
using System.Linq;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Class ProductFamilyProvider
    /// </summary>
    public class ProductFamilyProvider : IProductFamilyProvider
    {
        private readonly IProductFamilyRepository _productFamilyRepository;
        private readonly IProductFamilyAssociationRepository<ProductFamilyAttributeAssociation> _attributeAssociationRepository;
        private readonly IProductFamilyAssociationRepository<ProductFamilyFeatureAssociation> _featureAssociationRepository;
        private readonly IProductFamilyFeatureAllowedValueRepository _productFamilyFeatureAllowedValueRepository;
        private readonly IProductFamilyFeatureValueRepository _productFamilyFeatureValueRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyProvider" /> class.
        /// </summary>
        /// <param name="productFamilyRepository">The product family repository.</param>
        /// <param name="attributeAssociationRepository">The attribute association repository.</param>
        /// <param name="featureAssociationRepository">The feature association repository.</param>
        /// <param name="productFamilyFeatureAllowedValueRepository">The product family feature allowed value repository.</param>
        /// <param name="productFamilyFeatureValueRepository">The product family feature value repository.</param>
        public ProductFamilyProvider(IProductFamilyRepository productFamilyRepository, 
            IProductFamilyAssociationRepository<ProductFamilyAttributeAssociation> attributeAssociationRepository,
            IProductFamilyAssociationRepository<ProductFamilyFeatureAssociation> featureAssociationRepository,
            IProductFamilyFeatureAllowedValueRepository productFamilyFeatureAllowedValueRepository,
            IProductFamilyFeatureValueRepository productFamilyFeatureValueRepository)
        {
            _productFamilyRepository = productFamilyRepository;
            _attributeAssociationRepository = attributeAssociationRepository;
            _featureAssociationRepository = featureAssociationRepository;
            _productFamilyFeatureAllowedValueRepository = productFamilyFeatureAllowedValueRepository;
            _productFamilyFeatureValueRepository = productFamilyFeatureValueRepository;
        }

        /// <summary>
        ///     Creates the specified product family.
        /// </summary>
        /// <param name="productFamily">The product family.</param>
        /// <returns>Guid.</returns>
        public Guid Create(ProductFamily productFamily)
        {
            return _productFamilyRepository.Create(productFamily);
        }

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="productFamily">The product family.</param>
        public void Update(ProductFamily productFamily)
        {
            _productFamilyRepository.Update(productFamily);
        }

        /// <summary>
        ///     Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ProductFamily.</returns>
        public ProductFamily Get(Guid id)
        {
            return _productFamilyRepository.FindById(id);
        }

        /// <summary>
        ///     Gets the product families by business unit.
        /// </summary>
        /// <param name="businessUnitId">The business unit id.</param>
        /// <returns>IReadOnlyDictionary{GuidProductFamily}.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IReadOnlyDictionary<Guid, ProductFamily> GetProductFamiliesByBusinessUnit(Guid businessUnitId)
        {
            return _productFamilyRepository.GetProductFamiliesByBusinessUnit(businessUnitId);
        }

        /// <summary>
        /// Saves the product family attribute associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <param name="models">The product family attribute association ids.</param>
        public void SaveProductFamilyAttributeAssociations(Guid productFamilyId, IList<ProductFamilyAttributeAssociation> models)
        {
            foreach (var model in models)
            {
                _attributeAssociationRepository.Create(model);
            } 
        }

        /// <summary>
        /// Removes the product family attribute associations.
        /// </summary>
        /// <param name="productFamilyAttributeAssociationIds">The product family attribute association ids.</param>
        public void RemoveProductFamilyAttributeAssociations(IEnumerable<Guid> productFamilyAttributeAssociationIds)
        {
            foreach (var productFamilyAttributeAssociationId in productFamilyAttributeAssociationIds)
            {
                _attributeAssociationRepository.Remove(productFamilyAttributeAssociationId);
            }
        }

        /// <summary>
        /// Gets the product family attribute associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyAttributeAssociation> GetProductFamilyAttributeAssociations(Guid productFamilyId)
        {
            return _attributeAssociationRepository.GetByFamilyId(productFamilyId);
        }

        /// <summary>
        /// Gets the product family Feature associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureAssociation> GetProductFamilyFeatureAssociations(Guid productFamilyId)
        {
            return _featureAssociationRepository.GetByFamilyId(productFamilyId);
        }

        /// <summary>
        /// Saves the product family feature associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <param name="models">The product family feature association ids.</param>
        public void SaveProductFamilyFeatureAssociations(Guid productFamilyId, IList<ProductFamilyFeatureAssociation> models)
        {
            foreach (var model in models)
            {
                _featureAssociationRepository.Create(model);
            }
        }

        /// <summary>
        /// Saves the product family feature associations.
        /// </summary>
        /// <param name="productFamilyFeatureAssociationIds">The product family feature association ids.</param>
        public void RemoveProductFamilyFeatureAssociations(IEnumerable<Guid> productFamilyFeatureAssociationIds)
        {
            foreach (var productFamilyFeatureAssociationId in productFamilyFeatureAssociationIds)
            {
                _featureAssociationRepository.Remove(productFamilyFeatureAssociationId);
            }
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductFamily> GetAll()
        {
            return _productFamilyRepository.FindAll();
        }

        /// <summary>
        /// Gets the product family feature value by id and value.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <param name="value">The value.</param>
        /// <returns>ProductFamilyFeatureValue.</returns>
        public ProductFamilyFeatureValue GetProductFamilyFeatureValueByFeatureIdAndValue(Guid featureId, string value)
        {
            var featureValues = _productFamilyFeatureValueRepository.FindByFeatureId(featureId);
            return featureValues.FirstOrDefault(featureValue => featureValue.Value == value);
        }

        /// <summary>
        /// Creates the product family feature value.
        /// </summary>
        /// <param name="productFamilyFeature">The product family feature.</param>
        /// <param name="value">The value.</param>
        /// <param name="unitOfMeasureId"></param>
        /// <returns>ProductFamilyFeatureValue.</returns>
        public ProductFamilyFeatureValue CreateProductFamilyFeatureValue(ProductFamilyFeature productFamilyFeature, string value, Guid? unitOfMeasureId)
        {
            var productFamilyFeatureValue = new ProductFamilyFeatureValue
                {
                    Id = Guid.NewGuid(),
                    CreatedById = productFamilyFeature.CreatedById,
                    CreatedDateTime = productFamilyFeature.CreatedDateTime,
                    FeatureId = productFamilyFeature.Id.Value,
                    UpdatedById = productFamilyFeature.UpdatedById,
                    UpdatedDateTime = productFamilyFeature.UpdatedDateTime,
                    UnitOfMeasure = unitOfMeasureId.HasValue ? new UnitOfMeasure{Id = unitOfMeasureId} : null,
                    Value = value
                };
            _productFamilyFeatureValueRepository.Add(productFamilyFeatureValue);
            return productFamilyFeatureValue;
        }

        /// <summary>
        /// Removes the product family allowed feature value.
        /// </summary>
        /// <param name="familyId">The family id.</param>
        /// <param name="featureValueId">The feature value id.</param>
        public void RemoveProductFamilyAllowedFeatureValue(Guid familyId , Guid featureValueId)
        {
            var allowedFeatureValues = _productFamilyFeatureAllowedValueRepository.FindByFamilyId(familyId).Where(x => x.FeatureValue.Id == featureValueId);

            foreach(var allowFeatureValue in allowedFeatureValues)
                _productFamilyFeatureAllowedValueRepository.Remove(allowFeatureValue.Id.Value);
        }

        /// <summary>
        /// Removes the product family allowed feature value.
        /// </summary>
        /// <param name="allowedFeatureValueId">The allowed feature value id.</param>
        public void RemoveProductFamilyAllowedFeatureValue(Guid allowedFeatureValueId)
        {

                _productFamilyFeatureAllowedValueRepository.Remove(allowedFeatureValueId);
        }

        /// <summary>
        /// Creates the product family allowed feature value.
        /// </summary>
        /// <param name="familyId">The family id.</param>
        /// <param name="productFamilyFeatureValue">The product family feature value.</param>
        /// <returns>ProductFamilyFeatureAllowedValue.</returns>
        public ProductFamilyFeatureAllowedValue CreateProductFamilyAllowedFeatureValue(Guid familyId, ProductFamilyFeatureValue productFamilyFeatureValue)
        {
            var productFamilyFeatureAllowedValue = new ProductFamilyFeatureAllowedValue
                {
                    Id = Guid.NewGuid(),
                    FamilyId = familyId,
                    FeatureValue = productFamilyFeatureValue
                };
            _productFamilyFeatureAllowedValueRepository.Add(productFamilyFeatureAllowedValue);
            return productFamilyFeatureAllowedValue;
        }
    }
}