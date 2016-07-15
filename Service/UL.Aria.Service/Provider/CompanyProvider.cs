using System;
using System.Collections.Generic;
using System.Linq;
using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Company provider class.
    /// </summary>
    public class CompanyProvider : ICompanyProvider
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IBusinessMessageProvider _businessMessageProvider;
        private readonly IPrincipalResolver _principalResolver;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyProvider" /> class.
        /// </summary>
        /// <param name="companyRepository">The company repository.</param>
        /// <param name="businessMessageProvider">The business message provider.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public CompanyProvider(ICompanyRepository companyRepository, IBusinessMessageProvider businessMessageProvider, IPrincipalResolver principalResolver, ITransactionFactory transactionFactory)
        {
            _companyRepository = companyRepository;
            _businessMessageProvider = businessMessageProvider;
            _principalResolver = principalResolver;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Gets the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Company FetchById(Guid id)
        {
            return _companyRepository.FetchById(id);
        }

        /// <summary>
        ///     Gets the company by external id.
        /// </summary>
        /// <param name="externalId">The external id.</param>
        /// <returns>Company.</returns>
        public Company FetchByExternalId(string externalId)
        {
            return _companyRepository.FetchByExternalId(externalId);
        }

        /// <summary>
        ///     Publishes the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns>
        ///     The published company
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Company Create(Company company)
        {
            if (null == company)
            company.UpdatedById = company.CreatedById = _principalResolver.UserId;
            var createdCompany = _companyRepository.Create(company);
            PublishLogMessage(AuditMessageIdEnumDto.CreateCompany, "Company Created", createdCompany);
            
            return createdCompany;
        }

        /// <summary>
        ///     Updates the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Company Update(Company company)
        {
            company.UpdatedById = _principalResolver.UserId;
            var updatedCompany = _companyRepository.Update(company);
            PublishLogMessage(AuditMessageIdEnumDto.UpdateCompany, "Company Updated", updatedCompany);

            return updatedCompany;
        }

        /// <summary>
        ///     Deletes the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(Guid id)
        {
            var data = new Dictionary<string, string>();
            data.Add("Id", id.ToString());
            _businessMessageProvider.Publish(AuditMessageIdEnumDto.DeleteCompany, "Company Deleted", data);
            _companyRepository.Delete(id);
        }

        private void PublishLogMessage(AuditMessageIdEnumDto messageId, string message, Company company)
        {
            var data = new Dictionary<string, string>();
            data.Add("Id", company.Id.Value.ToString());
            data.Add("Name", company.Name);
            if (null != company.ExternalIds)
                data.Add("ExternalIds", string.Join(", ", company.ExternalIds));
            _businessMessageProvider.Publish(messageId, message, data);
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        public IList<Company> FetchAll()
        {
            return _companyRepository.FetchAll();
        }

        /// <summary>
        /// Fetches all count.
        /// </summary>
        /// <returns></returns>
        public int FetchAllCount()
        {
            return _companyRepository.FetchAllCount();
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        public CompanySearchResultSet Search(SearchCriteria searchCriteria)
        {
            return _companyRepository.Search(searchCriteria);
        }
    }
}