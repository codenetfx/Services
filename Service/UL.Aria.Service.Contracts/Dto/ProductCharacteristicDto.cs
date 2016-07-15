using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Product Family Attribute Value
    /// </summary>
    [DataContract]
    public class ProductCharacteristicDto : TrackedEntityDto
    {
        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
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
        ///     Gets or sets the group.
        /// </summary>
        /// <value>
        ///     The group.
        /// </value>
        [DataMember]
        public string Group { get; set; }

        /// <summary>
        ///     Gets or sets the product family characteristic id.
        /// </summary>
        /// <value>
        ///     The product family characteristic id.
        /// </value>
        [DataMember]
        public Guid ProductFamilyCharacteristicId { get; set; }

        /// <summary>
        ///     Gets or sets the type of the product characteristic.
        /// </summary>
        /// <value>
        ///     The type of the product characteristic.
        /// </value>
        [DataMember]
        public ProductFamilyCharacteristicTypeDto ProductFamilyCharacteristicType { get; set; }

        /// <summary>
        ///     Gets or sets the product id.
        /// </summary>
        /// <value>
        ///     The product id.
        /// </value>
        [DataMember]
        public Guid ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the type of the data.
        /// </summary>
        /// <value>
        ///     The type of the data.
        /// </value>
        [DataMember]
        public ProductFamilyCharacteristicDataType DataType { get; set; }

        /// <summary>
        ///     Gets or sets the parent id.
        /// </summary>
        /// <value>The parent id.</value>
        [DataMember]
        public Guid? ParentId { get; set; }

        /// <summary>
        ///     Gets or sets the type of the child.
        /// </summary>
        /// <value>The type of the child.</value>
        [DataMember]
        public ProductCharacteristicChildType? ChildType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is multivalue allowed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is multivalue allowed; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsMultivalueAllowed { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is range allowed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is range allowed; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsRangeAllowed { get; set; }

        /// <summary>
        ///     Gets or sets the sort order.
        /// </summary>
        /// <value>The sort order.</value>
        [DataMember]
        public int SortOrder { get; set; }
    }
}