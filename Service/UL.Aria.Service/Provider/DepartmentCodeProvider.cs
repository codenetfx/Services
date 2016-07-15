using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	///     provider for <see cref="DepartmentCode"/> entities.
	/// </summary>
	public class DepartmentCodeProvider : SearchProviderBase<DepartmentCode>, IDepartmentCodeProvider
	{
		private readonly IDepartmentCodeRepository _repository;

		/// <summary>
		/// Initializes a new instance of the <see cref="DepartmentCodeProvider" /> class.
		/// </summary>
		/// <param name="repository">The repository.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		public DepartmentCodeProvider(IDepartmentCodeRepository repository, IPrincipalResolver principalResolver)
			: base(repository, principalResolver)
		{
			_repository = repository;
		}

		/// <summary>
		///     Gets all.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<DepartmentCode> FetchAll()
		{
			return _repository.FetchAll();
		}

		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>ServiceCode.</returns>
		public DepartmentCode FetchByExternalId(string externalId)
		{
			return _repository.FindByExternalId(externalId);
		}
	}
}