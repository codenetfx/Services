using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Product provider interface.
    /// </summary>
    public interface IProductProvider
    {
        /// <summary>
        ///     Creates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        Guid Create(Product product);

        /// <summary>
        ///     Creates the specified product upload result.
        /// </summary>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <returns>Guid.</returns>
        Guid Create(ProductUploadResult productUploadResult);

        /// <summary>
        ///     Creates the specified product upload message.
        /// </summary>
        /// <param name="productUploadMessage">The product upload message.</param>
        /// <returns>Guid.</returns>
        Guid Create(ProductUploadMessage productUploadMessage);

        /// <summary>
        ///     Updates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        void Update(Product product);

        /// <summary>
        ///     Updates the specified product upload.
        /// </summary>
        /// <param name="productUpload">The product upload.</param>
        void Update(ProductUpload productUpload);

        /// <summary>
        ///     Gets the specified product id.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        Product Get(Guid productId);

        /// <summary>
        ///     Gets the by product family id.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        IEnumerable<Product> GetByProductFamilyId(Guid productFamilyId);

        /// <summary>
        ///     Uploads the specified file.
        /// </summary>
        /// <param name="productUpload">The product upload.</param>
        /// <returns>Confirmation Id.</returns>
        Guid Upload(ProductUpload productUpload);

        /// <summary>
        /// Fetches the next available <see cref="ProductUpload"/> for processing.
        /// </summary>
        /// <returns>The next <see cref="ProductUpload"/>. May return null (will not throw) if there is none.</returns>
        ProductUpload FetchProductUploadNextForProcessing();

        /// <summary>
        ///     Fetches the by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="rowStartIndex">Start index of the row.</param>
        /// <param name="rowEndIndex">End index of the row.</param>
        /// <returns>ProductUploadSearchResultSet.</returns>
        ProductUploadSearchResultSet FetchByUserId(Guid userId, long rowStartIndex, long rowEndIndex);

        /// <summary>
        ///     Gets the by product upload id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUploadResultSearchResultSet.</returns>
        ProductUploadResultSearchResultSet GetByProductUploadId(Guid productUploadId);

        /// <summary>
        ///     Gets the by id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUpload.</returns>
        ProductUpload GetById(Guid productUploadId);

        /// <summary>
        /// Deletes the specified product id.
        /// </summary>
        /// <param name="productId">The product id.</param>
        void Delete(Guid productId);

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <param name="status">The status.</param>
        /// <param name="submittedDateTime"></param>
        void UpdateStatus(Guid productId, ProductStatus status, DateTime? submittedDateTime);

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        ProductStatus GetStatus(Guid productId);

        /// <summary>
        /// Gets the product upload header.
        /// </summary>
        /// <param name="productUploadId">The product upload unique identifier.</param>
        /// <returns></returns>
        ProductUpload GetProductUploadHeader(Guid productUploadId);

        /// <summary>
        /// Gets the header. NON TRANSACTIONAL
        /// </summary>
        /// <param name="productId">The product unique identifier.</param>
        /// <returns>The <see cref="Product"/></returns>
        /// <remarks>NON TRANSACTIONAL</remarks>
        Product GetHeader(Guid productId);
    }
}