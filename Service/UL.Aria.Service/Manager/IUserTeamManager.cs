using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// User team manager interface definition.
    /// </summary>
    public interface IUserTeamManager
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
        /// Creates the specified user team.
        /// </summary>
        /// <param name="userTeam">The user team.</param>
        /// <returns></returns>
        UserTeam Create(UserTeam userTeam);

        /// <summary>
        /// Updates the specified user team.
        /// </summary>
        /// <param name="userTeam">The user team.</param>
        void Update(UserTeam userTeam);

        /// <summary>
        /// Deletes the specified user team identifier.
        /// </summary>
        /// <param name="userTeamId">The user team identifier.</param>
        void Delete(Guid userTeamId);
    }
}
