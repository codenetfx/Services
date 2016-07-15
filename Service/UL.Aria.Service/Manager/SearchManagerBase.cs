using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Common base for managers with search capabilties.
    /// </summary>
    /// <typeparam name="TDomainEntity">The type of the domain entity.</typeparam>
    public abstract class SearchManagerBase<TDomainEntity>
        :ManagerBase<TDomainEntity>, ISearchManagerBase<TDomainEntity>
        where TDomainEntity : AuditableEntity, IAuditableEntity, ISearchResult, new()
        
    {
        private readonly ISearchProviderBase<TDomainEntity> _provider;

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual ISearchResultSet<TDomainEntity> Search(ISearchCriteria searchCriteria)
        {
            return _provider.Search(searchCriteria);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchManagerBase{TDomainEntity}" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        protected SearchManagerBase(ISearchProviderBase<TDomainEntity> provider)
            : base(provider)
        {
            _provider = provider;
        }
    }
}