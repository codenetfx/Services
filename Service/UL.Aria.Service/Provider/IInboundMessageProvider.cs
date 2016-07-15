using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface IInboundMessageProvider
	/// </summary>
    public interface IInboundMessageProvider
    {
	    /// <summary>
	    /// Saves the successful message.
	    /// </summary>
	    /// <param name="messageId">The message identifier.</param>
	    /// <param name="message">The message.</param>
	    /// <param name="metadata">The metadata.</param>
	    void SaveSuccessfulMessage(string messageId, string message, IDictionary<string, string> metadata = null);

	    /// <summary>
	    /// Saves the failed message.
	    /// </summary>
	    /// <param name="messageId">The message identifier.</param>
	    /// <param name="message">The message.</param>
	    /// <param name="metadata">The metadata.</param>
	    void SaveFailedMessage(string messageId, string message, IDictionary<string, string> metadata = null);

        /// <summary>
        /// Saves the order message.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <param name="orderMessage">The order message.</param>
        /// <param name="metadata">The metadata.</param>
        void SaveOrderMessage(string orderNumber, string orderMessage, IDictionary<string, string> metadata = null);

		/// <summary>
		/// Saves the new message.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		/// <param name="message">The message.</param>
		/// <param name="metadata">The metadata.</param>
		void SaveNewMessage(string messageId, string message, IDictionary<string, string> metadata = null);

		/// <summary>
		/// Queues the new message.
		/// </summary>
		/// <param name="inboundMessage">The inbound message (The notificaiton of the message).</param>
		/// <param name="message">The the actual message as a string.</param>
		/// <param name="metadata">The metadata.</param>
		void QueueNewMessage(InboundMessageDto inboundMessage, string message, IDictionary<string, string> metadata = null);

		/// <summary>
		/// Creates the new queue.
		/// </summary>
		void CreateNewQueue();

        /// <summary>
        /// Fetches the order message.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>System.String.</returns>
        InboundMessageBlob FetchOrderMessage(string orderNumber);

		/// <summary>
		/// Fetches the new message.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		/// <returns>InboundMessageBlob.</returns>
		InboundMessageBlob FetchNewMessage(string messageId);

        /// <summary>
        /// Deletes the order message.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        void DeleteOrderMessage(string orderNumber);

	    /// <summary>
	    /// Deletes the new message.
	    /// </summary>
	    /// <param name="messageId">The message identifier.</param>
	    void DeleteNewMessage(string messageId);

		/// <summary>
		/// Deletes the successful message.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		void DeleteSuccessfulMessage(string messageId);

		/// <summary>
		/// Cleanups the failed messages.
		/// </summary>
	    void CleanupFailedMessages();

		/// <summary>
		/// Cleanups the order messages.
		/// </summary>
	    void CleanupOrderMessages();

		/// <summary>
		/// Cleanups the new messages.
		/// </summary>
	    void CleanupNewMessages();

        /// <summary>
        /// Pings this instance.
        /// </summary>
        /// <returns></returns>
        string Ping();
    }
}