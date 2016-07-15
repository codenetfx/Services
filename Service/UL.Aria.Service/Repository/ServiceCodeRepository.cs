using System.Data;
using System.Data.Common;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Repository for <see cref="ServiceCode" />
    /// </summary>
    public class ServiceCodeRepository : LookupCodeRepositoryBase<ServiceCode, ServiceCodeSearchResult, ServiceCodeSearchResultSet>, IServiceCodeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCodeRepository"/> class.
        /// </summary>
        public ServiceCodeRepository() : base(EntityTypeEnumDto.ServiceCode) { }

        /// <summary>
        /// Gets the name of the identifier field.
        /// </summary>
        /// <value>
        /// The name of the identifier field.
        /// </value>
        protected override string IdFieldName
        {
            get { return "ServiceCodeID"; }
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        protected override string TableName
        {
            get { return "ServiceCode"; }
        }
    }
}