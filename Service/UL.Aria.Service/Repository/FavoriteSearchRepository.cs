using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using System.Linq;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Favorite search repository class.
    /// </summary>
    public class FavoriteSearchRepository : TrackedDomainEntityRepositoryBase<FavoriteSearch>, IFavoriteSearchRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteSearchRepository"/> class.
        /// </summary>
        public FavoriteSearchRepository() : base("FavoriteSearchID", "FavoriteSearch")
        {
        }

        /// <summary>
        /// Adds the table row fields.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isNew">if set to <c>true</c> [is new].</param>
        /// <param name="isDirty">if set to <c>true</c> [is dirty].</param>
        /// <param name="isDelete">if set to <c>true</c> [is delete].</param>
        /// <param name="dr">The dr.</param>
        protected override void AddTableRowFields(FavoriteSearch entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
        {
            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);

            dr["UserId"] = entity.UserId;
            dr[TableName + "Name"] = entity.Name;
            dr[TableName + "Location"] = entity.Location;
            dr["ActiveDefault"] = entity.ActiveDefault;
            dr["AvailableDefault"] = entity.AvailableDefault || entity.ActiveDefault; // force true if active default

            MemoryStream memoryStream = null;
            try
            {
                memoryStream = new MemoryStream();
                var serializer = new DataContractSerializer(typeof (SearchCriteria));
                serializer.WriteObject(memoryStream, entity.SearchCriteria);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var streamReader = new StreamReader(memoryStream))
                {
                    memoryStream = null;
                    dr[TableName + "Criteria"] = streamReader.ReadToEnd();
                }
            }
            finally
            {
                if (memoryStream != null)
                    memoryStream.Dispose();
            }
        }

        /// <summary>
        /// Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override FavoriteSearch ConstructEntity(IDataReader reader)
        {
            var entity =  base.ConstructEntity(reader);

            entity.UserId = reader.GetValue<Guid>("UserId");
            entity.Name = reader.GetValue<string>(TableName + "Name");
            entity.Location = reader.GetValue<string>(TableName + "Location");
            entity.ActiveDefault = reader.GetValue<bool>("ActiveDefault");
            entity.AvailableDefault = reader.GetValue<bool>("AvailableDefault");

            var serializer = new DataContractSerializer(typeof (SearchCriteria));
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(reader.GetValue<string>(TableName + "Criteria"))))
            {
                var searchCriteria = serializer.ReadObject(memoryStream) as SearchCriteria;
                entity.SearchCriteria = searchCriteria;
            }
            
            return entity;
        }

        /// <summary>
        /// Fetches the by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// User's saved searches.
        /// </returns>
        public IEnumerable<FavoriteSearch> FindByUserId(Guid userId)
        {
            return ExecuteReaderCommand(database => InitializeFindByUserCommand(userId, database), ConstructEntity);
        }

        /// <summary>
        /// Fetches the by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="location">The location.</param>
        /// <returns>
        /// User's saved searches for a location.
        /// </returns>
        public IEnumerable<FavoriteSearch> FindByUserIdAndLocation(Guid userId, string location)
        {
            return ExecuteReaderCommand(database => InitializeFindByUserAndLocationCommand(userId, location, database), ConstructEntity);
        }

        /// <summary>
        /// Finds the active by user identifier and location.
        /// </summary>        
        /// <param name="userId">The user identifier.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public FavoriteSearch FindActiveByUserIdAndLocation(Guid userId, string location)
        {
            var favoriteSearch = ExecuteReaderCommand(database => InitializeFindActiveByUserAndLocationCommand(userId, location, database), ConstructEntity).FirstOrDefault();

            if (favoriteSearch == null)
                throw new DatabaseItemNotFoundException();

            return favoriteSearch;            
        }

        /// <summary>
        /// Finds the default by user identifier and location.
        /// </summary>        
        /// <param name="userId">The user identifier.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public IEnumerable<FavoriteSearch> FindAvailableByUserIdAndLocation(Guid userId, string location)
        {
            return ExecuteReaderCommand(database => InitializeFindAvailableByUserAndLocationCommand(userId, location, database), ConstructEntity);
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// User's favorite searches matching keyword.
        /// </returns>
        public FavoriteSearchSearchResult Search(SearchCriteria searchCriteria, Guid userId)
        {
            var results = ExecuteReaderCommand(database => InitializeSearchCommand(searchCriteria.Keyword, userId, database), ConstructEntity);

            var searchResultTotal = results.Count;

            //Sort the results
            var sortedList = ResultBuilder.Sort(searchCriteria.Sorts, results);

            //Page the results
            var pagedResult = ResultBuilder.Page(sortedList, (int)searchCriteria.StartIndex, (int)searchCriteria.EndIndex);

            //Construct the summary
            return new FavoriteSearchSearchResult
            {
                Criteria = searchCriteria,
                FavoriteSearches = pagedResult,
                Summary = new SearchSummary
                {
                    EndIndex = searchResultTotal> searchCriteria.EndIndex?  searchCriteria.EndIndex :  searchResultTotal-1 ,
                    StartIndex = searchCriteria.StartIndex,
                    TotalResults = searchResultTotal
                }
            };
        }

        /// <summary>
        /// Initializes the find by user command.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        protected DbCommand InitializeFindByUserCommand(Guid userId, Database db)
        {
            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[p" + TableName + "_Get]");

            var sqlParameter = new SqlParameter("UserId", SqlDbType.UniqueIdentifier) { Value = userId };
            command.Parameters.Add(sqlParameter);

            return command;
        }

        /// <summary>
        /// Initializes the find by user and location command.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="location">The location.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        protected DbCommand InitializeFindByUserAndLocationCommand(Guid userId, string location, Database db)
        {
            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[p" + TableName + "_Get]");

            var sqlParameter = new SqlParameter("UserId", SqlDbType.UniqueIdentifier) { Value = userId };
            command.Parameters.Add(sqlParameter);

            sqlParameter = new SqlParameter(TableName + "Location", SqlDbType.NVarChar) { Value = location };
            command.Parameters.Add(sqlParameter);

            return command;
        }

        /// <summary>
        /// Initializes the find active by user and location command.
        /// </summary>        
        /// <param name="userId">The user identifier.</param>
        /// <param name="location">The location.</param>
        /// <param name="db">The database.</param>
        /// <returns></returns>
        protected DbCommand InitializeFindActiveByUserAndLocationCommand(Guid userId, string location, Database db)
        {
            bool activeDefault = true;

            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[p" + TableName + "_GetActive]");

            var sqlParameter = new SqlParameter("UserId", SqlDbType.UniqueIdentifier) { Value = userId };
            command.Parameters.Add(sqlParameter);

            sqlParameter = new SqlParameter(TableName + "Location", SqlDbType.NVarChar) { Value = location };
            command.Parameters.Add(sqlParameter);

            sqlParameter = new SqlParameter(TableName + "ActiveDefault", SqlDbType.Bit) { Value = activeDefault };
            command.Parameters.Add(sqlParameter);

            return command;
        }

        /// <summary>
        /// Initializes the find available by user and location command.
        /// </summary>        
        /// <param name="userId">The user identifier.</param>
        /// <param name="location">The location.</param>
        /// <param name="db">The database.</param>
        /// <returns></returns>
        protected DbCommand InitializeFindAvailableByUserAndLocationCommand(Guid userId, string location, Database db)
        {
            bool availableDefault = true;

            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[p" + TableName + "_GetAvailable]");

            var sqlParameter = new SqlParameter("UserId", SqlDbType.UniqueIdentifier) { Value = userId };
            command.Parameters.Add(sqlParameter);

            sqlParameter = new SqlParameter(TableName + "Location", SqlDbType.NVarChar) { Value = location };
            command.Parameters.Add(sqlParameter);

            sqlParameter = new SqlParameter(TableName + "AvailableDefault", SqlDbType.Bit) { Value = availableDefault };
            command.Parameters.Add(sqlParameter);

            return command;
        }

        /// <summary>
        /// Initializes the search command.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        protected DbCommand InitializeSearchCommand(string keyword, Guid userId, Database db)
        {
            var command = (SqlCommand)db.GetStoredProcCommand("[dbo].[p" + TableName + "_Search]");

            var sqlParameter = new SqlParameter("UserId", SqlDbType.UniqueIdentifier) { Value = userId };
            command.Parameters.Add(sqlParameter);

            if(!string.IsNullOrWhiteSpace(keyword))
            {
                sqlParameter = new SqlParameter("Keyword", SqlDbType.NVarChar) { Value = keyword };
                command.Parameters.Add(sqlParameter);                
            }
            return command;
        }
    }
}