using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductFamilyCharacteristicOption:TrackedDomainEntity
    {
        /// <summary>
        /// Gets or sets the product family characteristic id.
        /// </summary>
        /// <value>
        /// The product family characteristic id.
        /// </value>
        public Guid ProductFamilyCharacteristicId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

    }

    /// <summary>
    /// List of available names for options.
    /// </summary>
    public static class ProductFamilyCharacteristicOptionName
    {
        /// <summary>
        /// The allow value types
        /// </summary>
        public const string AllowValueTypes = "AllowValueTypes";
    }


    /// <summary>
    /// List of available values for options.
    /// </summary>
    public static class ProductFamilyCharacteristicOptionValue
    {
        /// <summary>
        /// The multiple value
        /// </summary>
        public const string MultipleValue = "Multiple";
        /// <summary>
        /// The range value
        /// </summary>
        public const string RangeValue = "Range";
    }
}
