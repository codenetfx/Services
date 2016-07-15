using System;

using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Interface ICategoryRepository
    /// </summary>
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        /// <summary>
        ///     Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Guid.</returns>
        Guid Create(Category entity);

        /// <summary>
        ///     Gets the by id.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Category.</returns>
        Category GetById(Guid categoryId);
    }
}