using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class CategoryDto
    /// </summary>
    [DataContract]
    public class CategoryDto
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the created by id.
        /// </summary>
        /// <value>The created by id.</value>
        [DataMember]
        public Guid CreatedById { get; set; }

        /// <summary>
        ///     Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        [DataMember]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the updated by id.
        /// </summary>
        /// <value>The updated by id.</value>
        [DataMember]
        public Guid UpdatedById { get; set; }

        /// <summary>
        ///     Gets or sets the updated date time.
        /// </summary>
        /// <value>The updated date time.</value>
        [DataMember]
        public DateTime UpdatedDateTime { get; set; }
    }
}