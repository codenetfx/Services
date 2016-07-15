using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Class DocumentDto.
	/// </summary>
	[DataContract]
	public class DocumentDto
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[DataMember]
		public Guid? Id { get; set; }

		/// <summary>
		/// Gets or sets the document version identifier.
		/// </summary>
		/// <value>The document version identifier.</value>
		[DataMember]
		public Guid DocumentVersionId { get; set; }

		/// <summary>
		/// Gets or sets the created by identifier.
		/// </summary>
		/// <value>The created by identifier.</value>
		[DataMember]
		public Guid CreatedById { get; set; }

		/// <summary>
		/// Gets or sets the created date time.
		/// </summary>
		/// <value>The created date time.</value>
		[DataMember]
		public DateTime CreatedDateTime { get; set; }

		/// <summary>
		/// Gets or sets the updated date time.
		/// </summary>
		/// <value>The updated date time.</value>
		[DataMember]
		public DateTime UpdatedDateTime { get; set; }

		/// <summary>
		/// Gets or sets the updated by identifier.
		/// </summary>
		/// <value>The updated by identifier.</value>
		[DataMember]
		public Guid UpdatedById { get; set; }

		/// <summary>
		/// Gets or sets the hash value.
		/// </summary>
		/// <value>The hash value.</value>
		[DataMember]
		public string HashValue { get; set; }
	}
}