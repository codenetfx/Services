using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Web;
using Microsoft.ServiceBus;
using UL.Enterprise.Foundation.Client;
using UL.Enterprise.Foundation.Logging;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Logging;

namespace UL.Aria.Service.Proxy
{
    /// <summary>
    ///     Implements a proxy for <see cref="IMessageService" />
    /// </summary>
    public class MessageServiceProxy : ServiceAgentManagedProxy<IMessageService>, IMessageService
    {
        private readonly IProxyConfigurationSource _configurationSource;
        private readonly ILogManager _logManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageServiceProxy" /> class.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        /// <param name="logManager">The log manager.</param>
        public MessageServiceProxy(IProxyConfigurationSource configurationSource, ILogManager logManager):
            this(configurationSource, new WebChannelFactory<IMessageService>(configurationSource.MessageServiceBinding, configurationSource.MessageServiceUri))
        {
            
            _logManager = logManager;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="MessageServiceProxy" /> class from being created.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        /// <param name="serviceProxyFactory">The service proxy factory.</param>
        private MessageServiceProxy(IProxyConfigurationSource configurationSource, ChannelFactory<IMessageService> serviceProxyFactory):base(serviceProxyFactory)
        {
            _configurationSource = configurationSource;
            if (null != _configurationSource.TokenProvider)
            {
                serviceProxyFactory.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior
                {
                    TokenProvider = _configurationSource.TokenProvider
                });
            }
        }


        /// <summary>
        ///     Publishes the specified unique identifier.
        /// </summary>
        /// <param name="message">The message.</param>
        public void PublishProjectStatusMessage(ProjectStatusMessageDto message)
        {
            var receiveLogMessage = new LogMessage(
                LogEventIds.MessageService.ProjectMessageReceivedProxy,
                LogPriority.Low,
                TraceEventType.Information,
                "Project Message Received at Proxy.",
                LogCategory.MessageServiceProxy
                );
            message.DecorateLogMessage(receiveLogMessage);
            _logManager.Log(receiveLogMessage);

            ExecuteAction(() => ClientProxy.PublishProjectStatusMessage(message));

            var publishLogMessage = new LogMessage(
                LogEventIds.MessageService.ProjectMessagePublishedProxy,
                LogPriority.Low,
                TraceEventType.Verbose,
                "Project Message Published at Proxy.",
                LogCategory.MessageServiceProxy
                );
            message.DecorateLogMessage(publishLogMessage);
            _logManager.Log(publishLogMessage);
        }

        /// <summary>
        ///     Pings the specified message.
        /// </summary>
        /// <returns>System.String.</returns>
        public string Ping()
        {
            string ping = "";
            ExecuteAction(() => ping = ClientProxy.Ping());

            return ping;
        }
    }
}
