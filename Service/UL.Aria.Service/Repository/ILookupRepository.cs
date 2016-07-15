using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Lookup;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Interface for Lookup Repository
    /// </summary>
    public interface ILookupRepository
    {
        /// <summary>
        ///     FetchAll Business Unit
        /// </summary>
        /// <returns></returns>
        IEnumerable<BusinessUnit> FetchAllBusinessUnits();

        /// <summary>
        /// Fetches the business unit by entity.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        IEnumerable<BusinessUnit> FetchBusinessUnitByEntity(Guid parentId);
    }
}
