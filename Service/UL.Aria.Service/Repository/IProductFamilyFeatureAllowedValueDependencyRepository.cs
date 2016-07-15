using System;
using System.Collections.Generic;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Defines persistance of <see cref="ProductFamilyFeatureAllowedValueDependency"/>
    /// </summary>
    public interface IProductFamilyFeatureAllowedValueDependencyRepository:IRepositoryBase<ProductFamilyFeatureAllowedValueDependency>
    {
        /// <summary>
        /// Finds <see cref="ProductFamilyFeatureAllowedValueDependency"/> by family feature allowed value id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeatureAllowedValueDependency> FindByFamilyAllowedFeatureId(Guid id);

        /// <summary>
        /// Finds dependencies by parent family allowed feature value id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeatureAllowedValueDependency> FindByParentFamilyAllowedFeatureValueId(Guid id);
    }
}