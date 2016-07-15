using System;
using System.Collections.Generic;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Interface IProductFamilyRepository
    /// </summary>
    public interface IProductFamilyRepository : IRepositoryBase<ProductFamily>
    {
        /// <summary>
        ///     Creates the specified product family.
        /// </summary>
        /// <param name="productFamily">The product family.</param>
        /// <returns>Guid.</returns>
        Guid Create(ProductFamily productFamily);

        /// <summary>
        ///     Gets the product familes by business unit.
        /// </summary>
        /// <param name="businessUnitId">The business unit id.</param>
        /// <returns>IReadOnlyDictionary{GuidProductFamily}.</returns>
        IReadOnlyDictionary<Guid, ProductFamily> GetProductFamiliesByBusinessUnit(Guid businessUnitId);
    }
}