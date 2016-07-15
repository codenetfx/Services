using System.Collections.Generic;

namespace UL.Aria.Service.Domain.SharePoint
{
	/// <summary>
	/// Class SharePointContainerList
	/// </summary>
	public class SharePointContainerList
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SharePointContainerList"/> class.
		/// </summary>
		public SharePointContainerList()
		{
			GroupPermissions = new List<SharePointContainerGroupPermission>();
		}

		/// <summary>
		/// Gets or sets the name of the list.
		/// </summary>
		/// <value>The name of the list.</value>
		public string ListName { get; set; }

		/// <summary>
		/// Gets or sets the type of the list.
		/// </summary>
		/// <value>The type of the list.</value>
		public SharePointContainerListType ListType { get; set; }

		/// <summary>
		/// Gets or sets the group permissions.
		/// </summary>
		/// <value>The group permissions.</value>
		public List<SharePointContainerGroupPermission> GroupPermissions { get; set; }
	}
}