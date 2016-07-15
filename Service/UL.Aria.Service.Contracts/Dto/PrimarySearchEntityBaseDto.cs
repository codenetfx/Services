using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Container data transfer object class.
    /// </summary>
    [DataContract]
    public class PrimarySearchEntityBaseDto
    {
		/// <summary>
		/// Gets the id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[DataMember]
		public Guid? Id { get; set; }

		/// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary> 
        /// Gets or sets the type of the container.
        /// </summary>
        /// <value>
        /// The type of the container.
        /// </value>
        [DataMember]
        public EntityTypeEnumDto Type { get; set; }

        /// <summary>
        /// Gets or sets the company id.
        /// </summary>
        /// <value>
        /// The company id.
        /// </value>
        [DataMember]
        public Guid? CompanyId { get; set; }
    }
}