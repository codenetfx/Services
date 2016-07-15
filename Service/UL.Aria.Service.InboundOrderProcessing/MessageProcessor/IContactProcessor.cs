using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.InboundOrderProcessing.MessageProcessor
{
    /// <summary>
    ///     Defines operations for contacts for an <see cref="IncomingOrderDto">incoming order</see>.
    /// </summary>
    public interface IContactProcessor
    {
		/// <summary>
		/// Processes the contacts.
		/// </summary>
		/// <param name="contactOrder">The contact order.</param>
        void Process(ContactOrderDto contactOrder);
    }
}