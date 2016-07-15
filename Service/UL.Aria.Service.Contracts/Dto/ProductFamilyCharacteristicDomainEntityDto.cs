using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ProductFamilyCharacteristicDomainEntityDto
    /// </summary>
    [DataContract]
    public class ProductFamilyCharacteristicDomainEntityDto
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
        ///     The value required
        /// </summary>
        [DataMember]
        public bool IsValueRequired { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The display name.</value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the scope id.
        /// </summary>
        /// <value>The scope.</value>
        [DataMember]
        public Guid ScopeId { get; set; }

        /// <summary>
        ///     Gets or sets the characteristic type id.
        /// </summary>
        /// <value>
        ///     The characteristic type id.
        /// </value>
        [DataMember]
        public Guid CharacteristicTypeId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is required.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsRequired { get; set; }

        /// <summary>
        ///     Gets or sets the options.
        /// </summary>
        /// <value>
        ///     The options.
        /// </value>
        [DataMember]
        public IList<ProductFamilyCharacteristicOptionDto> Options { get; set; }

        /// <summary>
        ///     Gets or sets the unit of measure id.
        /// </summary>
        /// <value>
        ///     The unit of measure id.
        /// </value>
        [DataMember]
        public Guid? UnitOfMeasureId { get; set; }

        /// <summary>
        ///     Gets or sets the sort order.
        /// </summary>
        /// <value>The sort order.</value>
        [DataMember]
        public int SortOrder { get; set; }

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