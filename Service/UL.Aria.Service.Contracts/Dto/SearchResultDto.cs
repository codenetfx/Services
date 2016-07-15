using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Search result data transfer object class.
    /// </summary>
    [DataContract]
    public class SearchResultDto
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultDto"/> class.
        /// </summary>
        public SearchResultDto()
        {
            Metadata = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember]
        public EntityTypeEnumDto EntityType { get; set; }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        [DataMember]
        public DateTime ChangeDate { get; set; }

        /// <summary>
        /// Gets or sets the metadata dictionary.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        [DataMember]
        public IDictionary<string, string> Metadata { get; set; }
    }
}