using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class TaskTypeRepository.
	/// </summary>
    public class TaskTypeRepository : PrimaryRepository<TaskType>, ITaskTypeRepository
	{
		/// <summary>
        /// Adds the search parameters.
		/// </summary>      
        /// <param name="db">The database.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="command">The command.</param>
        protected override void AddSearchParameters(Database db, ISearchCriteria searchCriteria, DbCommand command)
		{
            db.AddInParameter(command, "IncludeDeleted", DbType.Boolean, searchCriteria.IncludeDeletedRecords);
		}

		/// <summary>
        /// Defines the mappings.
		/// </summary>
        /// <param name="mapper">The mapper.</param>
        protected override void DefineMappings(IMapperRegistry mapper)
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
            get { return "TaskTypeId"; }
					}

		/// <summary>
        /// Maps the primary entity to data reader.
		/// </summary>
        /// <param name="expressionChain">The expression chain.</param>
        protected override void MapPrimaryEntityToDataReader(AutoMapper.IMappingExpression<IDataReader, TaskType> expressionChain)
		{

					}

		/// <summary>
        /// Gets the name of the table.
		/// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        protected override string TableName
		{
            get { return "TaskType"; }
		}

		/// <summary>
        /// Gets the lookups.
		/// </summary>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
		/// <returns></returns>
        public IEnumerable<Lookup> GetLookups(bool includeDeleted = false)
        {
           return ExecuteReaderCommand( (db) =>
		{
                var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[pTaskType_GetLookups]");
                var sqlParameter = new SqlParameter("IncludeDeleted", SqlDbType.Bit) { Value = includeDeleted };
                command.Parameters.Add(sqlParameter);
                return command;
            }, 
            ConstructTaskTypeLookups);
		}
		/// <summary>
		/// Constructs the task type lookups.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>Lookup.</returns>
		private Lookup ConstructTaskTypeLookups(IDataReader reader)
		{
			return new Lookup
			{
				Id = reader.GetValue<Guid>("TaskTypeId"),
				Name = reader.GetValue<string>("Name")
			};
		}

		/// <summary>
        /// Fetches the active by identifier.
		/// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
		/// <returns></returns>
        public TaskType Fetch(Guid id, bool isDeleted)
        {
            return FetchEntityById<TaskType>(id, (db, cmd) => 
		{
                    db.AddInParameter(cmd, "IsDeleted", DbType.Boolean, isDeleted );
                });
		}

		/// <summary>
        /// Fetches the active by identifier.
		/// </summary>
		/// <returns></returns>
        public IList<TaskType> FetchAll()
		{
            return ExecuteReaderCommand(db => InitializeFetchCommand(Guid.Empty, db),
                 ConstructEntity<TaskType>);
		}
	}
}
