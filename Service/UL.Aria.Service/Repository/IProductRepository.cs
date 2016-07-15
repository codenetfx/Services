using System;
using System.Collections.Generic;

using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Product Repository
    /// </summary>
    public interface IProductRepository : IRepositoryBase<Product>
    {
        /// <summary>
        ///     Creates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        Guid Create(Product product);

        /// <summary>
        ///     Gets all <see cref="Product" />s that match the given product family id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        IEnumerable<Product> GetByProductFamilyId(Guid id);

        /// <summary>
        /// Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        new void Remove(Guid entityId);

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
        /// Gets the status.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        Product GetProductForStatusOnly(Guid productId);
    }
}