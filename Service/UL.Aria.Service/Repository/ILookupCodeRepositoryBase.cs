using System.Collections.Generic;
using UL.Aria.Service.Domain.Repository;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Base repository interface for lookup codes
    /// </summary>
    /// <typeparam name="TDomainEntity">The type of the domain entity.</typeparam>
    public interface ILookupCodeRepositoryBase<TDomainEntity> : IPrimaryEntityRepository<TDomainEntity>
        where TDomainEntity : AuditableEntity, ISearchResult, new()
    {

        /// <summary>
        /// Finds the by external identifier.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <returns>IndustryCode.</returns>
        TDomainEntity FindByExternalId(string externalId);
        
        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TDomainEntity> FetchAll();
    }
}