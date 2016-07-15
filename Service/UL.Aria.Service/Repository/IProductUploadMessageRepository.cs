using System;
using System.Collections.Generic;

using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Interface IProductUploadMessageRepository
    /// </summary>
    public interface IProductUploadMessageRepository : IRepositoryBase<ProductUploadMessage>
    {
        /// <summary>
        ///     Creates the specified product upload message.
        /// </summary>
        /// <param name="productUploadMessage">The product upload message.</param>
        /// <returns>Guid.</returns>
        Guid Create(ProductUploadMessage productUploadMessage);

        /// <summary>
        ///     Gets the by product upload result id.
        /// </summary>
        /// <param name="productUploadResultId">The product upload result id.</param>
        /// <returns>IEnumerable{ProductUploadMessage}.</returns>
        IEnumerable<ProductUploadMessage> GetByProductUploadResultId(Guid productUploadResultId);
    }
}