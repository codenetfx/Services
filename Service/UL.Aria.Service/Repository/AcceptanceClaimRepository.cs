using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     deal with persistance of Acceptance Claim table
    /// </summary>
    public class AcceptanceClaimRepository : RepositoryBase<AcceptanceClaim>, IAcceptanceClaimRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AcceptanceClaimRepository" /> class.
        /// </summary>
        public AcceptanceClaimRepository()
            : base("AcceptanceClaim")
        {
        }

        /// <summary>
        ///     Finds all.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override IList<AcceptanceClaim> FindAll()
        {
            var all = new List<AcceptanceClaim>();
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand command = InitializeFindAllCommand(db))
            {
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        Guid entityId;
                        all.Add(ConstructAcceptanceClaim(reader, out entityId));
                    }
                }
            }

            return all;
        }

        /// <summary>
        ///     Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        public override AcceptanceClaim FindById(Guid entityId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand command = InitializeFindByCommand(entityId, db))
            {
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        return ConstructAcceptanceClaim(reader, out entityId);
                    }
                }

                throw new DatabaseItemNotFoundException(entityId.ToString());
            }
        }

        /// <summary>
        ///     Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(AcceptanceClaim entity)
        {
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand command = InitializeAddCommand(entity, db))
            {
                db.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override int Update(AcceptanceClaim entity)
        {
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand command = InitializeUpdateCommand(entity, db))
            {
                return db.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        ///     Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        public override void Remove(Guid entityId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand command = InitializeRemoveCommand(entityId, db))
            {
                db.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        ///     Initializes the find by command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        private DbCommand InitializeFindByCommand(Guid entity, Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pAcceptanceClaim_Get]");
            db.AddInParameter(command, "AcceptanceClaimId", DbType.Guid, entity);
            return command;
        }

        /// <summary>
        ///     Initializes the find all command.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        private DbCommand InitializeFindAllCommand(Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pAcceptanceClaim_Get]");
            db.AddInParameter(command, "AcceptanceClaimId", DbType.Guid, DBNull.Value);
            return command;
        }


        /// <summary>
        ///     Initializes the add command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        private DbCommand InitializeAddCommand(AcceptanceClaim entity, Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pAcceptanceClaim_Insert]");

            db.AddInParameter(command, "AcceptanceClaimId", DbType.Guid, entity.Id);
            db.AddInParameter(command, "Accepted", DbType.Boolean, entity.Accepted);
            db.AddInParameter(command, "AcceptedDT", DbType.DateTime2, entity.AcceptedDateTime);
            db.AddInParameter(command, "UserId", DbType.Guid, entity.UserId);
            db.AddInParameter(command, "TermsAndConditionsId", DbType.Guid, entity.TermsAndConditionsId);


            return command;
        }


        /// <summary>
        ///     Constructs the acceptance claim.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="AcceptanceClaimId">The acceptance claim id.</param>
        /// <returns></returns>
        private static AcceptanceClaim ConstructAcceptanceClaim(IDataReader reader, out Guid AcceptanceClaimId)
        {
            AcceptanceClaimId = reader.GetValue<Guid>("AcceptanceClaimId");

            return new AcceptanceClaim(AcceptanceClaimId)
                {
                    Accepted = reader.GetValue<bool>("Accepted"),
                    AcceptedDateTime = reader.GetValue<DateTime>("AcceptedDT"),
                    UserId = reader.GetValue<Guid>("UserId"),
                    TermsAndConditionsId = reader.GetValue<Guid>("TermsAndConditionsId")
                };
        }

        /// <summary>
        ///     Initializes the update command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        private DbCommand InitializeUpdateCommand(AcceptanceClaim entity, Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pAcceptanceClaim_Update]");

            db.AddInParameter(command, "AcceptanceClaimId", DbType.Guid, entity.Id);
            db.AddInParameter(command, "Accepted", DbType.Boolean, entity.Accepted);
            db.AddInParameter(command, "AcceptedDT", DbType.DateTime2, entity.AcceptedDateTime);
            db.AddInParameter(command, "UserId", DbType.Guid, entity.UserId);
            db.AddInParameter(command, "TermsAndConditionsId", DbType.Guid, entity.TermsAndConditionsId);


            return command;
        }

        /// <summary>
        ///     Initializes the remove command.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        private DbCommand InitializeRemoveCommand(Guid id, Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pAcceptanceClaim_Remove]");

            db.AddInParameter(command, "AcceptanceClaimId", DbType.Guid, id);

            return command;
        }
    }
}