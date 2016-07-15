using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class DocumentVersionRepository. This class cannot be inherited.
	/// </summary>
	public sealed class DocumentVersionRepository : TrackedDomainEntityRepositoryBase<DocumentVersion>,
		IDocumentVersionRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentVersionRepository"/> class.
		/// </summary>
		public DocumentVersionRepository()
			: base("DocumentVersionID", "DocumentVersion")
		{
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
		protected override void AddTableRowFields(DocumentVersion entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
		{
			base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);

			dr["HashValue"] = entity.HashValue;
		}

		/// <summary>
		///     Constructs the entity.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		protected override DocumentVersion ConstructEntity(IDataReader reader)
		{
			var entity = base.ConstructEntity(reader);

			entity.HashValue = reader.GetValue<string>("HashValue");

			return entity;
		}

		/// <summary>
		/// Gets the document de dup by hash value.
		/// </summary>
		/// <param name="hashValue">The hash value.</param>
		/// <returns>DocumentVersion.</returns>
		/// <exception cref="UL.Enterprise.Foundation.Data.DatabaseItemNotFoundException"></exception>
		public DocumentVersion GetDocumentVersionByHashValue(string hashValue)
		{
			var result = ExecuteReaderCommand(database => InitializeGetDocumentVersionByHashValueCommand(hashValue, database), ConstructEntity);

			if (result.Count == 0)
				throw new DatabaseItemNotFoundException();
			return result.First();
		}

		private static DbCommand InitializeGetDocumentVersionByHashValueCommand(string hashValue, Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[pDocumentVersion_GetByHashValue]");
			db.AddInParameter(command, "@HashValue", DbType.String, hashValue);
			return command;
		}
	}
}