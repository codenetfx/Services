using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class Container
    /// </summary>
    [DataContract]
    public class ContainerDto
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
        ///     Gets or sets the company id.
        /// </summary>
        /// <value>The company id.</value>
        [DataMember]
        public Guid CompanyId { get; set; }

        /// <summary>
        ///     Gets or sets the primary search entity id.
        /// </summary>
        /// <value>The primary search entity id.</value>
        [DataMember]
        public Guid PrimarySearchEntityId { get; set; }

        /// <summary>
        ///     Gets or sets the type of the primary search entity.
        /// </summary>
        /// <value>The type of the primary search entity.</value>
        [DataMember]
        public string PrimarySearchEntityType { get; set; }

        /// <summary>
        ///     Gets or sets the available claims.
        /// </summary>
        /// <value>The available claims.</value>
        [DataMember]
        public List<ContainerAvailableClaimDto> AvailableClaims { get; set; }

        /// <summary>
        ///     Gets or sets the permission groupings.
        /// </summary>
        /// <value>The permission groupings.</value>
        [DataMember]
        public List<ContainerListDto> ContainerLists { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool IsDeleted { get; set; }
    }
}