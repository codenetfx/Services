using System.Collections.Generic;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class InboundMessageBlob.
	/// </summary>
	public class InboundMessageBlob
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InboundMessageBlob"/> class.
		/// </summary>
		public InboundMessageBlob()
		{
			Metadata = new Dictionary<string, string>();
		}

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the metadata.
		/// </summary>
		/// <value>The metadata.</value>
		public IDictionary<string, string> Metadata { get; set; }
	}
}