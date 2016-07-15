using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class TaskComment
    /// </summary>    
    [Serializable]
    public class TaskComment
    {
        /// <summary>
        ///     Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }

        /// <summary>
        ///     Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public string CreatedBy { get; set; }

        /// <summary>
        ///     Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime CreatedDate { get; set; }
    }
}