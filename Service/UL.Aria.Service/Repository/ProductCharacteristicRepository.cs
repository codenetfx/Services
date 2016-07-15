using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Product Characteristic Repository
    /// </summary>
    public sealed class ProductCharacteristicRepository : TrackedDomainEntityRepositoryBase<ProductCharacteristic>,
                                                          IProductCharacteristicRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TrackedDomainEntityRepositoryBase{TTrackedDomainEntity}" /> class.
        /// </summary>
        public ProductCharacteristicRepository()
            : base("CharacteristicValueID", "ProductCharacteristic")
        {
        }

        /// <summary>
        ///     Finds the by product id.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        public IList<ProductCharacteristic> FindByProductId(Guid? productId)
        {
            var database = DatabaseFactory.CreateDatabase();
            using (var command = InitializeFindByProductIdCommand(productId ?? Guid.Empty, database))
            {
                var result = new List<ProductCharacteristic>();

                using (var reader = database.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        result.Add(ConstructEntity(reader));
                    }
                }

                return result;
            }
        }

        /// <summary>
        ///     Deletes the children.
        /// </summary>
        /// <param name="id">The id.</param>
        public void DeleteChildren(Guid id)
        {
            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeDeleteChildrenCommand(id, db))
            {
                db.ExecuteNonQuery(command);
            }
        }

        private DbCommand InitializeDeleteChildrenCommand(Guid id, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProducCharacteristic_DeleteChildren]");

            db.AddInParameter(command, "ParentId", DbType.Guid, id);

            return command;
        }

        private DbCommand InitializeFindByProductIdCommand(Guid productId, Database database)
        {
            var command = (SqlCommand) database.GetStoredProcCommand("[dbo].[pProductCharacteristic_GetByProductId]");

            var sqlParameter = new SqlParameter("ProductId", SqlDbType.UniqueIdentifier) {Value = productId};
            command.Parameters.Add(sqlParameter);

            return command;
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
        protected override void AddTableRowFields(ProductCharacteristic entity, bool isNew, bool isDirty, bool isDelete,
                                                  DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);
            dr["CharacteristicType"] = entity.ProductFamilyCharacteristicType.ToString();
            dr["CharacteristicID"] = entity.ProductFamilyCharacteristicId;
            dr["ProductID"] = entity.ProductId;
            dr["CharacteristicValue"] = entity.Value;

            if (entity.ParentId.HasValue)
                dr["ParentId"] = entity.ParentId.Value;
            else
                dr["ParentId"] = DBNull.Value;

            if (entity.ChildType.HasValue)
                dr["ChildType"] = (int)entity.ChildType.Value;
            else
                dr["ChildType"] = DBNull.Value;
        }

        /// <summary>
        /// Constructs the product characteristic.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public ProductCharacteristic ConstructProductCharacteristic(IDataReader reader)
        {
            return ConstructEntity(reader);
        }

        /// <summary>
        ///     Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override ProductCharacteristic ConstructEntity(IDataReader reader)
        {
            var entity = base.ConstructEntity(reader);

            entity.ProductFamilyCharacteristicId = reader.GetValue<Guid>("CharacteristicID");
            entity.ProductFamilyCharacteristicType =
                (ProductFamilyCharacteristicType) Enum.Parse(typeof (ProductFamilyCharacteristicType),
                                                             reader.GetValue<string>("CharacteristicType"));
            entity.Name = reader.GetValue<string>("CharacteristicName");
            entity.Description = reader.GetValue<string>("CharacteristicDescription");
            entity.Value = reader.GetValue<string>("CharacteristicValue");
            entity.Group = reader.GetValue<string>("CharacteristicGroup");
            entity.ProductId = reader.GetValue<Guid>("ProductId");
            entity.DataType = (ProductFamilyCharacteristicDataType) reader.GetValue<int>("DataTypeId");
            entity.ParentId = reader.GetValue<Guid?>("ParentId");
            entity.ChildType = (ProductCharacteristicChildType?)reader.GetValue<int>("ChildType");
            entity.SortOrder = reader.GetValue<int>("SortOrder");

            return entity;
        }
    }
}