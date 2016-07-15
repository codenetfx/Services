using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provides an interface for a User Team Provider.
    /// </summary>
    public interface IUserTeamProvider
    {
        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        UserTeam FetchById(Guid id);

        /// <summary>
        /// Fetches the by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        IEnumerable<UserTeam> FetchByUserId(Guid userId);

        /// <summary>
        /// Deletes the specified user team identifier.
        /// </summary>
        /// <param name="userTeamId">The user team identifier.</param>
        void Delete(Guid userTeamId);

        /// <summary>
        /// Creates the specified userteam.
        /// </summary>
        /// <param name="userteam">The userteam.</param>
        /// <returns></returns>
        UserTeam Create(UserTeam userteam);

        /// <summary>
        /// Updates the specified user team.
        /// </summary>
        /// <param name="userTeam">The user team.</param>
        void Update(UserTeam userTeam);
    }
}
