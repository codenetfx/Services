using System;
using System.Collections.Generic;
using System.IO;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class AzureStorageBlobInternal.
	/// </summary>
	public class AzureStorageBlobInternal
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AzureStorageBlobInternal"/> class.
		/// </summary>
		public AzureStorageBlobInternal()
		{
			Metadata = new Dictionary<string, string>();
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type of the content.
		/// </summary>
		/// <value>The type of the content.</value>
		public string ContentType { get; set; }

		/// <summary>
		/// Gets or sets the URI.
		/// </summary>
		/// <value>The URI.</value>
		public Uri Uri { get; set; }

		/// <summary>
		/// Gets or sets the hash value.
		/// </summary>
		/// <value>The hash value.</value>
		public string HashValue { get; set; }

		/// <summary>
		/// Gets or sets the last modified.
		/// </summary>
		/// <value>The last modified.</value>
		public DateTime LastModified { get; set; }

		/// <summary>
		/// Gets or sets the stream.
		/// </summary>
		/// <value>The stream.</value>
		public Stream Stream { get; set; }

		/// <summary>
		/// Gets or sets the metadata.
		/// </summary>
		/// <value>The metadata.</value>
		public IDictionary<string, string> Metadata { get; set; }
	}
}