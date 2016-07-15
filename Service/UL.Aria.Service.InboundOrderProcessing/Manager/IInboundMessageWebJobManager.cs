using UL.Aria.Service.Contracts;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.InboundOrderProcessing.Manager
{
	/// <summary>
	/// Interface IInboundMessageWebJobManager
	/// </summary>
	public interface IInboundMessageWebJobManager
	{
		/// <summary>
		/// Processes the specified inbound message.
		/// </summary>
		/// <param name="inboundMessage">The inbound message.</param>
		/// <param name="message">The message.</param>
		void Process(InboundMessageDto inboundMessage, string message);
	}
}