using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;

namespace UL.Aria.Service.InboundOrderProcessing.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class NewOrderServiceProxy : ServiceAgentManagedProxy<INewOrderService>, INewOrderService
    {

                /// <summary>
        /// Initializes a new instance of the <see cref="NewOrderServiceProxy"/> class.
        /// </summary>
        public NewOrderServiceProxy(IProxyConfigurationSource configurationSource)
            : this(new WebChannelFactory<INewOrderService>(new WebHttpBinding(), configurationSource.NewOrderService))
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewOrderServiceProxy"/> class.
        /// </summary>
        /// <param name="serviceProxyFactory">The service proxy factory.</param>
        private NewOrderServiceProxy(WebChannelFactory<INewOrderService> serviceProxyFactory)
            : base(serviceProxyFactory)
        {
        }

        /// <summary>
        /// Creates the specified new order.
        /// </summary>
        /// <param name="newOrder">The new order.</param>
        /// <returns>
        /// Guid.
        /// </returns>
        public string Create(NewOrderDto newOrder)
        {
            INewOrderService service = RetrieveManagedProxy();
            return service.Create(newOrder);
        }

        /// <summary>
        /// Updates the specified new order id.
        /// </summary>
        /// <param name="newOrderId">The new order id.</param>
        /// <param name="newOrder">The new order.</param>
        public void Update(string newOrderId, NewOrderDto newOrder)
        {
            INewOrderService service = RetrieveManagedProxy();
            service.Update(newOrderId, newOrder);
        }
    }
}

