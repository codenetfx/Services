using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// 
	/// </summary>
	public class TaskCategoryRepository : TrackedDomainEntityRepositoryBase<TaskCategory>, ITaskCategoryRepository
	{

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCategoryRepository" /> class.
        /// </summary>       
        public TaskCategoryRepository()
			: base("TaskCategoryId", "TaskCategory" )
		{
		}
		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public Domain.Entity.TaskCategory FetchById(Guid id)
		{
			return FindById(id);
		}

		/// <summary>
		/// Fetches the active by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public TaskCategory FetchActiveById(Guid id)
		{
			var result = ExecuteReaderCommand(database => InitializeFindActiveCommand(id, database), ConstructEntity);
			return result.FirstOrDefault();
		}

		/// <summary>
		/// Constructs the entity.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		protected override TaskCategory ConstructEntity(IDataReader reader)
		{
			base.ConstructEntity(reader);

			return ConstructTaskCategory(reader);
		}


		/// <summary>
		/// Adds the table row fields.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="isNew">if set to <c>true</c> [is new].</param>
		/// <param name="isDirty">if set to <c>true</c> [is dirty].</param>
		/// <param name="isDelete">if set to <c>true</c> [is delete].</param>
		/// <param name="dr">The dr.</param>
		protected override void AddTableRowFields(TaskCategory entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
		{
			base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);
			dr["Name"] = entity.Name;
			dr["Description"] = entity.Description;

		}



		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		public Domain.Search.TaskCategorySearchResultSet Search(Domain.Search.SearchCriteria searchCriteria)
		{

			var results = ExecuteReaderCommand(database => InitializeSearchCommand(searchCriteria, database), ConstructEntity);

			var searchResultTotal = results.Count;

			//Sort the results
			var sortedList = ResultBuilder.Sort(searchCriteria.Sorts, results);

			//Page the results
			var pagedResult = ResultBuilder.Page(sortedList, (int)searchCriteria.StartIndex, (int)searchCriteria.EndIndex);

			//Construct the summary
			var searchResults = new TaskCategorySearchResultSet
			{
				Criteria = searchCriteria,
				Summary = new SearchSummary
				{
					EndIndex = searchResultTotal > searchCriteria.EndIndex ? searchCriteria.EndIndex : searchResultTotal - 1,
					StartIndex = searchCriteria.StartIndex,
					TotalResults = searchResultTotal
				},
				Results = pagedResult

			};



			return searchResults;
		}

		private static TaskCategory ConstructTaskCategory(IDataReader reader)
		{
			var taskTemplate = new TaskCategory
			{
				Id = reader.GetValue<Guid>("TaskCategoryId"),
				Name = reader.GetValue<string>("Name"),
				Description = reader.GetValue<string>("Description"),

			};
			return taskTemplate;
		}

		private static DbCommand InitializeFetchCountAllCommand(Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[pTaskCategory_GetAllCount]");
			return command;
		}

		/// <summary>
		/// Fetches all count.
		/// </summary>
		/// <returns></returns>
		public int FetchAllCount()
		{
			var db = DatabaseFactory.CreateDatabase();
			using (var command = InitializeFetchCountAllCommand(db))
			{
				using (var reader = db.ExecuteReader(command))
				{
					if (reader.Read())
					{
						return reader.GetInt32(0);
					}
				}
			}
			return 0;
		}


		/// <summary>
		/// Initializes the search command.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <param name="db">The db.</param>
		/// <returns></returns>
		protected DbCommand InitializeSearchCommand(SearchCriteria searchCriteria, Database db)
		{
			var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[p" + TableName + "_Search]");

			//
			// only use the first Sort specified
			//
			string sortBy = null;
			var sortDirection = SortDirection.Ascending;
			if (searchCriteria.Sorts.Count > 0)
			{
				sortBy = searchCriteria.Sorts[0].FieldName;
				sortDirection = searchCriteria.Sorts[0].Order;
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(searchCriteria.SortBy))
				{
					sortBy = searchCriteria.SortBy;
					sortDirection = searchCriteria.SortDirection;
				}
			}


			if (!string.IsNullOrWhiteSpace(searchCriteria.Keyword))
			{
				var sqlParameter = new SqlParameter("Keyword", SqlDbType.NVarChar) { Value = searchCriteria.Keyword };
				command.Parameters.Add(sqlParameter);
			}
			if (!string.IsNullOrWhiteSpace(sortBy))
			{
				db.AddInParameter(command, "SortBy", DbType.String, sortBy);
			}
			db.AddInParameter(command, "SortDirection", DbType.String,
				sortDirection == SortDirection.Descending ? "DESC" : "ASC");
			db.AddInParameter(command, "StartIndex", DbType.Int64, searchCriteria.StartIndex);
			db.AddInParameter(command, "EndIndex", DbType.Int64, searchCriteria.EndIndex);
			return command;
		}


		/// <summary>
		/// Initializes the find active command.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		/// <param name="db">The database.</param>
		/// <returns></returns>
		protected  DbCommand InitializeFindActiveCommand(Guid entityId, Database db)
		{
			var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[p" + TableName + "_GetActive]");
			var sqlParameter = new SqlParameter(IdFieldName, SqlDbType.UniqueIdentifier) { Value = entityId };
			command.Parameters.Add(sqlParameter);

			return command;
		}

	}
}
