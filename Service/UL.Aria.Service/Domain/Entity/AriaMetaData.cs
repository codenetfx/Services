using System;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Domain.Entity
{
	/// <summary>
	/// Class AriaMetaData.
	/// </summary>
	public class AriaMetaData : DomainEntity
	{
		/// <summary>
		/// Gets or sets the parent asset identifier.
		/// </summary>
		/// <value>The parent asset identifier.</value>
		public Guid ParentAssetId { get; set; }

		/// <summary>
		/// Gets or sets the URI.
		/// </summary>
		/// <value>The URI.</value>
		public string Uri { get; set; }

		/// <summary>
		/// Gets or sets the name of the asset.
		/// </summary>
		/// <value>The name of the asset.</value>
		public string AssetName { get; set; }

		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		/// <value>The version.</value>
		public string Version { get; set; }

		/// <summary>
		/// Gets or sets the meta data.
		/// </summary>
		/// <value>The meta data.</value>
		public string MetaData { get; set; }

		/// <summary>
		/// Gets or sets the claims.
		/// </summary>
		/// <value>The claims.</value>
		public string Claims { get; set; }

		/// <summary>
		/// Gets or sets the security descriptor.
		/// </summary>
		/// <value>The security descriptor.</value>
		public byte[] SecurityDescriptor { get; set; }

		/// <summary>
		/// Gets or sets the last modified time.
		/// </summary>
		/// <value>The last modified time.</value>
		public DateTime LastModifiedTime { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is parsed.
		/// </summary>
		/// <value><c>true</c> if this instance is parsed; otherwise, <c>false</c>.</value>
		public bool IsParsed { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is deleted.
		/// </summary>
		/// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
		public bool IsDeleted { get; set; }

		/// <summary>
		/// Gets or sets the row number.
		/// </summary>
		/// <value>The row number.</value>
		public long RowNumber { get; set; }

		/// <summary>
		/// Gets or sets the available claims.
		/// </summary>
		/// <value>The available claims.</value>
		public string AvailableClaims { get; set; }
	}
}