using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Repository;
using AutoMapper;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDomainEntity">The type of the domain entity.</typeparam>
    /// <typeparam name="TSearchResult">The type of the search result.</typeparam>
    /// <typeparam name="TSearchResultSet">The type of the search result set.</typeparam>
    public abstract class LookupCodeRepositoryBase<TDomainEntity, TSearchResult, TSearchResultSet>
        : PrimaryRepository<TDomainEntity>
        where TDomainEntity : LookupCodeBase, ISearchResult, new()
        where TSearchResult : LookupCodeSearchResult<TDomainEntity>, new()
        where TSearchResultSet : SearchResultSetBase<TSearchResult>, new()
    {
        private readonly EntityTypeEnumDto _entityType;

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        protected EntityTypeEnumDto EntityType
        {
            get { return _entityType; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupCodeRepositoryBase{TDomainEntity, TSearchResult, TSearchResultSet}" /> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        protected LookupCodeRepositoryBase(EntityTypeEnumDto entityType)          
        {
            _entityType = entityType;
        }

        /// <summary>
        /// Defines the mappings.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        protected override void DefineMappings(Enterprise.Foundation.Mapper.IMapperRegistry mapper)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the search parameters.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="command">The command.</param>
        protected override void AddSearchParameters(Database db, ISearchCriteria searchCriteria, DbCommand command)
        {
            //throw new NotImplementedException();
        }
        
        /// <summary>
        /// Maps the primary entity to data reader.
        /// </summary>
        /// <param name="expressionChain">The expression chain.</param>
        protected override void MapPrimaryEntityToDataReader(IMappingExpression<IDataReader, TDomainEntity> expressionChain)
        {
            expressionChain.ForMember(x => x.Label, x => x.MapFrom(y => y.GetValue<string>("Label")))
                .ForMember(x => x.ExternalId, x => x.MapFrom(y => y.GetValue<string>("ExternalID")));
        }       

        /// <summary>
        /// Finds the by external identifier.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <returns>IndustryCode.</returns>
        /// <exception cref="UL.Enterprise.Foundation.Data.DatabaseItemNotFoundException"></exception>
        public virtual TDomainEntity FindByExternalId(string externalId)
        {
            var result = ExecuteReaderCommand(database => InitializeFindByExternalIdCommand(externalId, database), ConstructEntity<TDomainEntity>);

            if (result.Count == 0)
                throw new DatabaseItemNotFoundException();
            return result.First();
        }

      
        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TDomainEntity> FetchAll()
        {
            return base.ExecuteReaderCommand(db =>
            {
                return InitializeFetchCommand(Guid.Empty, db);
            },
            ConstructEntity<TDomainEntity>);
        }

        /// <summary>
        /// Constructs the search result.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual TSearchResult ConstructSearchResult(IDataReader reader)
        {
            var result = new TSearchResult
            {
                Id = reader.GetValue<Guid>(IdFieldName),
                EntityType = this.EntityType,
                Name = reader.GetValue<string>("ExternalID"),
                Title = reader.GetValue<string>("Label"),
                ChangeDate = reader.GetValue<DateTime>("UpdatedDT"),
                LookupCode = ConstructEntity<TDomainEntity>(reader)
            };

            return result;
        }

        /// <summary>
        /// Initializes the find by external identifier command.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <param name="db">The database.</param>
        /// <returns></returns>
        protected DbCommand InitializeFindByExternalIdCommand(string externalId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[p" + TableName + "_GetByExternalId]");
            db.AddInParameter(command, "@ExternalId", DbType.String, externalId);
            return command;
        }
    }
}