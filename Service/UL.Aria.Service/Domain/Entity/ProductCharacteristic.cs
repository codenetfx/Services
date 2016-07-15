using System;

using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Represents a value saved for a product characteristic
    /// </summary>
    public class ProductCharacteristic : TrackedDomainEntity
    {
        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the group.
        /// </summary>
        /// <value>
        ///     The group.
        /// </value>
        public string Group { get; set; }

        /// <summary>
        ///     Gets or sets the characteristic id.
        /// </summary>
        /// <value>
        ///     The characteristic id.
        /// </value>
        public Guid ProductFamilyCharacteristicId { get; set; }

        /// <summary>
        ///     Gets or sets the type of the product characteristic.
        /// </summary>
        /// <value>
        ///     The type of the product characteristic.
        /// </value>
        public ProductFamilyCharacteristicType ProductFamilyCharacteristicType { get; set; }

        /// <summary>
        ///     Gets or sets the product id.
        /// </summary>
        /// <value>
        ///     The product id.
        /// </value>
        public Guid ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the type of the data.
        /// </summary>
        /// <value>
        ///     The type of the data.
        /// </value>
        public ProductFamilyCharacteristicDataType DataType { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is multivalue allowed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is multivalue allowed; otherwise, <c>false</c>.
        /// </value>
        public bool IsMultivalueAllowed { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is range allowed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is range allowed; otherwise, <c>false</c>.
        /// </value>
        public bool IsRangeAllowed { get; set; }

        /// <summary>
        ///     Gets or sets the parent id.
        /// </summary>
        /// <value>The parent id.</value>
        public Guid? ParentId { get; set; }

        /// <summary>
        ///     Gets or sets the type of the child.
        /// </summary>
        /// <value>The type of the child.</value>
        public ProductCharacteristicChildType? ChildType { get; set; }

        /// <summary>
        ///     Gets or sets the sort order.
        /// </summary>
        /// <value>The sort order.</value>
        public int SortOrder { get; set; }

        /// <summary>
        ///     Parses <see cref="Value" /> into multiple values based on standard delimiters.
        /// </summary>
        /// <param name="parseCombinations">if set to <c>true</c> combination values will also be parsed into their components.</param>
        /// <returns></returns>
        public string[] ParseMultiValue(bool parseCombinations = true)
        {
            var splits = parseCombinations ? new[] {ExcelTemplateKeys.StandardDelimiter, '/', '&'} : new[] {ExcelTemplateKeys.StandardDelimiter};
            return Value.SplitAndTrim(splits);
        }

        /// <summary>
        ///     Parses <see cref="Value" />  into ranges.
        /// </summary>
        /// <returns></returns>
        public string[] ParseRange()
        {
            return Value.SplitAndTrim('-');
        }
    }
}