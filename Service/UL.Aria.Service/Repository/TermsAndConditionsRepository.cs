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
    /// class that implements CRUD for Terms and Conditions
    /// </summary>
    public sealed class TermsAndConditionsRepository : RepositoryBase<TermsAndConditions>, ITermsAndConditionsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TermsAndConditionsRepository" /> class.
        /// </summary>
        public TermsAndConditionsRepository()
            : base("TermsAndConditions")
        {
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override IList<TermsAndConditions> FindAll()
        {
            List<TermsAndConditions> all = new List<TermsAndConditions>();
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeFindAllCommand(db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        Guid entityId;
                        all.Add(ConstructTermsAndConditions(reader, out entityId));
                    }
                }
            }

            return all;
        }

        /// <summary>
        /// Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        public override TermsAndConditions FindById(Guid entityId)
        {
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeFindByCommand(entityId, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        return ConstructTermsAndConditions(reader, out entityId);
                    }
                }

                throw new DatabaseItemNotFoundException(entityId.ToString());
            }
        }

        /// <summary>
        /// Finds the by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        /// <exception cref="DatabaseItemNotFoundException"></exception>
        public IEnumerable<TermsAndConditions> FindByUserId(Guid userId)
        {
            var db = DatabaseFactory.CreateDatabase();
	        List<TermsAndConditions> terms = new List<TermsAndConditions>();

            using (var command = InitializeFindByUserCommand(userId, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        Guid entityId;
                            
                        terms.Add(ConstructTermsAndConditions(reader, out entityId));
                    }
                }
            } 

			if(terms.Count == 0)
				throw new DatabaseItemNotFoundException(userId.ToString());

	        return terms;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(TermsAndConditions entity)
        {
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeAddCommand(entity, db))
            {
                db.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override int Update(TermsAndConditions entity)
        {
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeUpdateCommand(entity, db))
            {
                return db.ExecuteNonQuery(command);
            }
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

        /// <summary>
        /// Initializes the find by command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        private DbCommand InitializeFindByCommand(Guid entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pTermsAndConditions_Get]");
            db.AddInParameter(command, "TermsAndConditionsId", DbType.Guid, entity);
            return command;
        }

        /// <summary>
        /// Initializes the find by user id command.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        private DbCommand InitializeFindByUserCommand(Guid userId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pTermsAndConditions_GetByUserId]");
            db.AddInParameter(command, "UserId", DbType.Guid, userId);
            return command;
        }

        /// <summary>
        /// Initializes the find all command.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        private DbCommand InitializeFindAllCommand(Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pTermsAndConditions_Get]");
            db.AddInParameter(command, "TermsAndConditionsId", DbType.Guid, DBNull.Value);
            return command;
        }


        /// <summary>
        /// Initializes the add command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        private DbCommand InitializeAddCommand(TermsAndConditions entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pTermsAndConditions_Insert]");

            db.AddInParameter(command, "TermsAndConditionsId", DbType.Guid, entity.Id);
            db.AddInParameter(command, "Type", DbType.String, entity.Type.ToString("G"));
            db.AddInParameter(command, "Version", DbType.String, entity.Version);
            db.AddInParameter(command, "LegalText", DbType.String, entity.LegalText);
            db.AddInParameter(command, "CreatedDT", DbType.DateTime2, entity.CreatedDateTime);
            db.AddInParameter(command, "CreatedBy", DbType.Guid, entity.CreatedById);
            db.AddInParameter(command, "UpdatedDT", DbType.DateTime2, entity.UpdatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, entity.UpdatedById);

            return command;
        }

        /// <summary>
        /// Constructs the terms and conditions.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="termsAndConditionsId">The terms and conditions id.</param>
        /// <returns></returns>
        private static TermsAndConditions ConstructTermsAndConditions(IDataReader reader, out Guid termsAndConditionsId)
        {
            termsAndConditionsId = reader.GetValue<Guid>("TermsAndConditionsId");

            TermsAndConditionsType type;
        
            return new TermsAndConditions(termsAndConditionsId)
            {
                Type = Enum.TryParse(reader.GetValue<string>("Type"), true, out type) ? type : TermsAndConditionsType.None,
                Version = reader.GetValue<int>("Version"),
                LegalText = reader.GetValue<string>("LegalText"),
                CreatedById = reader.GetValue<Guid>("CreatedBy"),
                CreatedDateTime = reader.GetValue<DateTime>("CreatedDT"),
                UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedDT")
            };

        }
        /// <summary>
        /// Initializes the update command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        private DbCommand InitializeUpdateCommand(TermsAndConditions entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pTermsAndConditions_Update]");

            db.AddInParameter(command, "TermsAndConditionsId", DbType.Guid, entity.Id);
            db.AddInParameter(command, "Type", DbType.String, entity.Type.ToString("G"));
            db.AddInParameter(command, "Version", DbType.String, entity.Version);
            db.AddInParameter(command, "LegalText", DbType.String, entity.LegalText);
            db.AddInParameter(command, "CreatedDT", DbType.DateTime2, entity.CreatedDateTime);
            db.AddInParameter(command, "CreatedBy", DbType.Guid, entity.CreatedById);
            db.AddInParameter(command, "UpdatedDT", DbType.DateTime2, entity.UpdatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, entity.UpdatedById);

            return command;
        }

        /// <summary>
        /// Initializes the remove command.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        private DbCommand InitializeRemoveCommand(Guid id, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pTermsAndConditions_Remove]");

            db.AddInParameter(command, "TermsAndConditionsId", DbType.Guid, id);

            return command;
        }
    }
}
