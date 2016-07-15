using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Parser;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Manager for Orders.
    /// </summary>
    public class OrderManager : IOrderManager
    {
        private readonly IOrderProvider _orderProvider;
	    private readonly ITransactionFactory _transactionFactory;
	    private readonly IInboundMessageProvider _inboundMessageProvider;
	    private readonly IXmlParser _incomingOrderParser;

	    /// <summary>
		/// Initializes a new instance of the <see cref="OrderManager" /> class.
		/// </summary>
		/// <param name="orderProvider">The order provider.</param>
		/// <param name="transactionFactory">The transaction factory.</param>
		/// <param name="inboundMessageProvider">The inbound message provider.</param>
		/// <param name="incomingOrderParser">The incoming order parser.</param>
        public OrderManager(IOrderProvider orderProvider, ITransactionFactory transactionFactory, IInboundMessageProvider inboundMessageProvider, IXmlParser incomingOrderParser)
	    {
		    _orderProvider = orderProvider;
		    _transactionFactory = transactionFactory;
		    _inboundMessageProvider = inboundMessageProvider;
		    _incomingOrderParser = incomingOrderParser;
	    }

	    /// <summary>
        ///     Gets the order by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public Order FetchById(Guid id)
        {
            return _orderProvider.FindById(id);
        }

        /// <summary>
        ///     Gets the order by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>Order.</returns>
        public Order FetchByOrderNumber(string orderNumber)
        {
            return _orderProvider.FindByOrderNumber(orderNumber);
        }

		/// <summary>
		/// Creates the order.
		/// </summary>
		/// <param name="orderXml">The order.</param>
		/// <returns>The created order id.</returns>
        public Guid Create(string orderXml)
		{
			Guid orderId;
			using (var transactionScope = _transactionFactory.Create())
			{
				var messageId = Guid.NewGuid().ToString();
				orderId = _orderProvider.Create(messageId, orderXml);
				var blobMetadata = new Dictionary<string, string> { { "MessageId", messageId }, { "ExternalMessageId", Guid.NewGuid().ToString() }, { "Receiver", "general" }, { "Originator", "general" } };
				_inboundMessageProvider.SaveSuccessfulMessage(messageId, orderXml, blobMetadata);
				transactionScope.Complete();
			}

			return orderId;
		}

		/// <summary>
		/// Updates the order.
		/// </summary>
		/// <param name="orderXml">The order.</param>
		public void Update(string orderXml)
        {
			using (var transactionScope = _transactionFactory.Create())
			{
				var messageId = Guid.NewGuid().ToString();
				_orderProvider.Update(messageId, orderXml);
				var blobMetadata = new Dictionary<string, string> { { "MessageId", messageId }, { "ExternalMessageId", Guid.NewGuid().ToString() }, { "Receiver", "general" }, { "Originator", "general" } };
				_inboundMessageProvider.SaveSuccessfulMessage(messageId, orderXml, blobMetadata);
				transactionScope.Complete();
			}
			var orderDto = _incomingOrderParser.Parse(orderXml) as IncomingOrderDto;
			var originalOrder = _orderProvider.FindByOrderNumber(orderDto.OrderNumber);
			_inboundMessageProvider.DeleteSuccessfulMessage(originalOrder.MessageId);
		}

        /// <summary>
        ///     Deletes the order by id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(Guid id)
        {
			using (var transactionScope = _transactionFactory.Create())
			{
				var order = _orderProvider.FindById(id);
				_orderProvider.Delete(id);
				_inboundMessageProvider.DeleteSuccessfulMessage(order.MessageId);
				transactionScope.Complete();
			}
		}
    }
}