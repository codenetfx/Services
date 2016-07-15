using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provider for <see cref="TaskTypeAvailableBehaviorField"/> objects.
    /// </summary>
    public interface ITaskTypeAvailableBehaviorFieldProvider :ISearchProviderBase<TaskTypeAvailableBehaviorField>
    {
        /// <summary>
        /// Finds the by task type available behavior identifier.
        /// </summary>
        /// <param name="taskTypeAvailableBehaviorId">The task type available behavior identifier.</param>
        /// <returns></returns>
        IEnumerable<TaskTypeAvailableBehaviorField> FindByTaskTypeAvailableBehaviorId(Guid taskTypeAvailableBehaviorId);
    }
}
