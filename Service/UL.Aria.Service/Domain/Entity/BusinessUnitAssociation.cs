using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Business Unit Association
    /// </summary>
    public class BusinessUnitAssociation:TrackedDomainEntity
    {
        /// <summary>
        /// Gets or sets the business unit identifier.
        /// </summary>
        /// <value>
        /// The business unit identifier.
        /// </value>
        public Guid BusinessUnitId { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public Guid ParentId { get; set; }
    }
}
