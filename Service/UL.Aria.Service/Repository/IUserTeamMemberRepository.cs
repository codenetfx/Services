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
    /// Provides a Repository interface for User Team Members.
    /// </summary>
    public interface IUserTeamMemberRepository : IPrimaryAssocatedRepository<UserTeamMember>
    {
        /// <summary>
        /// Finds the by user team identifier.
        /// </summary>
        /// <param name="userTeamId">The user team identifier.</param>
        /// <returns></returns>
        IEnumerable<UserTeamMember> FindByUserTeamId(Guid userTeamId);
       
    }
}
