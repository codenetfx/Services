using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ProductFamilyFeatureAllowedValueDependencyUpload
    /// </summary>
    [DataContract]
    public class ProductFamilyFeatureDependencyDto
    {
        /// <summary>
        ///     Gets or sets the parent.
        /// </summary>
        /// <value>
        ///     The parent.
        /// </value>
        [DataMember]
        public Guid ParentId { get; set; }

        /// <summary>
        ///     Gets or sets the parent value ids.
        /// </summary>
        /// <value>The parent value ids.</value>
        [DataMember]
        public IEnumerable<Guid> ParentValueIds { get; set; }

        /// <summary>
        ///     Gets or sets the child.
        /// </summary>
        /// <value>
        ///     The child.
        /// </value>
        [DataMember]
        public Guid ChildId { get; set; }

        /// <summary>
        ///     Gets or sets the child value ids.
        /// </summary>
        /// <value>The child value ids.</value>
        [DataMember]
        public IEnumerable<Guid> ChildValueIds { get; set; }
    }
}