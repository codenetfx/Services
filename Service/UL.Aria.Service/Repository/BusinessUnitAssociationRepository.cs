using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Domain;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Provides a repository implementation for LinkAssociations.
    /// </summary>
    public class BusinessUnitAssociationRepository : TrackedDomainEntityRepositoryBase<BusinessUnitAssociation>, IBusinessUnitAssociationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessUnitAssociationRepository"/> class.
        /// </summary>
        public BusinessUnitAssociationRepository() : base("", "BusinessUnitAssociation") { }

        /// <summary>
        /// Updates the bulk.
        /// </summary>
        /// <param name="businessUnitAssociations">The business unit associations.</param>
        /// <param name="parentId">The parent identifier.</param>
        public void UpdateBulk(IEnumerable<BusinessUnitAssociation> businessUnitAssociations, Guid parentId)
        {
            ExecuteNonQueryCommand(db => InitializeBusinessUnitAssociationBulkUpdateCommand(businessUnitAssociations, db, parentId));
        }


        /// <summary>
        /// Finds the by parent.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<BusinessUnitAssociation> FindByParent(Guid parentId)
        {
            return ExecuteReaderCommand(db => InitializeBusinessUnitAssociationGetByParentCommand(db, parentId), ConstructEntity);
        }


        /// <summary>
        /// Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override BusinessUnitAssociation ConstructEntity(IDataReader reader)
        {
            return new BusinessUnitAssociation()
            {
                ParentId = reader.GetValue<Guid>("ParentId"),
                BusinessUnitId = reader.GetValue<Guid>("BusinessUnitId"),            
                CreatedById = reader.GetValue<Guid>("CreatedBy"),
                UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                CreatedDateTime = reader.GetValue<DateTime>("CreatedDT"),
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedDT")
            };
        }

        /// <summary>
        /// Initializes the business unit association get by parent command.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        protected DbCommand InitializeBusinessUnitAssociationGetByParentCommand(Database db, Guid parentId)
        {
            var cmd = db.GetStoredProcCommand("[dbo].[pBusinessUnitAssociation_GetByParent]");
            var parentIdParam = cmd.CreateParameter();
            parentIdParam.ParameterName = "ParentId";
            parentIdParam.DbType = DbType.Guid;
            parentIdParam.Value = parentId;
            cmd.Parameters.Add(parentIdParam);
            return cmd;
        }

        /// <summary>
        /// Initializes the link association bulk update command.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="db">The database.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        protected DbCommand InitializeBusinessUnitAssociationBulkUpdateCommand(IEnumerable<BusinessUnitAssociation> entities, Database db, Guid parentId)
        {
            var cmd = InitializeMultiSaveCommand(entities, db, SaveEnum.Update);
            var parentIdParam = cmd.CreateParameter();
            parentIdParam.ParameterName = "ParentId";
            parentIdParam.DbType = DbType.Guid;
            parentIdParam.Value = parentId;
            cmd.Parameters.Add(parentIdParam);
            return cmd;
        }

        /// <summary>
        /// Adds the table row fields.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isNew">if set to <c>true</c> [is new].</param>
        /// <param name="isDirty">if set to <c>true</c> [is dirty].</param>
        /// <param name="isDelete">if set to <c>true</c> [is delete].</param>
        /// <param name="dr">The dr.</param>
        protected override void AddTableRowFields(BusinessUnitAssociation entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
        {
            dr["ParentId"] = entity.ParentId;
            dr["BusinessUnitId"] = entity.BusinessUnitId;
            dr["CreatedBy"] = entity.CreatedById;
            dr["UpdatedBy"] = entity.UpdatedById;
            dr["CreatedDT"] = entity.CreatedDateTime;
            dr["UpdatedDT"] = entity.UpdatedDateTime;
        }


        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public override void Add(BusinessUnitAssociation entity)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the specified entity identifier.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public override void Remove(Guid entityId)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public override IList<BusinessUnitAssociation> FindAll()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public override BusinessUnitAssociation FindById(Guid entityId)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public override int Update(BusinessUnitAssociation entity)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public override Guid Create(BusinessUnitAssociation entity)
        {
            throw new NotSupportedException();
        }



      
    }
}

