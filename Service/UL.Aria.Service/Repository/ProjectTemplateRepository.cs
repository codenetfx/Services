using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Class ProjectTemplateRepository
    /// </summary>
    public sealed class ProjectTemplateRepository : TrackedDomainEntityRepositoryBase<ProjectTemplate>, IProjectTemplateRepository
    {
        private const string ProjectTemplateNotFoundIdMessage = "Project Template Not Found Id: '{0}'";
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectTemplateRepository" /> class.
        /// </summary>
        /// <param name="transactionFactory">The transaction factory.</param>
        public ProjectTemplateRepository(ITransactionFactory transactionFactory)
            : base("ProjectTemplateId", "ProjectTemplate")
        {
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Finds all.
        /// </summary>
        /// <returns>IList{ProjectTemplate}.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override IList<ProjectTemplate> FindAll()
        {
            return ExecuteReaderCommand(db => InitializeFetchTemplatesCommand(db), ConstructEntity);
        }

        /// <summary>
        ///     Finds the project templates with the same correlationId.
        /// </summary>
        /// <param name="correlationId">The project template correlation id.</param>
        /// <returns>IList{ProjectTemplate}.</returns>
        public IList<ProjectTemplate> GetAllByCorrelationId(Guid correlationId)
        {
            return ExecuteReaderCommand(db => InitializeGetAllByCorrelationIdCommand(correlationId, db), ConstructEntity);
        }

        /// <summary>
        ///     Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>ProjectTemplate.</returns>
        public override ProjectTemplate FindById(Guid entityId)
        {
            return GetById(entityId);
        }

        /// <summary>
        ///     Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(ProjectTemplate entity)
        {
            Create(entity);
        }

        /// <summary>
        ///     Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Guid.</returns>
        public override Guid Create(ProjectTemplate entity)
        {
            var id = Guid.Empty;
            ExecuteNonQueryCommand(db => InitializeInsertCommand(entity, db), entity,
                cmd => { id = (Guid)cmd.Parameters["@ProjectTemplateId"].Value; });
            return id;
        }

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Int32.</returns>
        public override int Update(ProjectTemplate entity)
        {
            int count;
            Enterprise.Foundation.Framework.Guard.IsNotNull(entity.Id, "entity.Id != null");
            count = ExecuteNonQueryCommand(db => InitializedUpdateCommand(entity, db), entity);
            return count;
        }

        /// <summary>
        ///     Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        public override void Remove(Guid entityId)
        {
            ExecuteNonQueryCommand(db => InitializeRemoveCommand(entityId, db));
        }

        /// <summary>
        ///     Gets the by id.
        /// </summary>
        /// <param name="projectTemplateId">The project template id.</param>
        /// <returns>ProjectTemplate.</returns>
        public ProjectTemplate GetById(Guid projectTemplateId)
        {
            var results = ExecuteReaderCommand(db => InitializedGetByIdCommand(projectTemplateId, db), ConstructEntity);
            if (!results.Any())
                throw new DatabaseItemNotFoundException("Project Template Not found.");
            return results.First();
        }

        /// <summary>
        /// Adds the search parameters.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="command">The command.</param>
        protected override void AddSearchParameters(Database db, SearchCriteria searchCriteria, DbCommand command)
        {

            db.AddInParameter(command, "IncludeDeleted", DbType.Int64, searchCriteria.IncludeDeletedRecords);
            AddFilter(command, searchCriteria, AssetFieldNames.ProjectTemplateBusinessUnitCode, "BusinessUnitCode");
            AddFilter(command, searchCriteria, AssetFieldNames.ProjectTemplateBusinessUnitId, "BusinessUnitId");
            AddFilter(command, searchCriteria, AssetFieldNames.ProjectTemplateIsDraft, "IsDraft");
         
        }        
    
        private static DbCommand InitializeFetchTemplatesCommand(Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pProjectTemplate_Get]");
            return command;
        }

        private static DbCommand InitializeGetAllByCorrelationIdCommand(Guid id, Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pProjectTemplate_GetAllByCorrelationId]");
            db.AddInParameter(command, "@CorrelationId", DbType.Guid, id);
            return command;
        }

        private DbCommand InitializeRemoveCommand(Guid entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectTemplate_Delete]");

            db.AddInParameter(command, "ProjectTemplateId", DbType.Guid, entity);

            return command;
        }

        private DbCommand InitializeInsertCommand(ProjectTemplate projectTemplate, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectTemplate_Insert]");

            db.AddParameter(command, "@ProjectTemplateId", DbType.Guid, ParameterDirection.InputOutput, "ProjectTemplateId", DataRowVersion.Current, projectTemplate.Id);
            db.AddInParameter(command, "@CorrelationId", DbType.Guid, projectTemplate.CorrelationId);
            db.AddInParameter(command, "@IsDraft", DbType.String, projectTemplate.IsDraft);
            db.AddInParameter(command, "@Version", DbType.Decimal, projectTemplate.Version);
            db.AddInParameter(command, "@Name", DbType.String, projectTemplate.Name);
            db.AddInParameter(command, "@Description", DbType.String, projectTemplate.Description);
            db.AddInParameter(command, "@CreatedBy", DbType.Guid, projectTemplate.CreatedById);
            db.AddInParameter(command, "@CreatedOn", DbType.DateTime2, projectTemplate.CreatedDateTime);
            db.AddInParameter(command, "@UpdatedBy", DbType.Guid, projectTemplate.UpdatedById);
            db.AddInParameter(command, "@UpdatedOn", DbType.DateTime2, projectTemplate.UpdatedDateTime);
            db.AddInParameter(command, "@Tasks", DbType.String, DBNull.Value);
			db.AddInParameter(command, "@AutoCompleteProject", DbType.Boolean, projectTemplate.AutoCompleteProject);

            return command;
        }

        private DbCommand InitializedUpdateCommand(ProjectTemplate projectTemplate, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectTemplate_Update]");

            db.AddInParameter(command, "@ProjectTemplateId", DbType.Guid, projectTemplate.Id);
            db.AddInParameter(command, "@CorrelationId", DbType.Guid, projectTemplate.CorrelationId);
            db.AddInParameter(command, "@IsDraft", DbType.String, projectTemplate.IsDraft);
            db.AddInParameter(command, "@Version", DbType.Decimal, projectTemplate.Version);
            db.AddInParameter(command, "@Name", DbType.String, projectTemplate.Name);
            db.AddInParameter(command, "@Description", DbType.String, projectTemplate.Description);
            db.AddInParameter(command, "@UpdatedBy", DbType.Guid, projectTemplate.UpdatedById);
            db.AddInParameter(command, "@UpdatedOn", DbType.DateTime2, projectTemplate.UpdatedDateTime);
            db.AddInParameter(command, "@Tasks", DbType.String, DBNull.Value);
			db.AddInParameter(command, "@AutoCompleteProject", DbType.Boolean, projectTemplate.AutoCompleteProject);

            return command;
        }

        private DbCommand InitializedGetByIdCommand(Guid projectTemplateId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectTemplate_GetById]");
            db.AddInParameter(command, "@ProjectTemplateId", DbType.Guid, projectTemplateId);
            return command;
        }

        private DbCommand InitializeInsertCommandProjectTemplateBusinessUnit(Guid id, BusinessUnit businessUnit,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectTemplateBusinessUnit_Insert]");
            db.AddInParameter(command, "ProjectTemplateId", DbType.Guid, id);
            db.AddInParameter(command, "BusinessUnitId", DbType.Guid, businessUnit.Id);
            return command;
        }

        private DbCommand InitializeDeleteCommandBusinessUnitsForProjectTemplate(Guid id, ProjectTemplate projectTemplate, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectTemplateBusinessUnit_DeleteByProjectTemplateId]");
            db.AddInParameter(command, "ProjectTemplateId", DbType.Guid, id);
            return command;
        }

        private ProjectTemplate ConstructCompleteEntity(IDataReader reader)
        {
            var projectTemplate = ConstructEntity(reader);
            //task templates are left here for auto migration purposes.
            var taskXml = reader.GetValue<string>("Tasks");
            projectTemplate.TaskTemplates = (taskXml == null) ? new List<TaskTemplate>() : ConvertXmlToTaskTemplates(taskXml).ToList();
            return projectTemplate;
        }

        /// <summary>
        ///     Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>ProjectTemplate.</returns>
        protected override ProjectTemplate ConstructEntity(IDataReader reader)
        {
            var entity = base.ConstructEntity(reader);
            //task templates are left here for auto migration purposes.
            entity.TaskTemplates = ConvertXmlToTaskTemplates(reader.GetValue<string>("Tasks"));
            return entity;
        }

        /// <summary>
        ///     Deserializes the Xml to TaskTemplates.
        /// </summary>
        /// <param name="xmlTaskTemplates">The xml Task Templates.</param>
        /// <returns>List{TaskTemplate}.</returns>
        private List<TaskTemplate> ConvertXmlToTaskTemplates(string xmlTaskTemplates)
        {
            if (string.IsNullOrWhiteSpace((xmlTaskTemplates)))
                return new List<TaskTemplate>();

            // Deserialize TaskTemplates from XML to List<Entity>
            var xmlSerializer = new XmlSerializer(typeof(TaskTemplates));
            var taskTemplates =
                (TaskTemplates)xmlSerializer.Deserialize(new StringReader(xmlTaskTemplates));
            return taskTemplates.Tasks;

        }


    }
}