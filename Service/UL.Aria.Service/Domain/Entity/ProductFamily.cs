using System;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Product Family domain entity.
    /// </summary>
    public sealed class ProductFamily : TrackedDomainEntity
    {
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }


        /// <summary>
        ///     Gets or sets the category id.
        /// </summary>
        /// <value>
        ///     The category id.
        /// </value>
        public Guid CategoryId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [allow changes].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [allow changes]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowChanges { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets the product family status.
        /// </summary>
        /// <value>
        ///     The product family status.
        /// </value>
        public ProductFamilyStatus Status
        {
            get
            {
                if (IsDisabled)
                    return ProductFamilyStatus.Inactive;
                if (!AllowChanges)
                    return ProductFamilyStatus.Active;
                return ProductFamilyStatus.Draft;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is disabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is disabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets the business unit id.
        /// </summary>
        /// <value>
        /// The business unit id.
        /// </value>
        public Guid? BusinessUnitId { get; set; }
    }
}