using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Service contract for customer information.
    /// </summary>
    [ServiceContract]
    public interface ICustomerService
    {
        /// <summary>
        /// Gets the customer by either external id or customer id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{id}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IList<CustomerOrganizationDto> Fetch(string id);
    }
}
