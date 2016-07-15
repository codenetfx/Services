using System;
using System.Collections.Generic;
using System.IO;
using UL.Aria.Service.Auditing;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Content provider interface for assets like documents and tasks.
    /// </summary>
    [Audit]
    public interface IAssetProvider
    {
        /// <summary>
        ///     Fetches all assets in a container.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="assetType"></param>
        /// <returns>SearchResultSet.</returns>
		[AuditIgnore]
		SearchResultSet FetchAllAssets(Guid containerId, EntityTypeEnumDto? assetType = null);

        /// <summary>
        ///     Fetches the specified entity content.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <returns>The content</returns>
		[AuditIgnore]
		Stream FetchContent(Guid assetId);

        /// <summary>
        ///     Fetches the specified entity metadata.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <returns>The metadata.</returns>
		[AuditIgnore]
		IDictionary<string, string> Fetch(Guid assetId);

        /// <summary>
        ///     Creates entity content.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="contentStream">The content stream.</param>
        /// <returns>The created content id.</returns>
		[AuditIgnore]
		string CreateContent(Guid assetId, Stream contentStream);

        /// <summary>
        ///     Creates entity metadata.
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="metadataStream">The metadata stream.</param>
        /// <param name="newEntityId"></param>
        /// <returns>The created content id.</returns>
		[AuditIgnore]
		string Create(Guid containerId, IDictionary<string, string> metadataStream, Guid newEntityId);

        /// <summary>
        ///     Updates entity content.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="contentStream">The content stream.</param>
		[AuditIgnore]
		void UpdateContent(Guid assetId, Stream contentStream);

	    /// <summary>
		/// Updates the content's uri and size.
		/// </summary>
	    /// <param name="assetId">The asset identifier.</param>
	    /// <param name="contentUri">The content URI.</param>
	    /// <param name="size">The size.</param>
	    [AuditIgnore]
	    void UpdateContentUriAndSize(Guid assetId, Uri contentUri, long size);

        /// <summary>
        ///     Updates entity metadata.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="metadataStream">The metadata stream.</param>
		[AuditIgnore]
		void Update(Guid assetId, IDictionary<string, string> metadataStream);

        /// <summary>
        ///     Updates entity metadata.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="primarySearchEntityBase">The container.</param>
		[AuditIgnore]
		void Update(Guid assetId, PrimarySearchEntityBase primarySearchEntityBase);

        /// <summary>
        ///     Deletes the content.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
		[AuditIgnore]
		void DeleteContent(Guid assetId);

        /// <summary>
        ///     Creates the specified new container id.
        /// </summary>
        /// <param name="newContainerId">The new container id.</param>
        /// <param name="newAssetId">The new asset id.</param>
        /// <param name="primarySearchEntityBase">The primary search entity base.</param>
        /// <returns></returns>
		[AuditIgnore]
		Guid Create(Guid newContainerId, Guid newAssetId, PrimarySearchEntityBase primarySearchEntityBase);

		/// <summary>
		/// Creates the asset link.
		/// </summary>
		/// <param name="assetLink">The asset link.</param>
	    [AuditResource("assetLink", ActionType = "AssetLink Added")]
		void CreateAssetLink(AssetLink assetLink);

		/// <summary>
		/// Deletes the asset link.
		/// </summary>
		/// <param name="assetLink">The asset link.</param>
		[AuditResource("assetLink", ActionType = "AssetLink Removed")]
		void DeleteAssetLink(AssetLink assetLink);

        /// <summary>
        ///     Fetches the asset links.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <returns>IList{System.String}.</returns>
		[AuditIgnore]
		IList<string> FetchAssetLinks(Guid assetId);

        /// <summary>
        ///     Fetches the parent asset links.
        /// </summary>
        /// <param name="parentId">The parent id.</param>
        /// <returns>System.Collections.Generic.IList{System.String}.</returns>
		[AuditIgnore]
		IList<string> FetchParentAssetLinks(Guid parentId);

        /// <summary>
        ///     Deletes the specified asset id.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
		[AuditIgnore]
		void Delete(Guid assetId);

        /// <summary>
        ///     Fetches the multiple parent asset links.
        /// </summary>
        /// <param name="parentIds">The parent ids.</param>
        /// <returns>IEnumerable{MetaDataLink}.</returns>
		[AuditIgnore]
		IEnumerable<MetaDataLink> FetchMultipleParentAssetLinks(IEnumerable<Guid> parentIds);

        /// <summary>
        ///     Saves the assets.
        /// </summary>
        /// <param name="entities">The entities.</param>
		[AuditIgnore]
		void SaveAssets(IEnumerable<PrimarySearchEntityBase> entities);

        /// <summary>
        /// Fetches all documents.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <returns></returns>
		[AuditIgnore]
		SearchResultSet FetchAllDocuments(Guid containerId);
    }
}