using System.Data;
using System.Data.Common;

using AutoMapper;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Domain.Repository;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;
using System.Collections.Generic;
using System;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class DocumentTemplateRepository.
	/// </summary>
	public class DocumentTemplateRepository : AssociatedPrimaryRepository<DocumentTemplate>,
		IDocumentTemplateRepository
	{
		/// <summary>
		/// Gets the name of the identifier field.
		/// </summary>
		/// <value>The name of the identifier field.</value>
		protected override string IdFieldName
		{
			get { return "DocumentTemplateId"; }
		}

		/// <summary>
		/// Gets the name of the table.
		/// </summary>
		/// <value>
		/// The name of the table.
		/// </value>
		protected override string TableName
		{
			get { return "DocumentTemplate"; }
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
		/// Defines the mappings.
		/// </summary>
		/// <param name="mapper">The mapper.</param>
		protected override void DefineMappings(Enterprise.Foundation.Mapper.IMapperRegistry mapper)
		{
			//no opp
            mapper.Configuration.CreateMap<IDataReader, ItemUsage>();
		}

		/// <summary>
		/// Maps the primary entity to data reader.
		/// </summary>
		/// <param name="expressionChain">The expression chain.</param>
		protected override void MapPrimaryEntityToDataReader(IMappingExpression<IDataReader, DocumentTemplate> expressionChain)
		{
		}

		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		public ISearchResultSet<DocumentTemplate> Search(SearchCriteria searchCriteria)
		{
			return DefaultSearch(searchCriteria);
		}

		/// <summary>
		/// When implemented in derived classes, allows for additional situation specific parameters to be added beyond
		/// the minimum parameters handled by the InitializeSearchCommand method.
		/// </summary>
		/// <param name="db">The database.</param>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <param name="command">The command.</param>
		protected override void AddSearchParameters(Database db, ISearchCriteria searchCriteria, DbCommand command)
		{
			db.AddInParameter(command, "IncludeDeleted", DbType.Boolean, true);
		}

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
	    public IEnumerable<DocumentTemplate> FetchAll()
        {
            var documentTemplates = new List<DocumentTemplate>();
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeFetchAll(db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        var lookup = ConstructDocumentTemplateLookups(reader);
                        documentTemplates.Add(lookup);
                    }
                }
            }

            return documentTemplates;
        }

        /// <summary>
        /// Initializes the get lookups.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <returns></returns>
	    private static DbCommand InitializeFetchAll(Database db)
	    {
	        var command = db.GetStoredProcCommand("[dbo].[pDocumentTemplate_GetAll]");
	        return command;
	    }

        /// <summary>
        /// Constructs the document template lookups.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
	    private static DocumentTemplate ConstructDocumentTemplateLookups(IDataReader reader)
	    {
	        return new DocumentTemplate
	        {
	            Id = reader.GetValue<Guid>("DocumentTemplateId"),
                FileName = reader.GetValue<string>("FileName")
	        };
	    }
        /// <summary>
        /// Fetches the document templates by entity.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns></returns>
        public IEnumerable<DocumentTemplate> FetchDocumentTemplatesByEntity(Guid entityId)
        {
            return ExecuteReaderCommand(db =>
            {
                var cmd = db.GetStoredProcCommand("dbo.pDocumentTemplate_GetByEntity");
                db.AddInParameter(cmd, "ParentId", DbType.Guid, entityId);
                return cmd;
            }, ConstructEntity<DocumentTemplate>);
        }
	}
}