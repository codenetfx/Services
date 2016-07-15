using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web.Script.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Handles persistance for <see cref="ProjectStatusMessage"/> objects.
    /// </summary>
    public class ProjectStatusMessageRepository : TrackedDomainEntityRepositoryBase<ProjectStatusMessage>, IProjectStatusMessageRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackedDomainEntityRepositoryBase{TTrackedDomainEntity}" /> class.
        /// </summary>
        public ProjectStatusMessageRepository() : base("ProjectStatusMessageId", "ProjectStatusMessage")
        {
        }
        /// <summary>
        /// Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override ProjectStatusMessage ConstructEntity(System.Data.IDataReader reader)
        {
            var entity = base.ConstructEntity(reader);
            entity.ProjectId = reader.GetValue<Guid>("ProjectId");
            entity.CorrelationId = reader.GetValue<Guid>("CorrelationId");
            entity.OldStatus = (ProjectStatusEnumDto)Enum.Parse(typeof(ProjectStatusEnumDto), reader.GetValue<string>("OldStatus"));
            entity.NewStatus = (ProjectStatusEnumDto)Enum.Parse(typeof(ProjectStatusEnumDto), reader.GetValue<string>("NewStatus"));
            entity.ProjectServiceLineStatuses = reader.GetValue<string>("Data").FromJson<List<ProjectStatusMessageServiceLine>>();
            entity.EventId = reader.GetValue<short>("EventId");
            entity.EventId = reader.GetValue<short>("EventId");
            entity.ExternalProjectId = reader.GetValue<string>("ExternalProjectId");
            entity.ProjectName = reader.GetValue<string>("ProjectName");
            entity.ProjectNumber = reader.GetValue<string>("ProjectNumber");
            entity.WorkOrderBusinessComponentId = reader.GetValue<string>("WorkOrderBusinessComponentId");
            entity.WorkOrderId = reader.GetValue<string>("WorkOrderId");
            entity.OrderNumber = reader.GetValue<string>("OrderNumber");
             return entity;
        }

        /// <summary>
        /// Adds the table row fields.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isNew">if set to <c>true</c> [is new].</param>
        /// <param name="isDirty">if set to <c>true</c> [is dirty].</param>
        /// <param name="isDelete">if set to <c>true</c> [is delete].</param>
        /// <param name="dr">The dr.</param>
        protected override void AddTableRowFields(ProjectStatusMessage entity, bool isNew, bool isDirty, bool isDelete, System.Data.DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);
            dr["ProjectId"] = entity.ProjectId;
            dr["OldStatus"] = entity.OldStatus.ToString();
            dr["NewStatus"] = entity.NewStatus.ToString();
            dr["EventId"] = entity.EventId;
            dr["ExternalProjectId"] = entity.ExternalProjectId;
            dr["ProjectName"] = entity.ProjectName;
            dr["ProjectNumber"] = entity.ProjectNumber;
            dr["OrderNumber"] = entity.OrderNumber;
            dr["WorkOrderId"] = entity.WorkOrderId;
            dr["WorkOrderBusinessComponentId"] = entity.WorkOrderBusinessComponentId;
            dr["CorrelationId"] = entity.CorrelationId == default(Guid) ? System.Diagnostics.Trace.CorrelationManager.ActivityId : entity.CorrelationId;
            dr["Data"] = entity.ProjectServiceLineStatuses.ToJson();
        }

        
        /// <summary>
        /// Gets the next <see cref="ProjectStatusMessage"/>.
        /// </summary>
        /// <returns></returns>
        public ProjectStatusMessage GetNext()
        {
            var database = DatabaseFactory.CreateDatabase();
            var command = InitializeGetNextCommand(database);
            ProjectStatusMessage entity = null;

            using (var reader = database.ExecuteReader(command))
            {
                if (!reader.Read())
                {
                    return null;
                }
                entity = ConstructEntity(reader);
                
            }
            return entity;
        }

        private DbCommand InitializeGetNextCommand(Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[p" + TableName + "_GetNext]");

            return command;
        }
    }
}