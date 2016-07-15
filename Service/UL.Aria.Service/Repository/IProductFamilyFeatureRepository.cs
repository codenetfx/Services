using System;
using System.Collections.Generic;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Repository for the <see cref="ProductFamilyFeature" />
    /// </summary>
    public interface IProductFamilyFeatureRepository : IRepositoryBase<ProductFamilyFeature>
    {
        /// <summary>
        ///     Creates the specified product family feature.
        /// </summary>
        /// <param name="productFamilyFeature">The product family feature.</param>
        /// <returns></returns>
        Guid Create(ProductFamilyFeature productFamilyFeature);

        /// <summary>
        /// Gets the by product family id.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        IList<ProductFamilyFeature> FindByProductFamilyId(Guid productFamilyId);

        /// <summary>
        /// Finds the features by id list.
        /// </summary>
        /// <param name="productFamilyIds">The product family ids.</param>
        /// <returns></returns>
        IList<ProductFamilyFeature> FindByIds(IList<Guid> productFamilyIds);

        /// <summary>
        /// Finds the by scope id.
        /// </summary>
        /// <param name="scopeId">The scope id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeature> FindByScopeId(Guid scopeId);
    }
}