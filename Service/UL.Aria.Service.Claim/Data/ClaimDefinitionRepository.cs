using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Claim.Domain;

namespace UL.Aria.Service.Claim.Data
{
    /// <summary>
    ///     Claim definition repository
    /// </summary>
    public class ClaimDefinitionRepository : RepositoryBase<ClaimDefinition>, IClaimDefinitionRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClaimDefinitionRepository" /> class.
        /// </summary>
        public ClaimDefinitionRepository() : base("claimId")
        {
        }

        /// <summary>
        ///     Finds all.
        /// </summary>
        /// <returns></returns>
        public override IList<ClaimDefinition> FindAll()
        {
            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeFindAll(db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    return CreateClaimDefinitions(reader);
                }
            }
        }

        private DbCommand InitializeFindAll(Database db)
        {
            var command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pClaimDefinition_GetAll]";

            return command;
        }

        /// <summary>
        ///     Finds the by claim id.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <returns></returns>
        public ClaimDefinition FindByClaimId(Uri claimId)
        {
            Guard.IsNotNull(claimId, "claimId");
            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeFindByClaimId(claimId, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    var claimDefinitions = CreateClaimDefinitions(reader);
                    if (claimDefinitions.Count > 0)
                    {
                        return claimDefinitions[0];
                    }
                    throw new DatabaseItemNotFoundException(string.Format(CultureInfo.InvariantCulture,
                                                                          "Unable to find claim for claim Id ({0})",
                                                                          claimId));
                }
            }
        }

        private DbCommand InitializeFindByClaimId(Uri claimId, Database db)
        {
            var command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pClaimDefinition_GetByClaimId]";

            var parameter = command.CreateParameter();

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "ClaimId";
            parameter.Value = claimId.ToString();
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            return command;
        }


        /// <summary>
        ///     Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        public override ClaimDefinition FindById(Guid entityId)
        {
            Guard.IsNotEmptyGuid(entityId, "entityId");
            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeFindById(entityId, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    var claimDefinitions = CreateClaimDefinitions(reader);
                    if (claimDefinitions.Count > 0)
                    {
                        return claimDefinitions[0];
                    }
                    throw new DatabaseItemNotFoundException(string.Format(CultureInfo.InvariantCulture,
                                                                          "Unable to find user claim for Id ({0})",
                                                                          entityId));
                }
            }
        }

        private IList<ClaimDefinition> CreateClaimDefinitions(IDataReader reader)
        {
            var claimDefinitions = new Dictionary<Guid, ClaimDefinition>();

            while (reader.Read())
            {
                var claimDefinitionId = reader.GetGuid(reader.GetOrdinal("ClaimDefinitionId"));
                var claimId = new Uri(reader.GetString(reader.GetOrdinal("ClaimId")));

                var claimDefinition = new ClaimDefinition
                    {
                        Id = claimDefinitionId,
                        ClaimId = claimId
                    };

                claimDefinitions.Add(claimDefinition.Id.Value, claimDefinition);
            }

            reader.NextResult();


            while (reader.Read())
            {
                var valueClaimDefinitionId = reader.GetGuid(reader.GetOrdinal("ClaimDefinitionId"));
                var domainValue = reader.GetString(reader.GetOrdinal("Value"));


                claimDefinitions[valueClaimDefinitionId].ClaimDomainValues.Add(domainValue);
            }

            return claimDefinitions.Values.ToList();
        }

        private DbCommand InitializeFindById(Guid entityId, Database db)
        {
            var command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pClaimDefinition_Get]";

            var parameter = command.CreateParameter();

            parameter = command.CreateParameter();
            parameter.DbType = DbType.Guid;
            parameter.ParameterName = "ClaimDefinitionId";
            parameter.Value = entityId;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            return command;
        }

        /// <summary>
        ///     Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(ClaimDefinition entity)
        {
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeAddCommand(entity, db))
            {
                db.ExecuteNonQuery(command);
            }
        }

        private DbCommand InitializeAddCommand(ClaimDefinition entity, Database db)
        {
            var command = db.DbProviderFactory.CreateCommand() as SqlCommand;

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pClaimDefinition_Insert]";

            var parameter = command.CreateParameter();

            parameter = command.CreateParameter();
            parameter.DbType = DbType.Guid;
            parameter.ParameterName = "ClaimDefinitionId";
            parameter.Value = entity.Id;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "ClaimId";
            parameter.Value = entity.ClaimId.ToString();
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.ParameterName = "ClaimDomainValues";
            parameter.Value = ConvertToDataTable(entity.ClaimDomainValues);
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            return command;
        }

        private DataTable ConvertToDataTable(IList<string> claimDomainValues)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Value");
            foreach (var claimDomainValue in claimDomainValues)
            {
                dataTable.Rows.Add(claimDomainValue);
            }
            return dataTable;
        }

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override int Update(ClaimDefinition entity)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        public override void Remove(Guid entityId)
        {
            Guard.IsNotEmptyGuid(entityId, "entityId");
            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeRemove(entityId, db))
            {
                db.ExecuteNonQuery(command);
            }
        }


        private DbCommand InitializeRemove(Guid entityId, Database db)
        {
            var command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pClaimDefinition_Remove]";

            var parameter = command.CreateParameter();

            parameter = command.CreateParameter();
            parameter.DbType = DbType.Guid;
            parameter.ParameterName = "ClaimDefinitionId";
            parameter.Value = entityId;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            return command;
        }
    }
}