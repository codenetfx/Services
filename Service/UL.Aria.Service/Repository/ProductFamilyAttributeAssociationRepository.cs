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
    /// Implements operations for creating product family feature associations.
    /// </summary>
    public class ProductFamilyAttributeAssociationRepository : ProductFamilyCharacteristicAssociationRepositoryBase<ProductFamilyAttributeAssociation>, IProductFamilyAssociationRepository<ProductFamilyAttributeAssociation>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TrackedDomainEntityRepositoryBase{TTrackedDomainEntity}" /> class.
        /// </summary>
        public ProductFamilyAttributeAssociationRepository()
            : base("FamilyAllowedAttributeId", "FamilyAllowedAttribute", "AttributeOptionId")
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
        protected override void AddTableRowFields(ProductFamilyAttributeAssociation entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);
            dr["FamilyId"] = entity.ProductFamilyId;
            dr["AttributeId"] = entity.CharacteristicId;
            dr["IsDisabled"] = entity.IsDisabled;
        }

        /// <summary>
        /// Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override ProductFamilyAttributeAssociation ConstructEntity(IDataReader reader)
        {
            var entity = base.ConstructEntity(reader);

            entity.ProductFamilyId = reader.GetValue<Guid>("FamilyId");
            entity.CharacteristicId = reader.GetValue<Guid>("AttributeId");
            entity.IsDisabled = reader.GetValue<bool>("IsDisabled");
            return entity;
        }

        /// <summary>
        /// Gets the by family id.
        /// </summary>
        /// <param name="familyId">The family id.</param>
        /// <returns></returns>
        public IEnumerable<ProductFamilyAttributeAssociation> GetByFamilyId(Guid familyId)
        {
            return ExecuteReaderCommand(database => InitializeFindByFamilyCommand(familyId, database));
        }

        /// <summary>
        /// Initializes the find by family command.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        protected virtual DbCommand InitializeFindByFamilyCommand(Guid entityId, Database db)
        {
            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[p" + TableName + "_Get]");

            var sqlParameter = new SqlParameter("@FamilyID", SqlDbType.UniqueIdentifier) { Value = entityId };
            command.Parameters.Add(sqlParameter);

            return command;
        }
    }
}