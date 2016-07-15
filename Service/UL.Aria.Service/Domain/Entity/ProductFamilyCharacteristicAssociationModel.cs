using System;
using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class ProductFamilyCharacteristicUpload
    /// </summary>
    public class ProductFamilyCharacteristicAssociationModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyCharacteristicAssociationModel"/> class.
        /// </summary>
        public ProductFamilyCharacteristicAssociationModel()
        {
            OptionIds=new List<Guid>();
        }
        /// <summary>
        ///     Gets or sets the characteristic identifier.
        /// </summary>
        /// <value>The characteristic identifier.</value>
        public Guid CharacteristicId { get; set; }

        /// <summary>
        ///     Gets or sets the characteristic type id.
        /// </summary>
        /// <value>
        ///     The characteristic type id.
        /// </value>
        public ProductFamilyCharacteristicType CharacteristicType { get; set; }

        /// <summary>
        ///     Gets or sets the allowed feature value ids.
        /// </summary>
        /// <value>The allowed feature value ids.</value>
        public IList<Guid> AllowedFeatureValueIds { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public IList<Guid> OptionIds  { get; set; }
    }
}