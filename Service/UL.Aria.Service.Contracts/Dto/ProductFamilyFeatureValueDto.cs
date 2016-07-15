using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ProductFamilyFeatureValueDto
    /// </summary>
    [DataContract]
    public class ProductFamilyFeatureValueDto
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        ///     Gets or sets the product family feature id.
        /// </summary>
        /// <value>
        ///     The product family feature id.
        /// </value>
        [DataMember]
        public Guid FeatureId { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        ///     Gets or sets the xtype.
        /// </summary>
        /// <value>
        ///     The xtype.
        /// </value>
        [DataMember]
        public byte Xtype { get; set; }

        /// <summary>
        ///     Gets or sets the max.
        /// </summary>
        /// <value>
        ///     The max.
        /// </value>
        [DataMember]
        public string Maximum { get; set; }

        /// <summary>
        ///     Gets or sets the minimum.
        /// </summary>
        /// <value>
        ///     The minimum.
        /// </value>
        [DataMember]
        public string Minimum { get; set; }

        /// <summary>
        ///     Gets or sets the unit of measure.
        /// </summary>
        /// <value>
        ///     The unit of measure.
        /// </value>
        [DataMember]
        public UnitOfMeasureDto UnitOfMeasure { get; set; }

        /// <summary>
        ///     Gets or sets the created by id.
        /// </summary>
        /// <value>The created by id.</value>
        [DataMember]
        public Guid CreatedById { get; set; }

        /// <summary>
        ///     Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        [DataMember]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the updated by id.
        /// </summary>
        /// <value>The updated by id.</value>
        [DataMember]
        public Guid UpdatedById { get; set; }

        /// <summary>
        ///     Gets or sets the updated date time.
        /// </summary>
        /// <value>The updated date time.</value>
        [DataMember]
        public DateTime UpdatedDateTime { get; set; }
    }
}