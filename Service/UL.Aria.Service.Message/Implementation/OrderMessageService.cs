using System;
using System.Diagnostics;
using System.Transactions;
using UL.Aria.Service.Message.Logging;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Message.Domain;
using UL.Aria.Service.Message.Provider;

namespace UL.Aria.Service.Message.Implementation
{
    /// <summary>
    ///     Implements the <see cref="IOrderMessageService" /> interface.
    /// </summary>
    public class OrderMessageService : IOrderMessageService
    {
        private readonly ILogManager _logManager;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IOrderMessageProvider _orderMessageProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderMessageService" /> class.
        /// </summary>
        /// <param name="orderMessageProvider">The order message provider.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="logManager">The log manager.</param>
        public OrderMessageService(IOrderMessageProvider orderMessageProvider, IMapperRegistry mapperRegistry,
            ILogManager logManager)
        {
            _orderMessageProvider = orderMessageProvider;
            _mapperRegistry = mapperRegistry;
            _logManager = logManager;
        }

        /// <summary>
        ///     Enqueues the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Enqueue(OrderMessageDto message)
        {
            var logMessage = new LogMessage(MessageIds.OrderMessageServiceEnqueue, LogPriority.Medium, TraceEventType.Verbose,
                string.Format("ExternalMessageId:{0}, Originator:{1}, Receiver:{2}, Body:{3}",
                    message.ExternalMessageId, message.Originator, message.Receiver, message.Body),
                LogCategory.MessageHost);
            logMessage.LogCategories.Add(LogCategory.MessageHost);
            _logManager.Log(logMessage);

            using (
                var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                var messageBo = _mapperRegistry.Map<OrderMessage>(message);
                _orderMessageProvider.Enqueue(messageBo);
                scope.Complete();
            }
        }

        /// <summary>
        ///     Dequeues the top message.
        /// </summary>
        /// <returns></returns>
        public OrderMessageDto Dequeue()
        {
            using (
                var scope = new TransactionScope(TransactionScopeOption.Required)
                )
            {
                OrderMessage result = _orderMessageProvider.Dequeue();
                var logMessage = new LogMessage(MessageIds.OrderMessageServiceDequeue, LogPriority.Medium, TraceEventType.Verbose,
                    string.Format("ExternalMessageId:{0}, Originator:{1}, Receiver:{2}, Body:{3}",
                        result.ExternalMessageId, result.Originator, result.Receiver, result.Body),
                    LogCategory.MessageHost);
                logMessage.LogCategories.Add(LogCategory.MessageHost);
                _logManager.Log(logMessage);
                scope.Complete();
                return _mapperRegistry.Map<OrderMessageDto>(result);
            }
        }

        /// <summary>
        ///     Pings the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>System.String.</returns>
        public string Ping(string message)
        {
            return string.Format("Alive! Recieved message '{0}'", message);
        }
    }
}