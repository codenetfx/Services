using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ProductFamilyFeatureAllowedValueDependencyUploadDto
    /// </summary>
    [DataContract]
    public class ProductFamilyFeatureAllowedValueDependencyUploadDto
    {
        /// <summary>
        ///     Gets or sets the parent.
        /// </summary>
        /// <value>
        ///     The parent.
        /// </value>
        [DataMember]
        public string Parent { get; set; }

        /// <summary>
        ///     Gets or sets the parent values.
        /// </summary>
        /// <value>
        ///     The parent values.
        /// </value>
        [DataMember]
        public IEnumerable<string> ParentValues { get; set; }

        /// <summary>
        ///     Gets or sets the child.
        /// </summary>
        /// <value>
        ///     The child.
        /// </value>
        [DataMember]
        public string Child { get; set; }

        /// <summary>
        ///     Gets or sets the child values.
        /// </summary>
        /// <value>
        ///     The child values.
        /// </value>
        [DataMember]
        public IEnumerable<string> ChildValues { get; set; }

        /// <summary>
        ///     Gets or sets the upload action.
        /// </summary>
        /// <value>
        ///     The upload action.
        /// </value>
        [DataMember]
        public string UploadAction { get; set; }
    }
}