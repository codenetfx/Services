using System;

namespace UL.Aria.Service.Domain.View
{
    /// <summary>
    ///     Project creation emails
    /// </summary>
    public class ProjectAssignmentEmail
    {
        /// <summary>
        /// Gets or sets the project unique identifier.
        /// </summary>
        /// <value>
        /// The project unique identifier.
        /// </value>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the actor login unique identifier.
        /// </summary>
        /// <value>
        /// The actor login unique identifier.
        /// </value>
        public string ActorLoginId { get; set; }

        /// <summary>
        /// Gets the id of the acting user.
        /// </summary>
        public Guid ActorId { get; set; }

        /// <summary>
        /// Gets or sets the name of the recipient.
        /// </summary>
        /// <value>
        /// The name of the recipient.
        /// </value>
        public string RecipientName { get; set; }

        /// <summary>
        /// Gets or sets the name of the actor.
        /// </summary>
        ///<value>The actor name value.</value>         
        public string ActorName { get; set; }

        /// <summary>
        /// Gets or sets the due date.
        /// </summary>
        /// <value>
        /// The due date.
        /// </value>
        public DateTime? DueDate { get; set; }
    }
}