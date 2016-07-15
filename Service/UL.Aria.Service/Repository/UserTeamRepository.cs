using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// User Team Repository
    /// </summary>
    public class UserTeamRepository : PrimaryRepository<UserTeam>, IUserTeamRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserTeamRepository"/> class.
        /// </summary>
        public UserTeamRepository() { }

        /// <summary>
        /// Gets the name of the identifier field.
        /// </summary>
        /// <value>
        /// The name of the identifier field.
        /// </value>
        protected override string IdFieldName
        {
            get { return "UserTeamId"; }
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        protected override string TableName
        {
            get { return "UserTeam"; }
        }

        /// <summary>
        /// Defines the mappings.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        protected override void DefineMappings(IMapperRegistry mapper)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Maps the primary entity to data reader.
        /// </summary>
        /// <param name="expressionChain">The expression chain.</param>
        protected override void MapPrimaryEntityToDataReader(AutoMapper.IMappingExpression<IDataReader, UserTeam> expressionChain)
        {
            
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
        /// Fetches the by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public IEnumerable<UserTeam> FindByUserId(Guid userId)
        {
            return ExecuteReaderCommand(db =>
            {
                var cmd = db.GetStoredProcCommand("[dbo].[pUserTeam_GetByUserId]");
                var param = cmd.CreateParameter();
                param.ParameterName = "UserId";
                param.DbType = DbType.Guid;
                param.Value = userId;
                cmd.Parameters.Add(param);
                return cmd;

            }, ConstructEntity<UserTeam>);
        }



    }
}
