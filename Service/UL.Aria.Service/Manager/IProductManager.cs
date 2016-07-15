using System;
using System.Collections.Generic;
using System.IO;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Product Manager interface.
    /// </summary>
    public interface IProductManager
    {
        /// <summary>
        ///     Creates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        Guid Create(Product product);

        /// <summary>
        /// Updates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        void Update(Product product);

        /// <summary>
        ///     Gets the specified product by id.
        /// </summary>
        /// <param name="productId">The product by id.</param>
        /// <returns></returns>
        Product Get(Guid productId);

        /// <summary>
        /// Gets the download.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        Stream GetDownload(Guid productId);

        /// <summary>
        /// Gets the by product family id.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        IEnumerable<Product> GetByProductFamilyId(Guid productFamilyId);

        /// <summary>
        /// Gets the by family download.
        /// </summary>
        /// <param name="productFamilyId"></param>
        /// <returns></returns>
        Stream GetProductDownloadByProductFamilyId(Guid productFamilyId);

        /// <summary>
        /// Uploads the specified file.
        /// </summary>
        /// <param name="productUpload">The product upload.</param>
        /// <returns>Confirmation Id.</returns>
        Guid Upload(ProductUpload productUpload);

        /// <summary>
        /// Fetches the by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="rowStartIndex">Start index of the row.</param>
        /// <param name="rowEndIndex">End index of the row.</param>
        /// <returns>ProductUploadSearchResultSet.</returns>
        ProductUploadSearchResultSet FetchByUserId(Guid userId, long rowStartIndex, long rowEndIndex);

        /// <summary>
        /// Gets the by product upload id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUploadResultSearchResultSet.</returns>
        ProductUploadResultSearchResultSet GetByProductUploadId(Guid productUploadId);

        /// <summary>
        /// Gets the by id.
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
        /// Gets the product download by product upload id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        Stream GetProductDownloadByProductUploadId(Guid productUploadId);

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="product">The product.</param>
        void UpdateStatus(Product product);

        /// <summary>
        /// Gets the product upload header.
        /// </summary>
        /// <param name="productUploadId">The product upload unique identifier.</param>
        /// <returns></returns>
        ProductUpload GetProductUploadHeader(Guid productUploadId);
    }
}