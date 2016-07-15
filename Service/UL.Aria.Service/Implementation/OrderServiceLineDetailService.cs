using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     Class OrderServiceLineDetailService.
    /// </summary>
    [AutoRegisterRestServiceAttribute("OrderServiceLineDetail")]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class OrderServiceLineDetailService : IOrderServiceLineDetailService
    {
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IOrderServiceLineDetailProvider _orderServiceLineDetailProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderServiceLineDetailService" /> class.
        /// </summary>
        /// <param name="orderServiceLineDetailProvider">The order service line detail provider.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public OrderServiceLineDetailService(IOrderServiceLineDetailProvider orderServiceLineDetailProvider,
            IMapperRegistry mapperRegistry)
        {
            _orderServiceLineDetailProvider = orderServiceLineDetailProvider;
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        ///     Creates the specified order service line detail.
        /// </summary>
        /// <param name="orderServiceLineDetail">The order service line detail.</param>
        public void Create(OrderServiceLineDetailDto orderServiceLineDetail)
        {
            Guard.IsNotNull(orderServiceLineDetail, "orderServiceLineDetail");

            var mappedServiceLineDetail = _mapperRegistry.Map<OrderServiceLineDetail>(orderServiceLineDetail);

            _orderServiceLineDetailProvider.Create(mappedServiceLineDetail);
        }

        /// <summary>
        ///     Fetches all order service line details.
        /// </summary>
        /// <returns>IList{OrderServiceLineDetailDto}.</returns>
        public IList<OrderServiceLineDetailDto> FetchAll()
        {
            return _mapperRegistry.Map<List<OrderServiceLineDetailDto>>(_orderServiceLineDetailProvider.FindAll());
        }

        /// <summary>
        ///     Fetches a order service line detail by the specified order identifier and line identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="lineId">The line identifier.</param>
        /// <returns>UL.Aria.Service.Contracts.Dto.OrderServiceLineDetailDto.</returns>
        public OrderServiceLineDetailDto Fetch(string orderId, string lineId)
        {
            Guard.IsNotNullOrEmpty(orderId, "orderId");
            var orderIdGuid = orderId.ToGuid();
            Guard.IsNotEmptyGuid(orderIdGuid, "orderId");
            Guard.IsNotNullOrEmpty(lineId, "lineId");

            return
                _mapperRegistry.Map<OrderServiceLineDetailDto>(_orderServiceLineDetailProvider.FindByIds(orderIdGuid,
                    lineId));
        }

        /// <summary>
        ///     Searches for order service line details by the specified search criteria.
        ///     Note: Keyword should be a valid order id, sorting is fixed and not supported via the SearchCrieriaDto
        ///     and refiners are not supported.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>OrderServiceLineDetailSearchResultSetDto.</returns>
        public OrderServiceLineDetailSearchResultSetDto Search(SearchCriteriaDto searchCriteria)
        {
			Guard.IsNotNull(searchCriteria, "searchCriteria");
			Guard.IsNotNull(searchCriteria.Filters, "searchCriteria.Filters");
			//ensure we have an order id, and it is a guid
            UL.Aria.Common.Framework.Guard.FilterIsNotNull(searchCriteria, AssetFieldNames.AriaOrderId).ToGuid();

            var criteria = _mapperRegistry.Map<SearchCriteria>(searchCriteria);

            var orderServiceLineDetailSearchResultSet = _orderServiceLineDetailProvider.Search(criteria);
            var orderServiceLineDetailSearchResults = orderServiceLineDetailSearchResultSet.Results;
            orderServiceLineDetailSearchResultSet.Results = null;
            var orderServiceLineDetailSearchResultSetDto = _mapperRegistry.Map<OrderServiceLineDetailSearchResultSetDto>(
                    orderServiceLineDetailSearchResultSet);
            orderServiceLineDetailSearchResultSetDto.Results =
                orderServiceLineDetailSearchResults.Select(_mapperRegistry.Map<OrderServiceLineDetailSearchResultDto>)
                    .ToList();

            return orderServiceLineDetailSearchResultSetDto;
        }

        /// <summary>
        /// Updates the specified order service line detail.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <param name="orderServiceLineDetail">The order service line detail.</param>
        public void Update(string id, OrderServiceLineDetailDto orderServiceLineDetail)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var idGuid = id.ToGuid();
            Guard.IsNotEmptyGuid(idGuid, "id");
            Guard.IsNotNull(orderServiceLineDetail, "orderServiceLineDetail");

            var mappedServiceLineDetail = _mapperRegistry.Map<OrderServiceLineDetail>(orderServiceLineDetail);

            _orderServiceLineDetailProvider.Update(mappedServiceLineDetail);
        }

        /// <summary>
        ///     Deletes the order service line detail by the specified order identifier and line identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="lineId">The line identifier.</param>
        public void Delete(string orderId, string lineId)
        {
            Guard.IsNotNullOrEmpty(orderId, "orderId");
            var orderIdGuid = orderId.ToGuid();
            Guard.IsNotEmptyGuid(orderIdGuid, "orderId");
            Guard.IsNotNullOrEmpty(lineId, "lineId");

            _orderServiceLineDetailProvider.Delete(orderIdGuid, lineId);
        }
    }
}