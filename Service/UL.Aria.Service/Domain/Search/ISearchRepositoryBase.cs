using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Search
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDomainEntity">The type of the domain entity.</typeparam>
    public interface ISearchRepositoryBase<TDomainEntity>:IRepositoryBase<TDomainEntity>
        where TDomainEntity : UL.Enterprise.Foundation.Domain.DomainEntity, new()
    {
        /// <summary>
        /// Defaults the search.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        SearchResultSetBase<T> DefaultSearch<T>(SearchCriteria searchCriteria) where T : class, ISearchResult, new();
    }
}
