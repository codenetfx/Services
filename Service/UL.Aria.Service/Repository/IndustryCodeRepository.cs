using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Repository for <see cref="IndustryCode" />
    /// </summary>
    public class IndustryCodeRepository : LookupCodeRepositoryBase<IndustryCode, IndustryCodeSearchResult, IndustryCodeSearchResultSet>, IIndustryCodeRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IndustryCodeRepository" /> class.
        /// </summary>
        public IndustryCodeRepository() : base(EntityTypeEnumDto.IndustryCode) { }

        /// <summary>
        /// Gets the name of the identifier field.
        /// </summary>
        /// <value>
        /// The name of the identifier field.
        /// </value>
        protected override string IdFieldName
        {
            get { return "IndustryCodeID"; }
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        protected override string TableName
        {
            get { return "IndustryCode"; }
        }
    }

}
