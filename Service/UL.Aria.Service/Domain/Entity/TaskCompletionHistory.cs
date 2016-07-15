using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Class TaskCompletionHistoryDtoy
    /// </summary>
    [Serializable]
    public class TaskCompletionHistory : TaskHistoryBase
    {
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Completion { get; set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public override string Value
        {
            get { return Completion; }
        }
    }
}