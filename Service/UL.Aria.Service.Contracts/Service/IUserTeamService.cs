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
    /// Provides an interface for managing user team dto objects.
    /// </summary>
    [ServiceContract]
    public interface IUserTeamService
    {
        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        UserTeamDto FetchById(string id);

        /// <summary>
        /// Fetches the by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "FetchByUserId/{userId}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<UserTeamDto> FetchByUserId(string userId);

        /// <summary>
        /// Creates the specified user team.
        /// </summary>
        /// <param name="userTeam">The user team.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        UserTeamDto Create(UserTeamDto userTeam);

        /// <summary>
        /// Updates the specified user team.
        /// </summary>
        /// <param name="userTeam">The user team.</param>
        /// <param name="userTeamId">The user team identifier.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{userTeamId}", Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Update(string userTeamId, UserTeamDto userTeam);

        /// <summary>
        /// Deletes the specified user team identifier.
        /// </summary>
        /// <param name="userTeamId">The user team identifier.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{userTeamId}", Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Delete(string userTeamId);
    }
}
