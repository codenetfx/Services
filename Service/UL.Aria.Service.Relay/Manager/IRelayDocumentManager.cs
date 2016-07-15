using System;
using System.IO;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Relay.Manager
{
	/// <summary>
	/// Interface IRelayDocumentManager
	/// </summary>
	public interface IRelayDocumentManager
	{
		/// <summary>
		/// Saves the specified identifier.
		/// </summary>
		/// <param name="metadata">The metadata.</param>
		/// <param name="id">The identifier.</param>
		/// <param name="contentType">Type of the content.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>Document.</returns>
		Document Save(string metadata, Guid id, string contentType, Stream stream);

		/// <summary>
		/// Gets the document by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Document.</returns>
		Document GetDocumentById(Guid id);

		/// <summary>
		/// Pings this instance.
		/// </summary>
		/// <returns>System.String.</returns>
		string Ping();
	}
}