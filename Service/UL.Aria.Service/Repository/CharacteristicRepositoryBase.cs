using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Base implementation for <see cref="ProductFamilyCharacteristicDomainEntity"/> domain entities.
    /// </summary>
    /// <typeparam name="TCharacteristic">The type of the characteristic.</typeparam>
    public abstract class CharacteristicRepositoryBase<TCharacteristic> : TrackedDomainEntityRepositoryBase<TCharacteristic> where TCharacteristic : ProductFamilyCharacteristicDomainEntity, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacteristicRepositoryBase{TCharacteristic}" /> class.
        /// </summary>
        /// <param name="tableName">Name of the base field.</param>
        protected CharacteristicRepositoryBase(string tableName) : base(tableName + "Id", tableName)
        {
        }

        /// <summary>
        /// Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override TCharacteristic ConstructEntity(IDataReader reader)
        {
            TCharacteristic entity = base.ConstructEntity(reader);

            entity.Name = reader.GetValue<string>(TableName + "Name");
            entity.Description = reader.GetValue<string>(TableName + "Description");
            entity.ScopeId = reader.GetValue<Guid>("ScopeId");
            entity.CharacteristicTypeId = reader.GetValue<Guid>("CharacteristicTypeId");
            entity.IsRequired = reader.GetValue<bool>("IsRequired");
            entity.IsValueRequired = reader.GetValue<bool>("IsValueRequired");
            entity.SortOrder = reader.GetValue<int>("SortOrder");
            return entity;
        }

        /// <summary>
        /// Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        public override TCharacteristic FindById(Guid entityId)
        {
            if (null == Transaction.Current)
            {
                throw new TransactionException(
                    "Product Characteristics must be retrieved within a transaction scope due to multiple dependencies");
            }
            var characteristic = base.FindById(entityId);
            this.FillCharacteristicOptions(characteristic);
            return characteristic;
        }

        /// <summary>
        ///     Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override Guid Create(TCharacteristic entity)
        {
            var result = base.Create(entity);
            SaveOptions(entity);
            return result;
        }

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override int Update(TCharacteristic entity)
        {
            var result= base.Update(entity);
            SaveOptions(entity);
            return result;

        }

        /// <summary>
        /// Adds the table row fields.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isNew">if set to <c>true</c> [is new].</param>
        /// <param name="isDirty">if set to <c>true</c> [is dirty].</param>
        /// <param name="isDelete">if set to <c>true</c> [is delete].</param>
        /// <param name="dr">The dr.</param>
        protected override void AddTableRowFields(TCharacteristic entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);            

            dr["CharacteristicTypeId"] = entity.CharacteristicTypeId;
            dr["ScopeId"] = entity.ScopeId;
            dr[TableName + "Name"] = entity.Name;
            dr[TableName + "Description"] = entity.Description;
            dr["IsRequired"] = entity.IsRequired;
            dr["IsValueRequired"] = entity.IsValueRequired;
            dr["SortOrder"] = entity.SortOrder;
        }

        /// <summary>
        /// Constructs the options.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="attributes">The attributes.</param>
        protected void ConstructOptions(IDataReader reader, IList<TCharacteristic> attributes)
        {
            while (reader.Read())
            {
                ConstructOption(reader, attributes);
            }
        }

        private void ConstructOption(IDataReader reader, IList<TCharacteristic> attributes)
        {
            var id = reader.GetValue<Guid>(TableName + "OptionId");
            var option = new ProductFamilyCharacteristicOption();
            option.Id = id;
            option.ProductFamilyCharacteristicId = reader.GetValue<Guid>(TableName + "Id");
            option.Name = reader.GetValue<string>(TableName + "OptionName");
            option.Description = reader.GetValue<string>(TableName + "OptionDescription");
            option.Value = reader.GetValue<string>(TableName + "OptionValue");

            var attribute = attributes.FirstOrDefault(x => x.Id.Value == option.ProductFamilyCharacteristicId);
            if (null == attribute)
                return;
            attribute.Options.Add(option);
        }

        /// <summary>
        /// Fills the characteristic options.
        /// </summary>
        /// <param name="characteristic">The characteristic.</param>
        public void FillCharacteristicOptions(TCharacteristic characteristic)
        {
            var database = DatabaseFactory.CreateDatabase();
            var cmd = InitializeFindOptionsCommand(characteristic.Id.Value, database);

            using (var reader = database.ExecuteReader(cmd))
            {
               ConstructOptions(reader, new List<TCharacteristic>{characteristic});
            }
        }

        /// <summary>
        /// Initializes the find command.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        protected virtual DbCommand InitializeFindOptionsCommand(Guid entityId, Database db)
        {
            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[p" + TableName + "Option_GetBy" + TableName + "Id]");

            var sqlParameter = new SqlParameter(IdFieldName, SqlDbType.UniqueIdentifier) { Value = entityId };
            command.Parameters.Add(sqlParameter);

            return command;
        }

        /// <summary>
        /// Saves the options.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected virtual void SaveOptions(TCharacteristic entity) 
        {
            var database = DatabaseFactory.CreateDatabase();
            using (var cmd = InitializeSaveOptionsCommand(entity, database))
            {
                database.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// Initializes the save options command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="db">The database.</param>
        /// <returns></returns>
        protected virtual DbCommand InitializeSaveOptionsCommand(TCharacteristic entity, Database db)
        {
            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[p" + TableName +TableName+ "Option_SaveFor" + TableName +"]");

            var parameterTable = new DataTable();
            var optionIdName = TableName + "OptionId";
            parameterTable.Columns.Add(optionIdName, typeof(Guid));
            parameterTable.Columns.Add(IdFieldName, typeof(Guid));

            foreach (var option in entity.Options)
            {
                var row  = parameterTable.NewRow();
                row[optionIdName] = option.Id.Value;
                row[IdFieldName] = entity.Id.Value;
                parameterTable.Rows.Add(row);
            }

            var sqlParameter = new SqlParameter("@"  + TableName +TableName+ "Option", SqlDbType.Structured){Value = parameterTable};
            command.Parameters.Add(sqlParameter);
            
            return command;
        }

        /// <summary>
        /// Gets the by product family id.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        public IList<TCharacteristic> FindByProductFamilyId(Guid productFamilyId)
        {
            if (null == Transaction.Current)
            {
                throw new TransactionException(
                    "Product Characteristics must be retrieved within a transaction scope due to multiple dependencies");
            }
            var database = DatabaseFactory.CreateDatabase();
            var cmd = new Func<Database, DbCommand>(a =>
            {
                var command = (SqlCommand)database.GetStoredProcCommand("[dbo].[p"+TableName+"_GetByFamilyId]");

                var sqlParameter = new SqlParameter("@familyId", SqlDbType.UniqueIdentifier) { Value = productFamilyId };
                command.Parameters.Add(sqlParameter);

                return command;
            });
            var result = new List<TCharacteristic>();

            using (var reader = database.ExecuteReader(cmd(database)))
            {
                while (reader.Read())
                {
                    result.Add(ConstructEntity(reader));
                }
                reader.NextResult();
                ConstructOptions(reader, result);
            }
            return result;
        }

        /// <summary>
        /// Finds the by scope id.
        /// </summary>
        /// <param name="scopeId">The scope id.</param>
        /// <returns></returns>
        public IEnumerable<TCharacteristic> FindByScopeId(Guid scopeId)
        {
            var characteristics = ExecuteReaderCommand(database => InitializeFindByScopeCommand(scopeId, database), ConstructEntity);
            foreach (var characteristic in characteristics)
            {
                FillCharacteristicOptions(characteristic);
            }
            return characteristics;
        }

        /// <summary>
        ///     Finds all.
        /// </summary>
        /// <returns></returns>
        public override IList<TCharacteristic> FindAll()
        {
            var characteristics = base.FindAll();
            foreach (var characteristic in characteristics)
            {
                FillCharacteristicOptions(characteristic);
            }
            return characteristics;
        }

        /// <summary>
        /// Initializes the find command.
        /// </summary>
        /// <param name="scopeId">The scope id.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        protected virtual DbCommand InitializeFindByScopeCommand(Guid scopeId, Database db)
        {
            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[p" + TableName + "_Get]");

            var sqlParameter = new SqlParameter("@ScopeId", SqlDbType.UniqueIdentifier) { Value = scopeId };
            command.Parameters.Add(sqlParameter);

            return command;
        }
    }
}
