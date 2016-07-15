using System.Data;
using System.Data.Common;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;
using System.Collections.Generic;
using System;
using AutoMapper;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Repository for <see cref="LocationCode" />
    /// </summary>
    public class LocationCodeRepository : Domain.Repository.PrimaryRepository<LocationCode>, ILocationCodeRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LocationCodeRepository" /> class.
        /// </summary>
        public LocationCodeRepository()           
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
            get { return "LocationCodeID"; }
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        protected override string TableName
        {
            get { return "LocationCode"; }
        }

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LocationCode> FetchAll()
        {
            return base.ExecuteReaderCommand(db =>
            {
                return InitializeFetchCommand(Guid.Empty, db);
            },
            ConstructEntity<LocationCode>);
        }


        /// <summary>
        /// Maps the primary entity to data reader.
        /// </summary>
        /// <param name="expressionChain">The expression chain.</param>
        protected override void MapPrimaryEntityToDataReader(IMappingExpression<IDataReader, LocationCode> expressionChain)
        {
            expressionChain.ForMember(x => x.Label, x => x.MapFrom(y => y.GetValue<string>("Label")))
                .ForMember(x => x.ExternalId, x => x.MapFrom(y => y.GetValue<string>("ExternalID")));
        }       

        /// <summary>
        /// Adds the search parameters.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="command">The command.</param>
        protected override void AddSearchParameters(Database db, ISearchCriteria searchCriteria, DbCommand command)
        {
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// Defines the mappings.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        protected override void DefineMappings(Enterprise.Foundation.Mapper.IMapperRegistry mapper)
        {
            //throw new System.NotImplementedException();
        }

		/// <summary>
		/// Finds the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>LocationCode.</returns>
		/// <exception cref="UL.Enterprise.Foundation.Data.DatabaseItemNotFoundException"></exception>
		public LocationCode FindByExternalId(string externalId)
		{
			var result = ExecuteReaderCommand(database => InitializeFindByExternalIdCommand(externalId, database), ConstructEntity<LocationCode>);

			if (result.Count == 0)
				throw new DatabaseItemNotFoundException();
			return result.First();
		}

		private static DbCommand InitializeFindByExternalIdCommand(string externalId, Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[pLocationCode_GetByExternalId]");
			db.AddInParameter(command, "@ExternalId", DbType.String, externalId);
			return command;
		}
	}
}