using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity.Utility;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Manager;
using Guard = UL.Enterprise.Foundation.Framework.Guard;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     Provides operations for customer information.
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerManager _customerManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerService" /> class.
        /// </summary>
        /// <param name="customerManager">The customer manager.</param>
        public CustomerService(ICustomerManager customerManager)
        {
            _customerManager = customerManager;
        }

        /// <summary>
        /// Gets the customer.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<CustomerOrganizationDto> Fetch(string id)
        {
            Guard.IsNotNullOrEmptyTrimmed(id, "id");
            Guid companyId;
            if (Guid.TryParse(id, out companyId))
            {
                var bo = _customerManager.GetCustomer(companyId);
                if (null == bo)
                    return null;
                return bo;
            }
            return new []{ _customerManager.GetCustomer(id)};
        }
    }
}