using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    ///     Implements operations to Validate <see cref="ProductCharacteristic" /> objects with documents.
    /// </summary>
    public class ProductDocumentCharacteristicValidator: ICharacteristicValidator
    {
        private readonly ISearchProvider _searchProvider;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDocumentCharacteristicValidator"/> class.
        /// </summary>
        /// <param name="searchProvider">The search provider.</param>
        public ProductDocumentCharacteristicValidator(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
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
            var documentCharacteristics = characteristics.Where(x => x.DataType == ProductFamilyCharacteristicDataType.DocumentReference);
            if (!documentCharacteristics.Any())
                return;
            var documentsForProduct = GetDocumentsForProduct(product);
            
            var absentList = documentCharacteristics.Where
                (x =>
                {
                    if (string.IsNullOrEmpty(x.Value)) // handle with required validator, not this.
                        return false;
                    Guid id;
                    if (!Guid.TryParse(x.Value, out id))
                        return true;
                    return !documentsForProduct.Any(d => d.Metadata.ContainsKey(AssetFieldNames.AriaDocumentId) && new Guid(d.Metadata[AssetFieldNames.AriaDocumentId]) == id);
                });
            if (absentList.Any())
            {
                foreach (var item in absentList)
                {
                    var productFamilyAttribute = characteristicDefinitions[item.ProductFamilyCharacteristicId];
                    errors.Add(String.Format("There was no document found for attribute {0} ", productFamilyAttribute.Name));
                }
            }
        }

        /// <summary>
        ///     Gets the documents for product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        internal IQueryable<SearchResult> GetDocumentsForProduct(Product product)
        {
            Guid? productId = product.Id;

            var searchCriteria = new SearchCriteria
            {
                EntityType = EntityTypeEnumDto.Product,
                Keyword = string.Format("{0}:{1}", AssetFieldNames.AriaProductId, productId.ToString()),
                StartIndex = 0,
                EndIndex = 0
            };

            SearchResultSet searchResultSet = _searchProvider.Search(searchCriteria);

            if (searchResultSet.Results.Count > 0)
            {
                SearchResult result = searchResultSet.Results[0];

                var dictionary = new Dictionary<string, string>(result.Metadata);

                string parentAssetId = dictionary[AssetFieldNames.AriaContainerId];

                searchCriteria = new SearchCriteria
                {
                    EntityType = EntityTypeEnumDto.Document,
                    Keyword =
                        String.Format("{1}:{0}  ", parentAssetId,
                                      String.Concat('"', AssetFieldNames.AriaContainerId, '"')),
                    StartIndex = 0,
                    EndIndex = 499
                };
                
                searchResultSet = _searchProvider.Search(searchCriteria);

                IQueryable<SearchResult> searchResultDtos =
                    searchResultSet.Results.Where(i => i.EntityType == EntityTypeEnumDto.Document)
                                   .AsQueryable();

                return searchResultDtos;
            }

            return Enumerable.Empty<SearchResult>().AsQueryable();
        }
    }
}