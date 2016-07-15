using System.ServiceModel;

using UL.Enterprise.Foundation.Service.Proxy;

namespace UL.Aria.Service.InboundOrderProcessing.Service
{
    /// <summary>
    ///     Wrapper around WebChannelFactory to clean up after a channel faults
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ServiceAgentManagedProxy<T> : ServiceAgent<T> where T : class
    {
        /// <summary>
        ///     The _proxy lock
        /// </summary>
        private readonly object _proxyLock = new object();

        /// <summary>
        ///     The _service proxy factory
        /// </summary>
        private readonly ChannelFactory<T> _serviceProxyFactory;

        private T _clientProxy;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ServiceAgentManagedProxy{T}" /> class.
        /// </summary>
        /// <param name="serviceProxyFactory">The service proxy factory.</param>
        protected ServiceAgentManagedProxy(ChannelFactory<T> serviceProxyFactory)
        {
            _serviceProxyFactory = serviceProxyFactory;
        }

        /// <summary>
        ///     Gets or sets the client proxy.
        /// </summary>
        /// <value>
        ///     The client proxy.
        /// </value>
        protected override T ClientProxy
        {
            get
            {
                RetrieveManagedProxy();
                return _clientProxy;
            }

            set
            {
                throw new ActionNotSupportedException("Managed proxies are controlled by the ServiceAgentManagedProxy");
            }
        }

        /// <summary>
        ///     Retrieves the managed proxy.
        /// </summary>
        /// <returns></returns>
        protected T RetrieveManagedProxy()
        {
            lock (_proxyLock)
            {
                var proxy = _clientProxy as ICommunicationObject;

                if (null != proxy && CommunicationState.Opened != proxy.State)
                {
                    CloseManagedProxy();
                }

                if (null == _clientProxy)
                {
                    InitializeProxy();
                }
            }

            return _clientProxy;
        }

        /// <summary>
        ///     Closes the managed proxy.
        /// </summary>
        protected void CloseManagedProxy()
        {
            var proxy = _clientProxy as ICommunicationObject;

            lock (_proxyLock)
            {
                if (null != proxy)
                {
                    CloseProxy(proxy);
                    _clientProxy = null;
                }
            }
        }

        /// <summary>
        ///     Initializes the proxy.
        /// </summary>
        private void InitializeProxy()
        {
            _clientProxy = _serviceProxyFactory.CreateChannel();
        }
    }
}