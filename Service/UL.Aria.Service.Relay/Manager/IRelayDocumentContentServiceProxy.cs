using System.IO;

namespace UL.Aria.Service.Relay.Manager
{
	/// <summary>
	/// Interface IRelayDocumentContentServiceProxy
	/// </summary>
	public interface IRelayDocumentContentServiceProxy
	{
		/// <summary>
		/// Saves the specified stream.
		/// </summary>
		/// <param name="metadata">The metadata.</param>
		/// <param name="stream">The stream.</param>
		void Save(string metadata, Stream stream);
	}
}