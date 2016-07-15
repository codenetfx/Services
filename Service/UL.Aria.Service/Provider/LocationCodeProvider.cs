using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     stub
    /// </summary>
    public class LocationCodeProvider : SearchProviderBase<LocationCode>, ILocationCodeProvider
    {
        private readonly ILocationCodeRepository _repository;


        /// <summary>
        /// Initializes a new instance of the <see cref="LocationCodeProvider" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public LocationCodeProvider(ILocationCodeRepository repository, IPrincipalResolver principalResolver)
            :base(repository, principalResolver)

        {
            _repository = repository;
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LocationCode> FetchAll()
        {
            return _repository.FetchAll();
        }

		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>LocationCode.</returns>
		public LocationCode FetchByExternalId(string externalId)
		{
			return _repository.FindByExternalId(externalId);
		}

    
    }
}
