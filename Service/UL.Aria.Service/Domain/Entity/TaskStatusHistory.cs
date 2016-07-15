using System;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Class TaskStatusHistory
    /// </summary>
    [Serializable]
    public class TaskStatusHistory : TaskHistoryBase
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public TaskStatusEnumDto Status { get; set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public  override string Value
        {
            get { return Status.ToString(); }
        }
    }
}