namespace UL.Aria.Service.Domain.Entity
{
	/// <summary>
	/// Class EntityAssetLinkHistory. This class cannot be inherited.
	/// </summary>
	public sealed class EntityAssetLinkHistory : EntityHistory
	{
		/// <summary>
		/// Gets or sets the asset link.
		/// </summary>
		/// <value>The asset link.</value>
		public AssetLink AssetLink { get; set; }
	}
}