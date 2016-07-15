using System;
using System.Collections.Generic;
using System.Linq;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Implements a provider for <see cref="BusinessUnit"/> entities.
	/// </summary>
	public class BusinessUnitProvider : SearchProviderBase<BusinessUnit>, IBusinessUnitProvider
	{
		private readonly IBusinessUnitAssociationProvider _businessUnitAssociationProvider;
		private readonly IBusinessUnitRepository _businessUnitRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="BusinessUnitProvider" /> class.
		/// </summary>
		/// <param name="businessUnitRepository">The business unit repository.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="businessUnitAssociationProvider">The business unit association provider.</param>
		public BusinessUnitProvider(IBusinessUnitRepository businessUnitRepository, IPrincipalResolver principalResolver,
			IBusinessUnitAssociationProvider businessUnitAssociationProvider) : base(businessUnitRepository, principalResolver)
		{
			_businessUnitRepository = businessUnitRepository;
			_businessUnitAssociationProvider = businessUnitAssociationProvider;
		}

		/// <summary>
		/// Fetches all.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<BusinessUnit> FetchAll()
		{
			return _businessUnitRepository.FetchAll();
		}

		/// <summary>
		/// Updates the bulk.
		/// </summary>
		/// <param name="businessUnits">The business units.</param>
		/// <param name="parentId">The parent identifier.</param>
		public void UpdateBulk(IEnumerable<BusinessUnit> businessUnits, Guid parentId)
		{
			var associations = businessUnits.Select(x => new BusinessUnitAssociation
			{
				BusinessUnitId = x.Id.GetValueOrDefault(),
				ParentId = parentId
			});

			_businessUnitAssociationProvider.UpdateBulk(associations, parentId);
		}

		/// <summary>
		/// Fetches the group.
		/// </summary>
		/// <param name="parentId">The parent identifier.</param>
		/// <returns>IEnumerable&lt;BusinessUnit&gt;.</returns>
		public IEnumerable<BusinessUnit> FetchGroup(Guid parentId)
		{
			return _businessUnitRepository.FetchBusinessUnitByEntity(parentId);
		}
	}
}