using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// represent contextual information to be passed into the Add Document Method of the ITransferfile Service
    /// </summary>
    [DataContract]
    public class AddDocumentDto
    {		
        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>
        /// The permission.
        /// </value>
		[DataMember]
		public DocumentPermissionEnumDto Permission { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
		[DataMember]
		public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
		[DataMember]
		public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the document.
        /// </summary>
        /// <value>
        /// The type of the document.
        /// </value>
		[DataMember]
		public Guid DocumentTypeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AddDocumentDto" /> is overwrite.
        /// </summary>
        /// <value>
        ///   <c>true</c> if overwrite; otherwise, <c>false</c>.
        /// </value>
		[DataMember]
		public bool Overwrite { get; set; }

        /// <summary>
        /// Gets or sets the container id.
        /// </summary>
        /// <value>
        /// The container id.
        /// </value>
		[DataMember]
		public Guid ContainerId { get; set; }

		/// <summary>
		/// Gets or sets the user id.
		/// </summary>
		/// <value>
		/// The user id.
		/// </value>
		[DataMember]
		public Guid UserId { get; set; }
    }
}