using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Repository
{
    /// <summary>
    /// AuditableEntity interface
    /// </summary>
    public interface IAuditableEntity:IPrimaryAssocationEntity
    {
        /// <summary>
        ///     Gets or sets the user it was created by.
        /// </summary>
        /// <value>
        ///     The created by.
        /// </value>
        Guid CreatedById { get; set; }

        /// <summary>
        ///     Gets or sets the created on.
        /// </summary>
        /// <value>
        ///     The created on.
        /// </value>
        DateTime CreatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the updated on.
        /// </summary>
        /// <value>
        ///     The updated on.
        /// </value>
        DateTime UpdatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets who it was updated by.
        /// </summary>
        /// <value>
        ///     The updated by person.
        /// </value>
        Guid UpdatedById { get; set; }
    }
}
