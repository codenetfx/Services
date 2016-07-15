using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.OData.Query.SemanticAst;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Repository implementation for Task Templates.
    /// </summary>
    public class TaskTemplateRepository: TrackedDomainEntityRepositoryBase<TaskTemplate>, ITaskTemplateRepository
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkRepository" /> class.
        /// </summary>
        public TaskTemplateRepository()
            : base("ProjectTemplateTaskTemplateId", "ProjectTemplateTaskTemplate")
        {

        }

        /// <summary>
        /// Fetches the links by entity.
        /// </summary>
        /// <param name="projectTemplateId">The project template identifier.</param>
        /// <param name="Flattened">if set to <c>true</c> [flattened].</param>
        /// <returns></returns>
        public IEnumerable<TaskTemplate> FetchByProjectTemplate(Guid projectTemplateId, bool Flattened = false)
        {
            var taskTemplates = ExecuteReaderCommand(db => InitializeFetchByProjectTemplateCommand(db, projectTemplateId), ConstructEntity);

            if(!Flattened)
            {
                AttachSubTasks(taskTemplates);
            }

            return taskTemplates;
        }

        private DbCommand InitializeFetchByProjectTemplateCommand(Database db, Guid projectTemplateId)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pProjectTemplateTaskTemplate_GetByProjectTemplateId]");
            db.AddInParameter(command, "@ProjectTemplateId", DbType.Guid, projectTemplateId);
            return command;
        }

        /// <summary>
        ///     Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        public override void Remove(Guid entityId)
        {
            var entity = FindById(entityId);
            ExecuteNonQueryCommand(database => InitializeSaveCommand(entity, database, SaveEnum.Delete), entity);
        }

        /// <summary>
        /// Fetches the task template by project template.
        /// </summary>
        /// <param name="projectTemplateId">The project identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<TaskTemplate> FetchTaskTemplateByProjectTemplate(Guid projectTemplateId)
        {
            return ExecuteReaderCommand(db => InitializeFetchByProjectTemplateCommand(db, projectTemplateId), ConstructEntity);
        }

        /// <summary>
        /// Updates the a list of TaskTemplates in bulk.
        /// </summary>
        /// <param name="ProjectTemplateId"></param>
        /// <param name="taskTemplates">The task templates.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void UpdateBulk(Guid ProjectTemplateId, IEnumerable<TaskTemplate> taskTemplates)
        {
            ExecuteNonQueryCommand(db =>
            {
                var multiSaveCommand = InitializeMultiSaveCommand(taskTemplates, db, SaveEnum.Update);
                db.AddInParameter(multiSaveCommand, "ProjectTemplateId",DbType.Guid, ProjectTemplateId);
                return multiSaveCommand;
            });
        }

        /// <summary>
        /// Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override TaskTemplate ConstructEntity(IDataReader reader)
        {
            var taskTemplate = base.ConstructEntity(reader);
            taskTemplate.TaskTypeId = reader.GetValue<Guid?>("TaskTypeId").GetValueOrDefault();
            taskTemplate.ProjectTemplateId = reader.GetValue<Guid?>("ProjectTemplateId").GetValueOrDefault();
            taskTemplate.Title = reader.GetValue<string>("Title");
            taskTemplate.CreationOrder = reader.GetValue<int>("Order");
            taskTemplate.TaskNumber = reader.GetValue<int>("TaskNumber");
            taskTemplate.ParentTaskNumber = reader.GetValue<int?>("ParentTaskNumber");
	        taskTemplate.IsProjectHandlerRestricted = reader.GetValue<bool>("IsProjectHandlerRestricted");
	        taskTemplate.ShouldTriggerBilling = reader.GetValue<bool>("ShouldTriggerBilling");
			taskTemplate.Description = reader.GetValue<string>("Description");

            var predecessorTaskNumbers = reader.GetValue<string>("PredecessorTaskNumbers");
            if(!string.IsNullOrWhiteSpace( predecessorTaskNumbers))
            {
                var predecessorTaskNumbersList = predecessorTaskNumbers.Split(new char[]{','}).ToList();
                    taskTemplate.Predecessors.AddRange(predecessorTaskNumbersList
                        .ConvertAll<TaskPredecessor>(x=> new TaskPredecessor() { 
                            TaskNumber = Convert.ToInt32(x)
                }));;
            }
     
            return taskTemplate;
        }

        /// <summary>
        /// Adds the table row fields.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isNew">if set to <c>true</c> [is new].</param>
        /// <param name="isDirty">if set to <c>true</c> [is dirty].</param>
        /// <param name="isDelete">if set to <c>true</c> [is delete].</param>
        /// <param name="dr">The dr.</param>
        protected override void AddTableRowFields(TaskTemplate entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
        {
            if (entity.Id.GetValueOrDefault() == Guid.Empty)
                entity.Id = Guid.NewGuid();

            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);
            dr["TaskTypeId"] = entity.TaskTypeId.HasValue? entity.TaskTypeId : (object) DBNull.Value;
            dr["ProjectTemplateId"] = entity.ProjectTemplateId;
            dr["Title"] = entity.Title;
            dr["Order"] = entity.CreationOrder;
            dr["TaskNumber"] = entity.TaskNumber;
			dr["IsProjectHandlerRestricted"] = entity.IsProjectHandlerRestricted;
			dr["ShouldTriggerBilling"] = entity.ShouldTriggerBilling;
			dr["Description"] = entity.Description;
            
            if (entity.ParentTaskNumber.HasValue)
            {
                dr["ParentTaskNumber"] = entity.ParentTaskNumber;
            }
            else
            {
                dr["ParentTaskNumber"] = DBNull.Value;
            }
            
            if (null != entity.Predecessors)
            {
                dr["PredecessorTaskNumbers"] = string.Join(",", entity.Predecessors.Select(y => y.TaskNumber));
            }
            else
            {
                dr["PredecessorTaskNumbers"] = DBNull.Value;
            }
        }
        
        private static void AttachSubTasks(List<TaskTemplate> tasks)
        {
            foreach (var task in tasks)
            {
                if (task.ParentTaskNumber != null)
                {
                    var parentTask = tasks.FirstOrDefault(x => x.TaskNumber == task.ParentTaskNumber);
                    if (parentTask != null)
                    {
                        parentTask.SubTasks.Add(task);
                    }
                }
            }
        }

    
    }
}
