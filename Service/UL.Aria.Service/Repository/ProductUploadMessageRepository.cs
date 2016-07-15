using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Class ProductUploadMessageRepository
    /// </summary>
    public sealed class ProductUploadMessageRepository : TrackedDomainEntityRepositoryBase<ProductUploadMessage>,
                                                         IProductUploadMessageRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductUploadResultRepository" /> class.
        /// </summary>
        public ProductUploadMessageRepository()
            : base("ProductUploadMessageId", "ProductUploadMessage")
        {
        }

        /// <summary>
        ///     Gets the by product upload result id.
        /// </summary>
        /// <param name="productUploadResultId">The product upload result id.</param>
        /// <returns>IEnumerable{ProductUploadMessage}.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<ProductUploadMessage> GetByProductUploadResultId(Guid productUploadResultId)
        {
            return GetByCommand(InitializeGetByProductUploadResultIdCommand, productUploadResultId);
        }

        private static ProductUploadMessage ConstructProductUploadMessage(IDataReader reader)
        {
            return new ProductUploadMessage
                {
                    Id = reader.GetValue<Guid>("ProductUploadMessageId"),
                    ProductUploadResultId = reader.GetValue<Guid>("ProductUploadResultId"),
                    ProductFamilyCharacteristicId = reader.GetValue<Guid>("ProductFamilyCharacteristicId"),
                    Title = reader.GetValue<string>("Title"),
                    Detail = reader.GetValue<string>("Detail"),
                    MessageType = (ProductUploadMessageTypeEnumDto) reader.GetValue<Int16>("MessageType"),
                    CreatedById = reader.GetValue<Guid>("CreatedBy"),
                    CreatedDateTime = reader.GetValue<DateTime>("CreatedOn"),
                    UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                    UpdatedDateTime = reader.GetValue<DateTime>("UpdatedOn"),
                };
        }

        private static IEnumerable<ProductUploadMessage> GetByCommand<T>(Func<T, Database, DbCommand> initializeCommand,
                                                                         T id)
        {
            var db = DatabaseFactory.CreateDatabase();
            var results = new List<ProductUploadMessage>();

            using (var command = initializeCommand(id, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        results.Add(ConstructProductUploadMessage(reader));
                    }
                }
            }
            return results;
        }

        private static DbCommand InitializeGetByProductUploadResultIdCommand(Guid productUploadResultId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProductUploadMessage_GetByProductUploadResultId]");
            db.AddInParameter(command, "ProductUploadResultId", DbType.Guid, productUploadResultId);
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
        protected override void AddTableRowFields(ProductUploadMessage entity, bool isNew, bool isDirty, bool isDelete,
                                                  DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);

            dr["MessageType"] = entity.MessageType;
            dr["ProductFamilyCharacteristicId"] = entity.ProductFamilyCharacteristicId.HasValue ? (object) entity.ProductFamilyCharacteristicId : DBNull.Value;
            dr["ProductUploadResultId"] = entity.ProductUploadResultId;
            dr["Title"] = entity.Title;
            dr["Detail"] = entity.Detail;
        }

        /// <summary>
        ///     Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override ProductUploadMessage ConstructEntity(IDataReader reader)
        {
            var entity = base.ConstructEntity(reader);

            entity.MessageType = (ProductUploadMessageTypeEnumDto) reader.GetValue<Int16>("MessageType");
            entity.ProductFamilyCharacteristicId = reader.GetValue<Guid>("ProductFamilyCharacteristicId");
            entity.ProductUploadResultId = reader.GetValue<Guid>("ProductUploadResultId");
            entity.Title = reader.GetValue<string>("Title");
            entity.Detail = reader.GetValue<string>("Detail");

            return entity;
        }
    }
}