using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;


namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provides an Provider interface for Business Unit assocations.
    /// </summary>
    public interface IBusinessUnitAssociationProvider
    {
        /// <summary>
        /// Updates the bulk.
        /// </summary>
        /// <param name="businessUnits">The business units.</param>
        /// <param name="parentId">The parent identifier.</param>
        void UpdateBulk(IEnumerable<BusinessUnitAssociation> businessUnits, Guid parentId);
    }
}
