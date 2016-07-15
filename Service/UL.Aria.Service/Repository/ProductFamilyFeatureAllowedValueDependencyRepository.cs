using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Implements persistance of <see cref="ProductFamilyFeatureAllowedValueDependency" />
    /// </summary>
    public class ProductFamilyFeatureAllowedValueDependencyRepository : TrackedDomainEntityRepositoryBase<ProductFamilyFeatureAllowedValueDependency>, IProductFamilyFeatureAllowedValueDependencyRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TrackedDomainEntityRepositoryBase{TTrackedDomainEntity}" /> class.
        /// </summary>
        public ProductFamilyFeatureAllowedValueDependencyRepository()
            : base("FamilyAllowedFeatureValueDependencyId", "FamilyAllowedFeatureValueDependency")
        {
        }

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public override int Update(ProductFamilyFeatureAllowedValueDependency entity)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Finds <see cref="ProductFamilyFeatureAllowedValueDependency" /> by family feature allowed value id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureAllowedValueDependency> FindByFamilyAllowedFeatureId(Guid id)
        {
            return ExecuteReaderCommand(database => InitializeFamilyAllowedFeatureIdCommand(id, database, "@FamilyAllowedFeatureID"), ConstructEntity);
        }

        /// <summary>
        ///     Finds dependencies by parent family allowed feature value id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyFeatureAllowedValueDependency> FindByParentFamilyAllowedFeatureValueId(Guid id)
        {
            return ExecuteReaderCommand(database => InitializeFamilyAllowedFeatureIdCommand(id, database, "@ParentFamilyAllowedFeatureValueID"), ConstructEntity);
        }

        /// <summary>
        ///     Adds the table row fields.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isNew">if set to <c>true</c> [is new].</param>
        /// <param name="isDirty">if set to <c>true</c> [is dirty].</param>
        /// <param name="isDelete">if set to <c>true</c> [is delete].</param>
        /// <param name="dr">The dr.</param>
        protected override void AddTableRowFields(ProductFamilyFeatureAllowedValueDependency entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);
            dr["ParentFamilyAllowedFeatureValueID"] = entity.ParentProductFamilyFeatureAllowedValueId;
            dr["FamilyAllowedFeatureValueId"] = entity.ChildProductFamilyFeatureAllowedValueId;
        }

        /// <summary>
        ///     Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override ProductFamilyFeatureAllowedValueDependency ConstructEntity(IDataReader reader)
        {
            ProductFamilyFeatureAllowedValueDependency entity = base.ConstructEntity(reader);
            entity.ParentProductFamilyFeatureAllowedValueId = reader.GetValue<Guid>("ParentFamilyAllowedFeatureValueID");
            entity.ChildProductFamilyFeatureAllowedValueId = reader.GetValue<Guid>("FamilyAllowedFeatureValueId");

            return entity;
        }

        private DbCommand InitializeFamilyAllowedFeatureIdCommand(Guid id, Database db, string parameterName)
        {
            var command = (SqlCommand) db.GetStoredProcCommand("[dbo].[pFamilyAllowedFeatureValueDependency_Get]");

            var sqlParameter = new SqlParameter(parameterName, SqlDbType.UniqueIdentifier) {Value = id};
            command.Parameters.Add(sqlParameter);

            return command;
        }
    }
}