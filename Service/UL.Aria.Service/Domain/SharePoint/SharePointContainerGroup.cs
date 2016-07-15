using System.Collections.Generic;

namespace UL.Aria.Service.Domain.SharePoint
{
	/// <summary>
	/// Class SharePointContainerGroup
	/// </summary>
	public class SharePointContainerGroup
	{
		/// <summary>
		/// Gets or sets the name of the group.
		/// </summary>
		/// <value>The name of the group.</value>
		public string GroupName { get; set; }

		/// <summary>
		/// Gets or sets the claims.
		/// </summary>
		/// <value>The claims.</value>
		public List<SharePointContainerClaim> Claims { get; set; }
	}
}