using System;
using System.Collections.Generic;
using System.Linq;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Product provider.
    /// </summary>
    public class ProductProvider : IProductProvider
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductUploadMessageRepository _productUploadMessageRepository;
        private readonly IProductUploadRepository _productUploadRepository;
        private readonly IProductUploadResultRepository _productUploadResultRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductProvider" /> class.
        /// </summary>
        /// <param name="productRepository">The product repository.</param>
        /// <param name="productUploadRepository">The product upload repository.</param>
        /// <param name="productUploadResultRepository">The product upload result repository.</param>
        /// <param name="productUploadMessageRepository">The product upload message repository.</param>
        public ProductProvider(IProductRepository productRepository, IProductUploadRepository productUploadRepository,
            IProductUploadResultRepository productUploadResultRepository,
            IProductUploadMessageRepository productUploadMessageRepository)
        {
            _productRepository = productRepository;
            _productUploadRepository = productUploadRepository;
            _productUploadResultRepository = productUploadResultRepository;
            _productUploadMessageRepository = productUploadMessageRepository;
        }

        /// <summary>
        ///     Creates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        public Guid Create(Product product)
        {
            var productId = _productRepository.Create(product);
            return productId;
        }

        /// <summary>
        ///     Creates the specified product upload result.
        /// </summary>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <returns>Guid.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Guid Create(ProductUploadResult productUploadResult)
        {
            return _productUploadResultRepository.Create(productUploadResult);
        }

        /// <summary>
        ///     Creates the specified product upload message.
        /// </summary>
        /// <param name="productUploadMessage">The product upload message.</param>
        /// <returns>Guid.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Guid Create(ProductUploadMessage productUploadMessage)
        {
            return _productUploadMessageRepository.Create(productUploadMessage);
        }

        /// <summary>
        ///     Updates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        public void Update(Product product)
        {
            _productRepository.Update(product);
        }

        /// <summary>
        ///     Updates the specified product upload.
        /// </summary>
        /// <param name="productUpload">The product upload.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Update(ProductUpload productUpload)
        {
            _productUploadRepository.Update(productUpload);
        }

        /// <summary>
        ///     Gets the specified product id.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        public Product Get(Guid productId)
        {
            return _productRepository.FindById(productId);
        }

        /// <summary>
        ///     Gets the by product family id.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        public IEnumerable<Product> GetByProductFamilyId(Guid productFamilyId)
        {
            return _productRepository.GetByProductFamilyId(productFamilyId);
        }

        /// <summary>
        ///     Uploads the specified file.
        /// </summary>
        /// <param name="productUpload">The product upload.</param>
        /// <returns>Confirmation Id.</returns>
        public Guid Upload(ProductUpload productUpload)
        {
            return _productUploadRepository.Upload(productUpload);
        }

        /// <summary>
        ///     Fetches the next available <see cref="ProductUpload" /> for processing.
        /// </summary>
        /// <returns>The next <see cref="ProductUpload" />. May return null (will not throw) if there is none.</returns>
        public ProductUpload FetchProductUploadNextForProcessing()
        {
            return _productUploadRepository.FetchNextForProcessing();
        }

        /// <summary>
        ///     Fetches the by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="rowStartIndex">Start index of the row.</param>
        /// <param name="rowEndIndex">End index of the row.</param>
        /// <returns>ProductUploadSearchResultSet.</returns>
        public ProductUploadSearchResultSet FetchByUserId(Guid userId, long rowStartIndex, long rowEndIndex)
        {
            return _productUploadRepository.FetchByUserId(userId, rowStartIndex, rowEndIndex);
        }

        /// <summary>
        ///     Gets the product upload header.
        /// </summary>
        /// <param name="productUploadId">The product upload unique identifier.</param>
        /// <returns></returns>
        public ProductUpload GetProductUploadHeader(Guid productUploadId)
        {
            return _productUploadRepository.FindById(productUploadId);
        }

        /// <summary>
        ///     Gets the header. NON TRANSACTIONAL
        /// </summary>
        /// <param name="productId">The product unique identifier.</param>
        /// <returns>The <see cref="Product" /></returns>
        /// <remarks>NON TRANSACTIONAL</remarks>
        public Product GetHeader(Guid productId)
        {
            return _productRepository.GetProductForStatusOnly(productId);
        }

        /// <summary>
        ///     Gets the by product upload id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUploadResultSearchResultSet.</returns>
        public ProductUploadResultSearchResultSet GetByProductUploadId(Guid productUploadId)
        {
            var searchResultSet = _productUploadResultRepository.GetByProductUploadId(productUploadId);

            foreach (var searchResult in searchResultSet.Results)
            {
                var productUploadResult = searchResult.ProductUploadResult;
                var product = productUploadResult.Product;
                if (product != null && product.Id.HasValue)
                {
                    productUploadResult.Product =
                        _productRepository.GetProductForStatusOnly(productUploadResult.Product.Id.Value);

                    if (productUploadResult.Product != null)
                    {
                        searchResult.Name = productUploadResult.Product.Name;
                        searchResult.Title = productUploadResult.Product.Name;
                    }
                    else
                    {
                        productUploadResult.Product = product;
                    }
                }

                productUploadResult.Messages =
                    _productUploadMessageRepository.GetByProductUploadResultId(productUploadResult.Id.Value).ToList();
            }

            return searchResultSet;
        }

        /// <summary>
        ///     Gets the product upload by id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUpload.</returns>
        public ProductUpload GetById(Guid productUploadId)
        {
            return _productUploadRepository.FindById(productUploadId);
        }

        /// <summary>
        ///     Deletes the specified product id.
        /// </summary>
        /// <param name="productId">The product id.</param>
        public void Delete(Guid productId)
        {
            _productRepository.Remove(productId);
        }

        /// <summary>
        ///     Updates the status.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <param name="status">The status.</param>
        /// <param name="submittedDateTime"></param>
        public void UpdateStatus(Guid productId, ProductStatus status, DateTime? submittedDateTime)
        {
            _productRepository.UpdateStatus(productId, status, submittedDateTime);
        }

        /// <summary>
        ///     Gets the status.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        public ProductStatus GetStatus(Guid productId)
        {
            return _productRepository.GetStatus(productId);
        }
    }
}