using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Defines Repository operations for <see cref="TaskTypeAvailableBehaviorField"/> objects.
    /// </summary>
    public interface ITaskTypeAvailableBehaviorFieldRepository : IPrimaryEntityRepository<TaskTypeAvailableBehaviorField>
    {
        /// <summary>
        /// Finds the by task type available behavior identifier.
        /// </summary>
        /// <param name="taskTypeAvailableBehaviorId">The task type available behavior identifier.</param>
        /// <returns></returns>
        IEnumerable<TaskTypeAvailableBehaviorField> FindByTaskTypeAvailableBehaviorId(Guid taskTypeAvailableBehaviorId);
    }
}