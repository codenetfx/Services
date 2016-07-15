using System;
using System.Collections.Generic;

using UL.Aria.Common.Authorization;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Class OrderServiceLineDetailProvider.
    /// </summary>
    public class OrderServiceLineDetailProvider : IOrderServiceLineDetailProvider
    {
        private readonly IOrderServiceLineDetailRepository _orderServiceLineDetailRepository;
        private readonly IPrincipalResolver _principalResolver;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderServiceLineDetailProvider" /> class.
        /// </summary>
        /// <param name="orderServiceLineDetailRepository">The order service line detail repository.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public OrderServiceLineDetailProvider(IOrderServiceLineDetailRepository orderServiceLineDetailRepository,
            ITransactionFactory transactionFactory, IPrincipalResolver principalResolver)
        {
            _orderServiceLineDetailRepository = orderServiceLineDetailRepository;
            _transactionFactory = transactionFactory;
            _principalResolver = principalResolver;
        }

        /// <summary>
        ///     Creates the specified order service line detail.
        /// </summary>
        /// <param name="orderServiceLineDetail">The order service line detail.</param>
        public void Create(OrderServiceLineDetail orderServiceLineDetail)
        {
            var creationDateTime = DateTime.UtcNow;
            var createdBy = _principalResolver.UserId;
            orderServiceLineDetail.CreatedDateTime = creationDateTime;
            orderServiceLineDetail.CreatedById = createdBy;
            orderServiceLineDetail.UpdatedDateTime = creationDateTime;
            orderServiceLineDetail.UpdatedById = createdBy;
            using (var transactionScope = _transactionFactory.Create())
            {
                _orderServiceLineDetailRepository.Create(orderServiceLineDetail);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Finds all order service line details.
        /// </summary>
        /// <returns>IList{OrderServiceLineDetail}.</returns>
        public IList<OrderServiceLineDetail> FindAll()
        {
            return _orderServiceLineDetailRepository.FindAll();
        }

        /// <summary>
        ///     Finds a order service line detail by the specified order identifier and line identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="lineId">The line identifier.</param>
        /// <returns>OrderServiceLineDetail.</returns>
        public OrderServiceLineDetail FindByIds(Guid orderId, string lineId)
        {
            return _orderServiceLineDetailRepository.FindByIds(orderId, lineId);
        }

        /// <summary>
        ///     Searches for order service line details by the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>OrderServiceLineDetailSearchResultSet.</returns>
        public OrderServiceLineDetailSearchResultSet Search(SearchCriteria searchCriteria)
        {
            return _orderServiceLineDetailRepository.Search(searchCriteria);
        }

        /// <summary>
        ///     Updates the specified order service line detail.
        /// </summary>
        /// <param name="orderServiceLineDetail">The order service line detail.</param>
        public void Update(OrderServiceLineDetail orderServiceLineDetail)
        {
            var updateDateTime = DateTime.UtcNow;
            var updatedBy = _principalResolver.UserId;
            orderServiceLineDetail.UpdatedDateTime = updateDateTime;
            orderServiceLineDetail.UpdatedById = updatedBy;
            using (var transactionScope = _transactionFactory.Create())
            {
                _orderServiceLineDetailRepository.Update(orderServiceLineDetail);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Deletes the order service line detail by the specified order identifier and line identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="lineId">The line identifier.</param>
        public void Delete(Guid orderId, string lineId)
        {
            using (var transactionScope = _transactionFactory.Create())
            {
                _orderServiceLineDetailRepository.Delete(orderId, lineId);
                transactionScope.Complete();
            }
        }
    }
}