using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     defines contract for manipulating lookup items
    /// </summary>
    [ServiceContract]
    public interface ILookupService
    {
        /// <summary>
        ///     Fetch all business units
        /// </summary>
        /// <returns>
        ///     IEnumerable{BusinessUnitDto}
        /// </returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/List", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<BusinessUnitDto> FetchAllBusinessUnits();
       
    }
}