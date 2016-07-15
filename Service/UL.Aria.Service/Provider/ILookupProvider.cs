using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Lookup;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Interface for Lookup Provider
    /// </summary>
    public interface ILookupProvider
    {
        /// <summary>
        ///     Fetch all BusinessUnits  
        /// </summary>
        /// <returns>IEnumerable{BusinessUnits}.</returns>
        IEnumerable<BusinessUnit> FetchAllBusinessUnits();
        
        /// <summary>
        /// Fetches the business unit by entity.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        IEnumerable<BusinessUnit> FetchBusinessUnitByEntity(Guid parentId);

        /// <summary>
        /// Updates the bulk.
        /// </summary>
        /// <param name="businessUnits">The business units.</param>
        /// <param name="parentId">The parent identifier.</param>
        void UpdateBulk(IEnumerable<BusinessUnit> businessUnits, Guid parentId);
    }
}
