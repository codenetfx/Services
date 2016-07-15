using System;
using System.Diagnostics;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Logging;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements operations for working with <see cref="IncomingOrderContact"/> objects.
    /// </summary>
    public class QueuedContactManager : IContactManager
    {
        private readonly IContactOrderProvider _contactOrderProvider;
        private readonly ILogManager _logManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueuedContactManager"/> class.
        /// </summary>
        /// <param name="contactOrderProvider">The contact order provider.</param>
        /// <param name="logManager">The log manager.</param>
        public QueuedContactManager(IContactOrderProvider contactOrderProvider, ILogManager logManager)
        {
            _contactOrderProvider = contactOrderProvider;
            _logManager = logManager;
        }

        /// <summary>
        /// Updates all contacts for any <see cref="Project" /> or <see cref="IncomingOrder" /> with the supplied <paramref name="orderNumber" />.
        /// This will only be done for a <see cref="Project" /> if the project isn't closed (<see cref="T:ProjectStatusEnumDto.Completed" /> or <see cref="T:ProjectStatusEnumDto.Canceled" />).
        /// This method is implicitly asynchronous as it will queue a request.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void UpdateContactsByOrderNumberAsync(string orderNumber)
        {
            try
            {
                _contactOrderProvider.QueueContactOrder(new ContactOrderDto { OrderNumber = orderNumber, MessageId = Trace.CorrelationManager.ActivityId.ToString(), Receiver = "manualsync" });
                var logMessage = new LogMessage(MessageIds.ContactManagerContactOrderDtoQueued, LogPriority.Medium,
                    TraceEventType.Information, "QueuedContactManager.UpdateContactsByOrderNumberAsync requested.",
                    LogCategory.Request);
                logMessage.Data.Add("OrderNumber", orderNumber);
                _logManager.Log(logMessage);
            }
            catch (Exception ex)
            {
                var logMessage = ex.ToLogMessage(MessageIds.ContactManagerContactOrderDtoException, LogCategory.Request,
                    LogPriority.High, TraceEventType.Error);
                logMessage.Data.Add("OrderNumber", orderNumber);
                _logManager.Log(logMessage);
                throw;
            }
        }
    }
}