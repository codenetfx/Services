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
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Provides a repository implementation for LinkAssociations.
    /// </summary>
    public class LinkAssociationRepository : TrackedDomainEntityRepositoryBase<LinkAssociation>, ILinkAssociationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkAssociationRepository" /> class.
        /// </summary>
        public LinkAssociationRepository() : base("", "LinkAssociation") { }

        /// <summary>
        /// Updates the bulk.
        /// </summary>
        /// <param name="linkAssociation">The link association.</param>
        /// <param name="parentId">The parent identifier.</param>
        public void UpdateBulk(IEnumerable<LinkAssociation> linkAssociation, Guid parentId)
        {
            ExecuteNonQueryCommand(db => InitializeLinkAssociationBulkUpdateCommand(linkAssociation, db, parentId));
        }


        /// <summary>
        /// Finds the by parent.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        public IEnumerable<LinkAssociation> FindByParent(Guid parentId)
        {
            return ExecuteReaderCommand(db => InitializeLinkAssociationGetByParentCommand(db, parentId), ConstructEntity);
        }


        /// <summary>
        /// Initializes the link association get by parent command.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        protected DbCommand InitializeLinkAssociationGetByParentCommand(Database db, Guid parentId)
        {
            var cmd = db.GetStoredProcCommand("[dbo].[pLinkAssociation_GetByParent]");
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
        protected DbCommand InitializeLinkAssociationBulkUpdateCommand(IEnumerable<LinkAssociation> entities, Database db, Guid parentId)
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
        protected override void AddTableRowFields(LinkAssociation entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
        {
            dr["ParentId"] = entity.ParentId;
            dr["LinkId"] = entity.LinkId;
            dr["Order"] = entity.Order;
            dr["CreatedBy"] = entity.CreatedById;
            dr["UpdatedBy"] = entity.UpdatedById;
            dr["CreatedDT"] = entity.CreatedDateTime;
            dr["UpdatedDT"] = entity.UpdatedDateTime;
        }

        /// <summary>
        /// Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override LinkAssociation ConstructEntity(IDataReader reader)
        {
            return new LinkAssociation()
            {
                ParentId = reader.GetValue<Guid>("ParentId"),
                LinkId = reader.GetValue<Guid>("LinkId"),
                Order = reader.GetValue<int>("Order"),
                CreatedById = reader.GetValue<Guid>("CreatedBy"),
                UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                CreatedDateTime = reader.GetValue<DateTime>("CreatedDT"),
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedDT")
            };
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public override void Add(LinkAssociation entity)
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
        public override IList<LinkAssociation> FindAll()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public override LinkAssociation FindById(Guid entityId)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public override int Update(LinkAssociation entity)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public override Guid Create(LinkAssociation entity)
        {
            throw new NotSupportedException();
        }



    }
}
