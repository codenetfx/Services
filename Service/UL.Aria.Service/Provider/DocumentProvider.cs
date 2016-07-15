using System;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class DocumentProvider. This class cannot be inherited.
	/// </summary>
	public sealed class DocumentProvider : IDocumentProvider
	{
		private readonly IDocumentRepository _documentRepository;
		private readonly ITransactionFactory _transactionFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentProvider"/> class.
		/// </summary>
		/// <param name="transactionFactory">The transaction factory.</param>
		/// <param name="documentRepository">The document repository.</param>
		public DocumentProvider(ITransactionFactory transactionFactory,
			IDocumentRepository documentRepository)
		{
			_transactionFactory = transactionFactory;
			_documentRepository = documentRepository;
		}

		/// <summary>
		/// Deletes the document with the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Delete(Guid id)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				// remove the document
				_documentRepository.Delete(id);
				transactionScope.Complete();
			}
		}

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <returns>Document.</returns>
		public Document FetchById(Guid documentId)
		{
			return _documentRepository.FindById(documentId);
		}
	}
}