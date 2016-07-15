using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;
namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Implements persistance of <see cref="ProductFamilyFeatureAllowedValue"/>
    /// </summary>
    public class ProductFamilyFeatureAllowedValueRepository : TrackedDomainEntityRepositoryBase<ProductFamilyFeatureAllowedValue>, IProductFamilyFeatureAllowedValueRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TrackedDomainEntityRepositoryBase{TTrackedDomainEntity}" /> class.
        /// </summary>
        public ProductFamilyFeatureAllowedValueRepository() : base("FamilyAllowedFeatureValueId", "FamilyAllowedFeatureValue")
        {
        }

        /// <summary>
        /// Adds the table row fields.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isNew">if set to <c>true</c> [is new].</param>
        /// <param name="isDirty">if set to <c>true</c> [is dirty].</param>
        /// <param name="isDelete">if set to <c>true</c> [is delete].</param>
        /// <param name="dr">The dr.</param>
        protected override void AddTableRowFields(ProductFamilyFeatureAllowedValue entity, bool isNew, bool isDirty, bool isDelete, System.Data.DataRow dr)
        {
            base.AddTableRowFields(entity,isNew,isDirty, isDelete, dr);
            dr["FamilyId"] = entity.FamilyId;
            dr["FeatureValueId"] = entity.FeatureValue.Id.Value;
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public override int Update(ProductFamilyFeatureAllowedValue entity)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        public override void Remove(Guid entityId)
        {
            var entity = FindById(entityId);
            ExecuteNonQueryCommand(database => InitializeSaveCommand(entity, database, SaveEnum.Delete), entity);
        }
        /// <summary>
        /// Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override ProductFamilyFeatureAllowedValue ConstructEntity(System.Data.IDataReader reader)
        {
            var entity =  base.ConstructEntity(reader);
            entity.FamilyId = reader.GetValue<Guid>("FamilyId");
            entity.FeatureValue = new ProductFamilyFeatureValue();
            entity.FeatureValue.Id = reader.GetValue<Guid>("FeatureValueId");
            entity.FeatureValue.Value = reader.GetValue<string>("FeatureValue");
            entity.FeatureValue.Maximum = reader.GetValue<string>("FeatureValueMax");
            entity.FeatureValue.Minimum = reader.GetValue<string>("FeatureValueMin");
            entity.FeatureValue.Xtype = reader.GetValue<byte>("Xtype");
            entity.FeatureValue.FeatureId = reader.GetValue<Guid>("FeatureId");
            entity.FeatureValue.CreatedById = reader.GetValue<Guid>("fvCreatedBy");
            entity.FeatureValue.UpdatedById = reader.GetValue<Guid>("fvUpdatedBy");
            entity.FeatureValue.CreatedDateTime = reader.GetValue<DateTime>("fvCreatedDT");
            entity.FeatureValue.UpdatedDateTime = reader.GetValue<DateTime>("fvUpdatedDT");
            var unitofMeasure = reader.GetValue<Guid?>("UnitOfMeasureId");

            if (unitofMeasure.HasValue)
                entity.FeatureValue.UnitOfMeasure = new UnitOfMeasure() { Id = unitofMeasure.Value };
            return entity;

        }

        /// <summary>
        /// Gets a collection of <see cref="ProductFamilyFeatureAllowedValue"/> by feature id.
        /// </summary>
        /// <param name="featureId">The family id.</param>
        /// <param name="familyId"></param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureAllowedValue> FindByFeatureId(Guid featureId, Guid familyId)
        {
            return ExecuteReaderCommand(database => InitializeFindByFeatureIdCommand(featureId, familyId, database), ConstructEntity);
        }

        private DbCommand InitializeFindByFeatureIdCommand(Guid featureId, Guid familyId, Database db)
        {
            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[pFamilyAllowedFeatureValue_Get]");

            var sqlParameter = new SqlParameter("@FeatureId", SqlDbType.UniqueIdentifier) { Value = featureId };
            command.Parameters.Add(sqlParameter);
            sqlParameter = new SqlParameter("@FamilyId", SqlDbType.UniqueIdentifier) { Value = familyId };
            command.Parameters.Add(sqlParameter);

            return command;
        }

        /// <summary>
        /// Gets a collection of <see cref="ProductFamilyFeatureAllowedValue"/> by feature id.
        /// </summary>
        /// <param name="familyId"></param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureAllowedValue> FindByFamilyId(Guid familyId)
        {
            return ExecuteReaderCommand(database => InitializeFindByFamilyCommand(familyId, database), ConstructEntity);
        }

        private DbCommand InitializeFindByFamilyCommand(Guid familyId, Database db)
        {
            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[pFamilyAllowedFeatureValue_Get]");
            
            var sqlParameter = new SqlParameter("@FamilyId", SqlDbType.UniqueIdentifier) { Value = familyId };
            command.Parameters.Add(sqlParameter);

            return command;
        }
    }
}