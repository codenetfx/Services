using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Class CategoryRepository
    /// </summary>
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        /// <summary>
        ///     Containers the repository.
        /// </summary>
        public CategoryRepository()
            : this("CategoryId")
        {
        }

        private CategoryRepository(string dbIdFieldName)
            : base(dbIdFieldName)
        {
        }

        /// <summary>
        ///     Finds all.
        /// </summary>
        /// <returns>IList{Category}.</returns>
        public override IList<Category> FindAll()
        {
            return ExecuteReaderCommand(InitializedGetAllCommand,
                                        ConstructEntity);
        }

        /// <summary>
        ///     Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>Category.</returns>
        public override Category FindById(Guid entityId)
        {
            return GetById(entityId);
        }

        /// <summary>
        ///     Gets the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>Category.</returns>
        public Category GetById(Guid entityId)
        {
            return ExecuteReaderCommand(database => InitializedGetByIdCommand(entityId, database),
                                        ConstructEntity).FirstOrDefault();
        }

        /// <summary>
        ///     Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(Category entity)
        {
            Create(entity);
        }

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Int32.</returns>
        public override int Update(Category entity)
        {
            return ExecuteNonQueryCommand(db => InitializedUpdateCommand(entity, db), entity);
        }

        /// <summary>
        ///     Removes the specified entity id.
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
        ///     Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Guid.</returns>
        public Guid Create(Category entity)
        {
            //Guard.IsNotEmptyGuid(entity.PrimarySearchEntityId, "entityId");
            var id = Guid.Empty;
            ExecuteNonQueryCommand(db => InitializeInsertCommand(entity, db), entity,
                                   cmd => { id = (Guid) cmd.Parameters["@CategoryId"].Value; });
            return id;
        }

        /// <summary>
        ///     Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Category.</returns>
        protected override Category ConstructEntity(IDataReader reader)
        {
            Category entity = base.ConstructEntity(reader);
            entity.Name = reader.GetValue<string>("CategoryName");

            return entity;
        }

        private DbCommand InitializeInsertCommand(Category entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCategory_Insert]");

            db.AddInParameter(command, "@CategoryId", DbType.Guid, entity.Id);
            db.AddInParameter(command, "@CategoryName", DbType.String, entity.Name);
            db.AddInParameter(command, "CreatedBy", DbType.Guid, entity.CreatedById);
            db.AddInParameter(command, "CreatedOn", DbType.DateTime2, entity.CreatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, entity.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, entity.UpdatedDateTime);

            return command;
        }

        private DbCommand InitializedGetAllCommand(Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCategory_GetAll]");

            return command;
        }

        private DbCommand InitializedGetByIdCommand(Guid categoryId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCategory_GetById]");

            db.AddInParameter(command, "@CategoryId", DbType.Guid, categoryId);

            return command;
        }

        private DbCommand InitializedUpdateCommand(Category entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCategory_Update]");

            db.AddInParameter(command, "@CategoryId", DbType.Guid, entity.Id);
            db.AddInParameter(command, "@CategoryName", DbType.String, entity.Name);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, entity.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, entity.UpdatedDateTime);

            return command;
        }

        private DbCommand InitializeRemoveCommand(Guid entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCategory_Delete]");

            db.AddInParameter(command, "CategoryId", DbType.Guid, entity);

            return command;
        }
    }
}