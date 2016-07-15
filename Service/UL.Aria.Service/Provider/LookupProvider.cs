using System;
using System.Linq;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///  LookupProvider Provider
    /// </summary>
    public class LookupProvider : ILookupProvider
    {
        private readonly ILookupRepository _lookupRepository;
        private readonly IBusinessUnitAssociationProvider _businessUnitAssociationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupProvider" /> class.
        /// </summary>
        /// <param name="lookupRepository">The Lookup repository.</param>
        /// <param name="businessUnitAssociationProvider">The business unit association provider.</param>
        public LookupProvider(ILookupRepository lookupRepository, IBusinessUnitAssociationProvider businessUnitAssociationProvider)
        {
            _lookupRepository = lookupRepository;
            _businessUnitAssociationProvider = businessUnitAssociationProvider;
        }

        /// <summary>
        ///     Fetch all business units
        /// </summary>
        /// <returns>IEnumerable{BusinessUnit}.</returns>
        public IEnumerable<BusinessUnit> FetchAllBusinessUnits()
        {
            return _lookupRepository.FetchAllBusinessUnits();
        }

        /// <summary>
        /// Fetches the business unit by entity.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<BusinessUnit> FetchBusinessUnitByEntity(Guid parentId)
        {
            return _lookupRepository.FetchBusinessUnitByEntity(parentId);
        }

        /// <summary>
        /// Updates the bulk.
        /// </summary>
        /// <param name="businessUnits">The business units.</param>
        /// <param name="parentId">The parent identifier.</param>
        public void UpdateBulk(IEnumerable<BusinessUnit> businessUnits, Guid parentId)
        {
            var associations = businessUnits.Select(x =>
            {
                return new BusinessUnitAssociation()
                {
                    BusinessUnitId = x.Id.Value,
                    ParentId = parentId                    
                };
            });

            _businessUnitAssociationProvider.UpdateBulk(associations, parentId);
        }
    }
}
