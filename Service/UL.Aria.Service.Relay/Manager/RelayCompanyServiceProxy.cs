using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Enterprise.Foundation.Client;

namespace UL.Aria.Service.Relay.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public class RelayCompanyServiceProxy : ServiceAgentManagedProxy<ICompanyService>, ICompanyService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCompanyServiceProxy"/> class.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        public RelayCompanyServiceProxy(IProxyConfigurationSource configurationSource) :
            this(
            new WebChannelFactory<ICompanyService>(new WebHttpBinding(), configurationSource.CompanyService))
        {
            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAgentManagedProxy{T}" /> class.
        /// </summary>
        /// <param name="serviceProxyFactory">The service proxy factory.</param>
        private RelayCompanyServiceProxy(WebChannelFactory<ICompanyService> serviceProxyFactory)
            : base(serviceProxyFactory)
        {
           
        }

        /// <summary>
        /// Gets the company by id.
        /// </summary>
        /// <returns>
        /// Company Dto
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<CompanyDto> FetchAll()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Search based on the provided criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public CompanySearchModelDto Search(SearchCriteriaDto searchCriteria)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// Company Dto
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public CompanyDto FetchById(string id)
        {
            CompanyDto byId = null;
            ICompanyService companyService = ClientProxy;
            using (new OperationContextScope((IContextChannel)companyService))
            {
                byId = companyService.FetchById(id);
                return byId;
            }
        }

        /// <summary>
        /// Gets the company by external id.
        /// </summary>
        /// <param name="externalId">The external id.</param>
        /// <returns>
        /// Company.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public CompanyDto FetchByExternalId(string externalId)
        {
            CompanyDto byId = null;
            ICompanyService companyService = ClientProxy;
            using (new OperationContextScope((IContextChannel)companyService))
            {
                byId = companyService.FetchByExternalId(externalId);
                return byId;
            }
        }

        /// <summary>
        /// Publishes the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns>
        /// The published company
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public CompanyDto Create(CompanyDto company)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Updates the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public CompanyDto Update(CompanyDto company)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Deletes the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(string id)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Fetches all count.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public int FetchAllCount()
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Gets the ul company identifier.
        /// </summary>
        /// <returns></returns>
        public Guid GetULCompanyId()
        {
            Guid id = Guid.Empty;
            ICompanyService companyService = ClientProxy;
            using (new OperationContextScope((IContextChannel)companyService))
            {
                id = companyService.GetULCompanyId();
                return id;
            }
        }
    }
}