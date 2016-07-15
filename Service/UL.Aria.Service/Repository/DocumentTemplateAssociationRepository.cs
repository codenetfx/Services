using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Document Template Association repository class
    /// </summary>
    public class DocumentTemplateAssociationRepository : AssocationRepository<DocumentTemplateAssociation>, IDocumentTemplateAssociationRepository
    {
        /// <summary>
        /// Gets the name of the group parameter.
        /// </summary>
        /// <value>
        /// The name of the group parameter.
        /// </value>
        protected override string GroupParameterName
        {
            get { return "ParentId"; }
        }

        /// <summary>
        /// Adds the search parameters.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="command">The command.</param>
        protected override void AddSearchParameters(Database db, ISearchCriteria searchCriteria, DbCommand command)
        {
        }

        /// <summary>
        /// Defines the mappings.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        protected override void DefineMappings(IMapperRegistry mapper)
        {
            mapper.Configuration.CreateMap<IDataReader, ItemUsage>();
        }

        /// <summary>
        /// Gets the name of the identifier field.
        /// </summary>
        /// <value>
        /// The name of the identifier field.
        /// </value>
        protected override string IdFieldName
        {
            get { return "DocumentTemplateId"; }
        }

        /// <summary>
        /// Maps the primary entity to data reader.
        /// </summary>
        /// <param name="expressionChain">The expression chain.</param>
        protected override void MapPrimaryEntityToDataReader(IMappingExpression<IDataReader, DocumentTemplateAssociation> expressionChain)
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
            get { return "DocumentTemplateAssociation"; }
        }
    }
}
