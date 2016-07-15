using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

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
    ///     Class UnitOfMeasureService
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class UnitOfMeasureService : IUnitOfMeasureService
    {
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;
        private readonly IUnitOfMeasureProvider _unitOfMeasureProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnitOfMeasureService" /> class.
        /// </summary>
        /// <param name="unitOfMeasureProvider">The unit of measure provider.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public UnitOfMeasureService(IUnitOfMeasureProvider unitOfMeasureProvider, IMapperRegistry mapperRegistry,
            ITransactionFactory transactionFactory)
        {
            _unitOfMeasureProvider = unitOfMeasureProvider;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{UnitOfMeasureDto}.</returns>
        public IList<UnitOfMeasureDto> FetchAll()
        {
            return _unitOfMeasureProvider.GetAll().Select(_mapperRegistry.Map<UnitOfMeasureDto>).ToList();
        }

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>UnitOfMeasureDto.</returns>
        public UnitOfMeasureDto Fetch(string id)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");

            var unitOfMeasure = _unitOfMeasureProvider.Fetch(id.ToGuid());

            return _mapperRegistry.Map<UnitOfMeasureDto>(unitOfMeasure);
        }

        /// <summary>
        ///     Creates the specified unit of measure.
        /// </summary>
        /// <param name="unitOfMeasure">The unit of measure.</param>
        public void Create(UnitOfMeasureDto unitOfMeasure)
        {
            Guard.IsNotNull(unitOfMeasure, "UnitOfMeasure");

            var unitOfMeasureBo = _mapperRegistry.Map<UnitOfMeasure>(unitOfMeasure);

            using (var transactionScope = _transactionFactory.Create())
            {
                _unitOfMeasureProvider.Create(unitOfMeasureBo);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="unitOfMeasure">The unit of measure.</param>
        public void Update(string id, UnitOfMeasureDto unitOfMeasure)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");
            Guard.IsNotNull(unitOfMeasure, "UnitOfMeasure");

            var unitOfMeasureBo = _mapperRegistry.Map<UnitOfMeasure>(unitOfMeasure);

            using (var transactionScope = _transactionFactory.Create())
            {
                _unitOfMeasureProvider.Update(id.ToGuid(), unitOfMeasureBo);
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
                _unitOfMeasureProvider.Delete(convertedId);
                transactionScope.Complete();
            }
        }
    }
}