using System;
using System.Collections.Generic;
using UL.Aria.Service.Caching;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Caching.Behaviors;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// contract for getting user information
    /// </summary>
    public interface IProfileRepository
    {
        /// <summary>
        /// Finds the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [CacheResource(typeof(CacheReturnValueBehavior),
            Keys = new string[] { "id" })]
        ProfileBo FetchById(Guid id);

        /// <summary>
        /// Gets the name of the profile by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        [CacheResource(typeof(CacheIndexIdByUniqueKey), 
            Keys = new string[] { "userName" })]
        ProfileBo FetchByUserName(string userName);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>    
         [CacheResource(typeof(CacheIncomingTargetByIdBehavior),
            CacheTarget = "entity")]
        Guid Create(ProfileBo entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        [CacheResource(typeof(CacheIncomingTargetByIdBehavior),
            CacheTarget = "entity")]
        int Update(ProfileBo entity);

        /// <summary>
        /// Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <param name="modifyingUser">The modifying user.</param>
        [CacheResource(typeof(DeleteByKeysCachingBehavior),
            Keys = new string[] { "entityId" })]
        void Remove(Guid entityId, Guid modifyingUser);

        /// <summary>
        /// Searches the specified fuzzy search.
        /// </summary>
        /// <param name="c">The criteria to use in the search.</param>
        /// <returns></returns>      
        IList<ProfileBo> Search(ProfileSearchSpecification c);

        /// <summary>
        /// Gets the profiles by company id.
        /// </summary>
        /// <param name="companyId">The company id.</param>
        /// <returns></returns>
        [CacheResource(typeof(CachePassthroughBehavior))]
        IList<ProfileBo> FetchAllByCompanyId(Guid companyId);

        /// <summary>
        /// Fetches the list of user whom belong to the specified team.
        /// </summary>
        /// <param name="userTeamId">The user team identifier.</param>
        /// <param name="includeTeamMemberTeams">if set to <c>true</c> Includes the Team members of teams owned by team members recurcively.</param>
        /// <param name="maxDepth">The maximum depth of recursion.</param>
        /// <returns></returns>
        [CacheResource(typeof(CachePassthroughBehavior))]
        IList<ProfileBo> FetchByTeam(Guid userTeamId, bool includeTeamMemberTeams = false, int maxDepth = 2);
    }

}