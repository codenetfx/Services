using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Class TaskRepository.
    /// </summary>
    public class TaskRepository : TrackedDomainEntityRepositoryBase<Task>, ITaskRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRepository" /> class.
        /// </summary>
        public TaskRepository()
            : base("TaskId", "Task")
        {
        }


        /// <summary>
        /// Gets the count of tasks for the pr.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        /// <exception cref="UL.Enterprise.Foundation.Data.DatabaseItemNotFoundException"></exception>
        public List<Task> FetchByProject(Guid projectId)
        {
            return ExecuteReaderCommand(
                database =>
                    InitializeFindByPrimarySearchEntityIdCommand(projectId, false, database),
                ConstructEntity);
        }


        /// <summary>
        /// Fetches the predecessors by project.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        public List<TaskPredecessor> FetchPredecessorsByProject(Guid projectId)
        {
            return ExecuteReaderCommand(db =>
           {
               var cmd = db.GetStoredProcCommand("[dbo].[pTask_GetPredecessorsByProject]");
               db.AddInParameter(cmd, "@PrimarySearchEntityId", DbType.Guid, projectId);
               db.AddInParameter(cmd, "@IncludeDeleted", DbType.Boolean, false);
               return cmd;
           }, ConstructTaskPredecessor);
        }

        private static TaskPredecessor ConstructTaskPredecessor(IDataReader reader)
        {
            return new TaskPredecessor
            {
                TaskId = reader.GetValue<Guid>("PredecessorTaskId"),
                Title = reader.GetValue<string>("Title"),
                TaskNumber = reader.GetValue<int>("TaskNumber"),
                SuccessorId = reader.GetValue<Guid>("SuccessorTaskId"),
				Status = (TaskStatusEnumDto)reader.GetValue<short>("TaskStatusId"),
            };
        }


        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns>Task.</returns>
        /// <exception cref="UL.Enterprise.Foundation.Data.DatabaseItemNotFoundException"></exception>
        public new Task FindById(Guid entityId)
        {
            var result = ExecuteReaderCommandTasks(database => InitializeFindCommand(entityId, database));

            if (result.Count == 0)
                throw new DatabaseItemNotFoundException();

            var taskFound = result.First();
            var tasks = ExecuteReaderCommand(
                database =>
                    InitializeFindByPrimarySearchEntityIdCommand(taskFound.PrimarySearchEntityId, taskFound.IsDeleted, database),
                ConstructEntity);
            AttachSubTasks(tasks);
            var matchedTask = tasks.FirstOrDefault(x => x.Id == taskFound.Id);
            // ReSharper disable once PossibleNullReferenceException
            taskFound.SubTasks = matchedTask.SubTasks;

            return taskFound;
        }

        /// <summary>
        /// Gets the count of tasks for the pr.
        /// </summary>
        /// <param name="primarySearchEntityId">The entity identifier.</param>
        /// <returns></returns>
        /// <exception cref="UL.Enterprise.Foundation.Data.DatabaseItemNotFoundException"></exception>
        public int FetchCountByPrimarySearchEntityId(Guid primarySearchEntityId)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbCommand command = InitializeCountByPrimarySearchEntityIdCommand(primarySearchEntityId, false, database))
            {
                using (IDataReader reader = database.ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        return reader.GetValue<int>("TaskCount");
                    }
                }
                return 0;
            }

        }

        /// <summary>
        /// Finds the by identifier task only.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns>Task.</returns>
        /// <exception cref="UL.Enterprise.Foundation.Data.DatabaseItemNotFoundException"></exception>
        public Task FindByIdTaskOnly(Guid entityId)
        {
            var result = ExecuteReaderCommand(database => InitializeFindByIdCommandTaskOnly(entityId, database), ConstructEntity);

            if (result.Count == 0)
                throw new DatabaseItemNotFoundException();

            return result.First();
        }

        /// <summary>
        /// Finds the ids by primary search entity identifier.
        /// </summary>
        /// <param name="primarySearchEntityId">The primary search entity identifier.</param>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <returns>List&lt;Lookup&gt;.</returns>
        public List<Lookup> FindIdsByPrimarySearchEntityId(Guid primarySearchEntityId, bool includeDeleted = false)
        {
            return
                ExecuteReaderCommand(
                    database => InitializeFindIdsByPrimarySearchEntityIdCommand(primarySearchEntityId, includeDeleted, database),
                    ConstructIds);
        }

        /// <summary>
        /// Finds the by primary search entity identifier.
        /// </summary>
        /// <param name="primarySearchEntityId">The primary search entity identifier.</param>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <param name="flatList">if set to <c>true</c> [return flat list of tasks]</param>
        /// <returns>List&lt;Task&gt;.</returns>
        public List<Task> FindByPrimarySearchEntityId(Guid primarySearchEntityId, bool includeDeleted = false, bool flatList = false)
        {
            var tasks = ExecuteReaderCommand(
                database =>
                    InitializeFindWithHasCommentsByPrimarySearchEntityIdCommand(primarySearchEntityId, includeDeleted, database),
                ConstructEntity);
            if (flatList) return tasks;
            AttachSubTasks(tasks);
            tasks.RemoveAll(x => x.ParentId != Guid.Empty);
            return tasks;
        }

        /// <summary>
        /// Finds the by multiple primary search entity ids.
        /// </summary>
        /// <param name="primarySearchEntityIds">The primary search entity ids.</param>
        /// <returns>List&lt;Task&gt;.</returns>
        public List<Task> FindByMultiplePrimarySearchEntityIds(IList<Guid> primarySearchEntityIds)
        {
            var tasks = ExecuteReaderCommandTasks(database => InitializeFindByMultiplePrimarySearchEntityIdsCommand(primarySearchEntityIds, database));
            AttachSubTasks(tasks);
            return tasks;
        }

        /// <summary>
        /// Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <param name="userId">The user identifier.</param>
        public void Remove(Guid entityId, Guid userId)
        {
            var entity = new Task { Id = entityId, UpdatedById = userId };

            ExecuteNonQueryCommand(database => InitializeSaveCommand(entity, database, SaveEnum.Delete), entity);
        }

        /// <summary>
        /// Bulk create tasks.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        public void BulkCreate(IEnumerable<Task> tasks)
        {
            ExecuteNonQueryCommand(database => InitializeMultiSaveCommand(tasks, database, SaveEnum.Add));
        }

        /// <summary>
        /// Bulk update tasks.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        public void BulkUpdate(IEnumerable<Task> tasks)
        {
            ExecuteNonQueryCommand(database => InitializeMultiSaveCommand(tasks, database, SaveEnum.Update));
        }

        private static void AttachSubTasks(List<Task> tasks)
        {
            foreach (var task in tasks)
            {
                if (task.ParentId != Guid.Empty)
                {
                    var parentTask = tasks.FirstOrDefault(x => x.Id.GetValueOrDefault() == task.ParentId);
                    if (parentTask != null)
                    {
                        parentTask.SubTasks.Add(task);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the save command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="db">The db.</param>
        /// <param name="saveEnum">The save enum.</param>
        /// <returns>DbCommand.</returns>
        protected override DbCommand InitializeSaveCommand(Task entity, Database db, SaveEnum saveEnum)
        {
            var command = base.InitializeSaveCommand(entity, db, saveEnum);

            var taskRelatedTable = CreateTaskRelatedTable(entity.Id.GetValueOrDefault(), entity.PrimarySearchEntityId,
                entity.Predecessors.Select(x => x.TaskNumber).ToList());
            var sqlParameter = new SqlParameter("@TaskPredecessors", SqlDbType.Structured) { Value = taskRelatedTable };
            command.Parameters.Add(sqlParameter);

            taskRelatedTable = CreateTaskRelatedTable(entity.Id.GetValueOrDefault(), entity.PrimarySearchEntityId,
                entity.ChildTaskNumbers);
            sqlParameter = new SqlParameter("@TaskChildren", SqlDbType.Structured) { Value = taskRelatedTable };
            command.Parameters.Add(sqlParameter);

            return command;
        }

        /// <summary>
        /// Initializes the multi save command.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="db">The database.</param>
        /// <param name="saveEnum">The save enum.</param>
        /// <returns>DbCommand.</returns>
        protected override DbCommand InitializeMultiSaveCommand(IEnumerable<Task> entities, Database db, SaveEnum saveEnum)
        {
            var listEntities = entities.ToList();
            var command = base.InitializeMultiSaveCommand(listEntities, db, saveEnum);

            var taskPredecessorTable = CreateDataTable("TaskRelated");
            var taskChildTable = CreateDataTable("TaskRelated");
            foreach (var entity in listEntities)
            {
                foreach (var predecessor in entity.Predecessors)
                {
                    var dr = taskPredecessorTable.NewRow();
                    dr["TaskId"] = entity.Id.GetValueOrDefault();
                    dr["PrimarySearchEntityId"] = entity.PrimarySearchEntityId;
                    dr["RelatedTaskNumber"] = predecessor.TaskNumber;
                    taskPredecessorTable.Rows.Add(dr);
                }
                foreach (var childTaskNumber in entity.ChildTaskNumbers)
                {
                    var dr = taskChildTable.NewRow();
                    dr["TaskId"] = entity.Id.GetValueOrDefault();
                    dr["PrimarySearchEntityId"] = entity.PrimarySearchEntityId;
                    dr["RelatedTaskNumber"] = childTaskNumber;
                    taskChildTable.Rows.Add(dr);
                }
            }

            var sqlParameter = new SqlParameter("@TaskPredecessors", SqlDbType.Structured) { Value = taskPredecessorTable };
            command.Parameters.Add(sqlParameter);

            sqlParameter = new SqlParameter("@TaskChildren", SqlDbType.Structured) { Value = taskChildTable };
            command.Parameters.Add(sqlParameter);

            db.AddInParameter(command, "@BulkLoad", DbType.Boolean, entities.Count() > 1);

            return command;
        }

        private DataTable CreateTaskRelatedTable(Guid taskId, Guid primarySearchEntityId, IEnumerable<int> relatedTaskNumbers)
        {
            var taskRelatedTable = CreateDataTable("TaskRelated");
            foreach (var relatedTaskNumber in relatedTaskNumbers)
            {
                var dr = taskRelatedTable.NewRow();
                dr["TaskId"] = taskId;
                dr["PrimarySearchEntityId"] = primarySearchEntityId;
                dr["RelatedTaskNumber"] = relatedTaskNumber;
                taskRelatedTable.Rows.Add(dr);
            }

            return taskRelatedTable;
        }

        private DataTable CreateTaskNumberTable(IEnumerable<int> taskNumbers)
        {
            var taskRelatedTable = CreateDataTable("TaskNumber");
            foreach (var taskNumber in taskNumbers)
            {
                var dr = taskRelatedTable.NewRow();
                dr["TaskNumber"] = taskNumber;
                taskRelatedTable.Rows.Add(dr);
            }

            return taskRelatedTable;
        }

        /// <summary>
        /// Determines if there are pending tasks for the given entity.
        /// </summary>
        /// <param name="primarySearchEntityId">The primary search entity identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool PendingTasks(Guid primarySearchEntityId)
        {
            var returnValue = true;
            ExecuteNonQueryCommand(database => InitializePendingTasksCommand(primarySearchEntityId, database),
                cmd => { returnValue = (int)cmd.Parameters["@RETURN_VALUE"].Value == 1; });
            return returnValue;
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
        protected override void AddTableRowFields(Task entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);

            dr["PrimarySearchEntityId"] = entity.PrimarySearchEntityId;
            dr["PrimarySearchEntityType"] = entity.PrimarySearchEntityType;
            dr["ActualDuration"] = entity.ActualDuration == null ? DBNull.Value : (object)entity.ActualDuration;
            dr["Body"] = entity.Body;
            dr["Category"] = entity.Category;
            dr["ClientBarrierHours"] = entity.ClientBarrierHours == null ? DBNull.Value : (object)entity.ClientBarrierHours;
            dr["EstimatedDuration"] = entity.EstimatedDuration == null ? DBNull.Value : (object)entity.EstimatedDuration;
            dr["EstimatedStartDayNumber"] = entity.EstimatedStartDayNumber == null
                ? DBNull.Value
                : (object)entity.EstimatedStartDayNumber;
            dr["Group"] = entity.Group;
            dr["ParentTaskNumber"] = entity.ParentTaskNumber == null ? DBNull.Value : (object)entity.ParentTaskNumber;
            dr["PercentCompleted"] = entity.PercentComplete;
            dr["TaskProgressId"] = entity.Progress;
            dr["ReminderDate"] = entity.ReminderDate == null ? DBNull.Value : (object)entity.ReminderDate;
            dr["StartDate"] = entity.StartDate == null ? DBNull.Value : (object)entity.StartDate;
            dr["TaskStatusId"] = entity.Status;
            dr["TaskOwner"] = entity.TaskOwner;
            dr["Title"] = entity.Title;
            //dr["TaskClassificationId"] = entity.TaskClassificationId;
            dr["Comment"] = entity.Comment;
            dr["TaskNumber"] = entity.TaskNumber;
            dr["CreationOrder"] = entity.CreationOrder;
            dr["DueDate"] = entity.DueDate == null ? DBNull.Value : (object)entity.DueDate;
            dr["TaskTypeId"] = entity.TaskTypeId.HasValue ? (object)entity.TaskTypeId : DBNull.Value;
			dr["TaskTemplateId"] = entity.TaskTemplateId.HasValue ? (object)entity.TaskTemplateId : DBNull.Value;
			dr["Description"] = entity.Description;
        }

        /// <summary>
        ///     Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override Task ConstructEntity(IDataReader reader)
        {
	        var entity = new Task(reader.GetValue<Guid>("TaskId"))
	        {
		        CreatedById = reader.GetValue<Guid>("CreatedBy"),
		        CreatedDateTime = reader.GetValue<DateTime>("CreatedDT"),
		        UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
		        UpdatedDateTime = reader.GetValue<DateTime>("UpdatedDT"),
		        PrimarySearchEntityId = reader.GetValue<Guid>("PrimarySearchEntityId"),
		        PrimarySearchEntityType = reader.GetValue<string>("PrimarySearchEntityType"),
		        ActualDuration = (double?) reader.GetValue<decimal?>("ActualDuration"),
		        Body = reader.GetValue<string>("Body"),
		        Category = reader.GetValue<string>("Category"),
		        ClientBarrierHours = (double?) reader.GetValue<decimal?>("ClientBarrierHours"),
		        EstimatedDuration = (double?) reader.GetValue<decimal?>("EstimatedDuration"),
		        EstimatedStartDayNumber = reader.GetValue<int?>("EstimatedStartDayNumber"),
		        Group = reader.GetValue<string>("Group"),
		        IsDeleted = reader.GetValue<bool>("IsDeleted"),
		        ParentId = ((Guid?) reader.GetValue<Guid>("ParentTaskId")).GetValueOrDefault(),
		        PercentComplete = (double) reader.GetValue<decimal>("PercentCompleted"),
		        Progress = (TaskProgressEnumDto) reader.GetValue<short>("TaskProgressId"),
		        ReminderDate = reader.GetValue<DateTime?>("ReminderDate"),
		        StartDate = reader.GetValue<DateTime?>("StartDate"),
		        Status = (TaskStatusEnumDto) reader.GetValue<short>("TaskStatusId"),
		        TaskNumber = reader.GetValue<int>("TaskNumber"),
		        TaskOwner = reader.GetValue<string>("TaskOwner")
	        };
	        if (string.IsNullOrEmpty(entity.TaskOwner))
                entity.TaskOwner = "Unassigned";
            entity.Title = reader.GetValue<string>("Title");
            //entity.TaskClassificationId = reader.GetValue<string>("TaskClassificationId");
            entity.HasComments = reader.GetValue<bool>("HasComments");
            entity.DueDate = reader.GetValue<DateTime?>("DueDate");
            entity.Modified = entity.UpdatedDateTime;
            entity.Created = entity.CreatedDateTime;
            entity.TaskTypeId = reader.GetValue<Guid>("TaskTypeId");
            entity.TaskTypeName = reader.GetValue<string>("TaskTypeName");
			entity.Description = reader.GetValue<string>("Description");
			entity.IsProjectHandlerRestricted = reader.GetValue<bool>("IsProjectHandlerRestricted");
			entity.ShouldTriggerBilling = reader.GetValue<bool>("ShouldTriggerBilling");
			entity.PreventDeletion = reader.GetValue<bool>("PreventDeletion");
            entity.LastComment = reader.GetValue<string>("LastComment");
			entity.TaskTemplateId = reader.GetValue<Guid>("TaskTemplateId");
			entity.ProjectTaskShouldTriggerBillingCount = reader.GetValue<int>("ProjectTaskShouldTriggerBillingCount");
            entity.RecordVersion = reader.GetValue<byte[]>("RecordVersion"); 
            return entity;
        }

        private static DbCommand InitializeFindIdsByPrimarySearchEntityIdCommand(Guid primarySearchEntityId,
            bool includeDeleted, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pTask_GetIdsByPrimarySearchEntityId]");
            db.AddInParameter(command, "@PrimarySearchEntityId", DbType.Guid, primarySearchEntityId);
            db.AddInParameter(command, "@IncludeDeleted", DbType.Boolean, includeDeleted);
            return command;
        }

        private static DbCommand InitializeFindByPrimarySearchEntityIdCommand(Guid primarySearchEntityId, bool includeDeleted,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pTask_GetByPrimarySearchEntityId]");
            db.AddInParameter(command, "@PrimarySearchEntityId", DbType.Guid, primarySearchEntityId);
            db.AddInParameter(command, "@IncludeDeleted", DbType.Boolean, includeDeleted);
            return command;
        }

        private static DbCommand InitializeCountByPrimarySearchEntityIdCommand(Guid primarySearchEntityId, bool includeDeleted,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pTask_GetCountByPrimarySearchEntity]");
            db.AddInParameter(command, "@PrimarySearchEntityId", DbType.Guid, primarySearchEntityId);
            db.AddInParameter(command, "@IncludeDeleted", DbType.Boolean, includeDeleted);
            return command;
        }

        private static DbCommand InitializeFindWithHasCommentsByPrimarySearchEntityIdCommand(Guid primarySearchEntityId,
            bool includeDeleted, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pTask_GetWithHasCommentsByPrimarySearchEntityId]");
            db.AddInParameter(command, "@PrimarySearchEntityId", DbType.Guid, primarySearchEntityId);
            db.AddInParameter(command, "@IncludeDeleted", DbType.Boolean, includeDeleted);
            return command;
        }

        private DbCommand InitializeFindByMultiplePrimarySearchEntityIdsCommand(IEnumerable<Guid> primarySearchEntityIds,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pTask_GetByMultiplePrimarySearchEntityIds]");

            var primarySearchEntityIdTable = CreateDataTable("PrimarySearchEntityId");
            foreach (var primarySearchEntityId in primarySearchEntityIds)
            {
                var dr = primarySearchEntityIdTable.NewRow();
                dr["PrimarySearchEntityId"] = primarySearchEntityId;
                primarySearchEntityIdTable.Rows.Add(dr);
            }

            var sqlParameter = new SqlParameter("@PrimarySearchEntityId", SqlDbType.Structured)
            {
                Value = primarySearchEntityIdTable
            };
            command.Parameters.Add(sqlParameter);

            return command;
        }

        private DbCommand InitializeFindByIdCommandTaskOnly(Guid entityId, Database db)
        {
            var command = InitializeFindCommand(entityId, db);
            var sqlParameter = new SqlParameter("@TaskOnly", SqlDbType.Bit) { Value = true };
            command.Parameters.Add(sqlParameter);
            return command;
        }

        private static DbCommand InitializePendingTasksCommand(Guid primarySearchEntityId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pTask_PendingTasks]");
            db.AddInParameter(command, "@PrimarySearchEntityId", DbType.Guid, primarySearchEntityId);

            var returnValue = new SqlParameter("@RETURN_VALUE", SqlDbType.Bit) { Direction = ParameterDirection.ReturnValue };
            command.Parameters.Add(returnValue);

            return command;
        }

        private static TaskComment ConstructTaskComment(IDataReader reader, out Guid taskId)
        {
            taskId = reader.GetValue<Guid>("TaskId");
            return new TaskComment
            {
                Comment = reader.GetValue<string>("Text"),
                CreatedBy = reader.GetValue<string>("CreatedBy"),
                CreatedDate = reader.GetValue<DateTime>("CreatedDT")
            };
        }

        private static TaskPredecessor ConstructTaskPredecessor(IDataReader reader, out Guid successorTaskId)
        {
            successorTaskId = reader.GetValue<Guid>("SuccessorTaskId");
            return new TaskPredecessor
            {
                TaskId = reader.GetValue<Guid>("PredecessorTaskId"),
                Title = reader.GetValue<string>("Title"),
                TaskNumber = reader.GetValue<int>("TaskNumber"),
				 Status = (TaskStatusEnumDto) reader.GetValue<short>("TaskStatusId"),
            };
        }

        private static Lookup ConstructIds(IDataReader reader)
        {
            return new Lookup { Id = reader.GetValue<Guid>("TaskId") };
        }

        private static TaskNotification ConstructTaskNotification(IDataReader reader, out Guid taskId)
        {
            taskId = reader.GetValue<Guid>("TaskId");
            return new TaskNotification
            {
                Id = reader.GetValue<Guid>("TaskNotificationId"),
                TaskId = reader.GetValue<Guid>("TaskId"),
                Email = reader.GetValue<string>("Email"),
                CreatedById = reader.GetValue<Guid>("CreatedBy"),
                CreatedDateTime = reader.GetValue<DateTime>("CreatedDT"),
                UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedDT"),
            };
        }

        private List<Task> ExecuteReaderCommandTasks(Func<Database, DbCommand> initializeCommandDelegate)
        {
            var database = DatabaseFactory.CreateDatabase();
            using (var command = initializeCommandDelegate.Invoke(database))
            {
                var result = new Dictionary<Guid, Task>();

                using (var reader = database.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        var task = ConstructEntity(reader);
                        result.Add(task.Id.GetValueOrDefault(), task);
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        Guid successorTaskId;
                        var taskPredecessor = ConstructTaskPredecessor(reader, out successorTaskId);
                        var task = result[successorTaskId];
                        task.Predecessors.Add(taskPredecessor);
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        Guid taskId;
                        var taskComment = ConstructTaskComment(reader, out taskId);
                        var task = result[taskId];
                        task.Comments.Add(taskComment);
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        Guid taskId;
                        var taskNotification = ConstructTaskNotification(reader, out taskId);
                        var task = result[taskId];
                        task.Notifications.Add(taskNotification);
                    }
                }

                return result.Values.ToList();
            }
        }
    }
}