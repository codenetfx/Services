using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class CompanyUserModelDto
    {
        /// <summary>
        ///     Gets or sets the user's display name.
        /// </summary>
        /// <value>
        ///     The company display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the login id.
        /// </summary>
        /// <value>
        ///     The email.
        /// </value>
        public string LoginId { get; set; }

        /// <summary>
        /// Gets or sets the company id.
        /// </summary>
        /// <value>
        /// The company id.
        /// </value>
        public Guid CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is admin.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is admin; otherwise, <c>false</c>.
        /// </value>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has access.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has access; otherwise, <c>false</c>.
        /// </value>
        public bool HasAccess { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can access orders.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can access orders; otherwise, <c>false</c>.
        /// </value>
        public bool CanAccessOrders { get; set; }
    }
}

