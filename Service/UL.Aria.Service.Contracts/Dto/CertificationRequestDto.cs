using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Data Transfer Object for Certification Requests.
    /// </summary>
    [DataContract]
    public class CertificationRequestDto : CertificationManagementDto
    {
        /// <summary>
        /// Gets or sets the project's end date (estimated if project still active).
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [DataMember]
        public DateTime? ProjectEndDate { get; set; }
        
        /// <summary>
        /// Gets or sets the project handler.
        /// </summary>
        /// <value>
        /// The project handler.
        /// </value>
        [DataMember]
        public string ProjectHandler { get; set; }

        /// <summary>
        ///     Gets or sets the CCN.
        /// </summary>
        /// <value>The CCN.</value>
        [DataMember]
        public string CCN { get; set; }
        
        /// <summary>
        ///     Gets or sets the file number.
        /// </summary>
        /// <value>The file number.</value>
        [DataMember]
        public string FileNo { get; set; }

        /// <summary>
        /// Gets or sets the name of the contact.
        /// </summary>
        /// <value>
        /// The name of the contact.
        /// </value>
        [DataMember]
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets the contact email.
        /// </summary>
        /// <value>
        /// The contact email.
        /// </value>
        [DataMember]
        public string ContactEmail { get; set; }


        /// <summary>
        /// Gets or sets the subscriber number.
        /// </summary>
        /// <value>
        /// The subscriber number.
        /// </value>
        [DataMember]
        public string SubscriberNumber { get; set; }

        /// <summary>
        /// Gets or sets the project number.
        /// </summary>
        /// <value>
        /// The project number.
        /// </value>
        [DataMember]
        public string ProjectNumber { get; set; }
    }
}