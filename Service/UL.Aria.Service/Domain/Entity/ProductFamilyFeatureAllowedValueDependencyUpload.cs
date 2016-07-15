using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Upload payload class for <see cref="ProductFamilyFeatureAllowedValueDependency"/>
    /// </summary>
    public class ProductFamilyFeatureAllowedValueDependencyUpload
    {
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public string Parent { get; set; }

        /// <summary>
        /// Gets or sets the parent values.
        /// </summary>
        /// <value>
        /// The parent values.
        /// </value>
        public IEnumerable<string> ParentValues { get; set; }

        /// <summary>
        /// Gets or sets the child.
        /// </summary>
        /// <value>
        /// The child.
        /// </value>
        public string Child { get; set; }

        /// <summary>
        /// Gets or sets the child values.
        /// </summary>
        /// <value>
        /// The child values.
        /// </value>
        public IEnumerable<string> ChildValues { get; set; }

        /// <summary>
        /// Gets or sets the family feature allowed value dependency constructe from values.
        /// </summary>
        /// <value>
        /// The family feature allowed value dependency.
        /// </value>
        public ProductFamilyFeatureAllowedValueDependency FamilyFeatureAllowedValueDependency { get; set; }

        /// <summary>
        /// Gets or sets the upload action.
        /// </summary>
        /// <value>
        /// The upload action.
        /// </value>
        public UploadAction UploadAction { get; set; }
}
}