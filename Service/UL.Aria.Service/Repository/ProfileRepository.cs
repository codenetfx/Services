using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Profile Repo
    /// </summary>
    public class ProfileRepository : RepositoryBase<ProfileBo>, IProfileRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProfileRepository" /> class.
        /// </summary>
        public ProfileRepository() : base("UserId")
        {
        }

        /// <summary>
        ///     Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        public override ProfileBo FindById(Guid entityId)
        {
            Guard.IsNotEmptyGuid(entityId, "entityId");

            return GetByCommand(InitializeGetByIdCommand, entityId).FirstOrDefault();
        }

        /// <summary>
        ///     Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        public ProfileBo FetchById(Guid entityId)
        {
            Guard.IsNotEmptyGuid(entityId, "entityId");

            return GetByCommand(InitializeGetByIdCommand, entityId).FirstOrDefault();
        }

        /// <summary>
        ///     Gets the name of the profile by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public ProfileBo FetchByUserName(string userName)
        {
            Guard.IsNotNullOrEmpty(userName, "userName");

            return GetByCommand(InitializeGetByLoginIdCommand, userName).FirstOrDefault();
        }

        /// <summary>
        ///     Gets the profiles by compay id.
        /// </summary>
        /// <param name="companyId">The company id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<ProfileBo> FetchAllByCompanyId(Guid companyId)
        {
            Guard.IsNotEmptyGuid(companyId, "entityId");

            return GetByCommand(InitializeGetProfilesByCompanyIdCommand, companyId);
        }


        /// <summary>
        /// Fetches the list of user whom belong to the specified team.
        /// </summary>
        /// <param name="userTeamId">The user team identifier.</param>
        /// <param name="includeTeamMemberTeams">if set to <c>true</c> Includes the Team members of teams owned by team members recurcively.</param>
        /// <param name="maxDepth">The maximum depth of recursion.</param>
        /// <returns></returns>
        public IList<ProfileBo> FetchByTeam(Guid userTeamId, bool includeTeamMemberTeams = false, int maxDepth = 2)
        {
          
           return ExecuteReaderCommand(db => {
                var cmd = db.GetStoredProcCommand("dbo.pUser_GetByTeamId");
                db.AddInParameter(cmd, "UserTeamId", DbType.Guid, userTeamId);
                db.AddInParameter(cmd, "IncludeUserTeamMembers", DbType.Boolean, includeTeamMemberTeams);
                db.AddInParameter(cmd, "MaxDepth", DbType.Int32, maxDepth);
                return cmd;
            },
            ConstructProfile);
        }


        /// <summary>
        ///     Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(ProfileBo entity)
        {
            Create(entity);
        }

        /// <summary>
        ///     Searches the specified fuzzy search.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<ProfileBo> Search(ProfileSearchSpecification criteria)
        {
            return GetByCommand(InitializeSearchCommand, criteria);
        }

        /// <summary>
        ///     Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public Guid Create(ProfileBo entity)
        {
            var id = Guid.Empty;
            ExecuteCommand(InitializeAddCommand, entity, cmd => { id = (Guid) cmd.Parameters["@UserId"].Value; });
            return id;
        }

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override int Update(ProfileBo entity)
        {
            return ExecuteCommand(InitializeUpdateCommand, entity);
        }

        /// <summary>
        ///     Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <param name="modifyingUser">The modifying user.</param>
        public void Remove(Guid entityId, Guid modifyingUser)
        {
            ExecuteCommand(InitializeRemoveCommand, entityId);
        }

        /// <summary>
        ///     Please call the Remove(guid,guid) method, instead.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        public override void Remove(Guid entityId)
        {
            throw new InvalidOperationException("Please call the Remove(guid,guid) method, instead");
        }

        /// <summary>
        ///     FindAll not supported for profiles.  Use Search() instead.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override IList<ProfileBo> FindAll()
        {
            throw new NotImplementedException("FindAll not supported for profiles.  Use Search() instead.");
        }


        private static List<ProfileBo> GetByCommand<T>(Func<T, Database, DbCommand> initializeCommand, T id)
        {
            var db = DatabaseFactory.CreateDatabase();
            var results = new List<ProfileBo>();

            using (var command = initializeCommand(id, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        results.Add(ConstructProfile(reader));
                    }
                }
            }
            return results;
        }

        private static int ExecuteCommand<TEntity>(Func<TEntity, Database, DbCommand> commandInitializer, TEntity entity,
                                                   Action<DbCommand> afterExecute = null)
        {
            int count;
            var db = DatabaseFactory.CreateDatabase();
            var command = commandInitializer(entity, db);

            using (command)
            {
                count = db.ExecuteNonQuery(command);
            }
            if (afterExecute != null)
                afterExecute(command);

            return count;
        }


        private static DbCommand InitializeSearchCommand(ProfileSearchSpecification criteria, Database db)
        {
            var cmd = db.GetStoredProcCommand("[dbo].[pUser_Search]");

            db.AddInParameter(cmd, "Criteria", DbType.String, criteria.Keyword);
            db.AddInParameter(cmd, "StartIndex", DbType.Int32, (int) criteria.StartIndex);
            db.AddInParameter(cmd, "EndIndex", DbType.Int32, (int) criteria.EndIndex);
            db.AddInParameter(cmd, "CompanyId", DbType.Guid, criteria.CompanyId);

            return cmd;
        }

        private static DbCommand InitializeGetByIdCommand(Guid entityId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pUser_GetById]");
            db.AddInParameter(command, "UserId", DbType.Guid, entityId);
            return command;
        }

        private static DbCommand InitializeGetByLoginIdCommand(string userName, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pUser_GetByLoginId]");
            db.AddInParameter(command, "LoginId", DbType.String, userName);
            return command;
        }

        private static DbCommand InitializeGetProfilesByCompanyIdCommand(Guid companyId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pUser_GetProfilesByCompanyID]");
            db.AddInParameter(command, "CompanyId", DbType.Guid, companyId);
            return command;
        }

        private DbCommand InitializeAddCommand(ProfileBo entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pUser_Insert]");

            db.AddOutParameter(command, "UserId", DbType.Guid, 128);
            db.AddInParameter(command, "DisplayName", DbType.String, entity.DisplayName);
            db.AddInParameter(command, "LoginId", DbType.String, entity.LoginId);
            db.AddInParameter(command, "JobTitle", DbType.String, entity.Title);
            db.AddInParameter(command, "AboutMe", DbType.String, entity.AboutMe);
            db.AddInParameter(command, "CompanyId", DbType.Guid, entity.CompanyId);
            db.AddInParameter(command, "CreatedDT", DbType.DateTime2, entity.CreatedDateTime);
            db.AddInParameter(command, "CreatedBy", DbType.Guid, entity.CreatedById);
            db.AddInParameter(command, "UpdatedDT", DbType.DateTime2, entity.UpdatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, entity.UpdatedById);

            return command;
        }

        private DbCommand InitializeUpdateCommand(ProfileBo entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pUser_Update]");

            db.AddInParameter(command, "UserId", DbType.Guid, entity.Id);
            db.AddInParameter(command, "DisplayName", DbType.String, entity.DisplayName);
            db.AddInParameter(command, "LoginId", DbType.String, entity.LoginId);
            db.AddInParameter(command, "JobTitle", DbType.String, entity.Title);
            db.AddInParameter(command, "AboutMe", DbType.String, entity.AboutMe);
            db.AddInParameter(command, "CompanyId", DbType.Guid, entity.CompanyId);
            db.AddInParameter(command, "UpdatedDT", DbType.DateTime2, entity.UpdatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, entity.UpdatedById);

            return command;
        }

        private DbCommand InitializeRemoveCommand(Guid id, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pUser_Delete]");

            db.AddInParameter(command, "UserId", DbType.Guid, id);

            return command;
        }

        private static ProfileBo ConstructProfile(IDataReader reader)
        {
            return new ProfileBo
                {
                    Id = reader.GetValue<Guid>("UserId"),
                    DisplayName = reader.GetValue<string>("DisplayName"),
                    LoginId = reader.GetValue<string>("LoginId"),
                    Title = reader.GetValue<string>("JobTitle"),
                    CompanyId = reader.GetValue<Guid>("CompanyId"),
                    CreatedById = reader.GetValue<Guid>("CreatedBy"),
                    CreatedDateTime = reader.GetValue<DateTime>("CreatedDT"),
                    UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                    UpdatedDateTime = reader.GetValue<DateTime>("UpdatedDT"),
                    AboutMe = reader.GetValue<string>("AboutMe")
                };
        }
    }
}