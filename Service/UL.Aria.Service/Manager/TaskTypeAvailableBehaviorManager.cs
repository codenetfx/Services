using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements manager operations for <see cref="TaskTypeAvailableBehavior" /> objects.
    /// </summary>
    public class TaskTypeAvailableBehaviorManager :ManagerBase<TaskTypeAvailableBehavior>,  ITaskTypeAvailableBehaviorManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTypeAvailableBehaviorManager"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public TaskTypeAvailableBehaviorManager(ITaskTypeAvailableBehaviorProvider provider) : base(provider)
        {
        }
    }
}