using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using UL.Aria.Service.Logging;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements operations for customer management.
    /// </summary>
    public class CustomerManager : ICustomerManager
    {
        private readonly ICustomerPartyService _customerPartyService;
        private readonly ICompanyProvider _companyProvider;
	    private readonly ILogManager _logManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomerManager" /> class.
		/// </summary>
		/// <param name="customerPartyService">The customer party service.</param>
		/// <param name="companyProvider">The company manager.</param>
		/// <param name="logManager">The log manager.</param>
        public CustomerManager(ICustomerPartyService customerPartyService, ICompanyProvider companyProvider, ILogManager logManager)
        {
            _customerPartyService = customerPartyService;
            _companyProvider = companyProvider;
	        _logManager = logManager;
        }

        /// <summary>
        /// Gets the customer organization by external id.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public CustomerOrganizationDto GetCustomer(string externalId)
        {
			_logManager.Log(new LogMessage(MessageIds.CustomerManagerExteranlServiceStart, LogPriority.High, TraceEventType.Information, "CustomerManager GetCustomer Before making call to external service. ExternalID{0}", LogCategory.CustomerServiceManager));
            return _customerPartyService.Fetch(externalId);
        }

        /// <summary>
        /// Gets the customer organizations by company id.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<CustomerOrganizationDto> GetCustomer(Guid companyId)
        {
            var company = _companyProvider.FetchById(companyId);
			_logManager.Log(new LogMessage(MessageIds.CustomerManagerStart, LogPriority.Medium, TraceEventType.Information,
		        string.Format("CustomerManager GetCustomer start. Company Id {0}", companyId.ToString("N")), LogCategory.CustomerServiceManager));
			
            var results = new List<CustomerOrganizationDto>();
            foreach (var externalId in company.ExternalIds)
            {
                try
                {
                    var result = GetCustomer(externalId);
                    if (null != result)
                    {
                        results.Add(result);
                    }
                }
                catch (DatabaseItemNotFoundException)
                {
					_logManager.Log(new LogMessage(MessageIds.CustomerManagerDatabaseItemNotFoundException, LogPriority.Medium, TraceEventType.Information,
								   string.Format("CustomerManager GetCustomer Database Item Not Found Exception . ExternalId {0}", externalId), LogCategory.CustomerServiceManager));
                }
                catch (EndpointNotFoundException)
                {
					_logManager.Log(new LogMessage(MessageIds.CustomerManagerEndpointNotFoundException, LogPriority.Medium, TraceEventType.Information,
							   string.Format("CustomerManager GetCustomer Endpoint Not Found Exception. ExternalId {0}", externalId), LogCategory.CustomerServiceManager));
                }
            }

			_logManager.Log(new LogMessage(MessageIds.CustomerManagerEnd, LogPriority.Medium, TraceEventType.Information,
			 string.Format("CustomerManager GetCustomer End. Result Count {0}", results.Count), LogCategory.CustomerServiceManager));

            return results;
        }
    }
}