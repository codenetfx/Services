using System;
using UL.Enterprise.Foundation.Domain;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// domain entity dealing with Terms and Conditions
    /// </summary>
    public class TermsAndConditions : DomainEntity
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public TermsAndConditionsType Type { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the legal text.
        /// </summary>
        /// <value>
        /// The legal text.
        /// </value>
        public string LegalText { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the created by id.
        /// </summary>
        /// <value>
        /// The created by id.
        /// </value>
        public Guid CreatedById { get; set; }

        /// <summary>
        /// Gets or sets the updated date time.
        /// </summary>
        /// <value>
        /// The updated date time.
        /// </value>
        public DateTime UpdatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the updated by id.
        /// </summary>
        /// <value>
        /// The updated by id.
        /// </value>
        public Guid UpdatedById { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="TermsAndConditions" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public TermsAndConditions(Guid? id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TermsAndConditions" /> class.
        /// </summary>
        public TermsAndConditions() : this(Guid.NewGuid())
        {
        }

  
    }
}