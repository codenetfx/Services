using System.ServiceModel;

using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     Service for Container
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class ContainerService : IContainerService
    {
        private readonly IContainerManager _containerManager;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ContainerService" /> class.
        /// </summary>
        /// <param name="containerManager">The container manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public ContainerService(IContainerManager containerManager, IMapperRegistry mapperRegistry,
                                ITransactionFactory transactionFactory)
        {
            _containerManager = containerManager;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Gets the Container by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///     Container Dto
        /// </returns>
        public ContainerDto FetchById(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var idGuid = id.ToGuid();
            Guard.IsNotEmptyGuid(idGuid, "id");
            var container = _containerManager.FindById(idGuid);
            return _mapperRegistry.Map<ContainerDto>(container);
        }

        /// <summary>
        ///     Updates the container.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Update(ContainerDto container)
        {
            Guard.IsNotNull(container, "container");
            var containerBo = _mapperRegistry.Map<Container>(container);
            using (var transactionScope = _transactionFactory.Create())
            {
                _containerManager.Update(containerBo);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Deletes the container by id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var idGuid = id.ToGuid();
            Guard.IsNotEmptyGuid(idGuid, "id");
            using (var transactionScope = _transactionFactory.Create())
            {
                _containerManager.Delete(idGuid);
                transactionScope.Complete();
            }
        }
    }
}