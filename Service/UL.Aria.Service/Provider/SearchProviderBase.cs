using System;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Base for a <see cref="TrackedDomainEntity"/> provider.
	/// </summary>
	/// <typeparam name="TDomainEntity">The type of the domain entity.</typeparam>
	public abstract class SearchProviderBase<TDomainEntity> : ISearchProviderBase<TDomainEntity>
		where TDomainEntity : AuditableEntity, IAuditableEntity, ISearchResult, new()
	{
		private readonly IPrincipalResolver _principalResolver;
		private readonly IPrimaryEntityRepository<TDomainEntity> _repository;

		/// <summary>
		/// Initializes a new instance of the <see cref="SearchProviderBase{TDomainEntity}" /> class.
		/// </summary>
		/// <param name="repository">The repository.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		protected SearchProviderBase(IPrimaryEntityRepository<TDomainEntity> repository, IPrincipalResolver principalResolver)
		{
			_repository = repository;
			_principalResolver = principalResolver;
		}

		/// <summary>
		/// Fetches the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>
		/// IndustryCode
		/// </returns>
		public virtual TDomainEntity Fetch(Guid id)
		{
			return _repository.Fetch(id);
		}

		/// <summary>
		/// Creates the specified industry code.
		/// </summary>
		/// <param name="entity">The industry code.</param>
		public virtual void Create(TDomainEntity entity)
		{
			if (!entity.Id.HasValue)
				entity.Id = Guid.NewGuid();
			var currentDateTime = DateTime.UtcNow;
			entity.CreatedById = _principalResolver.UserId;
			entity.CreatedDateTime = currentDateTime;
			entity.UpdatedById = _principalResolver.UserId;
			entity.UpdatedDateTime = currentDateTime;
			_repository.Save(entity);
		}

		/// <summary>
		/// Updates the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="entity">The unit of measure.</param>
		public virtual void Update(Guid id, TDomainEntity entity)
		{
			entity.UpdatedById = _principalResolver.UserId;
			_repository.Save(entity);
		}

		/// <summary>
		/// Deletes the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		public virtual void Delete(Guid id)
		{
			_repository.Delete(id);
		}

		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		public ISearchResultSet<TDomainEntity> Search(ISearchCriteria searchCriteria)
		{
			return _repository.DefaultSearch(searchCriteria);
		}
	}
}