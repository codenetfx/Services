using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// class that facilitates creation, deletion, selection from the database
    /// </summary>
    public sealed class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyRepository" /> class.
        /// </summary>
        /// <param name="transactionFactory">The transaction factory.</param>
        public CompanyRepository(ITransactionFactory transactionFactory) : base("CompanyId")
        {
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        /// Gets the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        Company ICompanyRepository.FetchById(Guid id)
        {
            return FindById(id);
        }

        /// <summary>
        /// Gets the company by external id.
        /// </summary>
        /// <param name="externalId">The external id.</param>
        /// <returns>Company.</returns>
        Company ICompanyRepository.FetchByExternalId(string externalId)
        {
            Guard.IsNotNullOrEmpty(externalId, "externalId");

            Company company;

            using (var transactionScope = _transactionFactory.Create())
            {
                company = GetByCommand(InitializeFetchByExternalIdCommand, externalId).FirstOrDefault();
                transactionScope.Complete();
            }

            return company;
        }

        /// <summary>
        /// Initializes the fetch by external id command.
        /// </summary>
        /// <param name="externalId">The external id.</param>
        /// <param name="db">The db.</param>
        /// <returns>DbCommand.</returns>
        private static DbCommand InitializeFetchByExternalIdCommand(string externalId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCompany_GetByExternalId]");
            db.AddInParameter(command, "@ExternalId", DbType.String, externalId);
            return command;
        }


        /// <summary>
        /// Initializes the fetch external ids by company id.
        /// </summary>
        /// <param name="companyId">The company id.</param>
        /// <param name="db">The db.</param>
        /// <returns></returns>
        private static DbCommand InitializeFetchExternalIdsByCompanyId(Guid companyId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCompanyExternalId_GetByCompanyId]");
            db.AddInParameter(command, "@companyId", DbType.Guid, companyId);
            return command;
        }

        /// <summary>
        /// Publishes the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns>
        /// The published company
        /// </returns>
        Company ICompanyRepository.Create(Company company)
        {
            try
            {
                Company createdCompany;
                using (var transactionScope = _transactionFactory.Create())
                {
                    createdCompany = Insert(company);
                    ExecuteCommand(InitializeDeleteCompanyExternalIdCommand, company.Id.Value);
                    InsertCompanyExternalIds(company.Id.Value, company.ExternalIds);
                    createdCompany.ExternalIds = company.ExternalIds;
                    transactionScope.Complete();
                }
                return createdCompany;
            }
            catch (System.Data.SqlClient.SqlException exception)
            {
                if (exception.State == 2)
                    throw new DatabaseItemExistsException();
                throw;
            }
        }

        private void InsertCompanyExternalIds(Guid companyId, IEnumerable<string> externalIds)
        {
            var db = DatabaseFactory.CreateDatabase();

            foreach (var externalId in externalIds)
            {
                var command = InitializeInsertCompanyExternalIdCommand(companyId, externalId, db);

                using (command)
                {
                    db.ExecuteNonQuery(command);
                }
            }
        }

        private DbCommand InitializeInsertCompanyExternalIdCommand(Guid companyId, string externalId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCompanyExternalId_Insert]");

            db.AddInParameter(command, "@CompanyId", DbType.Guid, companyId);
            db.AddInParameter(command, "@ExternalId", DbType.String, externalId);

            return command;
        }

        private DbCommand InitializeDeleteCompanyExternalIdCommand(Guid entityId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCompanyExternalId_Delete]");

            db.AddInParameter(command, "@CompanyId", DbType.Guid, entityId);

            return command;
        }

        /// <summary>
        /// Updates the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        Company ICompanyRepository.Update(Company company)
        {
            Update(company);
            return company;
        }

        /// <summary>
        /// Deletes the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        void ICompanyRepository.Delete(Guid id)
        {
            Remove(id);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IList<Company> ICompanyRepository.FetchAll()
        {
            return FindAll();
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
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        public override IList<Company> FindAll()
        {
            IList<Company> companies;

            using (var transactionScope = _transactionFactory.Create())
            {
                companies = GetByCommandAll(InitializeFetchAllCommand);
                transactionScope.Complete();
            }

            var db = DatabaseFactory.CreateDatabase();

            foreach (Company company in companies)
            {
                GetExternalIdsByCompanyId(company, db);
            }

            return companies;
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        public CompanySearchResultSet Search(SearchCriteria searchCriteria)
        {
            Database db = DatabaseFactory.CreateDatabase();

            var set = new CompanySearchResultSet { SearchCriteria = searchCriteria };
            using (DbCommand command = InitializeSearchCommand(db, searchCriteria))
            {
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    ConstructSearchResultSet(reader, set);
                   
                }
            }
            return set;
        }

        private void ConstructSearchResultSet(IDataReader reader, CompanySearchResultSet set)
        {
            
            set.Summary = new SearchSummary();
            long? start = null;
            long? end = null;
            
            while (reader.Read())
            {
                set.Summary.TotalResults = reader.GetValue<long>("TotalRows");
                //returned on each row for convenience. will be same value on each.
                long row = reader.GetValue<long>("RowNumber") - 1;
                start = start ?? row;
                end = end ?? row;
                end = row > end ? row : end;
                start = row < start ? row : start;
                set.Results.Add(ConstructSearchResult(reader));
            }
            set.Summary.StartIndex = start ?? 0;
            set.Summary.EndIndex = end ?? 0;
            reader.NextResult();
            while (reader.Read())
            {
                var companyId = reader.GetValue<Guid>("CompanyId");
                var tempCompany = set.Results.FirstOrDefault(c => c.Id == companyId);
                if (null != tempCompany)
                {
                    tempCompany.Company.ExternalIds.Add(reader.GetValue<string>("ExternalId"));
                }
            }
        }

        private CompanySearchResult ConstructSearchResult(IDataReader reader)
        {
            var result = new CompanySearchResult
            {
                Id = reader.GetValue<Guid>("CompanyId"),
                EntityType = EntityTypeEnumDto.Company,
                Name = reader.GetValue<string>("Name"),
                Title = reader.GetValue<string>("Name"),
                ChangeDate = reader.GetValue<DateTime>("UpdatedOn"),
                Company=ConstructCompany(reader)
            };

            return result;
        }

        private static void GetExternalIdsByCompanyId(Company company, Database db)
        {
            DbCommand command = InitializeFetchExternalIdsByCompanyId(company.Id ?? Guid.Empty, db);
            List<string> externalIds = GetExternalIdsByCommand(command);
            externalIds.ForEach(company.ExternalIds.Add);
        }

        /// <summary>
        /// Initializes the ge all command.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <returns>DbCommand.</returns>
        private static DbCommand InitializeFetchAllCommand(Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCompany_SelectAll]");
            return command;
        }

        private static DbCommand InitializeFetchCountAllCommand(Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCompany_GetAllCount]");
            return command;
        }

	    /// <summary>
        /// Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        public override Company FindById(Guid entityId)
        {
            Guard.IsNotEmptyGuid(entityId, "CompanyId");

	        Company company;
	        using (var transactionScope = _transactionFactory.Create())
	        {
	            company = GetByCommand(InitializeFindByIdCommand, entityId).FirstOrDefault();

	            if (company == null)
	                throw new DatabaseItemNotFoundException("company not found", new Exception());

                transactionScope.Complete();
	        }

	        return company;
        }

        /// <summary>
        /// Initializes the find by id command.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <param name="db">The db.</param>
        /// <returns>DbCommand.</returns>
        private static DbCommand InitializeFindByIdCommand(Guid entityId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCompany_Get]");
            db.AddInParameter(command, "@CompanyId", DbType.Guid, entityId);
            return command;
        }

	    /// <summary>
	    /// Adds the specified entity.
	    /// </summary>
	    /// <param name="entity">The entity.</param>
	    public override void Add(Company entity)
	    {
			Insert(entity);
	    }


	    private Company Insert(Company entity)
        {
			Guard.IsNotEmptyGuid(entity.Id.Value, "entityId");

	        Company company;

	        using (var transactionScope = _transactionFactory.Create())
	        {
	            company = ExecuteGetByCommand(InitializeInsertCommand, entity);
                transactionScope.Complete();
	        }

	        return company;
        }

        /// <summary>
        /// Initializes the add command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="db">The db.</param>
        /// <returns>DbCommand.</returns>
        private DbCommand InitializeInsertCommand(Company entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCompany_Insert]");

            db.AddInParameter(command, "@CompanyId", DbType.Guid, entity.Id.Value);
            db.AddInParameter(command, "@Name", DbType.String, entity.Name);
            db.AddInParameter(command, "@Description", DbType.String, entity.Description);
            db.AddInParameter(command, "@CreatedOn", DbType.DateTime2, entity.CreatedDateTime.DefaultToUtcNow());
            db.AddInParameter(command, "@CreatedBy", DbType.Guid, entity.CreatedById);
            db.AddInParameter(command, "@UpdatedBy", DbType.Guid, entity.UpdatedById);
            db.AddInParameter(command, "@UpdatedOn", DbType.DateTime2, entity.UpdatedDateTime.DefaultToUtcNow());
            db.AddInParameter(command, "@ExternalIds", DbType.String, string.Join(",", entity.ExternalIds));

            return command;
        }

        private static DbCommand InitializeSearchCommand(Database db, SearchCriteria searchCriteria)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pCompany_Search]");

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
                db.AddInParameter(command, "Keyword", DbType.String, searchCriteria.Keyword.Replace("*", "%"));
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                db.AddInParameter(command, "SortBy", DbType.String, sortBy);
            }
            db.AddInParameter(command, "SortDirection", DbType.String,
                sortDirection == SortDirection.Descending ? "DESC" : "ASC");
            db.AddInParameter(command, "StartIndex", DbType.Int64, searchCriteria.StartIndex);
            db.AddInParameter(command, "EndIndex", DbType.Int64, searchCriteria.EndIndex);
            if (searchCriteria.Filters.ContainsKey("ExcludeDescription"))
            {
                db.AddInParameter(command, "@ExcludeDescription", DbType.Boolean, true);    
            }
            return command;
        }


        //Name, 
        //    [Description], 
        //    CreatedOn, 
        //    CreatedBy, 
        //    UpdatedOn, 
        //    UpdatedBy
        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override int Update(Company entity)
        {
            Guard.IsNotEmptyGuid(entity.Id.Value, "entityId");

            int count;
            using (var transactionScope = _transactionFactory.Create())
            {
                count = ExecuteCommand(InitializeUpdateCommand, entity);
                ExecuteCommand(InitializeDeleteCompanyExternalIdCommand, entity.Id.Value);
                InsertCompanyExternalIds(entity.Id.Value, entity.ExternalIds);
                transactionScope.Complete();
            }

            return count;
        }

        /// <summary>
        /// Initializes the update command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="db">The db.</param>
        /// <returns>DbCommand.</returns>
        private DbCommand InitializeUpdateCommand(Company entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCompany_Update]");

            db.AddInParameter(command, "@CompanyId", DbType.Guid, entity.Id.Value);
            db.AddInParameter(command, "@Name", DbType.String, entity.Name);
            db.AddInParameter(command, "@Description", DbType.String, entity.Description);
            db.AddInParameter(command, "@UpdatedBy", DbType.Guid, entity.UpdatedById);
            db.AddInParameter(command, "@UpdatedOn", DbType.DateTime2, entity.UpdatedDateTime.DefaultToUtcNow());

            return command;
        }

        /// <summary>
        /// Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        public override void Remove(Guid entityId)
        {
            Guard.IsNotEmptyGuid(entityId, "entityId");
            using (var transactionScope = _transactionFactory.Create())
            {
                ExecuteCommand(InitializeRemoveCommand, entityId);
                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Initializes the remove command.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <param name="db">The db.</param>
        /// <returns>DbCommand.</returns>
        private DbCommand InitializeRemoveCommand(Guid entityId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pCompany_Delete]");

            db.AddInParameter(command, "@CompanyId", DbType.Guid, entityId);

            return command;
        }

        /// <summary>
        /// Constructs the company.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Company.</returns>
        private static Company ConstructCompanyWithExternalIds(IDataReader reader)
        {
            var company = ConstructCompany(reader);

            reader.NextResult();
            while (reader.Read())
            {
                company.ExternalIds.Add(reader.GetValue<string>("ExternalId"));    
            }

            return company;
        }

        /// <summary>
        /// Constructs the company.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Company.</returns>
        private static Company ConstructCompany(IDataReader reader)
        {
            var company = new Company
            {
                Id = reader.GetValue<Guid>("CompanyId"),
                Name = reader.GetValue<string>("Name"),
                Description = reader.GetValue<string>("Description"),
                CreatedById = reader.GetValue<Guid>("CreatedBy"),
                CreatedDateTime = reader.GetValue<DateTime>("CreatedOn"),
                UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedOn")
            };

            return company;
        }

        /// <summary>
        /// Gets the by command.
        /// </summary>
        /// <param name="initializeCommand">The initialize command.</param>
        /// <returns>List{Company}.</returns>
        private static List<Company> GetByCommand(Func<Database, DbCommand> initializeCommand)
        {
            var db = DatabaseFactory.CreateDatabase();
            var results = new List<Company>();

            using (var command = initializeCommand(db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        results.Add(ConstructCompanyWithExternalIds(reader));
                    }
                }
            }
            return results;
        }


        private static List<string> GetExternalIdsByCommand(DbCommand command)
        {
            var db = DatabaseFactory.CreateDatabase();
            var results = new List<string>();

            using (command)
            {
                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        results.Add(reader.GetValue<string>("ExternalId"));
                    }
                }
            }

            return results;
        }

        private static List<Company> GetByCommandAll(Func<Database, DbCommand> initializeCommand)
        {
            var db = DatabaseFactory.CreateDatabase();
            var results = new List<Company>();

            using (var command = initializeCommand(db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        results.Add(ConstructCompany(reader));
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// Gets the by command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="initializeCommand">The initialize command.</param>
        /// <param name="id">The id.</param>
        /// <returns>IEnumerable{Company}.</returns>
        private static IEnumerable<Company> GetByCommand<T>(Func<T, Database, DbCommand> initializeCommand, T id)
        {
            var db = DatabaseFactory.CreateDatabase();
            var results = new List<Company>();

            using (var command = initializeCommand(id, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        results.Add(ConstructCompanyWithExternalIds(reader));
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// Gets the by command.
        /// </summary>
        /// <typeparam name="TEntity">The type of the T entity.</typeparam>
        /// <param name="initializeCommand">The initialize command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>IEnumerable{Company}.</returns>
        private static Company ExecuteGetByCommand<TEntity>(Func<TEntity, Database, DbCommand> initializeCommand, TEntity entity)
        {
            var db = DatabaseFactory.CreateDatabase();

            using (var command = initializeCommand(entity, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        return ConstructCompanyWithExternalIds(reader);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <typeparam name="TEntity">The type of the T entity.</typeparam>
        /// <param name="commandInitializer">The command initializer.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="afterExecute">The after execute.</param>
        /// <returns>System.Int32.</returns>
        private static int ExecuteCommand<TEntity>(Func<TEntity, Database, DbCommand> commandInitializer, TEntity entity, Action<DbCommand> afterExecute = null)
        {
            int count;
            var db = DatabaseFactory.CreateDatabase();
            var command = commandInitializer(entity, db);

            using (command)
            {
                count = db.ExecuteNonQuery(command);
            }
            if (afterExecute != null)
                afterExecute(command);

            return count;
        }
    }
}