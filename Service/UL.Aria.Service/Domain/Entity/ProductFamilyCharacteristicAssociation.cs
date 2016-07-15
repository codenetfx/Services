using System;
using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Base type for associations beweeen characteristics and product families.
    /// </summary>
    public abstract class ProductFamilyCharacteristicAssociation : TrackedDomainEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyCharacteristicAssociation"/> class.
        /// </summary>
        public ProductFamilyCharacteristicAssociation()
        {
            OptionIds=new List<Guid>();
        }

        /// <summary>
        /// Gets or sets the product family id.
        /// </summary>
        /// <value>
        /// The product family id.
        /// </value>
        public Guid ProductFamilyId { get; set; }

        /// <summary>
        /// Gets or sets the feature id.
        /// </summary>
        /// <value>
        /// The feature id.
        /// </value>
        public Guid CharacteristicId { get; set; }

        /// <summary>
        /// Gets or sets the is disabled.
        /// </summary>
        /// <value>
        /// The is disabled.
        /// </value>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public IList<Guid> OptionIds { get; set; }
    }
}