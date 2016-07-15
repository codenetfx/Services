using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Repository for <see cref="Product" /> instances
    /// </summary>
    public sealed class ProductUploadRepository : RepositoryBase<ProductUpload>, IProductUploadRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductRepository" /> class.
        /// </summary>
        public ProductUploadRepository() : base("ProductUploadId")
        {
        }

        /// <summary>
        ///     Finds all.
        /// </summary>
        /// <returns>IList{ProductUpload}.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override IList<ProductUpload> FindAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>ProductUpload.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override ProductUpload FindById(Guid entityId)
        {
            var productUpload = GetByCommand(InitializeFindByIdCommand, entityId).FirstOrDefault();

            if (productUpload == null)
                throw new DatabaseItemNotFoundException("product upload not found", new Exception());

            return productUpload;
        }

        /// <summary>
        ///     Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Add(ProductUpload entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override int Update(ProductUpload entity)
        {
            return ExecuteCommand(InitializeUpdateCommand, entity);
        }

        /// <summary>
        ///     Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Remove(Guid entityId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Uploads the specified file.
        /// </summary>
        /// <param name="productUpload">The product upload.</param>
        /// <returns>Confirmation Id.</returns>
        Guid IProductUploadRepository.Upload(ProductUpload productUpload)
        {
            return Insert(productUpload);
        }

        /// <summary>
        ///     Fetches the by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="rowStartIndex"></param>
        /// <param name="rowEndIndex"></param>
        /// <returns>List{ProductUpload}.</returns>
        public ProductUploadSearchResultSet FetchByUserId(Guid userId, long rowStartIndex, long rowEndIndex)
        {
            var db = DatabaseFactory.CreateDatabase();
            var set = new ProductUploadSearchResultSet
                {
                    SearchCriteria =
                        new SearchCriteria {UserId = userId, StartIndex = rowStartIndex, EndIndex = rowEndIndex}
                };
            using (var command = InitializeFetchByUserIdCommand(db, userId, rowStartIndex, rowEndIndex))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    ConstructSearchResultSet(reader, set);
                }
            }
            return set;
        }

        /// <summary>
        /// Fetches the next available <see cref="ProductUpload"/> for processing.
        /// </summary>
        /// <returns>The next <see cref="ProductUpload"/>. May return null (will not throw) if there is none.</returns>
        public ProductUpload FetchNextForProcessing()
        {
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeFetchNextForProcessingCommand(db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    if (reader.Read())
                        return ConstructProductUploadWithFile(reader);
                }
            }
            return null;
        }

        private DbCommand InitializeUpdateCommand(ProductUpload entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProductUpload_Update]");

            db.AddInParameter(command, "ProductUploadId", DbType.Guid, entity.Id);
            db.AddInParameter(command, "Status", DbType.Int16, entity.Status);
            db.AddInParameter(command, "CreatedByUserLoginId", DbType.String, entity.CreatedByUserLoginId);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, entity.UpdatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, entity.UpdatedById);

            return command;
        }

        private static DbCommand InitializeFetchByUserIdCommand(Database db, Guid userId, long rowStartIndex,
                                                                long rowEndIndex)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProductUpload_GetByUserId]");
            db.AddInParameter(command, "UserId", DbType.Guid, userId);
            db.AddInParameter(command, "StartIndex", DbType.Int64, rowStartIndex);
            db.AddInParameter(command, "EndIndex", DbType.Int64, rowEndIndex);
            return command;
        }

        private static DbCommand InitializeFetchNextForProcessingCommand(Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProductUpload_GetNextAvailable]");
            return command;
        }

        private void ConstructSearchResultSet(IDataReader reader, ProductUploadSearchResultSet set)
        {
            set.Summary = new SearchSummary();
            long? start = null;
            long? end = null;
            while (reader.Read())
            {
                set.Summary.TotalResults = reader.GetValue<long>("TotalRows");
                //returned on each row for convenience. will be same value on each.
                long row = reader.GetValue<long>("RowNumber") - 1;
                start = start ?? row;
                end = end ?? row;
                end = row > end ? row : end;
                start = row < start ? row : start;
                set.Results.Add(ConstructSearchResult(reader));
            }
            set.Summary.StartIndex = start ?? 0;
            set.Summary.EndIndex = end ?? 0;
        }

        private static ProductUploadSearchResult ConstructSearchResult(IDataReader reader)
        {
            var result = new ProductUploadSearchResult
                {
                    Id = reader.GetValue<Guid>("ProductUploadId"),
                    EntityType = EntityTypeEnumDto.ProductUpload,
                    Name = reader.GetValue<string>("FileName"),
                    Title = reader.GetValue<string>("FileName"),
                    ChangeDate = reader.GetValue<DateTime>("UpdatedOn"),
                    ProductUpload = ConstructProductUpload(reader)
                };
            return result;
        }

        private static ProductUpload ConstructProductUpload(IDataReader reader)
        {
            return new ProductUpload
                {
                    Id = reader.GetValue<Guid>("ProductUploadId"),
                    CompanyId = reader.GetValue<Guid>("CompanyId"),
                    FileName = reader.GetValue<string>("FileName"),
                    CreatedById = reader.GetValue<Guid>("CreatedBy"),
                    CreatedDateTime = reader.GetValue<DateTime>("CreatedOn"),
                    Status = (ProductUploadStatusEnumDto) reader.GetValue<Int16>("Status"),
                    CreatedByUserLoginId = reader.GetValue<string>("CreatedByUserLoginId"),
                    UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                    UpdatedDateTime = reader.GetValue<DateTime>("UpdatedOn")
                };
        }

        private static ProductUpload ConstructProductUploadWithFile(IDataReader reader)
        {
            return new ProductUpload
            {
                Id = reader.GetValue<Guid>("ProductUploadId"),
                CompanyId = reader.GetValue<Guid>("CompanyId"),
                FileName = reader.GetValue<string>("FileName"),
                FileContent = reader.GetValue<byte[]>("FileContent"),
                CreatedById = reader.GetValue<Guid>("CreatedBy"),
                CreatedDateTime = reader.GetValue<DateTime>("CreatedOn"),
                Status = (ProductUploadStatusEnumDto)reader.GetValue<Int16>("Status"),
                CreatedByUserLoginId = reader.GetValue<string>("CreatedByUserLoginId"),
                UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedOn")
            };
        }

        private Guid Insert(ProductUpload entity)
        {
            var id = default(Guid);

            ExecuteCommand(InitializeInsertCommand, entity,
                           cmd => { id = (Guid) cmd.Parameters["@ProductUploadId"].Value; });

            return id;
        }

        /// <summary>
        ///     Initializes the add command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="db">The db.</param>
        /// <returns>DbCommand.</returns>
        private static DbCommand InitializeInsertCommand(ProductUpload entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProductUpload_Insert]");

            db.AddOutParameter(command, "@ProductUploadId", DbType.Guid, 128);
            db.AddInParameter(command, "@CompanyId", DbType.Guid, entity.CompanyId);
            db.AddInParameter(command, "@Status", DbType.Int16, entity.Status);
            db.AddInParameter(command, "@FileContent", DbType.Binary, entity.FileContent);
            db.AddInParameter(command, "@FileName", DbType.String, entity.FileName);
            db.AddInParameter(command, "CreatedByUserLoginId", DbType.String, entity.CreatedByUserLoginId);
            db.AddInParameter(command, "@CreatedOn", DbType.DateTime2, entity.CreatedDateTime.DefaultToUtcNow());
            db.AddInParameter(command, "@CreatedBy", DbType.Guid, entity.CreatedById);
            db.AddInParameter(command, "@UpdatedBy", DbType.Guid, entity.UpdatedById);
            db.AddInParameter(command, "@UpdatedOn", DbType.DateTime2, entity.UpdatedDateTime.DefaultToUtcNow());

            return command;
        }

        private static int ExecuteCommand<TEntity>(Func<TEntity, Database, DbCommand> commandInitializer,
                                                   TEntity entity,
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

        private static IEnumerable<ProductUpload> GetByCommand<T>(Func<T, Database, DbCommand> initializeCommand, T id)
        {
            var db = DatabaseFactory.CreateDatabase();
            var results = new List<ProductUpload>();

            using (var command = initializeCommand(id, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        results.Add(ConstructProductUpload(reader));
                    }
                }
            }
            return results;
        }

        private static DbCommand InitializeFindByIdCommand(Guid entityId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProductUpload_Get]");
            db.AddInParameter(command, "@ProductUploadId", DbType.Guid, entityId);
            return command;
        }
    }
}