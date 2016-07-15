using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     fulfills operations for Lookup Services
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class LookupService : ILookupService
    {
        private readonly ILookupManager _lookupManager;
        private readonly IMapperRegistry _mapperRegistry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LookupService" /> class.
        /// </summary>
        /// <param name="lookupManager">The business unit manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public LookupService(ILookupManager lookupManager, IMapperRegistry mapperRegistry)
        {
            _lookupManager = lookupManager;
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        ///     Fetch all business units
        /// </summary>
        /// <returns>
        ///     IEnumerable{BusinessUnitDto}
        /// </returns>
        public IList<BusinessUnitDto> FetchAllBusinessUnits()
        {
            var businessunits = _lookupManager.FetchAllBusinessUnits();
            return businessunits.Select(businessUnit => _mapperRegistry.Map<BusinessUnitDto>(businessUnit)).ToList();
        }

       
    }
}