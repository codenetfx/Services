using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class AriaMetaDataRepository.
	/// </summary>
	public class AriaMetaDataRepository : RepositoryBase<AriaMetaData>, IAriaMetaDataRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AriaMetaDataRepository"/> class.
		/// </summary>
		public AriaMetaDataRepository()
			: base("AssetId")
		{
		}

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>AriaMetaData.</returns>
		AriaMetaData IAriaMetaDataRepository.FetchById(Guid id)
		{
			return FindById(id);
		}

		/// <summary>
		/// Deletes the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		void IAriaMetaDataRepository.Delete(Guid id)
		{
			Remove(id);
		}

		/// <summary>
		/// Creates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void IAriaMetaDataRepository.Create(AriaMetaData entity)
		{
			Add(entity);
		}

		/// <summary>
		/// Res the crawl asset.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		public void ReCrawlAsset(Guid entityId)
		{
			ExecuteNonQueryCommand(database => InitializeReCrawlAssetCommand(entityId, database), entityId);
		}

		/// <summary>
		/// Fetches the by parent identifier.
		/// </summary>
		/// <param name="parentAssetId">The parent asset identifier.</param>
		/// <returns>IList&lt;AriaMetaDataItem&gt;.</returns>
		public IList<AriaMetaDataItem> FetchByParentId(Guid parentAssetId)
		{
			return ExecuteReaderCommand(database => InitializeFindByParentIdCommand(parentAssetId, database),
				ConstructAriaMetaDataItems);
		}

		/// <summary>
		/// Fetches the available claims by container asset identifier.
		/// </summary>
		/// <param name="containerAssetId">The container asset identifier.</param>
		/// <returns>System.String.</returns>
		public string FetchAvailableClaimsByContainerAssetId(Guid containerAssetId)
		{
			var ariaMetaData =
				ExecuteReaderCommand(database => InitializeAvailableClaimsByContainerAssetIdCommand(containerAssetId, database),
					ConstructAriaMetaDataAvailableClaims).FirstOrDefault();
			return ariaMetaData == null ? null : ariaMetaData.AvailableClaims;
		}

		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>System.Int32.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public override int Update(AriaMetaData entity)
		{
			return ExecuteNonQueryCommand(database => InitializeUpdateCommand(entity, database), entity);
		}

		/// <summary>
		/// Finds all.
		/// </summary>
		/// <returns>IList&lt;AriaMetaData&gt;.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public override IList<AriaMetaData> FindAll()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Finds the by identifier.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		/// <returns>AriaMetaData.</returns>
		/// <exception cref="UL.Enterprise.Foundation.Data.DatabaseItemNotFoundException"></exception>
		public override AriaMetaData FindById(Guid entityId)
		{
			var ariaMetaData =
				ExecuteReaderCommand(database => InitializeFindByIdCommand(entityId, database), ConstructEntity).FirstOrDefault();

			if (ariaMetaData == null)
				throw new DatabaseItemNotFoundException(string.Format("Asset {0} not found", entityId));

			return ariaMetaData;
		}

		private static DbCommand InitializeFindByIdCommand(Guid entityId, Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[p_AriaMetaData_Get]");
			db.AddInParameter(command, "AssetId", DbType.Guid, entityId);
			return command;
		}

		/// <summary>
		/// Adds the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public override void Add(AriaMetaData entity)
		{
			ExecuteNonQueryCommand(database => InitializeCreateAssetCommand(entity, database), entity);
		}

		private static DbCommand InitializeCreateAssetCommand(AriaMetaData ariaMetaData, Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[p_AriaMetaData_Insert]");
			db.AddInParameter(command, "AssetId", DbType.Guid, ariaMetaData.Id);
			db.AddInParameter(command, "ParentAssetId", DbType.Guid, ariaMetaData.ParentAssetId);
			db.AddInParameter(command, "Uri", DbType.String, ariaMetaData.Uri);
			db.AddInParameter(command, "AssetName", DbType.String, ariaMetaData.AssetName);
			db.AddInParameter(command, "Version", DbType.String, ariaMetaData.Version);
			db.AddInParameter(command, "Claims", DbType.String, ariaMetaData.Claims);
			db.AddInParameter(command, "MetaData", DbType.String, ariaMetaData.MetaData);
			db.AddInParameter(command, "SecurityDescriptor", DbType.Binary, ariaMetaData.SecurityDescriptor);
			db.AddInParameter(command, "LastModifiedTime", DbType.DateTime2, DateTime.UtcNow);
			db.AddInParameter(command, "IsParsed", DbType.Boolean, ariaMetaData.IsParsed);
			db.AddInParameter(command, "IsDeleted", DbType.Boolean, ariaMetaData.IsDeleted);
			db.AddInParameter(command, "AvailableClaims", DbType.String, ariaMetaData.AvailableClaims);
			return command;
		}

		/// <summary>
		/// Removes the specified entity identifier.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		public override void Remove(Guid entityId)
		{
			ExecuteNonQueryCommand(database => InitializeRemoveCommand(entityId, database), entityId);
		}

		private static DbCommand InitializeUpdateCommand(AriaMetaData entity, Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[p_AriaMetaData_Update]");
			db.AddInParameter(command, "AssetId", DbType.Guid, entity.Id);
			db.AddInParameter(command, "ParentAssetId", DbType.Guid, entity.ParentAssetId);
			db.AddInParameter(command, "Uri", DbType.String, entity.Uri);
			db.AddInParameter(command, "AssetName", DbType.String, entity.AssetName);
			db.AddInParameter(command, "Version", DbType.String, entity.Version);
			db.AddInParameter(command, "Claims", DbType.String, entity.Claims);
			db.AddInParameter(command, "MetaData", DbType.String, entity.MetaData);
			db.AddInParameter(command, "SecurityDescriptor", DbType.Binary, entity.SecurityDescriptor);
			db.AddInParameter(command, "LastModifiedTime", DbType.DateTime2, DateTime.UtcNow);
			db.AddInParameter(command, "IsParsed", DbType.Boolean, entity.IsParsed);
			db.AddInParameter(command, "IsDeleted", DbType.Boolean, entity.IsDeleted);
			return command;
		}

		private static DbCommand InitializeRemoveCommand(Guid entityId, Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[p_AriaMetaData_DeleteRow]");
			db.AddInParameter(command, "AssetId", DbType.Guid, entityId);
			return command;
		}

		private static DbCommand InitializeReCrawlAssetCommand(Guid entityId, Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[p_AriaMetaData_UpdateForRecrawl]");
			db.AddInParameter(command, "AssetId", DbType.Guid, entityId);
			db.AddInParameter(command, "LastModifiedTime", DbType.DateTime2, DateTime.UtcNow);
			db.AddInParameter(command, "IsParsed", DbType.Boolean, false);
			return command;
		}

		private static DbCommand InitializeFindByParentIdCommand(Guid parentAssetId, Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[p_AriaMetaData_GetByParentId]");
			db.AddInParameter(command, "ParentAssetId", DbType.Guid, parentAssetId);
			return command;
		}

		private static AriaMetaDataItem ConstructAriaMetaDataItems(IDataReader dataReader)
		{
			return new AriaMetaDataItem
			{
				AssetId = dataReader.GetValue<Guid>("AssetId"),
				MetaData = dataReader.GetValue<string>("MetaData")
			};
		}

		private static DbCommand InitializeAvailableClaimsByContainerAssetIdCommand(Guid containerAssetId, Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[p_AriaMetaData_GetAvailableClaimsByContainerAssetId]");
			db.AddInParameter(command, "ContainerAssetId", DbType.Guid, containerAssetId);
			return command;
		}

		private static AriaMetaData ConstructAriaMetaDataAvailableClaims(IDataReader dataReader)
		{
			return new AriaMetaData
			{
				AvailableClaims = dataReader.GetValue<string>("AvailableClaims")
			};
		}
	}
}