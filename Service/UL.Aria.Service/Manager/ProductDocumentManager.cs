using System;
using System.Collections.Generic;
using System.IO;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements operations for getting documents built from <see cref="Product"/> objetcts.
    /// </summary>
    public class ProductDocumentManager : IProductDocumentManager
    {
        private readonly IProductDocumentBuilder _productDocumentBuilder;
        private readonly IProductFamilyManager _productFamilyManager;
        private readonly IProductFamilyCharacteristicGroupHelper _productFamilyCharacteristicGroupHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDocumentManager" /> class.
        /// </summary>
        /// <param name="productDocumentBuilder">The product document builder.</param>
        /// <param name="productFamilyManager">The product family manager.</param>
        /// <param name="productFamilyCharacteristicGroupHelper">The product family characteristic group helper.</param>
        public ProductDocumentManager(IProductDocumentBuilder productDocumentBuilder, IProductFamilyManager productFamilyManager, IProductFamilyCharacteristicGroupHelper productFamilyCharacteristicGroupHelper)
        {
            _productDocumentBuilder = productDocumentBuilder;
            _productFamilyManager = productFamilyManager;
            _productFamilyCharacteristicGroupHelper = productFamilyCharacteristicGroupHelper;
        }

        /// <summary>
        /// Fetches the specified product document.
        /// </summary>
        /// <param name="product">The product.</param>
        public Stream Get(Product product)
        {
            return Get(new List<Product>{product}, product.ProductFamilyId);
        }

        /// <summary>
        /// Gets the document for a group of products
        /// </summary>
        /// <param name="products">The products.</param>
        /// <param name="familyId"></param>
        /// <returns></returns>
        public Stream Get(IEnumerable<Product> products, Guid familyId)
        {
            var productFamily = _productFamilyManager.Get(familyId);
            IEnumerable<ProductFamilyCharacteristicDomainEntity> baseCharacteristics;
            IEnumerable<ProductFamilyCharacteristicDomainEntity> variableCharacteristics;
            var characteristics = _productFamilyManager.GetProductFamilyCharacteristics(productFamily.Id.Value);
            _productFamilyCharacteristicGroupHelper.GroupCharacteristics(characteristics, out baseCharacteristics, out variableCharacteristics);
            return _productDocumentBuilder.Build(products, productFamily, new ProfileBo(), baseCharacteristics, variableCharacteristics , new List<ProductFamilyFeatureAllowedValueDependencyMapping>());
       
        }
    }
}