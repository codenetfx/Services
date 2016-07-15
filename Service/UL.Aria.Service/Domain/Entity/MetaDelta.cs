using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Provides a data contract to show diferences at a field level for a specific version of an entity.
    /// </summary>
    public class MetaDelta
    {
        /// <summary>
        /// Gets or sets the orignal value.
        /// </summary>
        /// <value>
        /// The orignal value.
        /// </value>
        public string OrignalValue { get; set; }

        /// <summary>
        /// Gets or sets the modified value.
        /// </summary>
        /// <value>
        /// The modified value.
        /// </value>
        public string ModifiedValue { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the aria field.
        /// </summary>
        /// <value>
        /// The name of the aria field.
        /// </value>
        public string AriaFieldName { get; set; }

    }
}
