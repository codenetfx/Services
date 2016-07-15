using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Interface IAriaMetaDataLinkRepository
	/// </summary>
	public interface IAriaMetaDataLinkRepository
	{
		/// <summary>
		/// Fetches the asset links.
		/// </summary>
		/// <param name="assetId">The asset identifier.</param>
		/// <returns>IList&lt;MetaDataLink&gt;.</returns>
		IList<MetaDataLink> FetchAssetLinks(Guid assetId);

		/// <summary>
		/// Fetches the parent links.
		/// </summary>
		/// <param name="parentAssetId">The parent asset identifier.</param>
		/// <returns>IList&lt;MetaDataLink&gt;.</returns>
		IList<MetaDataLink> FetchParentLinks(Guid parentAssetId);

		/// <summary>
		/// Deletes the specified parent asset identifier.
		/// </summary>
		/// <param name="parentAssetId">The parent asset identifier.</param>
		/// <param name="assetId">The asset identifier.</param>
		void Delete(Guid parentAssetId, Guid assetId);

		/// <summary>
		/// Creates the specified parent asset identifier.
		/// </summary>
		/// <param name="parentAssetId">The parent asset identifier.</param>
		/// <param name="assetId">The asset identifier.</param>
		void Create(Guid parentAssetId, Guid assetId);
	}
}