using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace UL.Aria.Service.Claim.Contract
{
    /// <summary>
    /// Claim service interface.
    /// </summary>
    [ServiceContract]    
    public interface IClaimDefinitionService
    {
        /// <summary>
        /// Defines the claim.
        /// </summary>
        /// <param name="claimDefinitionDto">The claim definition.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void DefineClaim(ClaimDefinitionDto claimDefinitionDto);

        /// <summary>
        /// Removes the claim.
        /// </summary>
        /// <param name="claimDefinitionId">The claim id.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{claimDefinitionId}", Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void RemoveClaim(string claimDefinitionId);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<ClaimDefinitionDto> GetAll();
    }
}