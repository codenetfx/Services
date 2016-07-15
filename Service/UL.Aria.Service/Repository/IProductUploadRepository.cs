using System;

using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Product Repository
    /// </summary>
    public interface IProductUploadRepository : IRepositoryBase<ProductUpload>
    {
        /// <summary>
        ///     Uploads the specified file.
        /// </summary>
        /// <param name="productUpload">The product upload.</param>
        /// <returns>Confirmation Id.</returns>
        Guid Upload(ProductUpload productUpload);

        /// <summary>
        ///     Fetches the by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="rowStartIndex"></param>
        /// <param name="rowEndIndex"></param>
        /// <returns>List{ProductUpload}.</returns>
        ProductUploadSearchResultSet FetchByUserId(Guid userId, long rowStartIndex, long rowEndIndex);

        /// <summary>
        /// Fetches the next available <see cref="ProductUpload"/> for processing.
        /// </summary>
        /// <returns>The next <see cref="ProductUpload"/>. May return null (will not throw) if there is none.</returns>
        ProductUpload FetchNextForProcessing();
    }
}