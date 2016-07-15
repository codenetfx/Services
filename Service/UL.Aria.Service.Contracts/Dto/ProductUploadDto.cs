using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class NewProjectDto
    /// </summary>
    [DataContract]
    public class ProductUploadDto
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets the company id.
        /// </summary>
        /// <value>The company id.</value>
        [DataMember]
        public Guid CompanyId { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        ///     Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        ///     Gets or sets the created on.
        /// </summary>
        /// <value>The created on.</value>
        [DataMember]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        ///     Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        [DataMember]
        public Guid CreatedBy { get; set; }

        /// <summary>
        ///     Gets or sets the updated on.
        /// </summary>
        /// <value>The updated on.</value>
        [DataMember]
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        ///     Gets or sets the updated by.
        /// </summary>
        /// <value>The updated by.</value>
        [DataMember]
        public Guid UpdatedBy { get; set; }

        /// <summary>
        ///     Gets or sets the created by user login id.
        /// </summary>
        /// <value>The created by user login id.</value>
        [DataMember]
        public string CreatedByUserLoginId { get; set; }
    }
}