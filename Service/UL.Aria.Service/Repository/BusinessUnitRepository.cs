using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using AutoMapper;

using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Configuration;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Repository for <see cref="BusinessUnit" />
	/// </summary>
	public class BusinessUnitRepository : Domain.Repository.AssociatedPrimaryRepository<BusinessUnit>,
		IBusinessUnitRepository
	{
	    private readonly IServiceConfiguration _serviceConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessUnitRepository" /> class.
        /// </summary>
        /// <param name="serviceConfiguration">The service configuration.</param>
	    public BusinessUnitRepository(IServiceConfiguration serviceConfiguration)
	    {
	        _serviceConfiguration = serviceConfiguration;
	    }

	    /// <summary>
		/// Gets the name of the identifier field.
		/// </summary>
		/// <value>
		/// The name of the identifier field.
		/// </value>
		protected override string IdFieldName
		{
			get { return "BusinessUnitId"; }
		}

		/// <summary>
		/// Gets the name of the table.
		/// </summary>
		/// <value>
		/// The name of the table.
		/// </value>
		protected override string TableName
		{
			get { return "BusinessUnit"; }
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
		/// Fetches all.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<BusinessUnit> FetchAll()
		{
			return ExecuteReaderCommand(db => InitializeFetchCommand(Guid.Empty, db),
				ConstructEntity<BusinessUnit>);
		}

		/// <summary>
		/// Defines the mappings.
		/// </summary>
		/// <param name="mapper">The mapper.</param>
		protected override void DefineMappings(Enterprise.Foundation.Mapper.IMapperRegistry mapper)
		{
			//no opp
		}

		/// <summary>
		/// Maps the primary entity to data reader.
		/// </summary>
		/// <param name="expressionChain">The expression chain.</param>
		protected override void MapPrimaryEntityToDataReader(IMappingExpression<IDataReader, BusinessUnit> expressionChain)
		{
			expressionChain.ForMember(x => x.Name, x => x.MapFrom(y => y.GetValue<string>("BusinessUnitName")))
				.ForMember(x => x.Code, x => x.MapFrom(y => y.GetValue<string>("BusinessUnitCode")))
                .ForMember(x => x.IsDeletePrevented, opt => opt.MapFrom(y => y.GetValue<Guid>("BusinessUnitId") == _serviceConfiguration.AllBusinessUnitId))
                ;
		}

		/// <summary>
		/// Maps the primary entity to data row.
		/// </summary>
		/// <param name="src">The source.</param>
		/// <param name="dest">The dest.</param>
		protected override void MapPrimaryEntityToDataRow(BusinessUnit src, DataRow dest)
		{
			base.MapPrimaryEntityToDataRow(src, dest);
			dest["BusinessUnitName"] = src.Name;
			dest["BusinessUnitCode"] = src.Code;
			dest["Note"] = src.Note;
		}

		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		public ISearchResultSet<BusinessUnit> Search(SearchCriteria searchCriteria)
		{
			return DefaultSearch(searchCriteria);
		}

		/// <summary>
		/// When implemented in derived classes, allows for additional situation specific paramters to be added beyond
		/// the minimum parameters handled by the InitializeSearchCommand method.
		/// </summary>
		/// <param name="db">The database.</param>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <param name="command">The command.</param>
		protected override void AddSearchParameters(Database db, ISearchCriteria searchCriteria, DbCommand command)
		{
			db.AddInParameter(command, "IncludeDeleted", DbType.Boolean, false);
		}

		/// <summary>
		/// Fetches the business unit by entity.
		/// </summary>
		/// <param name="parentId">The parent identifier.</param>
		/// <returns></returns>
		public IEnumerable<BusinessUnit> FetchBusinessUnitByEntity(Guid parentId)
		{
			var businessUnits = new List<BusinessUnit>();
			var db = DatabaseFactory.CreateDatabase();

			using (var command = InitializeGetBusinessUnitByEntityCommand(db, parentId))
			{
				using (var reader = db.ExecuteReader(command))
				{
					while (reader.Read())
					{
						var link = ConstructEntity<BusinessUnit>(reader);
						businessUnits.Add(link);
					}
				}
			}

			return businessUnits;
		}

		private static DbCommand InitializeGetBusinessUnitByEntityCommand(Database db, Guid parentId)
		{
			var cmd = db.GetStoredProcCommand("[dbo].[pBusinessUnit_GetByEntity]");
			var param = cmd.CreateParameter();
			param.ParameterName = "ParentId";
			param.DbType = DbType.Guid;
			param.Value = parentId;
			cmd.Parameters.Add(param);
			return cmd;
		}
	}
}