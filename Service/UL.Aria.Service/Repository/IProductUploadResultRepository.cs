using System;

using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Product Repository
    /// </summary>
    public interface IProductUploadResultRepository : IRepositoryBase<ProductUploadResult>
    {
        /// <summary>
        ///     Creates the specified product upload result.
        /// </summary>
        /// <param name="productUploadResult">The product upload result.</param>
        /// <returns>Guid.</returns>
        Guid Create(ProductUploadResult productUploadResult);

        /// <summary>
        ///     Gets the by product upload id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUploadResultSearchResultSet.</returns>
        ProductUploadResultSearchResultSet GetByProductUploadId(Guid productUploadId);
    }
}