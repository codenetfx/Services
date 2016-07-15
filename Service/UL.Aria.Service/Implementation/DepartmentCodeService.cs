using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
	/// <summary>
	///     Class DepartmentCodeService
	/// </summary>
	[AutoRegisterRestService("DepartmentCode")]
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
		InstanceContextMode = InstanceContextMode.PerCall)]
	public class DepartmentCodeService : IDepartmentCodeService
	{
		private readonly IDepartmentCodeProvider _departmentCodeProvider;
		private readonly IMapperRegistry _mapperRegistry;
		private readonly ITransactionFactory _transactionFactory;

		/// <summary>
		///     Initializes a new instance of the <see cref="DepartmentCodeService" /> class.
		/// </summary>
		/// <param name="departmentCodeProvider">The Department code provider.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		/// <param name="transactionFactory">The transaction factory.</param>
		public DepartmentCodeService(IDepartmentCodeProvider departmentCodeProvider, IMapperRegistry mapperRegistry,
			ITransactionFactory transactionFactory)
		{
			_departmentCodeProvider = departmentCodeProvider;
			_mapperRegistry = mapperRegistry;
			_transactionFactory = transactionFactory;
		}

		/// <summary>
		///     Fetches all.
		/// </summary>
		/// <returns>IList{DepartmentCodeDto}.</returns>
		public IList<DepartmentCodeDto> FetchAll()
		{
			return _departmentCodeProvider.FetchAll().Select(_mapperRegistry.Map<DepartmentCodeDto>).ToList();
		}

		/// <summary>
		///     Fetches the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>DepartmentCodeDto</returns>
		public DepartmentCodeDto Fetch(string id)
		{
			Guard.IsNotNullOrEmpty(id, "Id");
			var convertedId = id.ToGuid();
			Guard.IsNotEmptyGuid(convertedId, "Id");

			var departmentCode = _departmentCodeProvider.Fetch(id.ToGuid());

			return _mapperRegistry.Map<DepartmentCodeDto>(departmentCode);
		}

		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>DepartmentCodeDto.</returns>
		public DepartmentCodeDto FetchByExternalId(string externalId)
		{
			Guard.IsNotNullOrEmpty(externalId, "externalId");

			var departmentCode = _departmentCodeProvider.FetchByExternalId(externalId.DecodeFrom64());

			return _mapperRegistry.Map<DepartmentCodeDto>(departmentCode);
		}

		/// <summary>
		///     Creates the specified Department code.
		/// </summary>
		/// <param name="departmentCode">The Department code.</param>
		public void Create(DepartmentCodeDto departmentCode)
		{
			Guard.IsNotNull(departmentCode, "DepartmentCode");

			var departmentCodeBo = _mapperRegistry.Map<DepartmentCode>(departmentCode);

			using (var transactionScope = _transactionFactory.Create())
			{
				_departmentCodeProvider.Create(departmentCodeBo);
				transactionScope.Complete();
			}
		}

		/// <summary>
		///     Updates the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="departmentCode">The Department code.</param>
		public void Update(string id, DepartmentCodeDto departmentCode)
		{
			Guard.IsNotNullOrEmpty(id, "Id");
			var convertedId = id.ToGuid();
			Guard.IsNotEmptyGuid(convertedId, "Id");
			Guard.IsNotNull(departmentCode, "DepartmentCode");

			var departmentCodeBo = _mapperRegistry.Map<DepartmentCode>(departmentCode);

			using (var transactionScope = _transactionFactory.Create())
			{
				_departmentCodeProvider.Update(id.ToGuid(), departmentCodeBo);
				transactionScope.Complete();
			}
		}

		/// <summary>
		///     Deletes the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		public void Delete(string id)
		{
			Guard.IsNotNullOrEmpty(id, "Id");
			var convertedId = id.ToGuid();
			Guard.IsNotEmptyGuid(convertedId, "Id");

			using (var transactionScope = _transactionFactory.Create())
			{
				_departmentCodeProvider.Delete(convertedId);
				transactionScope.Complete();
			}
		}

		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		public LookupCodeSearchResultSetDto Search(SearchCriteriaDto searchCriteria)
		{
			Guard.IsNotNull(searchCriteria, "searchCriteria");

			var criteria = _mapperRegistry.Map<SearchCriteria>(searchCriteria);
			return _mapperRegistry.Map<LookupCodeSearchResultSetDto>(_departmentCodeProvider.Search(criteria));
		}
	}
}