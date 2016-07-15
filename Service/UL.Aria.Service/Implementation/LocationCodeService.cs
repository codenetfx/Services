using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     Class LocationCodeService
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false, InstanceContextMode = InstanceContextMode.PerCall)]
    public class LocationCodeService : ILocationCodeService
    {
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;
        private readonly ILocationCodeProvider _locationCodeProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LocationCodeService" /> class.
        /// </summary>
        /// <param name="locationCodeProvider">The location code provider.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public LocationCodeService(ILocationCodeProvider locationCodeProvider, IMapperRegistry mapperRegistry, ITransactionFactory transactionFactory)
        {
            _locationCodeProvider = locationCodeProvider;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Fetches all.
        /// </summary>
		/// <returns>IList{LocationCodeDto}.</returns>
        public IList<LocationCodeDto> FetchAll()
        {
            return _locationCodeProvider.FetchAll().Select(_mapperRegistry.Map<LocationCodeDto>).ToList();
        }

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>LocationCodeDto</returns>
        public LocationCodeDto Fetch(string id)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");

            var locationCode = _locationCodeProvider.Fetch(id.ToGuid());

            return _mapperRegistry.Map<LocationCodeDto>(locationCode);
        }

		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>LocationCodeDto.</returns>
		public LocationCodeDto FetchByExternalId(string externalId)
		{
			Guard.IsNotNullOrEmpty(externalId, "externalId");

			var locationCode = _locationCodeProvider.FetchByExternalId(externalId.DecodeFrom64());

			return _mapperRegistry.Map<LocationCodeDto>(locationCode);
		}

        /// <summary>
        ///     Creates the specified Location code.
        /// </summary>
        /// <param name="locationCode">The location code.</param>
        public void Create(LocationCodeDto locationCode)
        {
            Guard.IsNotNull(locationCode, "LocationCode");

            var locationCodeBo = _mapperRegistry.Map<LocationCode>(locationCode);

            using (var transactionScope = _transactionFactory.Create())
            {
                _locationCodeProvider.Create(locationCodeBo);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="locationCode">The location code.</param>
        public void Update(string id, LocationCodeDto locationCode)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");
            Guard.IsNotNull(locationCode, "LocationCode");

            var locationCodeBo = _mapperRegistry.Map<LocationCode>(locationCode);

            using (var transactionScope = _transactionFactory.Create())
            {
                _locationCodeProvider.Update(id.ToGuid(), locationCodeBo);
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
                _locationCodeProvider.Delete(convertedId);
                transactionScope.Complete();
            }
        }
    }
}