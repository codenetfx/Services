using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Data;


namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// User team manager implementation.
    /// </summary>
    public class UserTeamManager : IUserTeamManager
    {
        private readonly IUserTeamProvider _userTeamProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserTeamManager"/> class.
        /// </summary>
        /// <param name="userTeamProvider">The user team provider.</param>
        public UserTeamManager(IUserTeamProvider userTeamProvider)
        {
            _userTeamProvider = userTeamProvider;
        }

        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public UserTeam FetchById(Guid id)
        {
            return _userTeamProvider.FetchById(id);
        }

        /// <summary>
        /// Fetches the by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public IEnumerable<UserTeam> FetchByUserId(Guid userId)
        {
            return _userTeamProvider.FetchByUserId(userId);
        }

        /// <summary>
        /// Creates the specified user team.
        /// </summary>
        /// <param name="userTeam">The user team.</param>
        /// <returns></returns>
        public UserTeam Create(UserTeam userTeam)
        {
            return _userTeamProvider.Create(userTeam);
        }

        /// <summary>
        /// Updates the specified user team.
        /// </summary>
        /// <param name="userTeam">The user team.</param>
        public void Update(UserTeam userTeam)
        {
            _userTeamProvider.Update(userTeam);
        }

        /// <summary>
        /// Deletes the specified user team identifier.
        /// </summary>
        /// <param name="userTeamId">The user team identifier.</param>
        public void Delete(Guid userTeamId)
        {
            _userTeamProvider.Delete(userTeamId);
        }
    }
}
