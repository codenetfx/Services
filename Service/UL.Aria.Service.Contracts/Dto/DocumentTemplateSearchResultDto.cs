using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Class DocumentTemplateSearchResultDto.
	/// </summary>
	[DataContract]
	public class DocumentTemplateSearchResultDto : SearchResultDto
	{
		/// <summary>
		/// Gets or sets the document template.
		/// </summary>
		/// <value>The document template.</value>
		[DataMember]
		public DocumentTemplateDto DocumentTemplate { get; set; }

		/// <summary>
		/// Gets or sets the created by.
		/// </summary>
		/// <value>The created by.</value>
		[DataMember]
		public string CreatedBy { get; set; }

		/// <summary>
		/// Gets or sets the last modified by.
		/// </summary>
		/// <value>The last modified by.</value>
		[DataMember]
		public string LastModifiedBy { get; set; }
	}
}