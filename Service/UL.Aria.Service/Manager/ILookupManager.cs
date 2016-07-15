using System.Collections.Generic;
using UL.Aria.Service.Domain.Lookup;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     contract for coordinating lookup oriented functionality
    /// </summary>
    public interface ILookupManager
    {
        /// <summary>
        ///     Fetch business units
        /// </summary>
        /// <returns>IEnumerable{BusinessUnit}</returns>
        IList<BusinessUnit> FetchAllBusinessUnits();

        
    }
}