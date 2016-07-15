using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Enterprise.Foundation.Client;

namespace UL.Aria.Service.Proxy
{
    /// <summary>
    /// 
    /// </summary>
    public class CertificationRequestServiceProxy : ServiceAgentManagedProxy<ICertificationRequestService>, ICertificationRequestService
    {
        private ICertificationRequestServiceConfigurationSource _configurationSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificationRequestServiceProxy"/> class.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        public CertificationRequestServiceProxy(ICertificationRequestServiceConfigurationSource configurationSource) :
            this(
            configurationSource,
            new WebChannelFactory<ICertificationRequestService>(configurationSource.CertificationRequestServiceBinding,
                configurationSource.CertificationRequestServiceUri))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificationRequestServiceProxy"/> class.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        /// <param name="serviceProxyFactory">The service proxy factory.</param>
        [ExcludeFromCodeCoverage]
        private CertificationRequestServiceProxy(ICertificationRequestServiceConfigurationSource configurationSource,
            ChannelFactory<ICertificationRequestService> serviceProxyFactory) : base(serviceProxyFactory)
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
        /// Submits a certification request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public string PublishCertificationRequest(CertificationRequestDto request)
        {
            return ExecuteFetch<string>(() => ClientProxy.PublishCertificationRequest(request));
        }
    }
}
