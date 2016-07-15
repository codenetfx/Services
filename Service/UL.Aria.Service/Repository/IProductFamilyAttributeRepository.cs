using System;
using System.Collections.Generic;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Interface IProductFamilyAttributeRepository
    /// </summary>
    public interface IProductFamilyAttributeRepository : IRepositoryBase<ProductFamilyAttribute>
    {
        /// <summary>
        /// Creates the specified product family attribute.
        /// </summary>
        /// <param name="productFamilyAttribute">The product family attribute.</param>
        /// <returns>Product family attribute id.</returns>
        Guid Create(ProductFamilyAttribute productFamilyAttribute);

        /// <summary>
        /// Gets the by product family id.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        IList<ProductFamilyAttribute> FindByProductFamilyId(Guid productFamilyId);

        /// <summary>
        /// Finds the attributes by id list.
        /// </summary>
        /// <param name="productFamilyIds">The product family ids.</param>
        /// <returns></returns>
        IList<ProductFamilyAttribute> FindByIds(IList<Guid> productFamilyIds);

        /// <summary>
        /// Finds the by scope id.
        /// </summary>
        /// <param name="scopeId">The scope id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyAttribute> FindByScopeId(Guid scopeId);
    }
}