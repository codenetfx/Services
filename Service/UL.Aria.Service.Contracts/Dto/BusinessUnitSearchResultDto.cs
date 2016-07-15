using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Class ProjectTemplateSearchResultDto.
    /// </summary>
    [DataContract]
    public class BusinessUnitSearchResultDto : SearchResultDto
    {
        /// <summary>
        /// Gets or sets the project template.
        /// </summary>
        /// <value>The project template.</value>
        [DataMember]
        public BusinessUnitDto BusinessUnit { get; set; }
    }
}