using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class DocumentMetaDataDto
    {
        /// <summary>
        ///     Gets or sets the document id.
        /// </summary>
        /// <value>
        ///     The document id.
        /// </value>
        [DataMember]
        public Guid DocumentId { get; set; }
    }
}