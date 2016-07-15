using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Domain.Repository
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AuditableEntity : DomainEntity, IAuditableEntity
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="AuditableEntity"/> class.
        /// </summary>
        protected AuditableEntity()
        {
        }
     
        /// <summary>
        ///     Gets or sets the user it was created by.
        /// </summary>
        /// <value>
        ///     The created by.
        /// </value>
        public Guid CreatedById { get; set; }

        /// <summary>
        ///     Gets or sets the created on.
        /// </summary>
        /// <value>
        ///     The created on.
        /// </value>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the updated on.
        /// </summary>
        /// <value>
        ///     The updated on.
        /// </value>
        public DateTime UpdatedDateTime { get; set; }

        /// <summary>
        ///     Gets or sets who it was updated by.
        /// </summary>
        /// <value>
        ///     The updated by person.
        /// </value>
        public Guid UpdatedById { get; set; }
    }
}
