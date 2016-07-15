using System;

namespace UL.Aria.Service.Domain.Entity
{
	/// <summary>
	/// Class Document. This class cannot be inherited.
	/// </summary>
	public sealed class Document : TrackedDomainEntity
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Document"/> class.
		/// </summary>
		public Document()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TrackedDomainEntity" /> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public Document(Guid? id) : base(id)
		{
		}

		/// <summary>
		/// Gets or sets the document version identifier.
		/// </summary>
		/// <value>The document version identifier.</value>
		public Guid DocumentVersionId { get; set; }

		/// <summary>
		/// Gets or sets the hash value.
		/// </summary>
		/// <value>The hash value.</value>
		public string HashValue { get; set; }
	}
}