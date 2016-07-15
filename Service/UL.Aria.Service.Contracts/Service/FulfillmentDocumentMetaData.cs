using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Contract for external systems which require fulfillment document information.
    /// </summary>
    [DataContract(Namespace = "http://portal.ul.com")]
    public class FulfillmentDocumentMetaData
    {
        /// <summary>
        /// Gets or sets the document unique identifier.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        [DataMember]
        public string DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the type of the document. 
        /// i.e. category of the document.
        /// </summary>
        /// <value>
        /// The type of the document.
        /// </value>
        [DataMember]
        public string DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the type of the content. This is a web mime type, e.g. text/plain.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        [DataMember]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the document permission.
        /// Indicates the visibility to customer users of the docoument.
        /// </summary>
        /// <value>
        /// The document permission.
        /// </value>
        [DataMember]
        public string DocumentPermission { get; set; }

        /// <summary>
        /// Gets or sets the name of the document file.
        /// </summary>
        /// <value>
        /// The name of the document file.
        /// </value>
        [DataMember]
        public string DocumentFileName { get; set; }

        /// <summary>
        /// Gets or sets the title of the document.
        /// This is not the file name.
        /// </summary>
        /// <value>
        /// The name of the document.
        /// </value>
        [DataMember]
        public string DocumentTitle { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>
        /// The last modified date.
        /// </value>
        [DataMember]
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the identity of the last user to modify this.
        /// </summary>
        /// <value>
        /// The modified by.
        /// </value>
        [DataMember]
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the name of the last user to modify this.
        /// </summary>
        /// <value>
        /// The modified by.
        /// </value>
        [DataMember]
        public string LastModifiedByName { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        [DataMember]
        public long FileSize { get; set; }
    }
}