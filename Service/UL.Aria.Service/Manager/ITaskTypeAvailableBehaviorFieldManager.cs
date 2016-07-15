using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines manager operations for <see cref="TaskTypeAvailableBehaviorField" /> objects.
    /// </summary>
    public interface ITaskTypeAvailableBehaviorFieldManager : IManagerBase<TaskTypeAvailableBehaviorField>
    {
        /// <summary>
        /// Finds the by task type available behavior identifier.
        /// </summary>
        /// <param name="taskTypeAvailableBehaviorId">The task type available behavior identifier.</param>
        /// <returns></returns>
        IEnumerable<TaskTypeAvailableBehaviorField> FindByTaskTypeAvailableBehaviorId(Guid taskTypeAvailableBehaviorId);
    }
}