using System;
using System.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class DocumentRepository. This class cannot be inherited.
	/// </summary>
	public sealed class DocumentRepository : TrackedDomainEntityRepositoryBase<Document>, IDocumentRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentRepository"/> class.
		/// </summary>
		public DocumentRepository() : base("DocumentID", "Document")
		{
		}

		/// <summary>
		/// Updates the specified document.
		/// </summary>
		/// <param name="document">The document.</param>
		public new void Update(Document document)
		{
			base.Update(document);
		}

		/// <summary>
		/// Deletes the specified entity identifier.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		public void Delete(Guid entityId)
		{
			Remove(entityId);
		}

		/// <summary>
		///     Adds the table row fields.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="isNew">
		///     if set to <c>true</c> [is new].
		/// </param>
		/// <param name="isDirty">
		///     if set to <c>true</c> [is dirty].
		/// </param>
		/// <param name="isDelete">
		///     if set to <c>true</c> [is delete].
		/// </param>
		/// <param name="dr">The dr.</param>
		protected override void AddTableRowFields(Document entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
		{
			base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);

			dr["DocumentVersionId"] = entity.DocumentVersionId;
		}

		/// <summary>
		///     Constructs the entity.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		protected override Document ConstructEntity(IDataReader reader)
		{
			var entity = base.ConstructEntity(reader);

			entity.DocumentVersionId = reader.GetValue<Guid>("DocumentVersionId");

			return entity;
		}
	}
}