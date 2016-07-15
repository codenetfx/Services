using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines manager operations for <see cref="BusinessUnit"/> entities.
    /// </summary>
    public interface IBusinessUnitManager : IManagerBase<BusinessUnit>, ISearchManagerBase<BusinessUnit>
    {

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<BusinessUnit> FetchAll();
    }
}
