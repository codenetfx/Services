using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Provides a data contract to show diferences at a field level for a specific version of an entity.
    /// </summary>
    [DataContract]
    public class MetaDeltaDto
    {
        /// <summary> 
        /// Gets or sets the orignal value.
        /// </summary>
        /// <value>
        /// The orignal value.
        /// </value>
        [DataMember]
        public string OrignalValue { get; set; }

        /// <summary>
        /// Gets or sets the modified value.
        /// </summary>
        /// <value>
        /// The modified value.
        /// </value>
        [DataMember]
        public string ModifiedValue { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the aria field.
        /// </summary>
        /// <value>
        /// The name of the aria field.
        /// </value>
        [DataMember]
        public string AriaFieldName { get; set; }

    }
}
