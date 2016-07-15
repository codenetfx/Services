using System;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class ProductUpload
    /// </summary>
    public class ProductUpload : TrackedDomainEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductUpload" /> class.
        /// </summary>
        public ProductUpload() : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductUpload" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public ProductUpload(Guid? id)
            : base(id)
        {
        }

        /// <summary>
        ///     Gets or sets the company id.
        /// </summary>
        /// <value>The company id.</value>
        public Guid CompanyId { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public ProductUploadStatusEnumDto Status { get; set; }

        /// <summary>
        ///     Gets or sets the excel file.
        /// </summary>
        /// <value>The excel file.</value>
        public byte[] FileContent { get; set; }

        /// <summary>
        ///     Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }

        /// <summary>
        ///     Gets or sets the created by user login id.
        /// </summary>
        /// <value>The created by user login id.</value>
        public string CreatedByUserLoginId { get; set; }
    }
}