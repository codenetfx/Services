using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Task Type Notification Repository
    /// </summary>
    public class TaskTypeNotificationRepository : Domain.Repository.AssociatedPrimaryRepository<TaskTypeNotification>, ITaskTypeNotificationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTypeNotificationRepository"/> class.
        /// </summary>
        public TaskTypeNotificationRepository() { }


        /// <summary>
        /// Gets the name of the identifier field.
        /// </summary>
        /// <value>
        /// The name of the identifier field.
        /// </value>
        protected override string IdFieldName
        {
            get { return "TaskTypeNotificationId"; }
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        protected override string TableName
        {
            get { return "TaskTypeNotification"; }
        }


        /// <summary>
        /// Gets the name of the group parameter.
        /// </summary>
        /// <value>
        /// The name of the group parameter.
        /// </value>
        protected override string GroupParameterName
        {
            get { return "TaskTypeId"; }
        }

        /// <summary>
        /// Maps the primary entity to data reader.
        /// </summary>
        /// <param name="expressionChain">The expression chain.</param>
        protected override void MapPrimaryEntityToDataReader(AutoMapper.IMappingExpression<IDataReader, TaskTypeNotification> expressionChain)
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
        /// Defines the mappings.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        protected override void DefineMappings(IMapperRegistry mapper)
        {
            
        }

        /// <summary>
        /// Finds the by task type identifier.
        /// </summary>
        /// <param name="taskTypeId">The task type notification identifier.</param>
        /// <returns></returns>
        public IEnumerable<TaskTypeNotification> FindByTaskTypeId(Guid taskTypeId)
        {
            return base.FetchGroup(taskTypeId);
        }
    }
}
