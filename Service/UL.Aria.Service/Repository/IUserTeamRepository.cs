using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Provides a Repository interface for User Teams.
    /// </summary>
    public interface IUserTeamRepository :  IPrimaryEntityRepository<UserTeam>
    {
        /// <summary>
        /// Finds the by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        IEnumerable<UserTeam> FindByUserId(Guid userId);
    }
}
