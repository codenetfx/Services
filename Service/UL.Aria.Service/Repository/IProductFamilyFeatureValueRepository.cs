using System;
using System.Collections.Generic;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Defines persistance of <see cref="ProductFamilyFeatureValue"/>
    /// </summary>
    public interface IProductFamilyFeatureValueRepository :IRepositoryBase<ProductFamilyFeatureValue>
    {
        /// <summary>
        /// Finds the values by feature id.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyFeatureValue> FindByFeatureId(Guid featureId);
    }
}