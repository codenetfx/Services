using System;
using System.Collections.Generic;
using UL.Aria.Service.Caching;
using UL.Aria.Service.Caching.Behaviors;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Repository methods for a company.
    /// </summary>
    public interface ICompanyRepository
    {
        /// <summary>
        /// Gets the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [CacheResource(typeof(CacheReturnValueBehavior))]
        Company FetchById(Guid id);

        /// <summary>
        /// Gets the company by external id.
        /// </summary>
        /// <param name="externalId">The external id.</param>
        /// <returns>Company.</returns>
        [CacheResource(typeof(CacheIndexIdByUniqueKey))]
        Company FetchByExternalId(string externalId);

        /// <summary>
        /// Publishes the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns>The published company</returns>
        [CacheResource(typeof(CacheIncomingTargetByIdBehavior))]
        Company Create(Company company);

        /// <summary>
        /// Updates the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        [CacheResource(typeof(CacheIncomingTargetByIdBehavior))]
        Company Update(Company company);

        /// <summary>
        /// Deletes the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        [CacheResource(typeof(DeleteByKeysCachingBehavior))]
        void Delete(Guid id);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IList<Company> FetchAll();

        /// <summary>
        /// Fetches all count.
        /// </summary>
        /// <returns></returns>
        int FetchAllCount();

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        CompanySearchResultSet Search(SearchCriteria searchCriteria);
    }
}
