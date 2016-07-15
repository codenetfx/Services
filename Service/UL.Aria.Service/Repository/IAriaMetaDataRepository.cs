using System;
using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Interface IAriaMetaDataRepository
	/// </summary>
	public interface IAriaMetaDataRepository
	{
		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>AriaMetaData.</returns>
		AriaMetaData FetchById(Guid id);

		/// <summary>
		/// Deletes the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		void Delete(Guid id);

		/// <summary>
		/// Creates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Create(AriaMetaData entity);

		/// <summary>
		/// Res the crawl asset.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		void ReCrawlAsset(Guid entityId);

		/// <summary>
		/// Fetches the by parent identifier.
		/// </summary>
		/// <param name="parentAssetId">The parent asset identifier.</param>
		/// <returns>IList&lt;AriaMetaDataItem&gt;.</returns>
		IList<AriaMetaDataItem> FetchByParentId(Guid parentAssetId);

		/// <summary>
		/// Fetches the available claims by container asset identifier.
		/// </summary>
		/// <param name="containerAssetId">The container asset identifier.</param>
		/// <returns>System.String.</returns>
		string FetchAvailableClaimsByContainerAssetId(Guid containerAssetId);

		/// <summary>
		/// Updates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>System.Int32.</returns>
		int Update(AriaMetaData entity);
	}
}