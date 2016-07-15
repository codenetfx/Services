using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Business Unit dto Model
    /// </summary>
    [DataContract]
    public class BusinessUnitDto
    {
        /// <summary>
        /// Primary BusinessUnitId
        /// </summary>
       [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        /// BusinessUnitName
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// BusinessUnitCode
        /// </summary>
       [DataMember]
        public string Code { get; set; }

       /// <summary>
       /// Gets or sets the note.
       /// </summary>
       /// <value>
       /// The note.
       /// </value>
      [DataMember]
       public string Note { get; set; }


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

       /// <summary>
       /// Gets or sets the updated by login identifier.
       /// </summary>
       /// <value>
       /// The updated by login identifier.
       /// </value>
       [DataMember]
       public string UpdatedByLoginId { get; set; }
       /// <summary>
       /// Gets or sets the created by login identifier.
       /// </summary>
       /// <value>
       /// The created by login identifier.
       /// </value>
       [DataMember]
       public string CreatedByLoginId { get; set; }

       /// <summary>
       /// Gets or sets a value indicating whether this instance is prevented from being deleted.
       /// </summary>
       /// <value>
       /// <c>true</c> if this instance is delete prevented; otherwise, <c>false</c>.
       /// </value>
        [DataMember]
       public bool IsDeletePrevented { get; set; }
    }
}
