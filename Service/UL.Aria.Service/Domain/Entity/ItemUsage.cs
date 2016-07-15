using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Provides an classifer for checking an entites usage.
    /// </summary>
    public class ItemUsage : DomainEntity
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; set; }
    }
}