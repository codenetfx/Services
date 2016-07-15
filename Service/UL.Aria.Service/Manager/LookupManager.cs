using System.Collections.Generic;
using System.Linq;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     implements concrete functionality for LookupManager
    /// </summary>
    public class LookupManager : ILookupManager
    {
        private readonly ILookupProvider _lookupProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupManager" /> class.
        /// </summary>
        /// <param name="lookupProvider">The lookup provider.</param>
        public LookupManager( ILookupProvider lookupProvider)
        {
            _lookupProvider = lookupProvider;
        }

        /// <summary>
        ///    Fetch all business units
        /// </summary>
        /// <returns>IEnumerable{BusinessUnit}</returns>
        public IList<BusinessUnit> FetchAllBusinessUnits()
        {
            var businessUnits = _lookupProvider.FetchAllBusinessUnits();
            return null ==businessUnits? new List<BusinessUnit>() : businessUnits.ToList();
        }
    }
}