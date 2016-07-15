using System;

namespace UL.Aria.Service.Domain.Entity
{
	/// <summary>
	/// Class DocumentVersion. This class cannot be inherited.
	/// </summary>
	public sealed class DocumentVersion : TrackedDomainEntity
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentVersion"/> class.
		/// </summary>
		public DocumentVersion()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TrackedDomainEntity" /> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public DocumentVersion(Guid? id)
			: base(id)
		{
		}

		/// <summary>
		/// Gets or sets the hash value.
		/// </summary>
		/// <value>The hash value.</value>
		public string HashValue { get; set; }
	}
}