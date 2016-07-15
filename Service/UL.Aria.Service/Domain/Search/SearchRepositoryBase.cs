using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Domain.Search
{
    /// <summary>
    /// Abstract Repository for adding a Base Search functionallity
    /// </summary>
    /// <typeparam name="TSearchDomainEntity">The type of the search domain entity.</typeparam>
    [ExcludeFromCodeCoverage] //remove with direct tests during sprint 0 itertation 2 mike
    public abstract class SearchRepositoryBase<TSearchDomainEntity> : RepositoryBase<TSearchDomainEntity>, ISearchRepositoryBase<TSearchDomainEntity>
        where TSearchDomainEntity : DomainEntity, new()
    {

        private readonly string _tableName;


        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRepositoryBase{T}" /> class.
        /// </summary>
        /// <param name="dbIdFieldName">Name of the database identifier field.</param>
        /// <param name="tableName">Name of the table.</param>
        protected SearchRepositoryBase(string dbIdFieldName, string tableName)
            : base(dbIdFieldName)
        {
            _tableName = tableName;
 
        }



        /// <summary>
        /// When implemented in derived classes, allows for additional situation specific paramters to be added beyond
        /// the minimum parameters handled by the InitializeSearchCommand method.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="command">The command.</param>
        protected virtual void AddSearchParameters(Database db, SearchCriteria searchCriteria, DbCommand command)
        {

        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>ProjectTemplateSearchResultSet.</returns>
        public virtual SearchResultSetBase<T> DefaultSearch<T>(SearchCriteria searchCriteria) where T : class, ISearchResult, new()
        {
            var result = ExecuteSearchCommand<T>(db => InitializeSearchCommand(db, searchCriteria));
            result.SearchCriteria = searchCriteria;
            return result;
        }

        /// <summary>
        /// Initializes the search command.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        protected virtual DbCommand InitializeSearchCommand(Database db, SearchCriteria searchCriteria)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[p" + TableName + "_Search]");

            // only use the first Sort specified         
            var sort = searchCriteria.Sorts.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.FieldName))
                ?? new Sort()
                {
                    FieldName = searchCriteria.SortBy,
                    Order = searchCriteria.SortDirection
                };


            if (!string.IsNullOrWhiteSpace(searchCriteria.Keyword))
            {
                db.AddInParameter(command, "Keyword", DbType.String, searchCriteria.Keyword.Replace("*", "%"));
            }
            if (!string.IsNullOrWhiteSpace(sort.FieldName))
            {
                db.AddInParameter(command, "SortBy", DbType.String, sort.FieldName);
                db.AddInParameter(command, "SortDirection", DbType.String,
                    sort.Order == SortDirection.Descending
                        ? "DESC"
                        : "ASC");
            }

            db.AddInParameter(command, "StartIndex", DbType.Int64, searchCriteria.StartIndex);
            db.AddInParameter(command, "EndIndex", DbType.Int64, searchCriteria.EndIndex);
            AddSearchParameters(db, searchCriteria, command);
            return command;
        }


        /// <summary>
        /// Executes the reader command.
        /// </summary>
        /// <typeparam name="T">Domain entity type</typeparam>
        /// <param name="InitializeCommandDelegate">The initialize command delegate.</param>
        /// <param name="ConstructSearchResultSetDelegate">The construct search result set delegate.</param>
        /// <param name="ConstructRefinersDelegate">The construct refiners delegate.</param>
        /// <returns></returns>
        protected SearchResultSetBase<T> ExecuteSearchCommand<T>(Func<Database, DbCommand> InitializeCommandDelegate,
            Func<IDataReader, SearchResultSetBase<T>> ConstructSearchResultSetDelegate = null,
            Func<IDataReader, Dictionary<string, List<IRefinementItem>>> ConstructRefinersDelegate = null) where T : class, ISearchResult, new()
        {
            SearchResultSetBase<T> resultSet = null;
            List<T> results = null;
            Database database = DatabaseFactory.CreateDatabase();

            using (DbCommand command = InitializeCommandDelegate.Invoke(database))
            {
                using (IDataReader reader = database.ExecuteReader(command))
                {
                    //load primary results set
                    if (ConstructSearchResultSetDelegate == null)
                    {
                        SetMapperConfiguration<T>();

                        results = Mapper.Map<IDataReader, List<T>>(reader);
                        resultSet = new SearchResultSetBase<T>()
                        {
                            Results = results
                        };
                    }
                    else
                    {
                        resultSet = ConstructSearchResultSetDelegate.Invoke(reader);
                    }


                    //load summary if it exists as 2nd result set
                    if (reader.NextResult())
                    {
                        //only one row is expected, additional are ignored.
                        Mapper.Configuration.CreateMap<IDataReader, SearchSummary>();
                        resultSet.Summary = Mapper.Map<IDataReader, List<SearchSummary>>(reader).FirstOrDefault();
                    }


                    //load all refiners if it exists as 3rd result set
                    //mulitple sets should use a union
                    if (reader.NextResult())
                    {
                        if (ConstructRefinersDelegate == null)
                        {
                            var refiners = new List<IRefinementItem>();
                            Mapper.Configuration.CreateMap<IDataReader, RefinementItem>();
                            refiners.AddRange(Mapper.Map<IDataReader, List<RefinementItem>>(reader));
                            resultSet.RefinerResults = refiners.GroupBy(x => x.Name)
                                .ToDictionary(x => x.Key, x => x.ToList());
                        }
                        else
                        {
                            resultSet.RefinerResults = ConstructRefinersDelegate.Invoke(reader);
                        }
                    }
                }

                return resultSet;
            }
        }

        /// <summary>
        /// Sets the mapper configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected virtual void SetMapperConfiguration<T>() where T : ISearchResult
        {
            Mapper.Configuration.CreateMap<IDataReader, T>()
                .ForMember("Id", x => x.MapFrom(y => y.GetValue<Guid>(this.IdFieldName)));
        }

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="assetFieldName">Name of the asset field.</param>
        /// <param name="parameterName">Name of the parameter.</param>       
        protected void AddFilter(DbCommand cmd, SearchCriteria searchCriteria, string assetFieldName,
            string parameterName)
        {
            if (searchCriteria.Filters.ContainsKey(assetFieldName))
            {
                var command = cmd as SqlCommand;
                SqlParameter parameter = command.CreateParameter();
                parameter.SqlDbType = SqlDbType.Structured;
                parameter.ParameterName = parameterName;
                parameter.Value = ConvertToDataTable(searchCriteria.Filters[assetFieldName]);
                parameter.Direction = ParameterDirection.Input;
                command.Parameters.Add(parameter);
            }         
        }


        private static DataTable ConvertToDataTable(IEnumerable<string> filterValues)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("FilterValue");
            foreach (string filterValue in filterValues)
            {
                dataTable.Rows.Add(filterValue);
            }
            return dataTable;
        }

        /// <summary>
        ///     Gets the name of the table.
        /// </summary>
        /// <value>
        ///     The name of the table.
        /// </value>
        protected string TableName
        {
            get { return _tableName; }
        }


    }
}
