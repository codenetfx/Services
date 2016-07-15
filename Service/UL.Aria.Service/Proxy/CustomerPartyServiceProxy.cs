using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Web;
using Microsoft.ServiceBus;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Enterprise.Foundation.Client;

namespace UL.Aria.Service.Proxy
{
    /// <summary>
    ///     Implements a proxy for <see cref="IMessageService" />
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CustomerPartyServiceProxy : ServiceAgentManagedProxy<ICustomerPartyService>, ICustomerPartyService
    {
        private readonly ICustomerPartyProxyConfigurationSource _configurationSource;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageServiceProxy" /> class.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        public CustomerPartyServiceProxy(ICustomerPartyProxyConfigurationSource configurationSource) :
            this(
            configurationSource,
            new WebChannelFactory<ICustomerPartyService>(configurationSource.CustomerPartyServiceBinding,
                configurationSource.CustomerPartyServiceUri))
        {
        }

        /// <summary>
        ///     Prevents a default instance of the <see cref="MessageServiceProxy" /> class from being created.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        /// <param name="serviceProxyFactory">The service proxy factory.</param>
        private CustomerPartyServiceProxy(ICustomerPartyProxyConfigurationSource configurationSource,
            ChannelFactory<ICustomerPartyService> serviceProxyFactory) : base(serviceProxyFactory)
        {
            _configurationSource = configurationSource;
            if (null != _configurationSource.TokenProvider)
            {
                serviceProxyFactory.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior
                {
                    TokenProvider = _configurationSource.TokenProvider
                });
                serviceProxyFactory.Endpoint.Binding.SendTimeout = new TimeSpan(0,0,2,0);
                serviceProxyFactory.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0,0,2,0);
                serviceProxyFactory.Endpoint.Binding.CloseTimeout = new TimeSpan(0,0,2,0);
                serviceProxyFactory.Endpoint.Binding.OpenTimeout = new TimeSpan(0,0,2,0);
            }
        }

        /// <summary>
        ///     Gets the customer.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public CustomerOrganizationDto Fetch(string externalId)
        {
            CustomerOrganizationDto value = null;
            ExecuteAction(() => value = ClientProxy.Fetch(externalId));
            return value;
        }


        /// <summary>
        ///     Gets the customer.
        /// </summary>
        /// <param name="orderNumber">The external identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IncomingOrderPartiesDto FetchParties(string orderNumber)
        {
            IncomingOrderPartiesDto value = null;
            ExecuteAction(() => value = ClientProxy.FetchParties(orderNumber));
            return value;
        }

        /// <summary>
        /// Fills the contact.
        /// </summary>
        /// <param name="recordToFill">The record to fill.</param>
        /// <param name="orderNumber">The order number.</param>
        /// <returns></returns>
        public IncomingOrderPartyDto FillContact(IncomingOrderPartyDto recordToFill, string orderNumber)
        {
            IncomingOrderPartyDto value = null;
            ExecuteAction(() => value = ClientProxy.FillContact(recordToFill, orderNumber));
            return value;
        }

        /// <summary>
        /// Fills the party.
        /// </summary>
        /// <param name="recordToFill">The record to fill.</param>
        /// <param name="orderNumber">The order number.</param>
        /// <returns></returns>
        public IncomingOrderPartyDto FillParty(IncomingOrderPartyDto recordToFill, string orderNumber)
        {
            IncomingOrderPartyDto value = null;
            ExecuteAction(() => value = ClientProxy.FillParty(recordToFill, orderNumber));
            return value;
        }
    }
}