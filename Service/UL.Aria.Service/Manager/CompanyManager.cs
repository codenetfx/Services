using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Manager for Companies.
    /// </summary>
    public class CompanyManager : ICompanyManager
    {
        private readonly ICompanyProvider _companyProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyManager"/> class.
        /// </summary>
        /// <param name="companyProvider">The company provider.</param>
        public CompanyManager(ICompanyProvider companyProvider)
        {
            _companyProvider = companyProvider;
        }

        /// <summary>
        /// Gets the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Company FetchById(Guid id)
        {
            return _companyProvider.FetchById(id);
        }

        /// <summary>
        /// Gets the company by external id.
        /// </summary>
        /// <param name="externalId">The external id.</param>
        /// <returns>Company.</returns>
        public Company FetchByExternalId(string externalId)
        {
            return _companyProvider.FetchByExternalId(externalId);
        }

        /// <summary>
        /// Publishes the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns>
        /// The published company
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Company Create(Company company)
        {
            return _companyProvider.Create(company);
        }

        /// <summary>
        /// Updates the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Company Update(Company company)
        {
            return _companyProvider.Update(company);
        }

        /// <summary>
        /// Deletes the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(Guid id)
        {
            _companyProvider.Delete(id);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IList<Company> FetchAll()
        {
            return _companyProvider.FetchAll();
        }

        /// <summary>
        /// Fetches all count.
        /// </summary>
        /// <returns></returns>
        public int FetchAllCount()
        {
            return _companyProvider.FetchAllCount();
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        public CompanySearchResultSet Search(SearchCriteria searchCriteria)
        {
            return _companyProvider.Search(searchCriteria);
        }
    }
}