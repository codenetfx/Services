using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Aria.Common.Authorization;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Repository for <see cref="Product" /> instances
    /// </summary>
    public sealed class ProductRepository : TrackedDomainEntityRepositoryBase<Product>, IProductRepository
    {
        private readonly IPrincipalResolver _principalResolver;
        private readonly IProductCharacteristicRepository _productCharacteristicRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductRepository" /> class.
        /// </summary>
        /// <param name="productCharacteristicRepository">The product characteristic repository.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public ProductRepository(IProductCharacteristicRepository productCharacteristicRepository,
            IPrincipalResolver principalResolver)
            : base("ProductId", "Product")
        {
            _productCharacteristicRepository = productCharacteristicRepository;
            _principalResolver = principalResolver;
        }

        /// <summary>
        ///     Gets all <see cref="Product" />s that match the given product family id.
        /// </summary>
        /// <param name="productFamilyId">The id.</param>
        /// <returns></returns>
        public IEnumerable<Product> GetByProductFamilyId(Guid productFamilyId)
        {
            var result =
                ExecuteReaderCommand(database => InitializedGetByProductFamilyCommand(productFamilyId, database),
                    ConstructEntity);

            return result;
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
        ///     Updates the status.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <param name="status">The status.</param>
        /// <param name="submittedDateTime"></param>
        public void UpdateStatus(Guid productId, ProductStatus status, DateTime? submittedDateTime)
        {
            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeUpdateStatusCommand(productId, status, submittedDateTime, db))
            {
                db.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        ///     Gets the status.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        public ProductStatus GetStatus(Guid productId)
        {
            var result =
                ExecuteReaderCommand(database => InitializeFindCommand(productId, database),
                    ConstructProductOnly);

            return result.First().Status;
        }

        /// <summary>
        ///     DOES A NON-TRANSACTIONAL READ. Gets the product for status.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <remarks>
        ///     This is intended to get product <b>WHILE</b> it is being updated or inserted and hence an uncommitted read is
        ///     desired.
        ///     <b>DO NOT USE </b> for transactional sensitive operations.
        /// </remarks>
        /// <returns></returns>
        public Product GetProductForStatusOnly(Guid productId)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbCommand command = InitializeFindHeaderCommand(productId, database))
            {
                Product result;
                using (IDataReader reader = database.ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        result = ConstructProductOnly(reader);
                        result.Characteristics = new List<ProductCharacteristic>();
                    }
                    else
                    {
                        return null;
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        result.Characteristics.Add(
                            _productCharacteristicRepository.ConstructProductCharacteristic(reader));
                    }
                }
                return result;
            }
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
        protected override void AddTableRowFields(Product entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);

            dr["ProductName"] = entity.Name;
            dr["ProductDescription"] = entity.Description;
            dr["FamilyId"] = entity.ProductFamilyId;
            dr["ProductStatusId"] = (byte) entity.Status;

            if (entity.SubmittedDateTime == null)
                dr["SubmittedDateTime"] = DBNull.Value;
            else
                dr["SubmittedDateTime"] = entity.SubmittedDateTime;
        }


        /// <summary>
        ///     Initializes the remove command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        private DbCommand InitializeRemoveCommand(Guid entity, Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pProduct_Remove]");

            db.AddInParameter(command, "ProductId", DbType.Guid, entity);

            return command;
        }

        private DbCommand InitializeFindHeaderCommand(Guid entityId, Database db)
        {
            var command = (SqlCommand) db.GetStoredProcCommand("[dbo].[pProduct_GetForStatus]");

            var sqlParameter = new SqlParameter(IdFieldName, SqlDbType.UniqueIdentifier) {Value = entityId};
            command.Parameters.Add(sqlParameter);

            return command;
        }

        /// <summary>
        ///     Initializes the remove command.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <param name="status">The status.</param>
        /// <param name="submittedDateTime">The submitted date time.</param>
        /// <param name="db">The db.</param>
        /// <returns>DbCommand.</returns>
        private DbCommand InitializeUpdateStatusCommand(Guid productId, ProductStatus status, DateTime? submittedDateTime, Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pProduct_Update_Status]");

            db.AddInParameter(command, "ProductId", DbType.Guid, productId);
            db.AddInParameter(command, "ProductStatusId", DbType.Byte, (byte) status);
            db.AddInParameter(command, "SubmittedDateTime", DbType.DateTime2, submittedDateTime);

            return command;
        }

        /// <summary>
        ///     Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override Product ConstructEntity(IDataReader reader)
        {
            var entity = ConstructProductOnly(reader);

            if (null == Transaction.Current)
            {
                throw new TransactionException(
                    "Products must be retrieved within a transaction scope due to multiple dependencies");
            }

            var characteristics = new List<ProductCharacteristic>();

            characteristics.AddRange(
                _productCharacteristicRepository.FindByProductId(entity.Id)
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Name));

            entity.Characteristics = characteristics;

            return entity;
        }

        private Product ConstructProductOnly(IDataReader reader)
        {
            var entity = base.ConstructEntity(reader);

            entity.Name = reader.GetValue<string>("ProductName");
            entity.Description = reader.GetValue<string>("ProductDescription");
            entity.ProductFamilyId = reader.GetValue<Guid>("FamilyId");
            entity.IsDeleted = reader.GetValue<bool>("IsDeleted");
            entity.Status = (ProductStatus) reader.GetValue<byte>("ProductStatusId");
            entity.SubmittedDateTime = reader.GetValue<DateTime?>("SubmittedDateTime");
            entity.ContainerId = reader.GetValue<Guid?>("ContainerId");
            entity.CompanyId = reader.GetValue<Guid>("CompanyId");
            return entity;
        }

        /// <summary>
        ///     Initializes the find command.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        private DbCommand InitializedGetByProductFamilyCommand(Guid entityId, Database db)
        {
            var command = (SqlCommand) db.GetStoredProcCommand("[dbo].[pProduct_GetByFamilyId]");

            var sqlParameter = new SqlParameter("FamilyId", SqlDbType.UniqueIdentifier) {Value = entityId};
            command.Parameters.Add(sqlParameter);

            sqlParameter = new SqlParameter("@UserClaims", SqlDbType.Structured)
            {
                Value = CreateClaimsParameterTable(_principalResolver.Current.Claims)
            };

            command.Parameters.Add(sqlParameter);
            return command;
        }
    }
}