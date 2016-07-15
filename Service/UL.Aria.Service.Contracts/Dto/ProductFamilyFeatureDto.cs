using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Features that describe product families.
    /// </summary>
    [DataContract]
    public class ProductFamilyFeatureDto : ProductFamilyCharacteristicDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether [allow changes].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow changes]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool AllowChanges { get; set; }
    }
}