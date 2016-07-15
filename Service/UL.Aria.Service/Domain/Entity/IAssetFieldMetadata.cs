using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Interface IAssetFieldMetadata
    /// </summary>
    public interface IAssetFieldMetadata
    {
        /// <summary>
        ///     Gets the container asset fields.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns>Dictionary{System.StringAssetField}.</returns>
        Dictionary<string, AssetFieldMetadata.AssetField> GetContainerAssetFields(AssetTypeEnumDto assetType);

        /// <summary>
        /// Gets the container asset fields.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>Dictionary{System.StringAssetField}.</returns>
        Dictionary<string, AssetFieldMetadata.AssetField> GetContainerAssetFields(EntityTypeEnumDto? entityType);

        /// <summary>
        ///     Gets the container asset field.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="assetFieldPropertyCharacteristicName">Name of the asset field property or characteristic.</param>
        /// <returns>AssetField.</returns>
        AssetFieldMetadata.AssetField GetContainerAssetField(AssetTypeEnumDto assetType,
                                                           string assetFieldPropertyCharacteristicName);

        /// <summary>
        /// Gets the container asset field.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="assetFieldPropertyCharacteristicName">Name of the asset field property characteristic.</param>
        /// <returns>System.Nullable{AssetField}.</returns>
        AssetFieldMetadata.AssetField GetContainerAssetField(EntityTypeEnumDto? entityType,
                                                              string assetFieldPropertyCharacteristicName);

        /// <summary>
        ///     Gets the select properties.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns>System.String.</returns>
        string GetSelectProperties(AssetTypeEnumDto assetType);

        /// <summary>
        /// Gets the select properties.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>System.String.</returns>
        string[] GetSelectProperties(EntityTypeEnumDto? entityType);

        /// <summary>
        /// Gets the container asset fields for container.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns>IEnumerable{KeyValuePair{System.StringAssetField}}.</returns>
        IEnumerable<KeyValuePair<string, AssetFieldMetadata.AssetField>> GetContainerAssetFieldsForContainer(AssetTypeEnumDto assetType);

        /// <summary>
        /// Gets the container asset fields for container.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>IEnumerable{KeyValuePair{System.StringAssetField}}.</returns>
        IEnumerable<KeyValuePair<string, AssetFieldMetadata.AssetField>> GetContainerAssetFieldsForContainer(EntityTypeEnumDto? entityType);
    }
}