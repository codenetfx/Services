using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Base repository for <see cref="TrackedDomainEntity" />
    /// </summary>
    /// <typeparam name="TTrackedDomainEntity">The type of the characteristic.</typeparam>
    public abstract class TrackedDomainEntityRepositoryBase<TTrackedDomainEntity> : SearchRepositoryBase<TTrackedDomainEntity>
        where TTrackedDomainEntity : TrackedDomainEntity, new()
    {


        #region DbType to .NET map
        // ReSharper disable StaticFieldInGenericType
        private static readonly Dictionary<string, Type> TypeMappingDictionary = new Dictionary<string, Type>
            {
                {"bigint", typeof (Int64)},
                {"binary", typeof (Byte[])},
                {"bit", typeof (Boolean)},
                {"char", typeof (String)},
                {"datetime", typeof (DateTime)},
                {"datetime2", typeof (DateTime)},
                {"decimal", typeof (Decimal)},
                {"float", typeof (Double)},
                {"image", typeof (Byte[])},
                {"int", typeof (Int32)},
                {"money", typeof (Decimal)},
                {"nchar", typeof (String)},
                {"ntext", typeof (String)},
                {"numeric", typeof (Decimal)},
                {"nvarchar", typeof (String)},
                {"real", typeof (Single)},
                {"smalldatetime", typeof (DateTime)},
                {"smallint", typeof (Int16)},
                {"smallmoney", typeof (Decimal)},
                {"sql_variant", typeof (Object)},
                {"text", typeof (String)},
                {"timestamp", typeof (Byte[])},
                {"tinyint", typeof (Byte)},
                {"uniqueidentifier", typeof (Guid)},
                {"varbinary", typeof (Byte[])},
                {"varchar", typeof (String)}
            };
        #endregion

        private static readonly ConcurrentDictionary<string, List<KeyValuePair<string, Type>>> DataTableDictionary =
            new ConcurrentDictionary<string, List<KeyValuePair<string, Type>>>();
        // ReSharper restore StaticFieldInGenericType

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackedDomainEntityRepositoryBase{TTrackedDomainEntity}" /> class.
        /// </summary>
        /// <param name="dbIdFieldName">Name of the db id field.</param>
        /// <param name="tableName">Name of the base field.</param>
        protected TrackedDomainEntityRepositoryBase(string dbIdFieldName, string tableName)
            : base(dbIdFieldName, tableName)
        {

        }



        /// <summary>
        /// Creates the data table.
        /// </summary>
        /// <returns>DataTable.</returns>
        protected DataTable CreateDataTable()
        {
            return CreateDataTable(TableName);
        }

        /// <summary>
        /// Creates the data table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>DataTable.</returns>
        protected DataTable CreateDataTable(string tableName)
        {
            List<KeyValuePair<string, Type>> columnList;
            if (!DataTableDictionary.TryGetValue(tableName, out columnList))
            {
                columnList = new List<KeyValuePair<string, Type>>();
                var db = DatabaseFactory.CreateDatabase();
                var sql =
                    "SELECT st.name as typeName, sc.name as columnName FROM sys.columns sc join sys.types st on sc.system_type_id = st.system_type_id WHERE st.name != 'sysname' and object_id IN ( SELECT type_table_object_id  FROM sys.table_types  WHERE name = '" +
                    tableName + "') ORDER BY sc.column_id";
                var command = db.GetSqlStringCommand(sql);

                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        columnList.Add(new KeyValuePair<string, Type>(reader.GetString(1),
                                                                      TypeMappingDictionary[reader.GetString(0)]));
                    }
                }

                DataTableDictionary.TryAdd(tableName, columnList);
            }

            var dataTable = new DataTable();

            foreach (var keyValuePair in columnList)
            {

                var col = dataTable.Columns.Add(keyValuePair.Key, keyValuePair.Value);
                col.AllowDBNull = true;
            }

            return dataTable;
        }


        /// <summary>
        ///     Creates the table row.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="isNew">
        ///     if set to <c>true</c> [is new].
        /// </param>
        /// <param name="isDirty">
        ///     if set to <c>true</c> [is dirty].
        /// </param>
        /// <param name="isDelete">
        ///     if set to <c>true</c> [is delete].
        /// </param>
        /// <returns></returns>
        protected DataRow CreateTableRow(DataTable dataTable, TTrackedDomainEntity entity, bool isNew, bool isDirty,
                                         bool isDelete)
        {
            var dr = dataTable.NewRow();

            AddTableRowFields(entity, isNew, isDirty, isDelete, dr);

            return dr;
        }

        /// <summary>
        ///     Adds the table row fields.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isNew">
        ///     if set to <c>true</c> [is new].
        /// </param>
        /// <param name="isDirty">
        ///     if set to <c>true</c> [is dirty].
        /// </param>
        /// <param name="isDelete">
        ///     if set to <c>true</c> [is delete].
        /// </param>
        /// <param name="dr">The dr.</param>
        protected virtual void AddTableRowFields(TTrackedDomainEntity entity, bool isNew, bool isDirty, bool isDelete,
                                                 DataRow dr)
        {
            dr[IdFieldName] = entity.Id;
            dr["CreatedBy"] = entity.CreatedById;
            dr["UpdatedBy"] = entity.UpdatedById;
            dr["CreatedDT"] = entity.CreatedDateTime;
            dr["UpdatedDT"] = entity.UpdatedDateTime;
            dr["IsNew"] = isNew;
            dr["IsDirty"] = isDirty;
            dr["IsDelete"] = isDelete;
        }


        /// <summary>
        ///     Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override TTrackedDomainEntity ConstructEntity(IDataReader reader)
        {
             var dynamicMap = Mapper.Configuration.CreateMap<IDataReader, TTrackedDomainEntity>()
                .ForMember(x => x.Id, x => x.MapFrom(y => y.GetValue<Guid>(this.IdFieldName)));

            if (reader.GetSchemaTable().Rows.OfType<DataRow>().Any(row => row["ColumnName"].ToString() == "CreatedDT"))
            {
                dynamicMap.ForMember(x => x.CreatedById, x => x.MapFrom(y => y.GetValue<Guid>("CreatedBy")))
                    .ForMember(x => x.CreatedDateTime, x => x.MapFrom(y => y.GetValue<DateTime>("CreatedDT")))
                    .ForMember(x => x.UpdatedById, x => x.MapFrom(y => y.GetValue<Guid>("UpdatedBy")))
                    .ForMember(x => x.UpdatedDateTime, x => x.MapFrom(y => y.GetValue<DateTime>("UpdatedDT")));
            }

            return Mapper.Map<IDataReader, TTrackedDomainEntity>(reader);
        }

        /// <summary>
        ///     Repository save enumeration.
        /// </summary>
        protected enum SaveEnum
        {
            /// <summary>
            ///     The add operation
            /// </summary>
            Add,

            /// <summary>
            ///     The update operation
            /// </summary>
            Update,

            /// <summary>
            ///     The delete operation
            /// </summary>
            Delete
        }

        /// <summary>
        /// Creates the claims parameter table.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <returns></returns>
        internal DataTable CreateClaimsParameterTable(IEnumerable<System.Security.Claims.Claim> claims)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Claim", typeof(string));
            dataTable.Columns.Add("ClaimValue", typeof(string));
            foreach (var claim in claims)
            {
                dataTable.Rows.Add(claim.Type, claim.Value);
            }
            return dataTable;
        }

        /// <summary>
        ///     Initializes the save command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="db">The db.</param>
        /// <param name="saveEnum">The save enum.</param>
        /// <returns></returns>
        protected virtual DbCommand InitializeSaveCommand(TTrackedDomainEntity entity, Database db, SaveEnum saveEnum)
        {
            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[p" + TableName + "_Save]");

            var dataTable = CreateDataTable();
            var dr = CreateTableRow(dataTable, entity, SaveEnum.Add.Equals(saveEnum), SaveEnum.Update.Equals(saveEnum),
                                    SaveEnum.Delete.Equals(saveEnum));
            dataTable.Rows.Add(dr);

            var sqlParameter = new SqlParameter("@" + TableName, SqlDbType.Structured) { Value = dataTable };
            command.Parameters.Add(sqlParameter);

            sqlParameter = new SqlParameter("@UpdatedDT", SqlDbType.DateTime2) { Value = DateTime.UtcNow };
            command.Parameters.Add(sqlParameter);

            return command;
        }

        /// <summary>
        /// Initializes the multi save command.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="db">The database.</param>
        /// <param name="saveEnum">The save enum.</param>
        /// <returns>DbCommand.</returns>
        protected virtual DbCommand InitializeMultiSaveCommand(IEnumerable<TTrackedDomainEntity> entities, Database db, SaveEnum saveEnum)
        {
            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[p" + TableName + "_Save]");

            var dataTable = CreateDataTable();

            foreach (var entity in entities)
            {
                var dr = CreateTableRow(dataTable, entity, SaveEnum.Add.Equals(saveEnum), SaveEnum.Update.Equals(saveEnum),
                                        SaveEnum.Delete.Equals(saveEnum));
                dataTable.Rows.Add(dr);
            }

            var sqlParameter = new SqlParameter("@" + TableName, SqlDbType.Structured) { Value = dataTable };
            command.Parameters.Add(sqlParameter);

            sqlParameter = new SqlParameter("@UpdatedDT", SqlDbType.DateTime2) { Value = DateTime.UtcNow };
            command.Parameters.Add(sqlParameter);

            return command;
        }

        /// <summary>
        ///     Finds all.
        /// </summary>
        /// <returns></returns>
        public override IList<TTrackedDomainEntity> FindAll()
        {
            return ExecuteReaderCommand(database => InitializeFindCommand(Guid.Empty, database), ConstructEntity);
        }

        /// <summary>
        ///     Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        public override TTrackedDomainEntity FindById(Guid entityId)
        {
            var result = ExecuteReaderCommand(database => InitializeFindCommand(entityId, database), ConstructEntity);

            if (result.Count == 0)
                throw new DatabaseItemNotFoundException();
            return result.First();
        }

        /// <summary>
        ///     Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(TTrackedDomainEntity entity)
        {
            Create(entity);
        }

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override int Update(TTrackedDomainEntity entity)
        {
            return ExecuteNonQueryCommand(database => InitializeSaveCommand(entity, database, SaveEnum.Update), entity);
        }

        /// <summary>
        /// Updates the collection of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        public int Update(IEnumerable<TTrackedDomainEntity> entities)
        {
            return ExecuteNonQueryCommand(database => InitializeMultiSaveCommand(entities, database, SaveEnum.Update));
        }   

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        public int Delete(IEnumerable<TTrackedDomainEntity> entities)
        {
            return ExecuteNonQueryCommand(database => InitializeMultiSaveCommand(entities, database, SaveEnum.Delete));
        }

        /// <summary>
        ///     Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        public override void Remove(Guid entityId)
        {
            var entity = new TTrackedDomainEntity { Id = entityId };

            ExecuteNonQueryCommand(database => InitializeSaveCommand(entity, database, SaveEnum.Delete), entity);
        }

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual Guid Create(TTrackedDomainEntity entity)
        {
            ExecuteNonQueryCommand(database => InitializeSaveCommand(entity, database, SaveEnum.Add),
                                   entity);

            // ReSharper disable once PossibleInvalidOperationException
            return entity.Id.Value;
        }


        /// <summary>
        /// Initializes the find command.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        protected virtual DbCommand InitializeFindCommand(Guid entityId, Database db)
        {
            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[p" + TableName + "_Get]");

            var sqlParameter = new SqlParameter(IdFieldName, SqlDbType.UniqueIdentifier) { Value = entityId };
            command.Parameters.Add(sqlParameter);

            return command;
        }




    }
}