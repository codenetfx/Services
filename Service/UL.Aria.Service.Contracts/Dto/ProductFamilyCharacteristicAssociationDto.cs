using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ProductFamilyCharacteristicUpload
    /// </summary>
    [DataContract]
    public class ProductFamilyCharacteristicAssociationDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductFamilyCharacteristicAssociationDto"/> class.
        /// </summary>
        public ProductFamilyCharacteristicAssociationDto()
        {
            OptionIds=new List<Guid>();
            AllowedFeatureValueIds = new List<Guid>();
        }
        /// <summary>
        ///     Gets or sets the characteristic identifier.
        /// </summary>
        /// <value>The characteristic identifier.</value>
        [DataMember]
        public Guid CharacteristicId { get; set; }

        /// <summary>
        ///     Gets or sets the characteristic type id.
        /// </summary>
        /// <value>
        ///     The characteristic type id.
        /// </value>
        [DataMember]
        public string CharacteristicType { get; set; }

        /// <summary>
        ///     Gets or sets the allowed feature value ids.
        /// </summary>
        /// <value>The allowed feature value ids.</value>
        [DataMember]
        public IList<Guid> AllowedFeatureValueIds { get; set; }


        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        [DataMember]
        public IList<Guid> OptionIds { get; private set; }
    }
}