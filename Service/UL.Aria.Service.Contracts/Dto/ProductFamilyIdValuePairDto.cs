using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Id value pair of <see cref="ProductFamilyDto" /> instances.
    /// </summary>
    [DataContract]
    public struct ProductFamilyIdValuePairDto
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>
        ///     The id.
        /// </value>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        [DataMember]
        public ProductFamilyDto Value { get; set; }
    }
}