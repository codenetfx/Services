using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    /// User team service implementation.
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall
        )]
    public class UserTeamService : IUserTeamService
    {
        private readonly IUserTeamManager _userTeamManager;
        private readonly IMapperRegistry _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserTeamService"/> class.
        /// </summary>
        /// <param name="userTeamManager">The user team manager.</param>
        /// <param name="mapper">The mapper.</param>
        public UserTeamService(IUserTeamManager userTeamManager, IMapperRegistry mapper)
        {
            _userTeamManager = userTeamManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public UserTeamDto FetchById(string id)
        {
            Guard.IsNotNullOrEmptyTrimmed(id, "id");
            return _mapper.Map<UserTeamDto>(_userTeamManager.FetchById(Guid.Parse(id)));
        }

        /// <summary>
        /// Fetches the by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public IEnumerable<UserTeamDto> FetchByUserId(string userId)
        {
            Guard.IsNotNullOrEmptyTrimmed(userId, "userId");
            return _mapper.Map<List<UserTeamDto>>(_userTeamManager.FetchByUserId(Guid.Parse(userId)));
        }

        /// <summary>
        /// Creates the specified user team.
        /// </summary>
        /// <param name="userTeam">The user team.</param>
        /// <returns></returns>
        public UserTeamDto Create(UserTeamDto userTeam)
        {
            Guard.IsNotNull(userTeam, "userTeam");
            var entity = _userTeamManager.Create(_mapper.Map<UserTeam>(userTeam));
            return _mapper.Map<UserTeamDto>(entity);
        }

        /// <summary>
        /// Updates the specified user team.
        /// </summary>
        /// <param name="userTeamId">The user team identifier.</param>
        /// <param name="userTeam">The user team.</param>
        public void Update(string userTeamId, UserTeamDto userTeam)
        {
            Guard.IsNotNullOrEmptyTrimmed(userTeamId, "userTeamId");
            Guard.IsNotNull(userTeam, "userTeam");
            _userTeamManager.Update(_mapper.Map<UserTeam>(userTeam));
        }

        /// <summary>
        /// Deletes the specified user team identifier.
        /// </summary>
        /// <param name="userTeamId">The user team identifier.</param>
        public void Delete(string userTeamId)
        {
            Guard.IsNotNullOrEmptyTrimmed(userTeamId, "userTeamId");
            _userTeamManager.Delete(Guid.Parse(userTeamId));
        }
    }
}
