using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class AriaMetaDataLinkRepository.
	/// </summary>
	public class AriaMetaDataLinkRepository : RepositoryCommon, IAriaMetaDataLinkRepository
	{
		/// <summary>
		/// Fetches the asset links.
		/// </summary>
		/// <param name="assetId">The asset identifier.</param>
		/// <returns>IList&lt;MetaDataLink&gt;.</returns>
		public IList<MetaDataLink> FetchAssetLinks(Guid assetId)
		{
			return ExecuteReaderCommand(database => InitializeFindAssetLinksCommand(assetId, database), ConstructAssetLink);
		}

		/// <summary>
		/// Fetches the parent links.
		/// </summary>
		/// <param name="parentAssetId">The parent asset identifier.</param>
		/// <returns>IList&lt;MetaDataLink&gt;.</returns> 
		public IList<MetaDataLink> FetchParentLinks(Guid parentAssetId)
		{
			return ExecuteReaderCommand(database => InitializeFindParentLinksCommand(parentAssetId, database),
				ConstructParentLink);
		}

		/// <summary>
		/// Deletes the specified parent asset identifier.
		/// </summary>
		/// <param name="parentAssetId">The parent asset identifier.</param>
		/// <param name="assetId">The asset identifier.</param>
		public void Delete(Guid parentAssetId, Guid assetId)
		{
			ExecuteNonQueryCommand(database => InitializeDeleteCommand(parentAssetId, assetId, database), parentAssetId);
		}

		/// <summary>
		/// Creates the specified parent asset identifier.
		/// </summary>
		/// <param name="parentAssetId">The parent asset identifier.</param>
		/// <param name="assetId">The asset identifier.</param>
		public void Create(Guid parentAssetId, Guid assetId)
		{
			ExecuteNonQueryCommand(database => InitializeCreateCommand(parentAssetId, assetId, database), parentAssetId);
		}

		private static DbCommand InitializeFindAssetLinksCommand(Guid assetId, Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[p_AriaMetaDataLink_GetAssetLinks]");
			db.AddInParameter(command, "AssetId", DbType.Guid, assetId);
			return command;
		}

		private static MetaDataLink ConstructAssetLink(IDataReader reader)
		{
			return new MetaDataLink
			{
				ParentId = reader.GetValue<Guid>("ParentAssetId")
			};
		}

		private static DbCommand InitializeFindParentLinksCommand(Guid parentAssetId, Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[p_AriaMetaDataLink_GetParentLinks]");
			db.AddInParameter(command, "ParentAssetId", DbType.Guid, parentAssetId);
			return command;
		}

		private static MetaDataLink ConstructParentLink(IDataReader reader)
		{
			return new MetaDataLink
			{
				AssetId = reader.GetValue<Guid>("AssetId")
			};
		}

		private static DbCommand InitializeDeleteCommand(Guid parentAssetId, Guid assetId, Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[p_AriaMetaDataLink_Delete]");
			db.AddInParameter(command, "ParentAssetId", DbType.Guid, parentAssetId);
			db.AddInParameter(command, "AssetId", DbType.Guid, assetId);
			return command;
		}

		private static DbCommand InitializeCreateCommand(Guid parentAssetId, Guid assetId, Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[p_AriaMetaDataLink_Insert]");
			db.AddInParameter(command, "ParentAssetId", DbType.Guid, parentAssetId);
			db.AddInParameter(command, "AssetId", DbType.Guid, assetId);
			return command;
		}
	}
}