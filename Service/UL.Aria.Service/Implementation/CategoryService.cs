using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     Class CategoryService
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryProvider _categoryProvider;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CategoryService" /> class.
        /// </summary>
        /// <param name="categoryProvider">The category provider.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public CategoryService(ICategoryProvider categoryProvider, IMapperRegistry mapperRegistry,
            ITransactionFactory transactionFactory)
        {
            _categoryProvider = categoryProvider;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{CategoryDto}.</returns>
        public IList<CategoryDto> FetchAll()
        {
            return _categoryProvider.FetchAll().Select(_mapperRegistry.Map<CategoryDto>).ToList();
        }

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>CategoryDto.</returns>
        public CategoryDto Fetch(string id)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");

            var category = _categoryProvider.Fetch(id.ToGuid());

            return _mapperRegistry.Map<CategoryDto>(category);
        }

        /// <summary>
        ///     Creates the specified category.
        /// </summary>
        /// <param name="category">The category.</param>
        public string Create(CategoryDto category)
        {
            Guard.IsNotNull(category, "Category");
            Guid id;

            var categoryBo = _mapperRegistry.Map<Category>(category);

            using (var transactionScope = _transactionFactory.Create())
            {
                id = _categoryProvider.Create(categoryBo);
                transactionScope.Complete();
            }

            return id.ToString();
        }

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="category">The category.</param>
        public void Update(string id, CategoryDto category)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");
            Guard.IsNotNull(category, "Category");

            var categoryBo = _mapperRegistry.Map<Category>(category);

            using (var transactionScope = _transactionFactory.Create())
            {
                _categoryProvider.Update(id.ToGuid(), categoryBo);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(string id)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");

            using (var transactionScope = _transactionFactory.Create())
            {
                _categoryProvider.Delete(convertedId);
                transactionScope.Complete();
            }
        }
    }
}