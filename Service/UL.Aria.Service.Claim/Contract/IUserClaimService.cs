using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Claim.Data;

namespace UL.Aria.Service.Claim.Contract
{
    /// <summary>
    /// User claim service interface.
    /// </summary>
    [ServiceContract]
    public interface IUserClaimService
    {
        /// <summary>
        /// Adds a user role..
        /// </summary>
        /// <param name="userClaimDto">The user role dto.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Add(UserClaimDto userClaimDto);

        /// <summary>
        /// Removes the specified user claim.
        /// </summary>
        /// <param name="userClaimId">The user claim id.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{userClaimId}", Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Remove(string userClaimId);

        /// <summary>
        /// Gets the user claim values.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{loginId}/{claimId}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<UserClaimDto> GetUserClaimValues(string claimId, string loginId);

        /// <summary>
        /// Finds the claims.
        /// </summary>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "Find/{claimValue}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<UserClaimDto> GetUserClaimsByValue(string claimValue);

        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "Find/{claimId}/{claimValue}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<UserClaimDto> GetUserClaimsByIdAndValue(string claimId, string claimValue);

        /// <summary>
        /// Gets the user claim history.
        /// </summary>
        /// <param name="claimId">The encode to64.</param>
        /// <param name="loginId">The login id.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{loginId}/{claimId}/History", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<UserClaimHistoryDto> GetUserClaimHistory(string claimId, string loginId);

        /// <summary>
        /// Gets all of the claim values for a user.
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "User/{loginId}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<UserClaimDto> GetUserClaims(string loginId);


        /// <summary>
        /// Removes claims for a user.
        /// </summary>
        /// <param name="loginId">The login unique identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "User/{loginId}", Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void RemoveUserClaims(string loginId);
    }
}