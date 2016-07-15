using System;
using System.Collections.Generic;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Defines persistance of <see cref="ProductFamilyFeatureAllowedValue"/>
    /// </summary>
    public interface IProductFamilyFeatureAllowedValueRepository:IRepositoryBase<ProductFamilyFeatureAllowedValue>
    {
        /// <summary>
        /// Gets a collection of <see cref="ProductFamilyFeatureAllowedValue"/> by family id.
        /// </summary>
        /// <param name="featureId">The family id.</param>
        /// <param name="familyId"></param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeatureAllowedValue> FindByFeatureId(Guid featureId, Guid familyId);

        /// <summary>
        /// Finds the by family id.
        /// </summary>
        /// <param name="familyId">The family id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeatureAllowedValue> FindByFamilyId(Guid familyId);
    }
}