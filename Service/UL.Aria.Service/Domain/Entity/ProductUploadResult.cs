using System;
using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Result of importing a <see cref="Product" />
    /// </summary>
    public class ProductUploadResult : TrackedDomainEntity
    {
        private IList<ProductUploadMessage> _messages = new List<ProductUploadMessage>();

        /// <summary>
        ///     Gets or sets the product upload id.
        /// </summary>
        /// <value>The product upload id.</value>
        public Guid ProductUploadId { get; set; }

        /// <summary>
        ///     Gets or sets the product.
        /// </summary>
        /// <value>
        ///     The product.
        /// </value>
        public Product Product { get; set; }

        /// <summary>
        ///     Gets or sets the errors.
        /// </summary>
        /// <value>
        ///     The errors.
        /// </value>
        public IList<ProductUploadMessage> Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid { get; set; }

        /// <summary>
        ///     Gets or sets the created by user login id.
        /// </summary>
        /// <value>The created by user login id.</value>
        public string CreatedByUserLoginId { get; set; }
    }
}