using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using AutoMapper;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// </summary>
    public class LinkRepository : AssociatedPrimaryRepository<Link>,
        ILinkRepository
    {
        /// <summary>
        /// Gets the name of the identifier field.
        /// </summary>
        /// <value>
        /// The name of the identifier field.
        /// </value>
        protected override string IdFieldName
        {
            get { return "LinkId"; }
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        protected override string TableName
        {
            get { return "Link"; }
        }

        /// <summary>
        /// Gets the name of the group parameter.
        /// </summary>
        /// <value>
        /// The name of the group parameter.
        /// </value>
        protected override string GroupParameterName
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public IList<Link> FindAll()
        {
            throw new NotSupportedException();
       //     return ExecuteReaderCommand(db =>
       //     {
       //         var cmd = db.GetStoredProcCommand("dbo.pLink_GetByEntity");
       //         return cmd;
       //     },
       //ConstructEntity<Link>);
        }

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns></returns>
        public Link FindById(Guid entityId)
        {
            return FetchEntityById<Link>(entityId);
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Add(Link entity)
        {
            base.Save(entity);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public int Update(Link entity)
        {
            base.Save(entity);
            return 1;
        }

        /// <summary>
        /// Removes the specified entity identifier.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        public void Remove(Guid entityId)
        {
            Delete(entityId);
        }

        /// <summary>
        /// Fetches the links by entity.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns></returns>
        public IEnumerable<Link> FetchLinksByEntity(Guid entityId)
        {
            return ExecuteReaderCommand(db =>
            {
                var cmd = db.GetStoredProcCommand("dbo.pLink_GetByEntity");
                db.AddInParameter(cmd, "ParentId", DbType.Guid, entityId);
                return cmd;
            },
                ConstructEntity<Link>);
        }

        /// <summary>
        /// Searches the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public ISearchResultSet<Link> Search(SearchCriteria criteria)
        {
            return DefaultSearch(criteria);
        }

        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Lookup> GetLookups()
        {
            return ExecuteReaderCommand(db =>
            {
                var cmd = db.GetStoredProcCommand("dbo.pLink_GetLookups");
                return cmd;
            },
                ConstructEntity<Lookup>);
        }

        /// <summary>
        /// Maps the primary entity to data reader.
        /// </summary>
        /// <param name="expressionChain">The expression chain.</param>
        protected override void MapPrimaryEntityToDataReader(IMappingExpression<IDataReader, Link> expressionChain)
        {
        }

        /// <summary>
        /// Defines the mappings.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        protected override void DefineMappings(IMapperRegistry mapper)
        {
            mapper.Configuration.CreateMap<IDataReader, Lookup>();
        }

        /// <summary>
        /// Adds the search parameters.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="command">The command.</param>
        protected override void AddSearchParameters(Database db, ISearchCriteria searchCriteria, DbCommand command)
        {
            db.AddInParameter(command, "IncludeDeleted", DbType.Boolean, true);
        }
    }
}