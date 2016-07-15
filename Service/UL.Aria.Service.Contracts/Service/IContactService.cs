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
    /// Defines operations for working with <see cref="IncomingOrderContactDto"/> objects.
    /// </summary>
    [ServiceContract]
    public interface IContactService
    {

        /// <summary>
        /// Forces updates of contacts based on Order Number. This operation will only trigger the operation.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Update?operation=queued&orderNumber={orderNumber}", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void UpdateByOrderNumber(string orderNumber);
    }
}
