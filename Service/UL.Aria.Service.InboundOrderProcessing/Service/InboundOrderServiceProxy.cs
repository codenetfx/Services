using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;

namespace UL.Aria.Service.InboundOrderProcessing.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class InboundOrderServiceProxy : ServiceAgentManagedProxy<IIncomingOrderService>, IIncomingOrderService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundOrderServiceProxy" /> class.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        public InboundOrderServiceProxy(IProxyConfigurationSource configurationSource)
            : this(new WebChannelFactory<IIncomingOrderService>(new WebHttpBinding(), configurationSource.NewOrderService))
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundOrderServiceProxy" /> class.
        /// </summary>
        /// <param name="serviceProxyFactory">The service proxy factory.</param>
        private InboundOrderServiceProxy(WebChannelFactory<IIncomingOrderService> serviceProxyFactory)
            : base(serviceProxyFactory)
        {
        }

        internal IIncomingOrderService ClientProxyInternal
        {
            set { ClientProxy = value; }
        }

        internal void CloseManagedProxyInternal()
        {
            CloseManagedProxy();
        }

        internal void CloseProxyInternal()
        {
// ReSharper disable once SuspiciousTypeConversion.Global
            CloseProxy(ClientProxy as ICommunicationObject);
        }

        internal IIncomingOrderService RetrieveManagedProxyInternal()
        {
            return RetrieveManagedProxy();
        }

        /// <summary>
        /// Creates the specified new order.
        /// </summary>
        /// <param name="newOrder">The new order.</param>
        /// <returns>
        /// Guid.
        /// </returns>
        public string Create(IncomingOrderDto newOrder)
        {
            string id = null;

            ExecuteAction(() => id = ClientProxy.Create(newOrder));

            return id;
        }

        /// <summary>
        /// Updates the specified new order id.
        /// </summary>
        /// <param name="newOrderId">The new order id.</param>
        /// <param name="newOrder">The new order.</param>
        public void Update(string newOrderId, IncomingOrderDto newOrder)
        {
            ExecuteAction(() => ClientProxy.Update(newOrderId, newOrder));
        }

        /// <summary>
        /// Search based on the provided criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        public IncomingOrderSearchResultSetDto Search(SearchCriteriaDto searchCriteria)
        {
            IncomingOrderSearchResultSetDto searchResultSetDto = null;

            ExecuteAction(() => searchResultSetDto = ClientProxy.Search(searchCriteria));

            return searchResultSetDto;
        }

        /// <summary>
        /// Creates the project from the specified new order.
        /// </summary>
        /// <param name="projectCreationRequest">The project creation request.</param>
        /// <returns>Guid.</returns>
        public string PublishProjectCreationRequest(ProjectCreationRequestDto projectCreationRequest)
        {
            string id = null;

            ExecuteAction(() => id = ClientProxy.PublishProjectCreationRequest(projectCreationRequest));

            return id;
        }

        /// <summary>
        /// Fetches the specified <see cref="IncomingOrderDto" />.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IncomingOrderDto Fetch(string id)
        {
            return ExecuteFetch(() => ClientProxy.Fetch(id));
        }

        /// <summary>
        /// Fetches the by service line.
        /// </summary>
        /// <param name="serviceLineId">The service line id.</param>
        /// <returns></returns>
        public IncomingOrderDto FetchByServiceLine(string serviceLineId)
        {
            return ExecuteFetch(() => ClientProxy.FetchByServiceLine(serviceLineId));
        }
    }
}

