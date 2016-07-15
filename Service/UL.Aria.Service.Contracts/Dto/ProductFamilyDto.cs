using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     ProductFamily defines a set of characteristics and validation
    ///     for products entered in the Aria systems.
    /// </summary>
    [DataContract]
    public class ProductFamilyDto
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
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]       
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>
        ///     The status.
        /// </value>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        ///     Gets or sets the category id.
        /// </summary>
        /// <value>
        ///     The category id.
        /// </value>
        [DataMember]
        public Guid CategoryId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [allow changes].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [allow changes]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool AllowChanges { get; set; }

        /// <summary>
        ///     Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        [DataMember]
        public Guid CreatedById { get; set; }

        /// <summary>
        ///     Gets or sets the updated by.
        /// </summary>
        /// <value>The updated by.</value>
        [DataMember]
        public Guid UpdatedById { get; set; }

        /// <summary>
        ///     Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        [DataMember]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the updated date time.
        /// </summary>
        /// <value>The updated date time.</value>
        [DataMember]
        public DateTime UpdatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is disabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsDisabled { get; set; }


        /// <summary>
        /// Gets or sets the business unit id.
        /// </summary>
        /// <value>
        /// The business unit id.
        /// </value>
        [DataMember]
        public Guid BusinessUnitId { get; set; }
    }
}