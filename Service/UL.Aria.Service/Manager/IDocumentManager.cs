using System;
using System.Collections.Generic;
using UL.Aria.Service.Auditing;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	/// Interface IDocumentManager
	/// </summary>
	[Audit]
    public interface IDocumentManager
	{
		/// <summary>
		/// Creates the specified container identifier.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="metaData">The meta data.</param>
		/// <returns>Guid.</returns>
		[AuditIgnore]
        Guid Create(Guid containerId, IDictionary<string, string> metaData);

		/// <summary>
		/// Updates the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="metaData">The meta data.</param>
		[AuditIgnore]
        void Update(Guid id, IDictionary<string, string> metaData);

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>IDictionary&lt;System.String, System.String&gt;.</returns>
        [AuditIgnore]
        IDictionary<string, string> FetchById(Guid id);

		/// <summary>
		/// Deletes the document with the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
        [AuditIgnore]
        void Delete(Guid id);

		/// <summary>
		/// Fetches the document by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Document.</returns>
        [AuditIgnore]
        Document FetchDocumentById(Guid id);

		/// <summary>
		/// Locks the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
        [AuditIgnore]
        void Lock(Guid id);

		/// <summary>
		/// Unlocks the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="verify">if set to <c>true</c> [verify].</param>
        [AuditResource("id", ActionType = "Unlock")]
        void Unlock(Guid id, bool verify);
	}
}