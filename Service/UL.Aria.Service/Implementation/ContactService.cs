using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.Utility;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    /// Implements operations for working with <see cref="IncomingOrderContactDto"/> objects.
    /// </summary>
    [AutoRegisterRestService]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
    InstanceContextMode = InstanceContextMode.PerCall)]
    public class ContactService : IContactService
    {
        private readonly IContactManager _contactManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactService"/> class.
        /// </summary>
        /// <param name="contactManager">The contact manager.</param>
        public ContactService(IContactManager contactManager)
        {
            _contactManager = contactManager;
        }

        /// <summary>
        /// Forces updates of contacts based on Order Number. This operation will only trigger the operation.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void UpdateByOrderNumber(string orderNumber)
        {
            Guard.ArgumentNotNullOrEmpty(orderNumber, "orderNumber");
            _contactManager.UpdateContactsByOrderNumberAsync(orderNumber);
        }
    }
}
