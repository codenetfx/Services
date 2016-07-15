using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ProductFamilyCharacteristicUploadDto
    /// </summary>
    [DataContract]
    public class ProductFamilyCharacteristicUploadDto
    {
        /// <summary>
        ///     Gets or sets the upload action.
        /// </summary>
        /// <value>
        ///     The upload action.
        /// </value>
        [DataMember]
        public string UploadAction { get; set; }

        /// <summary>
        ///     Gets or sets the entity.
        /// </summary>
        /// <value>
        ///     The entity.
        /// </value>
        [DataMember]
        public ProductFamilyCharacteristicDomainEntityDto Entity { get; set; }
    }
}