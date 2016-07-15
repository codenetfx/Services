using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Claim.Domain;

namespace UL.Aria.Service.Claim.Data
{
    /// <summary>
    /// User security repository class.
    /// </summary>
    public class UserClaimRepository : RepositoryBase<UserClaim>, IUserClaimRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaimRepository"/> class.
        /// </summary>
        public UserClaimRepository() : base("UserClaimId")
        {}

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        public override IList<UserClaim> FindAll()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        public override  UserClaim FindById(Guid entityId)
        {
            Guard.IsNotEmptyGuid(entityId, "entityId");
            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeFindById(entityId, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    var userClaims = CreateUserClaims(reader);
                    if (0 != userClaims.Count)
                    {
                        return userClaims[0];
                    }

                    throw new DatabaseItemNotFoundException(string.Format(CultureInfo.InvariantCulture,
                                                                          "Unable to find user claim for Id ({0})",
                                                                          entityId));
                }
            }
        }

        private static IList<UserClaim> CreateUserClaims(IDataReader reader)
        {
 
            
            var userClaims = new List<UserClaim>();

            while(reader.Read())
            {
                var userClaimId = reader.GetGuid(reader.GetOrdinal("UserClaimId"));
                var userId = reader.GetGuid(reader.GetOrdinal("UserId"));
                var claimId = new Uri(reader.GetString(reader.GetOrdinal("ClaimId")));
                var claimValue = reader.IsDBNull(reader.GetOrdinal("ClaimValue"))
                                     ? null
                                     : reader.GetString(reader.GetOrdinal("ClaimValue"));
                var loginId = reader.GetString(reader.GetOrdinal("LoginId"));
                userClaims.Add(new UserClaim
                    {
                        Id = userClaimId,
                        ClaimId = claimId,
                        ClaimValue = claimValue,
                        UserId = userId,
                        LoginId = loginId
                    });
            }

            return userClaims;
        }

        //private DbCommand InitializeFindByValue(string claimValue, Database db)
        //{
        //    var command = db.DbProviderFactory.CreateCommand();

        //    command.CommandType = CommandType.StoredProcedure;
        //    command.CommandText = "[dbo].[pUserClaim_GetByValue]";

        //    var parameter = command.CreateParameter();
        //    parameter = command.CreateParameter();
        //    parameter.DbType = DbType.String;
        //    parameter.ParameterName = "ClaimValue";
        //    parameter.Value = !String.IsNullOrWhiteSpace(claimValue) ? (object)claimValue : DBNull.Value;
        //    parameter.Direction = ParameterDirection.Input;
        //    command.Parameters.Add(parameter);

        //    return command;
        //}

        private DbCommand InitializeFindByIdAndValue(string claimId, string claimValue, Database db)
        {
            var command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pUserClaim_GetByClaimAndValue]";

            var parameter = command.CreateParameter();

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "ClaimId";
            parameter.Value = null != claimId ? (object) claimId: DBNull.Value;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "ClaimValue";
            parameter.Value =  null != claimValue ? (object)claimValue : DBNull.Value;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            return command;
        }

        private DbCommand InitializeFindByLoginId(string loginId, Database db)
        {
            var command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pUserClaim_GetByLoginId]";

            var parameter = command.CreateParameter();

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "LoginId";
            parameter.Value = loginId;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            return command;
        }

        private DbCommand InitializeFindById(Guid entityId, Database db)
        {
            var command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pUserClaim_Get]";

            var parameter = command.CreateParameter();

            parameter = command.CreateParameter();
            parameter.DbType = DbType.Guid;
            parameter.ParameterName = "UserClaimId";
            parameter.Value = entityId;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            return command;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override void Add(UserClaim entity)
        {
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeAddCommand(entity, db))
            {
                db.ExecuteNonQuery(command);
            }
        }

        private DbCommand InitializeAddCommand(UserClaim entity, Database db)
        {

            var command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pUserClaim_Insert]";

            var parameter = command.CreateParameter();

            parameter = command.CreateParameter();
            parameter.DbType = DbType.Guid;
            parameter.ParameterName = "UserClaimId";
            parameter.Value = entity.Id;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.DbType = DbType.Guid;
            parameter.ParameterName = "UserId";
            parameter.Value = entity.UserId;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.DbType = DbType.AnsiString;
            parameter.ParameterName = "ClaimId";
            parameter.Direction = ParameterDirection.Input;
            parameter.Value = entity.ClaimId.ToString();
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "ClaimValue";
            parameter.Value = entity.ClaimValue;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "LoginId";
            parameter.Value = entity.LoginId;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            return command;
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override int Update(UserClaim entity)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        public override void Remove(Guid entityId)
        {
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeRemoveCommand(entityId, db))
            {
                db.ExecuteNonQuery(command);
            }
        }


        private DbCommand InitializeRemoveUserClaimsCommand(string loginId, Database db)
        {

            var command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pUserClaim_DeleteByLoginId]";

            var parameter = command.CreateParameter();

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "LoginId";
            parameter.Value = loginId;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            return command;
        }

        private DbCommand InitializeRemoveCommand(Guid entityId, Database db)
        {

            var command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pUserClaim_Delete]";

            var parameter = command.CreateParameter();

            parameter = command.CreateParameter();
            parameter.DbType = DbType.Guid;
            parameter.ParameterName = "UserClaimId";
            parameter.Value = entityId;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            return command;
        }

        /// <summary>
        /// Gets the user claim values.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public IList<UserClaim> FindUserClaimValues(Uri claimId, string loginId)
        {
            Guard.IsNotNull(claimId, "claimId");
            Guard.IsNotNullOrEmpty(loginId, "loginId");
            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeFindUserClaimValues(claimId, loginId, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    return CreateUserClaims(reader);
                }
            }

        }

        /// <summary>
        /// Gets the user claim values.
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public IList<UserClaim> FindUserClaimValues(string loginId)
        {
            Guard.IsNotNullOrEmptyTrimmed(loginId, "loginId");
            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeFindByLoginId(loginId, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    return CreateUserClaims(reader);
                }
            }

        }

        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        public IList<UserClaim> FindUserClaimsByValue(string claimValue)
        {
            Guard.IsNotNull(claimValue, "claimValue");
            
            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeFindByIdAndValue(null, claimValue, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    return CreateUserClaims(reader);
                }
            }
        }

        /// <summary>
        /// Finds the user claim values.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        public IList<UserClaim> FindUserClaimsByIdAndValue(string claimId, string claimValue)
        {
            Guard.IsNotNull(claimValue, "claimValue");
            Guard.IsNotNull(claimValue, "claimId");

            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeFindByIdAndValue(claimId, claimValue, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    return CreateUserClaims(reader);
                }
            }
        }

        /// <summary>
        /// Finds the user claim history.
        /// </summary>
        /// <param name="claimId">The expected claim id.</param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public IList<UserClaimHistory> FindUserClaimHistory(Uri claimId, string loginId)
        {
            return new List<UserClaimHistory>
                {
                    new UserClaimHistory
                        {
                            ClaimId = claimId,
                            UserId = Guid.NewGuid(),
                            LoginId = loginId,
                            Description = "Changed to new value test",
                            ChangedBy = "John Q. Public",
                            ChangeDate = DateTime.UtcNow.AddDays(-2)
                        },

                    new UserClaimHistory
                        {
                            ClaimId = claimId,
                            UserId = Guid.NewGuid(),
                            LoginId = loginId,
                            Description = "Changed to new value blah",
                            ChangedBy = "Bart Simpson",
                            ChangeDate = DateTime.UtcNow.AddDays(-1)
                        }
                };
        }

        /// <summary>
        /// Removes the user claims.
        /// </summary>
        /// <param name="loginId"></param>
        public void RemoveUserClaims(string loginId)
        {
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeRemoveUserClaimsCommand(loginId, db))
            {
                db.ExecuteNonQuery(command);
            }
        }

        private DbCommand InitializeFindUserClaimValues(Uri claimId, string loginId, Database db)
        {
            var command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pUserClaim_GetByClaimAndUser]";

            var parameter = command.CreateParameter();

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "ClaimId";
            parameter.Value = claimId.ToString();
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "LoginId";
            parameter.Value = loginId;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            return command;
        }


    }
}

