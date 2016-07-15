using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface IContactOrderProvider
	/// </summary>
	public interface IContactOrderProvider
	{
		/// <summary>
		/// Queues the contact order.
		/// </summary>
		/// <param name="contactOrder">The contact order.</param>
		void QueueContactOrder(ContactOrderDto contactOrder);

		/// <summary>
		/// Creates the contact order queue.
		/// </summary>
		void CreateContactOrderQueue();
	}
}