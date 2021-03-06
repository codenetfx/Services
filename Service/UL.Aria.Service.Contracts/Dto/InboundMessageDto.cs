﻿using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Class InboundMessageDto. POCO, json serialized so no data contract attributes.
	/// </summary>
	public class InboundMessageDto
	{
		/// <summary>
		/// Gets or sets the message identifier.
		/// </summary>
		/// <value>The message identifier, blob name, generated by the JMS listener and test harness.</value>
		public string MessageId { get; set; }

		/// <summary>
		/// Gets or sets the external message identifier.
		/// </summary>
		/// <value>The external message identifier.</value>
		public string ExternalMessageId { get; set; }

		/// <summary>
		/// Gets or sets the receiver.
		/// </summary>
		/// <value>The receiver.</value>
		public string Receiver { get; set; }

		/// <summary>
		/// Gets or sets the originator.
		/// </summary>
		/// <value>The originator.</value>
		public string Originator { get; set; }
	}
}