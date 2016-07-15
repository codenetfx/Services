using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Dto for exposing T and C data to the caller
    /// </summary>
    [DataContract]
    public class TermsAndConditionsDto
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [DataMember]
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the legal text.
        /// </summary>
        /// <value>
        /// The legal text.
        /// </value>
        [DataMember]
        public string LegalText { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        [DataMember]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the created by id.
        /// </summary>
        /// <value>
        /// The created by id.
        /// </value>
        [DataMember]
        public Guid CreatedById { get; set; }

        /// <summary>
        /// Gets or sets the updated date time.
        /// </summary>
        /// <value>
        /// The updated date time.
        /// </value>
        [DataMember]
        public DateTime UpdatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the updated by id.
        /// </summary>
        /// <value>
        /// The updated by id.
        /// </value>
        [DataMember]
        public Guid UpdatedById { get; set; }

    }
}