namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Class ContactOrderDto. POCO, json serialized so no data contract attributes.
	/// </summary>
	public class ContactOrderDto
	{
		/// <summary>
		/// Gets or sets the message identifier.
		/// </summary>
		/// <value>The message identifier.</value>
		public string MessageId { get; set; }

		/// <summary>
		/// Gets or sets the order number.
		/// </summary>
		/// <value>The order number.</value>
		public string OrderNumber { get; set; }

		/// <summary>
		/// Gets or sets the receiver.
		/// </summary>
		/// <value>The receiver.</value>
		public string Receiver { get; set; }
	}
}