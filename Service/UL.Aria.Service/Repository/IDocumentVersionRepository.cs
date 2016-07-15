using System;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Interface IDocumentVersionRepository
	/// </summary>
	public interface IDocumentVersionRepository : IRepositoryBase<DocumentVersion>
	{
		/// <summary>
		/// Creates the specified document version.
		/// </summary>
		/// <param name="documentVersion">The document version.</param>
		/// <returns>Guid.</returns>
		Guid Create(DocumentVersion documentVersion);

		/// <summary>
		/// Gets the document version by hash value.
		/// </summary>
		/// <param name="hashValue">The hash value.</param>
		/// <returns>DocumentVersion.</returns>
		DocumentVersion GetDocumentVersionByHashValue(string hashValue);
	}
}