using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Implements Repository operations for <see cref="TaskTypeBehavior"/> objects.
    /// </summary>
    public class TaskTypeBehaviorRepository : PrimaryRepository<TaskTypeBehavior>, ITaskTypeBehaviorRepository
    {
        /// <summary>
        /// Maps the primary entity to data reader.
        /// </summary>
        /// <param name="expressionChain">The expression chain.</param>
        protected override void MapPrimaryEntityToDataReader(IMappingExpression<IDataReader, TaskTypeBehavior> expressionChain)
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
            get { return "TaskTypeBehaviorId"; }
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        protected override string TableName
        {
            get { return "TaskTypeBehavior"; }
        }

        /// <summary>
        /// Finds the by task type identifier.
        /// </summary>
        /// <param name="taskTypeId">The task type identifier.</param>
        /// <returns></returns>
        public IEnumerable<TaskTypeBehavior> FindByTaskTypeId(Guid taskTypeId)
        {
            return ExecuteReaderCommand(db =>
            {
                var cmd = db.GetStoredProcCommand("[dbo].[pTaskTypeBehavior_Get]");
                var param = cmd.CreateParameter();
                param.ParameterName = "TaskTypeId";
                param.DbType = DbType.Guid;
                param.Value = taskTypeId;
                cmd.Parameters.Add(param);
                return cmd;

            }, ConstructEntity<TaskTypeBehavior>);
        }
    }
}
