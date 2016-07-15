using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ProductFamilyFeatureAssociationDto
    /// </summary>
    [DataContract]
    public class ProductFamilyFeatureAssociationDto
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
        ///     Gets or sets the dependent.
        /// </summary>
        /// <value>
        ///     The dependent.
        /// </value>
        [DataMember]
        public string Dependent { get; set; }

        /// <summary>
        ///     Gets or sets the parent family allowed feature id.
        /// </summary>
        /// <value>
        ///     The parent family allowed feature id.
        /// </value>
        [DataMember]
        public Guid ParentFamilyAllowedFeatureId { get; set; }

        /// <summary>
        /// Gets or sets the product family id.
        /// </summary>
        /// <value>
        /// The product family id.
        /// </value>
        [DataMember]
        public Guid ProductFamilyId { get; set; }

        /// <summary>
        /// Gets or sets the feature id.
        /// </summary>
        /// <value>
        /// The feature id.
        /// </value>
        [DataMember]
        public Guid CharacteristicId { get; set; }

        /// <summary>
        /// Gets or sets the is disabled.
        /// </summary>
        /// <value>
        /// The is disabled.
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