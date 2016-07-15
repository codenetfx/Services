using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines operations for customer management.
    /// </summary>
    public interface ICustomerManager
    {
        /// <summary>
        /// Gets the customer organization by external id.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <returns></returns>
        CustomerOrganizationDto GetCustomer(string externalId);

        /// <summary>
        /// Gets the customer organizations by company id.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        IList<CustomerOrganizationDto> GetCustomer(Guid companyId);
    }
}
