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
    /// Interface defining contract for user business claim entity. Business claims are friendly versions of concrete claims.
    /// </summary>
    [ServiceContract]
    public interface IUserBusinessClaimService
    {
        /// <summary>
        /// Adds a user role..
        /// </summary>
        /// <param name="userClaimDto">The user role dto.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Add(UserBusinessClaimDto userClaimDto);

        /// <summary>
        /// Removes the specified user claim.
        /// </summary>
        /// <param name="userClaimId"></param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{userClaimId}", Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Remove(string userClaimId);

        /// <summary>
        /// Gets the user claim values.
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{loginId}/{claim}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<UserBusinessClaimDto> GetUserClaimValues(string claim, string loginId);

        /// <summary>
        /// Gets the user claim history.
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="loginId">To string.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{loginId}/{claim}/History", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<UserBusinessClaimHistoryDto> GetUserClaimHistory(string claim, string loginId);


        /// <summary>
        /// Finds the claims.
        /// </summary>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "Find/{claimValue}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<UserBusinessClaimDto> GetUserClaimsByValue(string claimValue);

        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "Find/{claim}/{claimValue}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<UserBusinessClaimDto> GetUserClaimsByClaimAndValue(string claim, string claimValue);
    }
}
