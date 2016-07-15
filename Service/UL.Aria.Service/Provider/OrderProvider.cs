using System;

using UL.Aria.Common.Authorization;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Parser;
using UL.Aria.Service.Repository;
using System.Collections.Generic;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Class OrderProvider
    /// </summary>
    public class OrderProvider : IOrderProvider
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IAuthorizationManager _authorizationManager;
        private readonly ICompanyProvider _companyProvider;
        private readonly IXmlParser _incomingOrderParser;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IOrderRepository _orderRepository;
        private readonly IPrincipalResolver _principalResolver;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderProvider" /> class.
        /// </summary>
        /// <param name="orderRepository">The order repository.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="assetProvider">The content repository.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="incomingOrderParser">The incoming order parser.</param>
        /// <param name="companyProvider">The company provider.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public OrderProvider(IOrderRepository orderRepository, ITransactionFactory transactionFactory,
            IAssetProvider assetProvider,
            IMapperRegistry mapperRegistry, IXmlParser incomingOrderParser,
            ICompanyProvider companyProvider,
            IAuthorizationManager authorizationManager,
            IPrincipalResolver principalResolver
            )
        {
            _orderRepository = orderRepository;
            _transactionFactory = transactionFactory;
            _assetProvider = assetProvider;
            _mapperRegistry = mapperRegistry;
            _incomingOrderParser = incomingOrderParser;
            _companyProvider = companyProvider;
            _authorizationManager = authorizationManager;
            _principalResolver = principalResolver;
        }

        /// <summary>
        /// Creates the specified new order.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="orderXml">The new order.</param>
        /// <returns>Guid.</returns>
        public Guid Create(string messageId, string orderXml)
        {
            var orderDto = _incomingOrderParser.Parse(orderXml) as IncomingOrderDto;
            // ReSharper disable once PossibleNullReferenceException
            orderDto.CreatedDateTime = DateTime.UtcNow;
            var company = _companyProvider.FetchByExternalId(orderDto.IncomingOrderCustomer.ExternalId);

            if (company != null)
            {
                orderDto.CompanyId = company.Id;
            }
            var order = _mapperRegistry.Map<Order>(orderDto);
            order.MessageId = messageId;
            order.UpdatedDateTime = DateTime.UtcNow;

            using (var transactionScope = _transactionFactory.Create())
            {
                order.Id = _orderRepository.Create(order);
                var containerId = Guid.NewGuid();
                // Call Content Repo to Create Order through CreateMetadata interface
                _assetProvider.Create(containerId, order.Id.Value, order);
                transactionScope.Complete();
                return order.Id.Value;
            }
        }

        /// <summary>
        /// Updates the specified order.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="orderXml">The order.</param>
        public void Update(string messageId, string orderXml)
        {
            var orderDto = _incomingOrderParser.Parse(orderXml) as IncomingOrderDto;
            // ReSharper disable once PossibleNullReferenceException
            orderDto.UpdatedDateTime = DateTime.UtcNow;
            var company = _companyProvider.FetchByExternalId(orderDto.IncomingOrderCustomer.ExternalId);

            if (company != null)
            {
                orderDto.CompanyId = company.Id;
            }
            Order orderExisting = FindByOrderNumber(orderDto.OrderNumber);
            orderDto.Id = orderExisting.Id.GetValueOrDefault();
            var order = _mapperRegistry.Map<Order>(orderDto);
            order.MessageId = messageId;
            order.UpdatedDateTime = DateTime.UtcNow;

            using (var transactionScope = _transactionFactory.Create())
            {
                _orderRepository.Update(order);
                _assetProvider.Update(orderExisting.Id.GetValueOrDefault(), order);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Deletes the specified <see cref="Order" />.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(Guid id)
        {
            using (var transactionScope = _transactionFactory.Create())
            {
                _orderRepository.Delete(id);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Finds the <see cref="Order" /> by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        public Order FindById(Guid entityId)
        {
            var order = _orderRepository.FindById(entityId);
            var messageId = order.MessageId;
            var orderDto = _incomingOrderParser.Parse(order.OriginalXmlParsed) as IncomingOrderDto;
            // ReSharper disable once PossibleNullReferenceException
            orderDto.CompanyId = order.CompanyId;
            orderDto.Id = order.Id.GetValueOrDefault();
            order = _mapperRegistry.Map<Order>(orderDto);
            order.MessageId = messageId;
            return order;
        }

        /// <summary>
        ///     Finds the <see cref="Order" /> by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns></returns>
        public Order FindByOrderNumber(string orderNumber)
        {
            var order = _orderRepository.FindByOrderNumber(orderNumber);
            var messageId = order.MessageId;
            var orderDto = _incomingOrderParser.Parse(order.OriginalXmlParsed) as IncomingOrderDto;
            // ReSharper disable once PossibleNullReferenceException
            orderDto.Id = order.Id.GetValueOrDefault();
            order = _mapperRegistry.Map<Order>(orderDto);
            order.MessageId = messageId;
            return order;
        }

        /// <summary>
        /// Fetches the order lookups.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Lookup> FindOrderLookups()
        {
            return _orderRepository.FindOrderLookups();
        }

        internal void ValidateAccessRights(string containerId, string securedAction)
        {
            var claimsPrincipal = _principalResolver.Current;
            var resourceClaim = new System.Security.Claims.Claim(SecuredResources.OrderInstance, containerId);
            var actionClaim = new System.Security.Claims.Claim(securedAction, containerId);
            var authorized = _authorizationManager.Authorize(claimsPrincipal, resourceClaim, actionClaim);

            if (!authorized)
                throw new UnauthorizedAccessException("You are not authorized to access this order");
        }


    
    }
}