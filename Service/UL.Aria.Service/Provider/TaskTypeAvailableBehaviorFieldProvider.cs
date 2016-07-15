using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Implements a provider for <see cref="TaskTypeAvailableBehaviorField"/> objects.
    /// </summary>
    public class TaskTypeAvailableBehaviorFieldProvider : SearchProviderBase<TaskTypeAvailableBehaviorField>, ITaskTypeAvailableBehaviorFieldProvider
    {
        private readonly ITaskTypeAvailableBehaviorFieldRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTypeAvailableBehaviorFieldProvider"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public TaskTypeAvailableBehaviorFieldProvider(ITaskTypeAvailableBehaviorFieldRepository repository, IPrincipalResolver principalResolver) : base(repository, principalResolver)
        {
            _repository = repository;
        }

        /// <summary>
        /// Finds the by task type available behavior identifier.
        /// </summary>
        /// <param name="taskTypeAvailableBehaviorId">The task type available behavior identifier.</param>
        /// <returns></returns>
        public IEnumerable<TaskTypeAvailableBehaviorField> FindByTaskTypeAvailableBehaviorId(Guid taskTypeAvailableBehaviorId)
        {
            return _repository.FindByTaskTypeAvailableBehaviorId(taskTypeAvailableBehaviorId);
        }
    }
}