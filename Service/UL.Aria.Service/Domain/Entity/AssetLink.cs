using System;

namespace UL.Aria.Service.Domain.Entity
{
	/// <summary>
	/// Class AssetLink.
	/// </summary>
	public class AssetLink : TrackedDomainEntity
	{
		/// <summary>
		/// Gets or sets the parent asset identifier.
		/// </summary>
		/// <value>The parent asset identifier.</value>
		public Guid ParentAssetId { get; set; }

		/// <summary>
		/// Gets or sets the asset identifier.
		/// </summary>
		/// <value>The asset identifier.</value>
		public Guid AssetId { get; set; }

		/// <summary>
		/// Gets or sets the type of the parent asset.
		/// </summary>
		/// <value>The type of the parent asset.</value>
		public string ParentAssetType { get; set; }

		/// <summary>
		/// Gets or sets the type of the asset.
		/// </summary>
		/// <value>The type of the asset.</value>
		public string AssetType { get; set; }

		/// <summary>
		/// Gets or sets the name of the asset.
		/// </summary>
		/// <value>The name of the asset.</value>
		public string AssetName { get; set; }
	}
}