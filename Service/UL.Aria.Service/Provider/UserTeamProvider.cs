using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// User Team Provider implementation
    /// </summary>
    public class UserTeamProvider : IUserTeamProvider
    {
        private readonly IUserTeamRepository _userTeamRepository;
        private readonly IUserTeamMemberRepository _userTeamMemberRepository;
        private readonly IPrincipalResolver _principalResolver;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserTeamProvider"/> class.
        /// </summary>
        /// <param name="userTeamRepository">The user team repository.</param>
        /// <param name="userTeamMemberRepository">The user team member repository.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public UserTeamProvider(IUserTeamRepository userTeamRepository, IUserTeamMemberRepository userTeamMemberRepository, 
            IPrincipalResolver principalResolver, ITransactionFactory transactionFactory)
        {
            _userTeamRepository = userTeamRepository;
            _userTeamMemberRepository = userTeamMemberRepository;
            _principalResolver = principalResolver;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public UserTeam FetchById(Guid id)
        {
            var userTeam = _userTeamRepository.Fetch(id);
            userTeam.TeamMembers = _userTeamMemberRepository.FindByUserTeamId(userTeam.Id.Value);

            return userTeam;
        }

        /// <summary>
        /// Fetches the by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public IEnumerable<UserTeam> FetchByUserId(Guid userId)
        {
            var userTeams = _userTeamRepository.FindByUserId(userId).ToList();
            userTeams.ForEach(ut => ut.TeamMembers = _userTeamMemberRepository.FindByUserTeamId(ut.Id.Value));

            return userTeams;
        }

        /// <summary>
        /// Deletes the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public void Delete(Guid userId)
        {
            _userTeamRepository.Delete(userId);
        }

        /// <summary>
        /// Creates the specified user team.
        /// </summary>
        /// <param name="userTeam">The user team.</param>
        /// <returns></returns>
        public UserTeam Create(UserTeam userTeam)
        {
            using (var transaction = _transactionFactory.Create())
            {
                SetupUserTeam(_principalResolver, userTeam);
                _userTeamRepository.Save(userTeam);
                _userTeamMemberRepository.Save(userTeam.TeamMembers, userTeam.Id.Value);
                transaction.Complete();
                return userTeam;
            }
        }

        /// <summary>
        /// Updates the specified user team.
        /// </summary>
        /// <param name="userTeam">The user team.</param>
        public void Update(UserTeam userTeam)
        {
            using (var transaction = _transactionFactory.Create())
            {
                SetupUserTeam(_principalResolver, userTeam);
                _userTeamRepository.Save(userTeam);
                _userTeamMemberRepository.Save(userTeam.TeamMembers, userTeam.Id.Value);      
                transaction.Complete();
            }
        }

		internal static void SetupUserTeam(IPrincipalResolver principalResolver, UserTeam userTeam)
		{
			if (!userTeam.Id.HasValue)
			{
				userTeam.Id = Guid.NewGuid();
			}
			var currentDateTime = DateTime.UtcNow;
			userTeam.CreatedById = principalResolver.UserId;
            userTeam.CreatedDateTime = currentDateTime;
            userTeam.UpdatedById = principalResolver.UserId;
            userTeam.UpdatedDateTime = currentDateTime;

            userTeam.TeamMembers = userTeam.TeamMembers.Select(utm => new UserTeamMember()
            {
                Id = utm.Id.HasValue ? utm.Id : Guid.NewGuid(),
                UserId = utm.UserId,
                UserTeamId = userTeam.Id.Value,
                IncludedUserTeamId = utm.IncludedUserTeamId,
                CreatedById = principalResolver.UserId,
                CreatedDateTime = currentDateTime,
                UpdatedById = principalResolver.UserId,
                UpdatedDateTime = currentDateTime,
            }).ToList();
		}
    }
}
