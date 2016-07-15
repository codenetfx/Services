using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Class ProjectTemplateSearchResultDto.
	/// </summary>
    [DataContract]
    public class ProjectTemplateSearchResultDto:SearchResultDto
    {
		/// <summary>
		/// Gets or sets the project template.
		/// </summary>
		/// <value>The project template.</value>
        [DataMember]
        public ProjectTemplateDto ProjectTemplate { get; set; }

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