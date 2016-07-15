using System;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Repository for <see cref="ProductUploadResult" /> instances
    /// </summary>
    public sealed class ProductUploadResultRepository : TrackedDomainEntityRepositoryBase<ProductUploadResult>,
                                                        IProductUploadResultRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductUploadResultRepository" /> class.
        /// </summary>
        public ProductUploadResultRepository()
            : base("ProductUploadResultId", "ProductUploadResult")
        {
        }

        /// <summary>
        ///     Gets the by product upload id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUploadResultSearchResultSet.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ProductUploadResultSearchResultSet GetByProductUploadId(Guid productUploadId)
        {
            var db = DatabaseFactory.CreateDatabase();
	        var set = new ProductUploadResultSearchResultSet();
            using (var command = InitializeFetchByProductUploadIdCommand(db, productUploadId))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    ConstructSearchResultSet(reader, set);
                }
            }
            return set;
        }

        private static DbCommand InitializeFetchByProductUploadIdCommand(Database db, Guid productUploadId)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProductUploadResult_GetByProductUploadId]");
            db.AddInParameter(command, "ProductUploadId", DbType.Guid, productUploadId);
            return command;
        }

        private void ConstructSearchResultSet(IDataReader reader, ProductUploadResultSearchResultSet set)
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

        private static ProductUploadResultSearchResult ConstructSearchResult(IDataReader reader)
        {
            var result = new ProductUploadResultSearchResult
                {
                    Id = reader.GetValue<Guid>("ProductUploadResultId"),
                    EntityType = EntityTypeEnumDto.ProductUpload,
                    Name = reader.GetValue<Guid>("ProductId").ToString(),
                    Title = reader.GetValue<Guid>("ProductId").ToString(),
                    ChangeDate = reader.GetValue<DateTime>("UpdatedOn"),
                    ProductUploadResult = ConstructProductUploadResult(reader)
                };
            return result;
        }

        private static ProductUploadResult ConstructProductUploadResult(IDataReader reader)
        {
            var productId = reader.GetValue<Guid?>("ProductId");    
            return new ProductUploadResult
                {
                    Id = reader.GetValue<Guid>("ProductUploadResultId"),
                    ProductUploadId = reader.GetValue<Guid>("ProductUploadId"),
                    Product = productId.HasValue ? new Product {Id = productId} :null,
                    IsValid = reader.GetValue<bool>("IsValid"),
                    CreatedById = reader.GetValue<Guid>("CreatedBy"),
                    CreatedDateTime = reader.GetValue<DateTime>("CreatedOn"),
                    UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                    UpdatedDateTime = reader.GetValue<DateTime>("UpdatedOn")
                };
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
        protected override void AddTableRowFields(ProductUploadResult entity, bool isNew, bool isDirty, bool isDelete,
                                                  DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);

            dr["IsValid"] = entity.IsValid;
            if (null == entity.Product)
                dr["ProductId"] = DBNull.Value;
            else
            dr["ProductId"] = entity.Product.Id;
            dr["ProductUploadId"] = entity.ProductUploadId;
        }

        /// <summary>
        ///     Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override ProductUploadResult ConstructEntity(IDataReader reader)
        {
            var entity = base.ConstructEntity(reader);

            entity.IsValid = reader.GetValue<bool>("IsValid");
            entity.Product.Id = reader.GetValue<Guid>("ProductId");
            entity.ProductUploadId = reader.GetValue<Guid>("ProductUploadId");

            return entity;
        }
    }
}