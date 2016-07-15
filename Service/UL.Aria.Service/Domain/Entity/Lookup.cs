using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Lookup object, contains name and minimum number of ids needed to do a entity lookup.
    /// </summary>
    public class Lookup : DomainEntity
    {

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the container identifier. 
        /// Note: this Property will be null when not applicable.
        /// </summary>
        /// <value>
        /// The container identifier.
        /// </value>
        public Guid? ContainerId { get; set; }
    }
}
