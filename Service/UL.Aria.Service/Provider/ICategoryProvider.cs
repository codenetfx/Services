using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Interface ICategoryProvider
    /// </summary>
    public interface ICategoryProvider
    {
        /// <summary>
        ///     Fetches all categories.
        /// </summary>
        /// <returns>IList{Category}.</returns>
        IList<Category> FetchAll();

        /// <summary>
        ///     Fetches the specified category by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Category.</returns>
        Category Fetch(Guid id);

        /// <summary>
        ///     Creates the specified category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>Guid.</returns>
        Guid Create(Category category);

        /// <summary>
        ///     Updates the specified category by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="category">The category.</param>
        void Update(Guid id, Category category);

        /// <summary>
        ///     Deletes the specified category by id.
        /// </summary>
        /// <param name="id">The id.</param>
        void Delete(Guid id);
    }
}