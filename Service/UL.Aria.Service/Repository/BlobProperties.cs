using System;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class BlobProperties.
	/// </summary>
	public class BlobProperties
	{
		/// <summary>
		/// Gets or sets the type of the content.
		/// </summary>
		/// <value>The type of the content.</value>
		public string ContentType { get; set; }

		/// <summary>
		/// Gets or sets the size.
		/// </summary>
		/// <value>The size.</value>
		public long Size { get; set; }

		/// <summary>
		/// Gets or sets the URI.
		/// </summary>
		/// <value>The URI.</value>
		public Uri Uri { get; set; }
	}
}