using System;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Product family domain entity.
    /// </summary>
    public class ProductFamilyAttribute : ProductFamilyCharacteristicDomainEntity
    {
        /// <summary>
        /// Gets or sets the data type id.
        /// </summary>
        /// <value>
        /// The data type id.
        /// </value>
        public ProductFamilyCharacteristicDataType DataTypeId { get; set; }
    }
}