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
    /// Implements Repository operations for <see cref="TaskTypeAvailableBehaviorField"/> objects.
    /// </summary>
    public class TaskTypeAvailableBehaviorFieldRepository : PrimaryRepository<TaskTypeAvailableBehaviorField>, ITaskTypeAvailableBehaviorFieldRepository
    {
        /// <summary>
        /// Maps the primary entity to data reader.
        /// </summary>
        /// <param name="expressionChain">The expression chain.</param>
        protected override void MapPrimaryEntityToDataReader(IMappingExpression<IDataReader, TaskTypeAvailableBehaviorField> expressionChain)
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
            get { return "TaskTypeAvailableBehaviorFieldId"; }
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        protected override string TableName
        {
            get { return "TaskTypeAvailableBehaviorField"; }
        }

        /// <summary>
        /// Finds the by task type available behavior identifier.
        /// </summary>
        /// <param name="taskTypeAvailableBehaviorId">The task type available behavior identifier.</param>
        /// <returns></returns>
        public IEnumerable<TaskTypeAvailableBehaviorField> FindByTaskTypeAvailableBehaviorId(Guid taskTypeAvailableBehaviorId)
        {
            return ExecuteReaderCommand(db =>
            {
                var cmd = db.GetStoredProcCommand("[dbo].[pTaskTypeAvailableBehaviorField_Get]");
                var param = cmd.CreateParameter();
                param.ParameterName = "TaskTypeAvailableBehaviorID";
                param.DbType = DbType.Guid;
                param.Value = taskTypeAvailableBehaviorId;
                cmd.Parameters.Add(param);
                return cmd;

            }, ConstructEntity<TaskTypeAvailableBehaviorField>);
        }
    }
}