using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;

namespace UL.Aria.Service.Relay.Manager
{
    /// <summary>
    /// <see cref="IRelayCompanyManager"/> implementation for companies.
    /// </summary>
    public class RelayCompanyManager : IRelayCompanyManager
    {
        private readonly ICompanyService _companyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCompanyManager"/> class.
        /// </summary>
        /// <param name="companyService">The company service.</param>
        public RelayCompanyManager(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>
        /// Gets the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public CompanyDto FetchById(Guid id)
        {
            try
            {
                return _companyService.FetchById(id.ToString());
            }
            catch (EndpointNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches the by external identifier.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <returns></returns>
        public CompanyDto FetchByExternalId(string externalId)
        {
            try
            {
                return _companyService.FetchByExternalId(externalId);
            }
            catch (EndpointNotFoundException)
            {
                return null;
            }
        }
    }
}
