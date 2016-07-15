using System;
using System.Collections.Generic;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements operations for managing <see cref="IncomingOrder" /> and related objects.
    /// </summary>
    public class IncomingOrderManager : IIncomingOrderManager
    {
        private readonly IIncomingOrderProvider _incomingOrderProvider;
        private readonly ICompanyProvider _companyProvider;
        private readonly ILogManager _logManager;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomingOrderManager" /> class.
        /// </summary>
        /// <param name="incomingOrderProvider">The incoming order provider.</param>
        /// <param name="companyProvider">The companyProvider.</param>
        /// <param name="logManager">The log companyProvider.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public IncomingOrderManager(IIncomingOrderProvider incomingOrderProvider, ICompanyProvider companyProvider, ILogManager logManager, ITransactionFactory transactionFactory)
        {
            _incomingOrderProvider = incomingOrderProvider;
            _companyProvider = companyProvider;
            _logManager = logManager;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        /// Creates the specified new order.
        /// </summary>
        /// <param name="incomingOrder">The new order.</param>
        /// <returns>
        /// Guid.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Guid Create(IncomingOrder incomingOrder)
        {
            return _incomingOrderProvider.Create(incomingOrder);
        }

        /// <summary>
        /// Updates the specified incoming order id.
        /// </summary>
        /// <param name="incomingOrderId">The incoming order id.</param>
        /// <param name="incomingOrder">The incoming order.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Update(Guid incomingOrderId, IncomingOrder incomingOrder)
        {
            _incomingOrderProvider.Update(incomingOrderId, incomingOrder);
        }

        /// <summary>
        /// Deletes the specified <see cref="IncomingOrder" />.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(Guid id)
        {
            _incomingOrderProvider.Delete(id);
        }

        /// <summary>
        /// Searches for <see cref="IncomingOrder" /> objects using the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IncomingOrderSearchResultSet Search(SearchCriteria searchCriteria)
        {
            return _incomingOrderProvider.Search(searchCriteria);
        }

        /// <summary>
        /// Finds the <see cref="IncomingOrder" /> by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IncomingOrder FindById(Guid entityId)
        {
            return  TrySetCompanyIdIfMissing(() =>_incomingOrderProvider.FindById(entityId));
        }

        /// <summary>
        /// Publishes the project creation request.
        /// </summary>
        /// <param name="projectCreationRequest">The project creation request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Guid PublishProjectCreationRequest(ProjectCreationRequest projectCreationRequest)
        {
            projectCreationRequest.ProjectHandler = projectCreationRequest.ProjectHandler.ToLower();
            return _incomingOrderProvider.PublishProjectCreationRequest(projectCreationRequest);
        }

        /// <summary>
        /// Finds the project by service line id.
        /// </summary>
        /// <param name="serviceLineId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IncomingOrder FindByServiceLineId(Guid serviceLineId)
        {
            return _incomingOrderProvider.FindByServiceLineId(serviceLineId);
        }

        /// <summary>
        /// Finds the by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>
        /// IncomingOrder.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IncomingOrder FindByOrderNumber(string orderNumber)
        {
            return  TrySetCompanyIdIfMissing(() => _incomingOrderProvider.FindByOrderNumber(orderNumber));
        }

        /// <summary>
        /// Adds the service line.
        /// </summary>
        /// <param name="serviceLine">The service line.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void AddServiceLine(IncomingOrderServiceLine serviceLine)
        {
            _incomingOrderProvider.AddServiceLine(serviceLine);
        }

        /// <summary>
        /// Deletes the service line.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void DeleteServiceLine(Guid id)
        {
            _incomingOrderProvider.DeleteServiceLine(id);
        }

        internal IncomingOrder TrySetCompanyIdIfMissing(Func<IncomingOrder> incomingOrderFindFunction)
        {
            using (var scope = _transactionFactory.Create())
            {
                var incomingOrder = incomingOrderFindFunction();
                if (Guid.Empty == incomingOrder.CompanyId.GetValueOrDefault(Guid.Empty) &&
                    null != incomingOrder.IncomingOrderCustomer)
                {
                    try
                    {
                        var company = _companyProvider.FetchByExternalId(incomingOrder.IncomingOrderCustomer.ExternalId);
                        if (null != company)
                        {
                            incomingOrder.CompanyId = company.Id;
                            _incomingOrderProvider.Update(incomingOrder.Id.Value, incomingOrder);
                        }
                    }
                    catch (DatabaseItemNotFoundException)
                    {
                        // fully expected.
                    }
                }
                scope.Complete();
                return incomingOrder;
            }
        }
    }
}