using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines manager operations for <see cref="TaskTypeAvailableBehaviorField" /> objects.
    /// </summary>
    public interface ITaskTypeBehaviorManager : IManagerBase<TaskTypeBehavior>
    {

        /// <summary>
        /// Finds the by task type identifier.
        /// </summary>
        /// <param name="taskTypeId">The task type identifier.</param>
        /// <returns></returns>
        IEnumerable<TaskTypeBehavior> FindByTaskTypeId(Guid taskTypeId);
    }
}