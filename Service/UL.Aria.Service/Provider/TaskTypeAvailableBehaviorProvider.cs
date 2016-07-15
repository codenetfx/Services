using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Implements a provider for <see cref="TaskTypeAvailableBehavior"/> objects.
    /// </summary>
    public class TaskTypeAvailableBehaviorProvider : SearchProviderBase<TaskTypeAvailableBehavior>, ITaskTypeAvailableBehaviorProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTypeAvailableBehaviorProvider"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public TaskTypeAvailableBehaviorProvider(ITaskTypeAvailableBehaviorRepository repository, IPrincipalResolver principalResolver) : base(repository, principalResolver)
        {
        }
    }
}