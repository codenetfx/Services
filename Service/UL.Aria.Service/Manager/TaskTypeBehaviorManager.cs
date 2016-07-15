using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements manager operations for <see cref="TaskTypeAvailableBehaviorField" /> objects.
    /// </summary>
    public class TaskTypeBehaviorManager : ManagerBase<TaskTypeBehavior>, ITaskTypeBehaviorManager
    {
        private readonly ITaskTypeBehaviorProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTypeAvailableBehaviorFieldManager"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public TaskTypeBehaviorManager(ITaskTypeBehaviorProvider provider)
            : base(provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Finds the by task type identifier.
        /// </summary>
        /// <param name="taskTypeId">The task type identifier.</param>
        /// <returns></returns>
        public IEnumerable<TaskTypeBehavior> FindByTaskTypeId(Guid taskTypeId)
        {
            return _provider.FindByTaskTypeId(taskTypeId);
        }
    }
}