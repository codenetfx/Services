using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// User Team Repository
    /// </summary>
    public class ProjectProjectTemplateRepository : Domain.Repository.AssociatedPrimaryRepository<ProjectProjectTemplate>, IProjectProjectTemplateRepository
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectProjectTemplateRepository"/> class.
        /// </summary>
        public ProjectProjectTemplateRepository() { }


        /// <summary>
        /// Gets the name of the identifier field.
        /// </summary>
        /// <value>
        /// The name of the identifier field.
        /// </value>
        protected override string IdFieldName
        {
            get { return "ProjectProjectTemplateId"; }
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        protected override string TableName
        {
            get { return "ProjectProjectTemplate"; }
        }


        /// <summary>
        /// Gets the name of the group parameter.
        /// </summary>
        /// <value>
        /// The name of the group parameter.
        /// </value>
        protected override string GroupParameterName
        {
            get { return "ProjectId"; }
        }

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProjectProjectTemplate> FetchAll()
        {
            return ExecuteReaderCommand(db => InitializeFetchCommand(Guid.Empty, db),
                ConstructEntity<ProjectProjectTemplate>);
        }

        /// <summary>
        /// Defines the mappings.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        protected override void DefineMappings(IMapperRegistry mapper)
        {

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
            
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressionChain"></param>
        protected override void MapPrimaryEntityToDataReader(IMappingExpression<IDataReader, ProjectProjectTemplate> expressionChain)
        {
            
        }
    }
}
