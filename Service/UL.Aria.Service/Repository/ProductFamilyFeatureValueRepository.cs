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
    /// Implements persistance of <see cref="ProductFamilyFeatureValue"/>
    /// </summary>
    public class ProductFamilyFeatureValueRepository:TrackedDomainEntityRepositoryBase<ProductFamilyFeatureValue>,IProductFamilyFeatureValueRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TrackedDomainEntityRepositoryBase{TTrackedDomainEntity}" /> class.
        /// </summary>
        public ProductFamilyFeatureValueRepository() : base("FeatureValueId", "FeatureValue")
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
        protected override void AddTableRowFields(ProductFamilyFeatureValue entity, bool isNew, bool isDirty, bool isDelete, System.Data.DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);
            dr["FeatureId"] = entity.FeatureId;
            dr["FeatureValue"] = entity.Value;
            dr["FeatureValueMax"] = entity.Maximum;
            dr["FeatureValueMin"] = entity.Minimum;
            dr["Xtype"] = entity.Xtype;
            if(null != entity.UnitOfMeasure && entity.UnitOfMeasure.Id.HasValue )
                dr["UnitOfMeasureId"] = entity.UnitOfMeasure.Id.Value;
            else
                dr["UnitOfMeasureId"] = DBNull.Value;
        }

        /// <summary>
        /// Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override ProductFamilyFeatureValue ConstructEntity(System.Data.IDataReader reader)
        {
            var entity =  base.ConstructEntity(reader);
            entity.Value = reader.GetValue<string>("FeatureValue");
            entity.Maximum = reader.GetValue<string>("FeatureValueMax");
            entity.Minimum = reader.GetValue<string>("FeatureValueMin");
            entity.Xtype = reader.GetValue<byte>("Xtype");
            entity.FeatureId = reader.GetValue<Guid>("FeatureId");
            var unitofMeasure = reader.GetValue<Guid?>("UnitOfMeasureId");
            
            if (unitofMeasure.HasValue)
                entity.UnitOfMeasure = new UnitOfMeasure() { Id = unitofMeasure.Value};

            return entity;
        }

        /// <summary>
        /// Finds the values by feature id.
        /// </summary>
        /// <param name="featureId">The feature id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureValue> FindByFeatureId(Guid featureId)
        {
            return ExecuteReaderCommand(database => InitializeFindByFeatureIdCommand(featureId, database), ConstructEntity);
        }

        private DbCommand InitializeFindByFeatureIdCommand(Guid featureId, Database db)
        {
            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[pFeatureValue_Get]");

            var sqlParameter = new SqlParameter("@FeatureId", SqlDbType.UniqueIdentifier) { Value = featureId };
            command.Parameters.Add(sqlParameter);

            return command;
        }
    }
}