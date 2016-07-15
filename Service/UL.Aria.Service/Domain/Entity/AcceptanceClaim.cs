using System;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// domain entity for acceptance claim
    /// </summary>
    public class AcceptanceClaim : DomainEntity
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptanceClaim" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public AcceptanceClaim(Guid? id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptanceClaim" /> class.
        /// </summary>
        public AcceptanceClaim()
            : this(Guid.NewGuid())
        {
        }


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AcceptanceClaim"/> is accepted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if accepted; otherwise, <c>false</c>.
        /// </value>
        public bool Accepted { get; set; }

        /// <summary>
        /// Gets or sets the accepted date time.
        /// </summary>
        /// <value>
        /// The accepted date time.
        /// </value>
        public DateTime AcceptedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the terms and conditions id.
        /// </summary>
        /// <value>
        /// The terms and conditions id.
        /// </value>
        public Guid TermsAndConditionsId { get; set; }
    }
}
