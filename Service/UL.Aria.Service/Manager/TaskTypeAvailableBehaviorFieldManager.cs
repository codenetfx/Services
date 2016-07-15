using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements manager operations for <see cref="TaskTypeAvailableBehaviorField" /> objects.
    /// </summary>
    public class TaskTypeAvailableBehaviorFieldManager : ManagerBase<TaskTypeAvailableBehaviorField>, ITaskTypeAvailableBehaviorFieldManager
    {
        private readonly ITaskTypeAvailableBehaviorFieldProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTypeAvailableBehaviorFieldManager"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public TaskTypeAvailableBehaviorFieldManager(ITaskTypeAvailableBehaviorFieldProvider provider)
            : base(provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Finds the by task type available behavior identifier.
        /// </summary>
        /// <param name="taskTypeAvailableBehaviorId">The task type available behavior identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<TaskTypeAvailableBehaviorField> FindByTaskTypeAvailableBehaviorId(Guid taskTypeAvailableBehaviorId)
        {
            return _provider.FindByTaskTypeAvailableBehaviorId(taskTypeAvailableBehaviorId);
        }
    }
}