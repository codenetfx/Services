using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Provides a Repository interface for Task Type Notifications.
    /// </summary>
    public interface ITaskTypeNotificationRepository : IPrimaryAssocatedRepository<TaskTypeNotification>
    {
        /// <summary>
        /// Finds the by task task identifier.
        /// </summary>
        /// <param name="taskTypeId">The task type identifier.</param>
        /// <returns></returns>
        IEnumerable<TaskTypeNotification> FindByTaskTypeId(Guid taskTypeId);
    }
}
