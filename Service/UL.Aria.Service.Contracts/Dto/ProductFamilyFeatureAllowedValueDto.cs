using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ProductFamilyFeatureAllowedValueDto
    /// </summary>
    [DataContract]
    public class ProductFamilyFeatureAllowedValueDto
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>
        ///     The id.
        /// </value>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the family.
        /// </summary>
        /// <value>
        /// The family id.
        /// </value>
        [DataMember]
        public Guid FamilyId { get; set; }

        /// <summary>
        /// Gets or sets the feature value.
        /// </summary>
        /// <value>
        /// The feature value id.
        /// </value>
        [DataMember]
        public ProductFamilyFeatureValueDto FeatureValue { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this instance is disabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsDisabled { get; set; }

        /// <summary>
        ///     Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        [DataMember]
        public Guid CreatedById { get; set; }

        /// <summary>
        ///     Gets or sets the updated by.
        /// </summary>
        /// <value>The updated by.</value>
        [DataMember]
        public Guid UpdatedById { get; set; }

        /// <summary>
        ///     Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        [DataMember]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the updated date time.
        /// </summary>
        /// <value>The updated date time.</value>
        [DataMember]
        public DateTime UpdatedDateTime { get; set; }
    }
}