using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     Class IncomingOrderService
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class IncomingOrderService : IIncomingOrderService
    {
        private readonly IIncomingOrderManager _incomingOrderManager;
        private readonly IMapperRegistry _mapperRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomingOrderService" /> class.
        /// </summary>
        /// <param name="incomingOrderManager">The incoming order provider.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public IncomingOrderService(IIncomingOrderManager incomingOrderManager, IMapperRegistry mapperRegistry)
        {
            _incomingOrderManager = incomingOrderManager;
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        ///     Creates the specified new order.
        /// </summary>
        /// <param name="incomingOrder">The new order.</param>
        /// <returns>Guid.</returns>
        public string Create(IncomingOrderDto incomingOrder)
        {
            Guard.IsNotNull(incomingOrder, "IncomingOrder");

            var mappedIncomingOrder = _mapperRegistry.Map<IncomingOrder>(incomingOrder);

            return _incomingOrderManager.Create(mappedIncomingOrder).ToString();
        }

        /// <summary>
        ///     Updates the specified incoming order id.
        /// </summary>
        /// <param name="incomingOrderId">The incoming order id.</param>
        /// <param name="incomingOrder">The incoming order.</param>
        public void Update(string incomingOrderId, IncomingOrderDto incomingOrder)
        {
            Guard.IsNotNullOrEmpty(incomingOrderId, "IncomingOrderId");
            var id = Guid.Parse(incomingOrderId);
            Guard.IsNotNull(incomingOrder, "IncomingOrder");

            var mappedIncomingOrder = _mapperRegistry.Map<IncomingOrder>(incomingOrder);

            _incomingOrderManager.Update(id, mappedIncomingOrder);
        }

        /// <summary>
        ///     Search based on the provided criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        public IncomingOrderSearchResultSetDto Search(SearchCriteriaDto searchCriteria)
        {
            Guard.IsNotNull(searchCriteria, "searchCriteria");

            var criteria = _mapperRegistry.Map<SearchCriteria>(searchCriteria);
            return _mapperRegistry.Map<IncomingOrderSearchResultSetDto>(_incomingOrderManager.Search(criteria));
        }

        /// <summary>
        ///     Creates the specified new order.
        /// </summary>
        /// <param name="projectCreationRequest">The project creation request.</param>
        /// <returns>Container Id.</returns>
        public string PublishProjectCreationRequest(ProjectCreationRequestDto projectCreationRequest)
        {
            Guard.IsNotNull(projectCreationRequest, "ProjectCreationRequest");

            var mappedProjectCreationRequest = _mapperRegistry.Map<ProjectCreationRequest>(projectCreationRequest);

            return _incomingOrderManager.PublishProjectCreationRequest(mappedProjectCreationRequest).ToString();
        }

        /// <summary>
        ///     Fetches the specified id or order number.
        /// </summary>
        /// <param name="idOrOrderNumber">The id or order number.</param>
        /// <returns>IncomingOrderDto.</returns>
        public IncomingOrderDto Fetch(string idOrOrderNumber)
        {
            Guid idGuid;
            if (Guid.TryParse(idOrOrderNumber, out idGuid))
                return _mapperRegistry.Map<IncomingOrderDto>(_incomingOrderManager.FindById(idGuid));

            return _mapperRegistry.Map<IncomingOrderDto>(_incomingOrderManager.FindByOrderNumber(idOrOrderNumber));
        }

        /// <summary>
        ///     Finds the project by service line.
        /// </summary>
        /// <returns></returns>
        public IncomingOrderDto FetchByServiceLine(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");

            return _mapperRegistry.Map<IncomingOrderDto>(_incomingOrderManager.FindByServiceLineId(Guid.Parse(id)));
        }
    }
}