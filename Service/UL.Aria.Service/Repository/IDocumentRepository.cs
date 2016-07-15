using System;
using System.Collections.Generic;
using UL.Aria.Service.Auditing;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Interface IDocumentRepository
	/// </summary>
	[Audit]
	public interface IDocumentRepository : IRepositoryBase<Document>
	{
		/// <summary>
		/// Creates the specified document.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns>Guid.</returns>
		[AuditResource("document", ActionType = "Created")]
		Guid Create(Document document);

		/// <summary>
		/// Updates the specified document.
		/// </summary>
		/// <param name="document">The document.</param>
		[AuditResource("document", ActionType = "Updated")]
		new void Update(Document document);

		/// <summary>
		/// Deletes the specified entity identifier.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		[AuditResource("entityId", ActionType = "Deleted")]
		void Delete(Guid entityId);
	}
}