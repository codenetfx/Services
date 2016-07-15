using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Defines Repository operations for <see cref="TaskTypeBehavior"/> objects.
    /// </summary>
    public interface ITaskTypeBehaviorRepository:  IPrimaryEntityRepository<TaskTypeBehavior>
    {
        /// <summary>
        /// Finds the by task type identifier.
        /// </summary>
        /// <param name="taskTypeId">The task type identifier.</param>
        /// <returns></returns>
        IEnumerable<TaskTypeBehavior> FindByTaskTypeId(Guid taskTypeId);
    }
}