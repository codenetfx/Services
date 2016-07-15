using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using AutoMapper;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Implements repository operations for working with <see cref="TaskProperty"/> objects.
    /// </summary>
    public class TaskPropertyRepository : PrimaryRepository<TaskProperty>, ITaskPropertyRepository
    {
        /// <summary>
        /// Defines the primary entity i data reader mapping.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        protected override void DefinePrimaryEntityIDataReaderMapping(IMapperRegistry mapper)
        {
            base.DefinePrimaryEntityIDataReaderMapping(mapper);
            var chain = mapper.Configuration.CreateMap<IDataReader, TaskProperty>()
                .AfterMap(MapType);

        }

        private void MapType(IDataReader dataReader, TaskProperty taskProperty)
        {
            taskProperty.TaskPropertyType = new TaskPropertyType
            {
                Id = taskProperty.TaskPropertyTypeId, 
                Name = dataReader.GetValue<string>("Name")
            };
        }

        /// <summary>
        /// Maps the primary entity to data reader.
        /// </summary>
        /// <param name="expressionChain">The expression chain.</param>
        protected override void MapPrimaryEntityToDataReader(IMappingExpression<IDataReader, TaskProperty> expressionChain)
        {
            
        }

        /// <summary>
        /// Defines the mappings.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        protected override void DefineMappings(IMapperRegistry mapper)
        {
            
        }

        /// <summary>
        /// Adds the search parameters.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="command">The command.</param>
        protected override void AddSearchParameters(Database db, ISearchCriteria searchCriteria, DbCommand command)
        {
            
        }

        /// <summary>
        /// Gets the name of the identifier field.
        /// </summary>
        /// <value>
        /// The name of the identifier field.
        /// </value>
        protected override string IdFieldName
        {
            get { return "TaskPropertyId"; }
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        protected override string TableName
        {
            get { return "TaskProperty"; }
        }
        

        /// <summary>
        /// Finds the by task property type identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="taskPropertyTypeId">The task property type identifier.</param>
        /// <returns></returns>
        public IList<TaskProperty> FindByTaskPropertyTypeId(Guid taskId, Guid taskPropertyTypeId)
        {
            return ExecuteReaderCommand(db =>
            {
                var cmd = db.GetStoredProcCommand("[dbo].[pTaskProperty_GetByTaskPropertyTypeId]");
                var param = cmd.CreateParameter();
                param.ParameterName = "TaskId";
                param.DbType = DbType.Guid;
                param.Value = taskId;
                cmd.Parameters.Add(param);
                param = cmd.CreateParameter();
                param.ParameterName = "TaskPropertyTypeId";
                param.DbType = DbType.Guid;
                param.Value = taskPropertyTypeId;
                cmd.Parameters.Add(param);
                return cmd;

            }, ConstructEntityDelegate);
        }

        private TaskProperty ConstructEntityDelegate(IDataReader dataReader)
        {
            var taskProperty = ConstructEntity<TaskProperty>(dataReader);
            taskProperty.TaskPropertyType = new TaskPropertyType
            {
                Id = taskProperty.TaskPropertyTypeId,
                Name = dataReader.GetValue<string>("Name")
            };
            return taskProperty;
        }

        /// <summary>
        /// Finds the by task identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        public IList<TaskProperty> FindByTaskId(Guid taskId)
        {
            return ExecuteReaderCommand(db =>
            {
                var cmd = db.GetStoredProcCommand("[dbo].[pTaskProperty_GetByTaskId]");
                var param = cmd.CreateParameter();
                param.ParameterName = "TaskId";
                param.DbType = DbType.Guid;
                param.Value = taskId;
                cmd.Parameters.Add(param);
                return cmd;

            }, ConstructEntityDelegate);
        }
    }
}