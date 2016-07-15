using System;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface IDocumentProvider
	/// </summary>
	public interface IDocumentProvider
	{
		/// <summary>
		/// Deletes the document with the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		void Delete(Guid id);

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <returns>Document.</returns>
		Document FetchById(Guid documentId);
	}
}