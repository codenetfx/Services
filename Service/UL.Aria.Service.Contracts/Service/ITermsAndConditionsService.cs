using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;


namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// defines the contract for which one must accept in order to gain access to features
    /// </summary>
    [ServiceContract]
    public interface ITermsAndConditionsService
    {
        /// <summary>
        /// Fetches the type of all by.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/List/{type}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<TermsAndConditionsDto> FetchAllByType(string type);

	    /// <summary>
        /// Fetches the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        TermsAndConditionsDto FetchById(string id);
    }
}
