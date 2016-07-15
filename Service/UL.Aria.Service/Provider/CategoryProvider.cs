using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Category provider class.
    /// </summary>
    public class CategoryProvider : ICategoryProvider
    {
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CategoryProvider" /> class.
        /// </summary>
        /// <param name="categoryRepository">The category repository.</param>
        public CategoryProvider(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Fetches all categories.
        /// </summary>
        /// <returns>IList{Category}.</returns>
        public IList<Category> FetchAll()
        {
            return _categoryRepository.FindAll();
        }

        /// <summary>
        /// Fetches the specified category by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Category.</returns>
        public Category Fetch(Guid id)
        {
            return _categoryRepository.FindById(id);
        }

        /// <summary>
        /// Creates the specified category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>Guid.</returns>
        public Guid Create(Category category)
        {
            return _categoryRepository.Create(category);
        }

        /// <summary>
        /// Updates the specified catgory by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="category">The category.</param>
        public void Update(Guid id, Category category)
        {
            _categoryRepository.Update(category);
        }

        /// <summary>
        /// Deletes the specified category by id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(Guid id)
        {
            _categoryRepository.Remove(id);
        }
    }
}