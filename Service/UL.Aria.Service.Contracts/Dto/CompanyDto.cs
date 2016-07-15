using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Company data transfer object.
    /// </summary>
    [DataContract]
    public class CompanyDto
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>
        ///     The id.
        /// </value>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the created on.
        /// </summary>
        /// <value>
        ///     The created on.
        /// </value>
        [DataMember]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the created by.
        /// </summary>
        /// <value>
        ///     The created by.
        /// </value>
        [DataMember]
        public Guid CreatedById { get; set; }

        /// <summary>
        ///     Gets or sets the updated on.
        /// </summary>
        /// <value>
        ///     The updated on.
        /// </value>
        [DataMember]
        public DateTime UpdatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets id of the person who last updated this.
        /// </summary>
        /// <value>
        ///     The updated by.
        /// </value>
        [DataMember]
        public Guid UpdatedById { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the external ids.
        /// </summary>
        /// <value>The external ids.</value>
        [DataMember]
        public IList<string> ExternalIds { get; set; }
    }
}