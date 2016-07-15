using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Transactions;

using UL.Enterprise.Foundation;
using UL.Aria.Common.Authorization;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     Service for Order
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class OrderService : IOrderService
    {
        private readonly IAuthorizationManager _authorizationManager;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IOrderManager _orderManager;
        private readonly IPrincipalResolver _principalResolver;
        private readonly IAssetProvider _assetProvider;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyService" /> class.
        /// </summary>
        /// <param name="orderManager">The order manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="assetProvider">The asset provider.</param>
        public OrderService(IOrderManager orderManager, IMapperRegistry mapperRegistry,
            ITransactionFactory transactionFactory,
            IAuthorizationManager authorizationManager,
            IPrincipalResolver principalResolver,
            IAssetProvider assetProvider
            )
        {
            _orderManager = orderManager;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
            _authorizationManager = authorizationManager;
            _principalResolver = principalResolver;
            _assetProvider = assetProvider;
        }

        /// <summary>
        ///     Gets the order by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///     Order Dto
        /// </returns>
        public IncomingOrderDto FetchById(string id)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var idGuid = id.ToGuid();
            Guard.IsNotEmptyGuid(idGuid, "Id");
            var orderMetadata = _assetProvider.Fetch(idGuid);
            var containerId = orderMetadata.GetValue(AssetFieldNames.AriaContainerId, default(Guid));
            ValidateAccessRights(containerId, SecuredActions.View);
            var order = _orderManager.FetchById(id.ToGuid());
            return MapOrder(order, containerId);
        }

        /// <summary>
        ///     Gets the order by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>Order Dto.</returns>
        public IncomingOrderDto FetchByOrderNumber(string orderNumber)
        {
            Guard.IsNotNullOrEmpty(orderNumber, "OrderNumber");

            var order = _orderManager.FetchByOrderNumber(orderNumber);

            if (order == null)
                return null;

	        // ReSharper disable once PossibleInvalidOperationException
            var orderMetadata = _assetProvider.Fetch(order.Id.Value);
            var containerId = orderMetadata.GetValue(AssetFieldNames.AriaContainerId, default(Guid));
            ValidateAccessRights(containerId, SecuredActions.View);

            return MapOrder(order, containerId);
        }

		/// <summary>
		/// Creates the order.
		/// </summary>
		/// <param name="orderXml">The order.</param>
		/// <returns>The created order id</returns>
        public string Create(string orderXml)
        {
			Guard.IsNotNullOrEmpty(orderXml, "OrderXml");
            Guid id;
            ValidateAccessRights(null, SecuredActions.Create);

            using (TransactionScope transactionScope = _transactionFactory.Create())
            {
                id = _orderManager.Create(orderXml);
                transactionScope.Complete();
            }
            return id.ToString();
        }

		/// <summary>
		/// Updates the order.
		/// </summary>
		/// <param name="orderXml">The order.</param>
        public void Update(string orderXml)
        {
			Guard.IsNotNullOrEmpty(orderXml, "OrderXml");
			using (var transactionScope = _transactionFactory.Create())
            {
				_orderManager.Update(orderXml);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Deletes the order by id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(string id)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var idGuid = id.ToGuid();
            Guard.IsNotEmptyGuid(idGuid, "Id");
            var orderMetadata = _assetProvider.Fetch(idGuid);
            var containerId = orderMetadata.GetValue(AssetFieldNames.AriaContainerId, default(Guid));
            ValidateAccessRights(containerId, SecuredActions.Update);
            using (var transactionScope = _transactionFactory.Create())
            {
                _orderManager.Delete(idGuid);
                transactionScope.Complete();
            }
        }

        internal void ValidateAccessRights(Guid? containerId, string securedAction)
        {
            var container = containerId == null ? string.Empty : containerId.ToString();
            var claimsPrincipal = _principalResolver.Current;
            var resourceClaim = new System.Security.Claims.Claim(SecuredResources.OrderInstance, container );
            var actionClaim = new System.Security.Claims.Claim(securedAction, container);
            var authorized = _authorizationManager.Authorize(claimsPrincipal, resourceClaim, actionClaim);

            if (!authorized)
                throw new UnauthorizedAccessException("You are not authorized to access this order");
        }

        internal IncomingOrderDto MapOrder(Order order, Guid containerId)
        {
            var serviceLines = order.ServiceLines;
            order.ServiceLines = null;
            var incomingOrderDto = _mapperRegistry.Map<IncomingOrderDto>(order);
            order.ServiceLines = serviceLines;
            incomingOrderDto.ServiceLines = new List<IncomingOrderServiceLineDto>();

            foreach (
                var serviceLineDto in
                    serviceLines.Select(serviceLine => _mapperRegistry.Map<IncomingOrderServiceLineDto>(serviceLine)))
                incomingOrderDto.ServiceLines.Add(serviceLineDto);

            incomingOrderDto.ContainerId = containerId;
            return incomingOrderDto;
        }
    }
}