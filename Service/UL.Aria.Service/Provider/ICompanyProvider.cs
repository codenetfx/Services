using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provider for Companies
    /// </summary>
    public interface ICompanyProvider
    {
        /// <summary>
        /// Gets the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        Company FetchById(Guid id);

        /// <summary>
        /// Gets the company by external id.
        /// </summary>
        /// <param name="externalId">The external id.</param>
        /// <returns>Company.</returns>
        Company FetchByExternalId(string externalId);

        /// <summary>
        /// Publishes the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns>The published company</returns>
        Company Create(Company company);

        /// <summary>
        /// Updates the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        Company Update(Company company);

        /// <summary>
        /// Deletes the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
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
