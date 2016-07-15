using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Transactions;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     Service for Company
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyManager _companyManager;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;
        private Configuration.IServiceConfiguration _serviceConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyService" /> class.
        /// </summary>
        /// <param name="companyManager">The company manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="serviceConfiguration">The service configuration.</param>
        public CompanyService(ICompanyManager companyManager, IMapperRegistry mapperRegistry,
                              ITransactionFactory transactionFactory, Configuration.IServiceConfiguration serviceConfiguration)
        {
            _companyManager = companyManager;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
            _serviceConfiguration = serviceConfiguration;
        }


        /// <summary>
        /// Gets the ul company identifier.
        /// </summary>
        /// <returns></returns>
        public Guid GetULCompanyId()
        {
            return _serviceConfiguration.UlCompanyId;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IList<CompanyDto> FetchAll()
        {                     
            return  _companyManager.FetchAll().Select(_mapperRegistry.Map<CompanyDto>).ToList();
        }

        /// <summary>
        /// Fetches all count.
        /// </summary>
        /// <returns></returns>
        public int FetchAllCount()
        {
            return _companyManager.FetchAllCount();
        }

        /// <summary>
        /// Search based on the provided criteria
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public CompanySearchModelDto Search(SearchCriteriaDto searchCriteria)
        {
            Guard.IsNotNull(searchCriteria, "searchCriteria");

            var criteria = _mapperRegistry.Map<SearchCriteria>(searchCriteria);
            return _mapperRegistry.Map<CompanySearchModelDto>(_companyManager.Search(criteria));
        }

        /// <summary>
        ///     Gets the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///     Company Dto
        /// </returns>
        public CompanyDto FetchById(string id)
        {
            Company company = _companyManager.FetchById(id.ToGuid());
            return _mapperRegistry.Map<CompanyDto>(company);
        }

        /// <summary>
        /// Gets the company by external id.
        /// </summary>
        /// <param name="externalId">The external id.</param>
        /// <returns>Company Dto.</returns>
        public CompanyDto FetchByExternalId(string externalId)
        {
            var company = _companyManager.FetchByExternalId(externalId);

            return company == null ? null : _mapperRegistry.Map<CompanyDto>(company);
        }

        /// <summary>
        ///     Publishes the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns>
        ///     The published company
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public CompanyDto Create(CompanyDto company)
        {
            var companyBo = _mapperRegistry.Map<Company>(company);
            Company retrievedCompany;

            using (TransactionScope transactionScope = _transactionFactory.Create())
            {
                retrievedCompany = _companyManager.Create(companyBo);
                transactionScope.Complete();
            }
            return _mapperRegistry.Map<CompanyDto>(retrievedCompany);
        }

        /// <summary>
        ///     Updates the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public CompanyDto Update(CompanyDto company)
        {
            var companyBo = _mapperRegistry.Map<Company>(company);
            Company retrievedCompany;
            using (TransactionScope transactionScope = _transactionFactory.Create())
            {
                
                retrievedCompany = _companyManager.Update(companyBo);

                transactionScope.Complete();
            }
            return _mapperRegistry.Map<CompanyDto>(retrievedCompany);
        }

        /// <summary>
        ///     Deletes the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(string id)
        {
            using (TransactionScope transactionScope = _transactionFactory.Create())
            {
                _companyManager.Delete(id.ToGuid());
                transactionScope.Complete();
            }
        }
    }
}