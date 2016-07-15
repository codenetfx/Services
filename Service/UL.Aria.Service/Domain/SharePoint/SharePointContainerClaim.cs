namespace UL.Aria.Service.Domain.SharePoint
{
	/// <summary>
	/// Class SharePointContainerClaim
	/// </summary>
	public class SharePointContainerClaim
	{
		/// <summary>
		/// Gets or sets the type of the claim.
		/// </summary>
		/// <value>The type of the claim.</value>
		public string ClaimType { get; set; }

		/// <summary>
		/// Gets or sets the claim value.
		/// </summary>
		/// <value>The claim value.</value>
		public string ClaimValue { get; set; }

		/// <summary>
		/// Gets or sets the type of the claim value.
		/// </summary>
		/// <value>The type of the claim value.</value>
		public string ClaimValueType { get; set; }
	}
}